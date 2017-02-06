using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]

public class PlayerController : MonoBehaviour {
    public int walkSpeed = 2;
    public int runSpeed = 5;
    public int rotationSpeed = 30;
    public float armor = 150;
    public float health = 100;
    public int weaponID = 0;
    public int bulletRound = 8;
    public float weaponDamage;

    public Transform rotationPivot;
    public EasyJoystick leftStick;
    public EasyJoystick rightStick;

    public Transform bullet;
    public Transform[] weaponList;
    public Transform[] bulletShell;
    public Transform[] weaponMag;
    public Transform[] weaponSpawn;
    public Transform[] bloodSplash;
    public Transform bulletSpawn;
    public Transform bulletShellSpawn;
    public Transform magSpawn;
    public Transform spark;
    public Transform dustParticle;
    public Transform zombieBlood;
    public Transform[] gunFire;
    public AudioClip[] weaponSound;
    public AudioClip[] changeMag;
    public static float damage2Bullet;
    public static Vector3 rotationAngle;

    private CharacterController playerController;
    private Transform mainCamera;
    private Transform currentWeapon = null;
    private bool isDead = false;

    private float moveStickX;
    private float moveStickY;
    private float rotateStickX;
    private float rotateStickY;

    private float shootInterval;
    private float shootTimer = 0;

    private Animation weaponAnim;
    private float pistolDamage = 20;
    private float sniperRifleDamage = 50;
    private float shotgunDamage = 50;
    private int loadRifleAnim;
    private int reloadRifleAnim;
    private int reloadShotgunAnim;
    private int loadShotgunAnim;
    private int bulletSwarm = 6;

    public static bool aim = false;
    public static bool isFire = false;
    public static int bulletNumber;
    public static Vector3 playerPosition;
    public static float armor2Data;
    public static float health2Data;

    private float layerWeight = 0;

    private GameObject touchScreenJoystick;
    private CharacterAnimation playerAnimation;
    private AudioSource playerAudio;
    private UIManager gameUI;

    private int currentAnim;

    void Awake() {
        playerController = this.GetComponent<CharacterController>();
        playerAnimation = this.GetComponent<CharacterAnimation>();
        playerAudio = this.GetComponent<AudioSource>();
        gameUI = GameObject.Find("GameManager").GetComponent<UIManager>();
        mainCamera = Camera.main.transform;
        touchScreenJoystick = GameObject.FindWithTag("Joystick");

        if (leftStick == null || rightStick == null) {
            Debug.LogError("No Joystick assigned! Please assign both left and right sticks");
            return;
        }

        if (bullet == null || bulletSpawn == null) {
            Debug.LogError("Please assign bullet prefab and bullet spawn to this Transform!");
            return;
        }

        switch (DataBase.d_Model) {
            #if UNITY_ANDROID
            case "Sony Ericsson R800i":
                break;
            #endif
            case "PS Vita":
                touchScreenJoystick.SetActive(false);
                break;
            default:
                break;
        }

        armor2Data = armor;
        health2Data = health;

        loadRifleAnim = Animator.StringToHash("ShootSniperRifle.shootSniperRifle");
        reloadRifleAnim = Animator.StringToHash("ShootSniperRifle.sniperRifleReload");
        reloadShotgunAnim = Animator.StringToHash("ShootShotGun.shotgunReload");
        loadShotgunAnim = Animator.StringToHash("ShootShotGun.shootShotgun");
    }

	// Use this for initialization
	void Start () {
        setWeapon(DataBase.currentWeaponID);
        bulletNumber = bulletRound;
	}
	
