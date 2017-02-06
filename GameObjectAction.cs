using UnityEngine;
using System.Collections;

public class GameObjectAction : MonoBehaviour {
    public Transform fakeZombie;
    public Transform areaLock;
    public ZombieController zombie;
    public Transform[] zombieTargetsArray;
    public Transform gate;

    private GameObject entranceDoor;
    private GameObject exitDoor;
    private Transform AreaRestrictSpawnPoint;
    private Transform areaLockClone;
    private Transform areaLockShape;
    private Transform player;
    private int zombieLimit = 20;
    private bool isRegenerateZombie;
    private UIManager gameUI;

    public static ZombieBlockerController[] zombieBlockerArray;

    private float zombieSpawnTimer = 0.5f;

    private Transform[] targetSpawnPointArray;
    private Transform[] moveTargetSpawnPointArray;
    private Transform[] staticFakeTargetArray;
    private Transform[] moveingFakeTargetArray;

    private Transform[] zombieGroup0SpawnPointArray;
    private Transform[] zombieGroup1SpawnPointArray;
    private Transform[] zombieGroup2SpawnPointArray;
    private Transform[] zombieGroup3SpawnPointArray;

    public static ArrayList zombieG0Targets = new ArrayList();
    public static ArrayList zombieG1Targets = new ArrayList();
    public static ArrayList zombieG2Targets = new ArrayList();
    public static ArrayList zombieG3Targets = new ArrayList();

    public static bool entranceDoorisOpen = false;
    public static int targetCount;
    public static int zombieCount;
    public static bool isSceneStarted = false;

