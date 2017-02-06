using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class PlayerUI : MonoBehaviour {
    public Transform[] weaponList;
    public Transform[] weaponMag;
    public Transform[] weaponSpawn;
    public Transform magazineSpawn;
    public AudioClip[] changeMag;
    public AudioSource playerAudio;

    private Transform playerUI;
    private float rotationSpeed = 1;
    private int animationStateID = 0;
    private int currentUIWeaponID = 0;
    private float animationClipInterval = 2.0f;
    private CharacterAnimation animationController;
    private Transform currentWeapon = null;
    private Animation weaponAnim;

    void Awake() {
        playerUI = this.transform;
        animationController = GetComponent<CharacterAnimation>();
        playerAudio = this.GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start() {
        setWeapon(DataBase.currentWeaponID);
        animationController.SetLayerWeight(DataBase.currentWeaponID, 1);
    }

    // Update is called once per frame
    void Update() {
        
    }

    void FixedUpdate() {
        animationClipInterval -= Time.deltaTime;
        if (animationClipInterval <= 0) {
            animationClipInterval = 2.0f;
            animationStateID++;
            if (animationStateID > 3) {
                animationStateID = 0;
            }

            if (animationStateID == 2) {
                creatMag();
                switch (DataBase.currentWeaponID) {
                    case 1:
                        weaponAnim.Play("sniperRifle@Reload");
                        break;
                    case 2:
                        weaponAnim.Play("shotgun@Reload");
                        break;
                }
            }
            animationController.SetAnimatorInt("StateID", animationStateID);

        }
        animationController.SetLayerWeight(DataBase.currentWeaponID, 1);
    }

    void OnDrag(Vector2 _delta) {
        UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
        playerUI.localRotation = Quaternion.Euler(0f, -0.5f * _delta.x * rotationSpeed, 0f) * playerUI.localRotation;
    }

    private void setWeapon(int _weaponID) {
        if (currentWeapon == null) {
            currentWeapon = Instantiate(weaponList[_weaponID], weaponSpawn[_weaponID].position, weaponSpawn[_weaponID].rotation) as Transform;
            currentWeapon.parent = weaponSpawn[_weaponID].transform;
            weaponAnim = currentWeapon.GetComponent<Animation>();
        }
        else {
            Destroy(currentWeapon.gameObject);
            StartCoroutine(setWeaponDelay());
        }
        if (weaponAnim != null) {
            weaponAnim.Stop();
        }
        currentUIWeaponID = DataBase.currentWeaponID;
    }

    public void changeWeapon() {
        switch (UIButton.current.ToString()) {
            case "pistolItemBtn (UIButton)":
                DataBase.currentWeaponID = 0;
                weaponIDCheck();
                break;
            case "sniperRifleItemBtn (UIButton)":
                DataBase.currentWeaponID = 1;
                weaponIDCheck();
                break;
            case "shotgunItemBtn (UIButton)":
                DataBase.currentWeaponID = 2;
                weaponIDCheck();
                break;
            default:
                break;
        }
    }

    private void weaponIDCheck() {
        if (currentUIWeaponID != DataBase.currentWeaponID) {
            setWeapon(DataBase.currentWeaponID);
        }
    }

    private void creatMag() {
        playerAudio.clip = changeMag[DataBase.currentWeaponID];
        playerAudio.Play();
        
        switch (DataBase.currentWeaponID) {
            case 0:
                magazineSpawn.localPosition = new Vector3(0, -0.2f, -0.13f);
                Instantiate(weaponMag[DataBase.currentWeaponID], magazineSpawn.position, magazineSpawn.rotation);
                break;
            case 1:
                magazineSpawn.localPosition = new Vector3(0, -0.2f, 0.1f);
                Instantiate(weaponMag[DataBase.currentWeaponID], magazineSpawn.position, magazineSpawn.rotation);
                break;
            default:
                break;
        }
    }

    private IEnumerator setWeaponDelay() {
        yield return new WaitForSeconds(0);
        currentWeapon = Instantiate(weaponList[DataBase.currentWeaponID], weaponSpawn[DataBase.currentWeaponID].position, weaponSpawn[DataBase.currentWeaponID].rotation) as Transform;
        currentWeapon.parent = weaponSpawn[DataBase.currentWeaponID].transform;
        weaponAnim = currentWeapon.GetComponent<Animation>();
    }

}