	// Update is called once per frame
	void Update () {
        if (UIManager.isPause == true) {
            return;
        }

        if (Application.loadedLevel == 2) {
            UIManager.armorLevel.value = armor2Data / armor;
            if (health2Data > 0) {
                UIManager.healthLevel.value = health2Data / health;
            }
            else if(health2Data <= 0){
                UIManager.healthLevel.value = 0;
            }
            if (UIManager.healthLevel.value <= 0.85f && UIManager.healthLevel.value > 0.55f) {
                UIManager.playerIcon.spriteName = "playerIcon_1";
            }
            else if (UIManager.healthLevel.value <= 0.55f && UIManager.healthLevel.value > 0.25f) {
                UIManager.playerIcon.spriteName = "playerIcon_2";
            }
            else if (UIManager.healthLevel.value <= 0.25f) {
                UIManager.playerIcon.spriteName = "playerIcon_3";
            }
        }

        if (health2Data <= 0) {
            playerAnimation.SetLayerWeight(DataBase.currentWeaponID + 1, 0);
            if (currentWeapon != null) {
                Destroy(currentWeapon.gameObject);
            }
            if (isDead == false) {
                isDead = true;
                Instantiate(bloodSplash[Random.Range(0, 4)], new Vector3(this.transform.position.x, bloodSplash[Random.Range(0, 4)].position.y + 0.015f, this.transform.position.z), Quaternion.Euler(new Vector3(0, Random.value * 360, 0)));
            }
            playerAnimation.SetAnimatorBool("IsDead", isDead);
            leftStick.enable = false;
            rightStick.enable = false;
            StartCoroutine(setDeadPanel());
            return;
        }

        switch (DataBase.d_Model) {
            #if UNITY_ANDROID
            case "Sony Ericsson R800i":
                if (XperiaInput.isKeypadAvailable == true) {
                    touchScreenJoystick.SetActive(false);
                    moveStickX = XperiaInput.XperiaLeftStick.x;
                    moveStickY = XperiaInput.XperiaLeftStick.y;
                    rotateStickX = XperiaInput.XperiaRightStick.x;
                    rotateStickY = XperiaInput.XperiaRightStick.y;
                }
                else {
                    touchScreenJoystick.SetActive(true);
                    moveStickX = leftStick.JoystickAxis.x;
                    moveStickY = leftStick.JoystickAxis.y;
                    rotateStickX = rightStick.JoystickAxis.x;
                    rotateStickY = rightStick.JoystickAxis.y;
                }
                break;
            #endif
            case "PS Vita":
                moveStickX = Input.GetAxis("LeftStickX");
                moveStickY = Input.GetAxis("LeftStickY");
                rotateStickX = Input.GetAxis("RightStickX");
                rotateStickY = Input.GetAxis("RightStickY");
                break;
            default:
                if (DataBase.isConsole == false) {
                    moveStickX = leftStick.JoystickAxis.x;
                    moveStickY = leftStick.JoystickAxis.y;
                    rotateStickX = rightStick.JoystickAxis.x;
                    rotateStickY = rightStick.JoystickAxis.y;
                }
                else {
                    moveStickX = Input.GetAxis("LeftStickX");
                    moveStickY = Input.GetAxis("LeftStickY");
                    rotateStickX = Input.GetAxis("RightStickX");
                    rotateStickY = Input.GetAxis("RightStickY");
                }
                break;
        }
        movePlayer(moveStickX, moveStickY, rotateStickX, rotateStickY);
        rotatePlayer(rotateStickX, rotateStickY);
	}

    private void movePlayer(float _moveStickX, float _moveStickY, float _rotateStickX, float _rotateStickY) {
        Vector3 movementSpace = mainCamera.TransformDirection(new Vector3(_moveStickX, 0, _moveStickY));
        movementSpace.y = 0;
        movementSpace.Normalize();

        Vector2 absAxis = new Vector2(Mathf.Abs(_moveStickX), Mathf.Abs(_moveStickY));
        
        if (absAxis.magnitude < 0.01f) { //Set Player Stop
            movementSpace *= 0 * ((absAxis.x > absAxis.y) ? absAxis.x : absAxis.y);
        }else if(absAxis.magnitude >= 0.01f && absAxis.magnitude < 0.7f){ //Set Player Walking
            movementSpace *= walkSpeed * ((absAxis.x > absAxis.y) ? absAxis.x : absAxis.y) * (health2Data / health);
        }else if(absAxis.magnitude >= 0.7f){ //Set Player Running
            movementSpace *= runSpeed * ((absAxis.x > absAxis.y) ? absAxis.x : absAxis.y) * (health2Data / health);
        }
        
        movementSpace += Physics.gravity;
        movementSpace *= Time.deltaTime;

        playerController.Move(movementSpace);

        if (UIManager.isPlayerEntered == true || UIManager.isPlayerinTrainingField == false) {
            _rotateStickX = 0;
            _rotateStickY = 0;
        }

        if (_rotateStickX != 0 || _rotateStickY != 0) {
            faceToDirection(false);
        }
        else {
            faceToDirection(true);
        }

        playerPosition = this.transform.position;

        if (absAxis.magnitude > 0 && UIManager.sentenceID == 1) {
            gameUI.changeDiologContent();
        }
    }