    void Awake() {
        #if UNITY_EDITOR
        QualitySettings.vSyncCount = 1;
        #endif
        #if UNITY_IOS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        #endif
        #if UNITY_ANDROID
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        #endif

        entranceDoor = GameObject.Find("innerDoorEntrance");
        exitDoor = GameObject.Find("innerDoorExit");
        
        player = GameObject.FindGameObjectWithTag("Player").transform;

        targetSpawnPointArray = new Transform[5];
        staticFakeTargetArray = new Transform[5];
        moveTargetSpawnPointArray = new Transform[5];
        moveingFakeTargetArray = new Transform[5];

        zombieGroup0SpawnPointArray = new Transform[5];
        zombieGroup1SpawnPointArray = new Transform[5];
        zombieGroup2SpawnPointArray = new Transform[5];
        zombieGroup3SpawnPointArray = new Transform[5];

        zombieBlockerArray = new ZombieBlockerController[4];

        if (Application.loadedLevel == 1) {
            gameUI = GameObject.Find("GameManager").GetComponent<UIManager>();
            targetCount = 0;
            AreaRestrictSpawnPoint = GameObject.Find("AreaRestrictSpawn").transform;
            for (int i = 0; i < targetSpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("targetSpawnPoint" + i).transform;
                targetSpawnPointArray[i] = _transform;
            }

            for (int i = 0; i < moveTargetSpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("moveTargetSpawnPoint" + i).transform;
                moveTargetSpawnPointArray[i] = _transform;
            }

            DataBase.killedTarget = 0;
            DataBase.totalTarget = 0;

            zombie.gameObject.SetActive(false);
        }
        else if (Application.loadedLevel == 2) {
            zombieCount = 0;
            isSceneStarted = false;
            for (int i = 0; i < zombieBlockerArray.Length; i++) {
                ZombieBlockerController _zombieBlocker = GameObject.Find("zombieBlocker" + i).GetComponent<ZombieBlockerController>();
                zombieBlockerArray[i] = _zombieBlocker;
            }

            for (int i = 0; i < zombieGroup0SpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("zombieG0_spawnPoint" + i).transform;
                zombieGroup0SpawnPointArray[i] = _transform;
            }

            for (int i = 0; i < zombieGroup1SpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("zombieG1_spawnPoint" + i).transform;
                zombieGroup1SpawnPointArray[i] = _transform;
            }

            for (int i = 0; i < zombieGroup2SpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("zombieG2_spawnPoint" + i).transform;
                zombieGroup2SpawnPointArray[i] = _transform;
            }

            for (int i = 0; i < zombieGroup3SpawnPointArray.Length; i++) {
                Transform _transform = GameObject.Find("zombieG3_spawnPoint" + i).transform;
                zombieGroup3SpawnPointArray[i] = _transform;
            }

            for (int i = 0; i < zombieTargetsArray.Length; i++) {
                switch (i) {
                    case 0:
                        foreach (Transform child in zombieTargetsArray[i]) {
                            zombieG0Targets.Add(child.position);
                        }
                        break;
                    case 1:
                        foreach (Transform child in zombieTargetsArray[i]) {
                            zombieG1Targets.Add(child.position);
                        }
                        break;
                    case 2:
                        foreach (Transform child in zombieTargetsArray[i]) {
                            zombieG2Targets.Add(child.position);
                        }
                        break;
                    case 3:
                        foreach (Transform child in zombieTargetsArray[i]) {
                            zombieG3Targets.Add(child.position);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Start() {
        
    }

    void Update() {
        if (DataBase.isConsole == true || DataBase.d_Model == "PS Vita") {
            if (Input.GetButton("Fire1") && UIManager.isPlayerEntered == true && entranceDoorisOpen == false) {
                openEntranceDoor();
                SendMessage("disableActionButton");
            }
        }

        if (targetCount <= 0 && UIManager.isPlayerinArea2 == false) {
            if (areaLockClone != null && areaLockShape.localScale.x > 0) {
                iTween.ScaleTo(areaLockShape.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.8f, "delay", 0.5f, "oncomplete", "removeAreaRestrict", "oncompletetarget", this.gameObject));
            }
            else if(areaLockClone != null && areaLockShape.localScale.x <= 0){
                removeAreaRestrict();
            }
        }
        else if (UIManager.isPlayerinArea2 == true) {
            if (UIManager.isPlayerinTrainingField == false) {
                removeAreaRestrict();
            }
        }

        if (areaLockClone != null) {
            if (Vector3.Distance(areaLockClone.position, player.position) < 4 && targetCount > 0 && UIManager.isPlayerinArea2 == false) {
                iTween.ScaleTo(areaLockShape.gameObject, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.5f));
            }
            else if (Vector3.Distance(areaLockClone.position, player.position) < 4 && UIManager.isPlayerinArea2 == true) {
                iTween.ScaleTo(areaLockShape.gameObject, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 0.5f));
            }
            else if (Vector3.Distance(areaLockClone.position, player.position) >= 4) {
                iTween.ScaleTo(areaLockShape.gameObject, iTween.Hash("scale", Vector3.zero, "time", 0.5f));
            }
        }

        if (Input.GetKeyUp(KeyCode.D)) {
            for (int i = 0; i < staticFakeTargetArray.Length; i++) {
                if (staticFakeTargetArray[i] != null) {
                    staticFakeTargetArray[i].GetComponent<fakeZombieController>().targetDead(0);
                }
            }

            for (int i = 0; i < moveingFakeTargetArray.Length; i++) {
                if (moveingFakeTargetArray[i] != null) {
                    moveingFakeTargetArray[i].GetComponent<fakeZombieController>().targetDead(0);
                }
            }

            targetCount = 0;
        }

        if (Application.loadedLevel == 2 && isSceneStarted == true) {
            zombieSpawnTimer -= Time.deltaTime;
            if (zombieSpawnTimer <= 0) {
                zombieSpawnTimer = 1;

                if (isRegenerateZombie == true) {
                    zombieCount++;
                    generateZombie(Random.Range(0, 4));
                }

                if (zombieCount <= 15) {
                    isRegenerateZombie = true;
                }
                else if (zombieCount > zombieLimit) {
                    isRegenerateZombie = false;
                }
            }
        }

        if (DataBase.killedTarget == 10 && UIManager.exitDoorIsOpen == false && UIManager.isPlayerinTrainingField == true) {
            if (UIManager.sentenceID == 5) {
                gameUI.changeDiologContent();
                UIManager.DioAnim.SetBool("hide", false);
                if (Application.loadedLevel == 1) {
                    zombie.gameObject.SetActive(true);
                }
            }
            openExitDoor();
            UIManager.exitDoorIsOpen = true;
        }

        if (DataBase.hangZombieisDead == true && UIManager.sentenceID == 7) {
            UIManager.DioAnim.SetBool("hide", false);
            gameUI.changeDiologContent();
        }
    }

    public void openEntranceDoor() {
        entranceDoor.animation.Play("EntranceDoor@Open");
        entranceDoorisOpen = true;
    }

    public void openExitDoor() {
        //exitDoor.animation.Play("ExitDoor@Open");
        exitDoor.animation.CrossFade("ExitDoor@Open", 0.2f);
    }

    public void closeEntranceDoor() {
        if(entranceDoorisOpen == true){
            entranceDoor.animation.CrossFade("EntranceDoor@Close", 0.2f);
            entranceDoorisOpen = false;
        }
        else {
            return;
        } 
    }

    public void closeExitDoor() {
        if (UIManager.exitDoorIsOpen == true && UIManager.isPlayerinTrainingField == false) {
            exitDoor.animation.CrossFade("ExitDoor@Close", 0.2f);
            UIManager.exitDoorIsOpen = false;
        }
    }

    public void spawnFakeTarget() {
        
        for (int i = 0; i < targetSpawnPointArray.Length; i++) {
            targetCount++;
            Transform _fzClone = Instantiate(fakeZombie, targetSpawnPointArray[i].position, Quaternion.Euler(new Vector3(0, Random.value * 360, 0))) as Transform;
            staticFakeTargetArray[i] = _fzClone;
            iTween.MoveFrom(_fzClone.gameObject, iTween.Hash("position", new Vector3(targetSpawnPointArray[i].position.x, targetSpawnPointArray[i].position.y - 2, targetSpawnPointArray[i].position.z), "time", 1, "delay", Random.Range(0.5f, 1.0f)));
        }
    }

    public void spawnMoveingFakeTarget() {
        for (int i = 0; i < moveTargetSpawnPointArray.Length; i++) {
            targetCount++;
            Transform _fzMClone = Instantiate(fakeZombie, moveTargetSpawnPointArray[i].position, Quaternion.Euler(new Vector3(0, Random.value * 360, 0))) as Transform;
            moveingFakeTargetArray[i] = _fzMClone;
            iTween.MoveFrom(moveingFakeTargetArray[i].gameObject, iTween.Hash("position", new Vector3(moveTargetSpawnPointArray[i].position.x, moveTargetSpawnPointArray[i].position.y - 2, moveTargetSpawnPointArray[i].position.z), "time", 1, "delay", Random.Range(0.5f, 1.0f), "oncomplete", "moveFakeTarget", "oncompleteparams", i, "oncompletetarget", this.gameObject));
        }
    }

    private void moveFakeTarget(int _i) {
        iTween.MoveTo(moveingFakeTargetArray[_i].gameObject, iTween.Hash("path", iTweenPath.GetPath("targetPath" + _i), "time", 1.5f + Random.Range(1.0f, 2.5f), "looptype", iTween.LoopType.pingPong, "easetype", iTween.EaseType.easeInOutQuad));
    }

    public void restrictArea() {
        areaLockClone = Instantiate(areaLock, AreaRestrictSpawnPoint.position, AreaRestrictSpawnPoint.rotation) as Transform;
        areaLockShape = GameObject.Find("lock_3DIcon_shape").transform;
        areaLockShape.localScale = Vector3.zero;
    }

    private void removeAreaRestrict() {
        if (areaLockClone != null) {
            Destroy(areaLockClone.gameObject);
        } 
    }

    public void openGate() {
        gate.animation.Play("Gate@Open");
    }

    private void generateZombie(int _groupID) {
        ZombieController _zombieClone;
        switch (_groupID) {
            case 0:
                _zombieClone = Instantiate(zombie, zombieGroup0SpawnPointArray[Random.Range(0, 5)].position, Quaternion.identity) as ZombieController;
                _zombieClone.groupID = _groupID;
                break;
            case 1:
                _zombieClone = Instantiate(zombie, zombieGroup1SpawnPointArray[Random.Range(0, 5)].position, Quaternion.identity) as ZombieController;
                _zombieClone.groupID = _groupID;
                break;
            case 2:
                _zombieClone = Instantiate(zombie, zombieGroup2SpawnPointArray[Random.Range(0, 5)].position, Quaternion.identity) as ZombieController;
                _zombieClone.groupID = _groupID;
                break;
            case 3:
                _zombieClone = Instantiate(zombie, zombieGroup3SpawnPointArray[Random.Range(0, 5)].position, Quaternion.identity) as ZombieController;
                _zombieClone.groupID = _groupID;
                break;
            default:
                break;
        }
    }
}
