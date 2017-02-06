using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {
    public float zombieSpeed = 3.5f;
    public int damage = 15;
    public float health = 50;
    public bool isNav = false;
    public bool isIdleInWalking = true;
    public bool isHang = false;
    public int groupID;
    public Vector3 zombieDestination;
    public Transform playerBlood;
    public Transform blockerDebris;
    public Transform[] zombiebloodSplash;

    public Transform zombieParts;
    public Transform zombieExplosionBlood;
    public AudioClip zombieMoan;

    private Transform zombie;
    private NavMeshAgent zombieNav;
    private CharacterAnimation zombieAnimation;
    private float attackTimeInterval = 0.5f;
    private bool isAttack = false;
    private bool isDead = false;
    private float resetDestinationTimer;
    private Collider capsuleCollider;
    private CharacterController zombieCollider;

    private AudioSource zombieSound;
    private int zombiePartsCount = 0;

    void Awake() {
        zombie = this.transform;
        zombieNav = this.GetComponent<NavMeshAgent>();
        zombieAnimation = this.GetComponent<CharacterAnimation>();
        capsuleCollider = this.GetComponent<CapsuleCollider>();
        zombieCollider = this.GetComponent<CharacterController>();
        zombieSound = this.GetComponent<AudioSource>();

        DataBase.hangZombieisDead = false;
    }

	// Use this for initialization
	void Start () {
        zombieNav.speed = zombieSpeed;
        if (isHang == false) {
            zombie.transform.rotation = Quaternion.Euler(0, Random.rotation.eulerAngles.y, 0);
        }
        zombieAnimation.SetAnimatorBool("IsRepeatAnyState", true);

        if (isIdleInWalking == true) {
            zombieAnimation.SetAnimatorBool("IsIdleWalk", true);
            iTween.MoveTo(zombie.gameObject, iTween.Hash("name", "idelWalking", "path", iTweenPath.GetPath("testPath"), "speed", 0.5f, "looptype", "loop", "orienttopath", true, "movetopath", false, "easetype", "linear"));
        }

        if (Application.loadedLevelName == "Level1_StreetCross") {
            if (GameObjectAction.zombieBlockerArray[groupID] != null) {
                switch (groupID) {
                    case 0:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, (Vector3)GameObjectAction.zombieG0Targets[Random.Range(0, GameObjectAction.zombieG0Targets.Count)]));
                        break;
                    case 1:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, (Vector3)GameObjectAction.zombieG1Targets[Random.Range(0, GameObjectAction.zombieG1Targets.Count)]));
                        break;
                    case 2:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, (Vector3)GameObjectAction.zombieG2Targets[Random.Range(0, GameObjectAction.zombieG2Targets.Count)]));
                        break;
                    case 3:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, (Vector3)GameObjectAction.zombieG3Targets[Random.Range(0, GameObjectAction.zombieG3Targets.Count)]));
                        break;
                    default:
                        break;
                }
            }
            else {
                switch (groupID) {
                    case 0:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, PlayerController.playerPosition));
                        break;
                    case 1:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, PlayerController.playerPosition));
                        break;
                    case 2:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, PlayerController.playerPosition));
                        break;
                    case 3:
                        StartCoroutine(delaySetDestination(Random.value * 1.25f, PlayerController.playerPosition));
                        break;
                    default:
                        break;
                }
            }
        }

        zombieSpeed += Random.Range(0.0f, 3.85f);
        resetDestinationTimer = Random.Range(0.3f, 1.0f);
        if(Application.loadedLevel == 1){
            zombieSound.clip = zombieMoan;
            zombieSound.Play();
        }
        else {
            zombieSound.clip = null;
            zombieSound.Stop();
        }
	}
	
	// Update is called once per frame
    void Update() {
        if (isHang == true) {
            zombieAnimation.SetAnimatorBool("IsHang", isHang);
            zombieAnimation.SetLayerWeight(1, 1);
        }
        else {
            if (isNav == true && isDead == false) {
                if (GameObjectAction.zombieBlockerArray[groupID] == null) {
                    resetDestinationTimer -= Time.deltaTime;
                    if (resetDestinationTimer <= 0) {
                        resetDestinationTimer = -1;
                        zombieDestination = PlayerController.playerPosition;
                    }
                }
                zombieNav.SetDestination(zombieDestination);
                zombieAnimation.SetAnimatorBool("IsNav", isNav);
                zombieAnimation.SetAnimatorFloat("Speed", zombieNav.velocity.magnitude);
                if (Vector3.Distance(zombieNav.transform.position, zombieDestination) < zombieNav.stoppingDistance) {
                    isAttack = true;
                }
                else {
                    isAttack = false;
                }
            }
            zombieAnimation.SetAnimatorBool("IsAttack", isAttack);
        }

        if (isAttack == true) {
            attackTimeInterval -= Time.deltaTime;
            if (attackTimeInterval <= 0) {
                attackTimeInterval = 0.5f;
                RaycastHit hitInfo;
                if (Physics.Raycast(new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), this.transform.forward, out hitInfo, 3)) {
                    switch(hitInfo.collider.name){
                        case "zombieBlocker0_collider":
                            attackBlocker(GameObjectAction.zombieBlockerArray[0]);
                            generateParticle(blockerDebris, hitInfo, true);
                            break;
                        case "zombieBlocker1_collider":
                            attackBlocker(GameObjectAction.zombieBlockerArray[1]);
                            generateParticle(blockerDebris, hitInfo, true);
                            break;
                        case "zombieBlocker2_collider":
                            attackBlocker(GameObjectAction.zombieBlockerArray[2]);
                            generateParticle(blockerDebris, hitInfo, true);
                            break;
                        case "zombieBlocker3_collider":
                            attackBlocker(GameObjectAction.zombieBlockerArray[3]);
                            generateParticle(blockerDebris, hitInfo, true);
                            break;
                        case "Player":
                            if (GameObjectAction.zombieBlockerArray[groupID] == null) {
                                generateParticle(playerBlood, hitInfo);
                                if (PlayerController.armor2Data > 0) {
                                    PlayerController.armor2Data -= (damage + 8);
                                }
                                else {
                                    PlayerController.health2Data -= (damage + 8);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider _collider) {
        if (_collider.gameObject.transform.tag == "Bullet") {
            health -= SelfDestroyObjectController.damage;
            if(DataBase.currentWeaponID != 1){
                Destroy(_collider.gameObject);
            }
            else {
                SelfDestroyObjectController.damage -= PlayerController.damage2Bullet * 0.45f;
                if (SelfDestroyObjectController.damage <= 0) {
                    Destroy(_collider.gameObject);
                }
            }
        }

        if (health <= 0) {
            iTween.Stop(this.gameObject);
            isDead = true;
            isNav = false;
            capsuleCollider.enabled = false;
            zombieCollider.enabled = false;
            if (zombiePartsCount == 0) {
                zombiePartsCount++;
                Instantiate(zombieParts, this.transform.position, this.transform.rotation);
                Instantiate(zombieExplosionBlood, new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z), Quaternion.identity);
                destroyThis();
            }
        }
    }

    private void destroyThis() {
        if (isHang == true) {
            DataBase.hangZombieisDead = true;
        }
        GameObjectAction.zombieCount--;
        Instantiate(zombiebloodSplash[Random.Range(0, 4)], new Vector3(this.transform.position.x, zombiebloodSplash[Random.Range(0, 4)].position.y + 0.015f, this.transform.position.z), Quaternion.Euler(new Vector3(0, Random.value * 360, 0)));
        Destroy(this.gameObject);
    }

    private void attackBlocker(ZombieBlockerController _zombieBlocker) {
        _zombieBlocker.health -= damage;
    }

    private IEnumerator delaySetDestination(float _second, Vector3 _destination) {
        yield return new WaitForSeconds(_second);
        zombieDestination = _destination;
    }

    private void generateParticle(Transform _transform, RaycastHit _hitInfo, bool _isOut = false) {
        Transform _debris = Instantiate(_transform, _hitInfo.point, Quaternion.identity) as Transform;
        if (_isOut == true) {
            _debris.forward = _hitInfo.normal;
        }
    }
}