    private void rotatePlayer(float _rotateStickX, float _rotateStickY) {
        if (UIManager.isPlayerEntered == true || UIManager.isPlayerinTrainingField == false) {
            aim = false;
            isFire = false;
            playerAnimation.SetAnimatorBool("ToAim", aim);
            return;
        }
        Vector3 rotationSpace = mainCamera.TransformDirection(new Vector3(_rotateStickX, 0, _rotateStickY));
        rotationSpace.y = 0;
        rotationSpace.Normalize();
        rotationSpace *= Time.deltaTime;

        Vector2 absAxis = new Vector2(Mathf.Abs(_rotateStickX), Mathf.Abs(_rotateStickY));
        rotationSpace *= (absAxis.x > absAxis.y) ? absAxis.x : absAxis.y;

        if (absAxis.magnitude > 0 && UIManager.sentenceID == 5) {
            UIManager.DioAnim.SetBool("hide", true);
        }

        if (absAxis.magnitude > 0 && UIManager.sentenceID == 7 && UIManager.isPlayerTrained == true) {
            UIManager.DioAnim.SetBool("hide", true);
        }

        if (Mathf.Abs(_rotateStickX) > 0 || Mathf.Abs(_rotateStickY) > 0) {
            float currentAngle = rotationPivot.eulerAngles.y;
            float targetAngle = MathAngle(rotationSpace.x, rotationSpace.z);
            rotationPivot.eulerAngles = new Vector3(0, Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed), 0);
            rotationAngle = rotationPivot.forward;
            aim = true;
            if (aim == true) {
                playerAnimation.SetAnimatorBool("IsReload", false);
                playerAnimation.SetAnimatorBool("IsShoot", false);
                currentAnim = playerAnimation.animationController.GetCurrentAnimatorStateInfo(DataBase.currentWeaponID + 1).nameHash;
                bulletNumber = bulletRound;
                UIManager.bulletCount.text = bulletRound.ToString();
                shootTimer -= Time.deltaTime;
                if (shootTimer <= 0) {
                    if (bulletRound > 0) {
                        shootTimer = shootInterval;

                        //Shoot & Creat Bullet //////////
                        if (DataBase.d_Model != "PS Vita") {
                            if (DataBase.isConsole == false) {
                                isFire = true;
                                createBullet();
                            }
                            else {
                                consoleShoot();
                            }
                        }
                        else if (DataBase.d_Model == "PS Vita") {
                            consoleShoot();
                        }
                        //Shoot & Creat Bullet //////////
                    }
                    else if(bulletRound <= 0){
                        creatMag();
                        switch (DataBase.currentWeaponID) {
                            case 0:
                                shootTimer = 1.0f;
                                bulletRound = DataBase.pistolRound;
                                break;
                            case 1:
                                shootTimer = 1.85f;
                                bulletRound = DataBase.sniperRifleRound;
                                break;
                            case 2:
                                shootTimer = 1.5f;
                                bulletRound = DataBase.shotgunRound;
                                break;
                            default:
                                break;
                        }
                        
                    }
                }

                if (DataBase.currentWeaponID == 1) {
                    if (currentAnim == loadRifleAnim) {
                        weaponAnim.Play("sniperRifle@Load");
                    }
                    else if (currentAnim == reloadRifleAnim) {
                        weaponAnim.Play("sniperRifle@Reload");
                    }
                    else {
                        weaponAnim.Play("sniperRifle@Stop");
                    }
                }
                else if (DataBase.currentWeaponID == 2) {
                    if (currentAnim == loadShotgunAnim) {
                        weaponAnim.Play("shotgun@Load");
                    }
                    else if (currentAnim == reloadShotgunAnim) {
                        weaponAnim.Play("shotgun@Reload");
                    }
                    else {
                        weaponAnim.Play("shotgun@Stop");
                    }
                }
            }
        }
        else {
            aim = false;
            isFire = false;
        }
        playerAnimation.SetAnimatorBool("ToAim", aim);
    }

    private void faceToDirection(bool _isFaceTo){
        Vector3 horizontalVelocity = playerController.velocity;
        float relativeAngle = 0;
        float relativeSpeed = 0;

        horizontalVelocity.y = 0; //Mute Vertical Movement Affection
        relativeSpeed = horizontalVelocity.magnitude / runSpeed * (health2Data / health);

        if (_isFaceTo == true) {
            if (horizontalVelocity.magnitude > 0.1f) {
                rotationPivot.transform.forward += horizontalVelocity.normalized * 45 * Time.deltaTime;
            }
            playerAnimation.SetLayerWeight(0, 1);
        }
        else {
            if (aim == true) {
                layerWeight = 1;
                playerAnimation.SetLayerWeight(DataBase.currentWeaponID + 1, layerWeight);
            }
            else if (aim == true && isFire == true) {
                layerWeight = 1;
                playerAnimation.SetLayerWeight(DataBase.currentWeaponID + 1, layerWeight);
            }
            else {
                playerAnimation.SetLayerWeight(DataBase.currentWeaponID + 1, 0);
            }
            //Calculate angle between facing direction and moving direction when Player is not going to face moving direction
            if (relativeSpeed == 0) {
                relativeAngle = 0;
            }
            else {
                relativeAngle = MathRelativeAngle();

            }
        }
        playerAnimation.SetAnimatorFloat("Direction", relativeAngle);
        playerAnimation.SetAnimatorFloat("Speed", relativeSpeed);
    }

    private float MathAngle(float _axisX, float _axisY) {
        float axisAngle = Mathf.Atan2(_axisX, _axisY) * Mathf.Rad2Deg;
        return axisAngle;
    }

    private float MathRelativeAngle() {
        float moveStickAngle = MathAngle(moveStickX, moveStickY);
        float rotateStickAngle = MathAngle(rotateStickX, rotateStickY);
        return (Mathf.DeltaAngle(moveStickAngle, rotateStickAngle));
    }

    private void createBullet() {
        RaycastHit hitInfo;
        Transform _gunFireClone;
        Transform[] bulletArray = new Transform[bulletSwarm];
        int shotgunAngleRange = 55;
        _gunFireClone = Instantiate(gunFire[DataBase.currentWeaponID], bulletSpawn.transform.position, bulletSpawn.rotation) as Transform;
        _gunFireClone.parent = bulletSpawn;
        if (DataBase.currentWeaponID == 0) {
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            StartCoroutine(creatShell(0, DataBase.currentWeaponID));
            gunFire[DataBase.currentWeaponID].localScale = new Vector3(Random.Range(0.2f, 1.3f), Random.Range(-1.0f, 1.0f), Random.Range(1.3f, 2.5f));
        }
        else if (DataBase.currentWeaponID == 1) {
            Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
            StartCoroutine(creatShell(0.5f, DataBase.currentWeaponID));
            gunFire[DataBase.currentWeaponID].localScale = new Vector3(Random.Range(2.8f, 3.5f), Random.Range(-3.5f, 3.5f), Random.Range(3.0f, 3.8f));
        }
        else if(DataBase.currentWeaponID == 2){
            gunFire[DataBase.currentWeaponID].localScale = new Vector3(Random.Range(1.5f, 2.2f), Random.Range(-2.8f, 2.8f), Random.Range(1.2f, 2.2f));
            StartCoroutine(creatShell(0.5f, DataBase.currentWeaponID));
            for (int i = 0; i < bulletArray.Length; i++) {
                bulletArray[i] = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation) as Transform;
                bulletArray[i].localEulerAngles = new Vector3(0, bulletSpawn.rotation.eulerAngles.y - shotgunAngleRange * 0.5f + shotgunAngleRange / bulletArray.Length * i, 0);
            }
        }
        
        gunFire[DataBase.currentWeaponID].localPosition = new Vector3(Random.Range(-1, 1), 0, -1);

        if (DataBase.currentWeaponID == 0) {
            if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hitInfo, 3.5f)) {
                if (hitInfo.collider.tag == "envCollider") {
                    Transform sparkClone;
                    sparkClone = Instantiate(spark, hitInfo.point, Quaternion.identity) as Transform;
                    sparkClone.forward = hitInfo.normal;
                }
                else if (hitInfo.collider.tag == "trainingTarget") {
                    Transform sparkClone;
                    sparkClone = Instantiate(dustParticle, hitInfo.point, Quaternion.identity) as Transform;
                    sparkClone.forward = hitInfo.normal;
                }
                else if (hitInfo.collider.tag == "Zombie") {
                    Transform zombieBloodClone;
                    zombieBloodClone = Instantiate(zombieBlood, hitInfo.point, Quaternion.identity) as Transform;
                    zombieBloodClone.forward = hitInfo.normal;
                }
            }
        }
        else if (DataBase.currentWeaponID == 1) {
            if (Physics.Raycast(bulletSpawn.transform.position, bulletSpawn.transform.forward, out hitInfo, 12.6f)) {
                if (hitInfo.collider.tag == "envCollider") {
                    Transform sparkClone;
                    sparkClone = Instantiate(spark, hitInfo.point, Quaternion.identity) as Transform;
                    sparkClone.forward = hitInfo.normal;
                }
                else if (hitInfo.collider.tag == "trainingTarget") {
                    Transform sparkClone;
                    sparkClone = Instantiate(dustParticle, hitInfo.point, Quaternion.identity) as Transform;
                    sparkClone.forward = hitInfo.normal;
                }
                else if (hitInfo.collider.tag == "Zombie") {
                    Transform zombieBloodClone;
                    zombieBloodClone = Instantiate(zombieBlood, hitInfo.point, Quaternion.identity) as Transform;
                    zombieBloodClone.forward = hitInfo.normal;
                }
            }
        }
        else {
            for (int i = 0; i < bulletArray.Length; i++) {
                if (Physics.Raycast(bulletSpawn.transform.position, bulletArray[i].transform.forward, out hitInfo, 4.9f)) {
                    if (hitInfo.collider.tag == "envCollider") {
                        Transform sparkClone;
                        sparkClone = Instantiate(spark, hitInfo.point, Quaternion.identity) as Transform;
                        sparkClone.forward = hitInfo.normal;
                    }
                    else if (hitInfo.collider.tag == "trainingTarget") {
                        Transform sparkClone;
                        sparkClone = Instantiate(dustParticle, hitInfo.point, Quaternion.identity) as Transform;
                        sparkClone.forward = hitInfo.normal;
                    }
                    else if (hitInfo.collider.tag == "Zombie") {
                        Transform zombieBloodClone;
                        zombieBloodClone = Instantiate(zombieBlood, hitInfo.point, Quaternion.identity) as Transform;
                        zombieBloodClone.forward = hitInfo.normal;
                    }
                }
            }    
        }
        
        playerAudio.clip = weaponSound[DataBase.currentWeaponID];
        playerAudio.Play();
        playerAnimation.SetAnimatorBool("IsShoot", true);
        bulletRound--;
    }

    private void creatMag() {
        playerAnimation.SetAnimatorBool("IsReload", true);
        playerAudio.clip = changeMag[DataBase.currentWeaponID];
        playerAudio.Play();
        switch (DataBase.currentWeaponID) {
            case 0:
                magSpawn.localPosition = new Vector3(0, -0.2f, -0.13f);
                Instantiate(weaponMag[DataBase.currentWeaponID], magSpawn.position, magSpawn.rotation);
                break;
            case 1:
                magSpawn.localPosition = new Vector3(0, -0.2f, 0.1f);
                Instantiate(weaponMag[DataBase.currentWeaponID], magSpawn.position, magSpawn.rotation);
                break;
            default:
                break;
        }
    }

    private void consoleShoot() {
        if (Input.GetButton("RightShoulder")) {
            isFire = true;
            createBullet();
        }
        else {
            shootTimer = 0;
            isFire = false;
        }
    }

    private void setWeapon(int _weaponID) {
        if (currentWeapon == null) {
            currentWeapon = Instantiate(weaponList[_weaponID], weaponSpawn[_weaponID].position, weaponSpawn[_weaponID].rotation) as Transform;
        }
        else {
            Destroy(currentWeapon.gameObject);
            currentWeapon = Instantiate(weaponList[_weaponID], weaponSpawn[_weaponID].position, weaponSpawn[_weaponID].rotation) as Transform;
        }
        currentWeapon.parent = weaponSpawn[_weaponID].transform;
        playerAnimation.SetAnimatorFloat("WeaponID", _weaponID);
        weaponAnim = currentWeapon.GetComponent<Animation>();
        if (weaponAnim != null) {
            weaponAnim.Stop();
        }
        switch (_weaponID) {
            case 0:
                shootInterval = 0.25f;
                    weaponDamage = pistolDamage;
                    damage2Bullet = weaponDamage;
                    bulletRound = DataBase.pistolRound;
                    bulletSpawn.localPosition = new Vector3(0.145f, 1.53f, 0.9f);
                    bulletShellSpawn.localPosition = Vector3.zero;
                    if (DataBase.weaponSet == false) {
                        UIManager.tweenWeaponLevel(pistolDamage / 80, 0.45f, (1 - shootInterval / 1.5f));
                        UIManager.checkMarkAnim.SetBool("hide", true);
                    }
                break;
            case 1:
                shootInterval = 1.26f;
                    weaponDamage = sniperRifleDamage;
                    damage2Bullet = weaponDamage;
                    bulletRound = DataBase.sniperRifleRound;
                    bulletSpawn.localPosition = new Vector3(0.18f, 1.58f, 1.5f);
                    bulletShellSpawn.localPosition = new Vector3(0.1f, 0, 0.1f);
                    if (DataBase.weaponSet == false) {
                        UIManager.tweenWeaponLevel(sniperRifleDamage / 80, 0.89f, (1 - shootInterval / 1.5f));
                        UIManager.checkMarkAnim.SetBool("hide", false);
                    }
                break;
            case 2:
                shootInterval = 1.0f;
                    weaponDamage = shotgunDamage / bulletSwarm;
                    damage2Bullet = weaponDamage;
                    bulletRound = DataBase.shotgunRound;
                    bulletSpawn.localPosition = new Vector3(0.18f, 1.38f, 1.25f);
                    bulletShellSpawn.localPosition = new Vector3(0.1f, 0, 0.1f);
                    if (DataBase.weaponSet == false) {
                        UIManager.tweenWeaponLevel(shotgunDamage / 80, 0.2f, (1 - shootInterval / 1.5f));
                        UIManager.checkMarkAnim.SetBool("hide", true);
                    }
                break;
            default:
                break;
        }
        shootTimer = 0.2f;
        DataBase.currentWeaponID = _weaponID;
        UIManager.bulletCount.text = bulletRound.ToString();
        UIManager.magCapacity.text = "/" + bulletRound.ToString();
    }

    public void changeWeapon() {
        switch (UIButton.current.ToString()) {
            case "pistolItemBtn (UIButton)":
                weaponID = 0;
                weaponIDCheck();
                break;
            case "sniperRifleItemBtn (UIButton)":
                weaponID = 1;
                weaponIDCheck();
                break;
            case "shotgunItemBtn (UIButton)":
                weaponID = 2;
                weaponIDCheck();
                break;
            default:
                break;
        }
    }

    private void weaponIDCheck() {
        if (DataBase.currentWeaponID != weaponID) {
            setWeapon(weaponID);
            gameUI.setWeaponInventoryIcon();
            UIManager.gunIcon.spriteName = UIManager.gunIconSpriteNameArray[DataBase.currentWeaponID];
        }
    }

    private IEnumerator creatShell(float waitSeconds, int _weaponID) {
        Transform bulletShellClone;
        Vector3 dircUp = rotationPivot.TransformDirection(Vector3.up);
        Vector3 dircRight = rotationPivot.TransformDirection(Vector3.right);
        Vector3 dircForward = rotationPivot.TransformDirection(Vector3.forward);
        yield return new WaitForSeconds(waitSeconds);
        bulletShellClone = Instantiate(bulletShell[_weaponID], bulletShellSpawn.position, bulletShellSpawn.rotation) as Transform;
        bulletShellClone.rigidbody.AddForce(dircRight * Random.Range(0.35f, 0.85f), ForceMode.Impulse);
        bulletShellClone.rigidbody.AddForce(dircUp * Random.Range(1.5f, 2.35f), ForceMode.Impulse);
        bulletShellClone.rigidbody.AddForce(dircForward * Random.Range(-0.1f, 0.1f), ForceMode.Impulse);
    }

    private IEnumerator setDeadPanel() {
        yield return new WaitForSeconds(1.25f);
        UIManager.deadPanelAnim.SetBool("hide", false);
    }
}
