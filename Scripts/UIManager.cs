using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    public UIButton[] weaponIconArray;
    public static bool isPlayerEntered;
    public static bool isPlayerinTrainingField;
    public static bool isPlayerMove;
    public static bool isPlayerinArea2;
    public static bool isPlayerTrained;
    public static UILabel bulletCount;
    public static UILabel magCapacity;
    public static UILabel killCount;
    public static UILabel totalTarget;
    public static UISprite playerIcon;
    public static UISlider healthLevel;
    public static UISlider armorLevel;
    public static UISlider zombieBlocker0Health;
    public static UISlider zombieBlocker1Health;
    public static UISlider zombieBlocker2Health;
    public static UISlider zombieBlocker3Health;
    public static UIButton dioFrameBtn;
    public static UIButton closeFrameBtn;
    public static UISprite actionButton;
    public static TweenPosition actionButtonAnimation;
    public static Animator DioAnim;
    public static int sentenceID;
    public static bool exitDoorIsOpen = false;
    public static bool interact2Engineer = false;
    public static bool interact2Nurse = false;
    public static UISprite gunIcon;
    public static Animator checkMarkAnim;
    public static string[] gunIconSpriteNameArray;
    public static bool isPause = false;
    public static Animator deadPanelAnim;

    private PlayerUI playerUI;
    private Camera UICamera3D;
    private EasyJoystick rotateJoystick;
    private EasyJoystick moveJoystick;
    private UISprite zombieIcon;
    private TweenPosition zombieIconPositionAnimation;
    private TweenAlpha zombieIconAlphaAnimation;
    private static TweenScale damageLevelTween;
    private static TweenScale accuracyLevelTween;
    private static TweenScale fireRateLevelTween;
    private UILabel weaponIntro;
    private string[] gameGuide;
    private string[] weaponIntroArray;
    private UILabel diolog;
    private Animator weaponInventoryAnim;
    private UIWidget weaponAnchor;
    private static bool initializedTweenPostion;
    private bool inventoryOpened;
    private bool tutorialisDone = false;
    private bool isReadytoOutside = false;
    private UIPanel weaponInventory;
    private string[] weaponNormalIconArray;
    private string[] weaponSelectIconArray;
    private TweenAlpha cameraFade;
    private TweenAlpha deadCamFade;
    private Animator pausedFrameAnim;
    private Collider camFadeCollider;
    private Collider deadCamFadeCollider;

    void Awake() {
        isPlayerEntered = false;
        isPlayerinTrainingField = false;
        isPlayerMove = false;
        isPlayerinArea2 = false;
        isPlayerTrained = false;
        initializedTweenPostion = false;
        isPause = false;

        if (Application.loadedLevel == 1 || Application.loadedLevel == 2) {
            playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUI>();
            UICamera3D = GameObject.Find("UICamera_3D").GetComponent<Camera>();
            rotateJoystick = GameObject.Find("Right Stick").GetComponent<EasyJoystick>();
            moveJoystick = GameObject.Find("Left Stick").GetComponent<EasyJoystick>();
            bulletCount = GameObject.Find("bulletCount").GetComponent<UILabel>();
            magCapacity = GameObject.Find("magCapacity").GetComponent<UILabel>();
            killCount = GameObject.Find("killedCount").GetComponent<UILabel>();
            totalTarget = GameObject.Find("totalTarget").GetComponent<UILabel>();
            gunIcon = GameObject.Find("gunIcon").GetComponent<UISprite>();
            playerIcon = GameObject.Find("playerIcon").GetComponent<UISprite>();
            zombieIcon = GameObject.Find("zombieIcon").GetComponent<UISprite>();
            checkMarkAnim = GameObject.Find("checkMark").GetComponent<Animator>();
            zombieIconPositionAnimation = zombieIcon.GetComponent<TweenPosition>();
            damageLevelTween = GameObject.Find("damageLevel").GetComponent<TweenScale>();
            accuracyLevelTween = GameObject.Find("accuracyLevel").GetComponent<TweenScale>();
            fireRateLevelTween = GameObject.Find("fireRateLevel").GetComponent<TweenScale>();
            diolog = GameObject.Find("diolog").GetComponent<UILabel>();
            DioAnim = GameObject.Find("diologFrame").GetComponent<Animator>();
            dioFrameBtn = GameObject.Find("frameBtn").GetComponent<UIButton>();
            closeFrameBtn = GameObject.Find("closeFrameBtn").GetComponent<UIButton>();
            weaponInventoryAnim = GameObject.Find("allContainer").GetComponent<Animator>();
            weaponAnchor = GameObject.Find("weaponAnchor").GetComponent<UIWidget>();
            cameraFade = GameObject.Find("cameraFade").GetComponent<TweenAlpha>();
            pausedFrameAnim = GameObject.Find("pausedFrame").GetComponent<Animator>();
            inventoryOpened = false;
            weaponIntro = GameObject.Find("weaponIntro").GetComponent<UILabel>();
            camFadeCollider = cameraFade.GetComponent<Collider>();
            weaponIntroArray = new string[]{
                "Pistol\r\n" + "Default weapon in normal fire rate.",
                "Sniper Rifle\r\n" + "Kick zombie ass a 100 miles away.",
                "Shotgun\r\n" + "Feeling like wind, Blow em away!"
            };
            weaponIntro.text = weaponIntroArray[DataBase.currentWeaponID];
            closeFrameBtn.gameObject.SetActive(false);
            weaponInventory = GameObject.Find("weaponInventory").GetComponent<UIPanel>();
            weaponNormalIconArray = new string[]{
                "PistolIcon_inventory",
                "SniperRifleIcon_inventory",
                "shotgunIcon_inventory"
            };
            weaponSelectIconArray = new string[]{
                "PistolIcon_inventory_selected",
                "SniperRifleIcon_inventory_selected",
                "shotgunIcon_inventory_selected"
            };
            gunIconSpriteNameArray = new string[]{
                "pistolIcon", "sniperRifleIcon", "shotgunIcon"
            };
            pausedFrameAnim.updateMode = AnimatorUpdateMode.UnscaledTime;
            camFadeCollider.enabled = false;
        }

        if (Application.loadedLevel == 1) {
            DataBase.killedTarget = 0;
            DataBase.weaponSet = false;
            DataBase.currentWeaponID = 0;
            setWeaponInventoryIcon();
            sentenceID = 0;
            actionButton = GameObject.Find("actionButton").GetComponent<UISprite>();
            actionButtonAnimation = actionButton.GetComponent<TweenPosition>();
            gameGuide = new string[]{
                "Hey, stranger! How lucky you are Vanessa brought you to our camp before you were torn up by monsters out here.",
                "The World has turned. I hope you're all fine. Now touch bottom-left of the screen to move around with the left-stick.",
                "Ah, huh. You look great so far. You should enter the training field to show us what you can do. Follow the Arrow.",
                "When you approached, press A at right of the screen to open the gate.",
                "Smart ass, huh! But I bit of doubt whether you’re smart enough to use firearm.",
                "Show me what you can with the right-stick at bottom-right of the screen. And destroy all the targets.",
                "Hum. Not bad, not bad. Oh, listen! It sounds like... Damn it. Check out what's happened at the gate of the camp.",
                "This is zombie, revived dead man. They are monsters n kill people. Shoot n kill it now.",
                "Good kill! When you see them again, dont be hesitated n Just put em down.",
                "Come over here. I have some good stuff for you to survive.",
                "You are welcomed to check me anytime if you regret what you've chosen.",
                "Have another run in the training field to try out your weapon. Or just go out to test your luck.",
                "You can talk to Vanessa, our nurse, to get first-aid and armor. Good luck pal!"
            };
            diolog.text = gameGuide[sentenceID];
            DioAnim.SetBool("hide", false);
            interact2Engineer = false;
            interact2Nurse = false;
            tutorialisDone = false;
            isReadytoOutside = false;
            playerUI.playerAudio.volume = 0;
            UICamera3D.gameObject.SetActive(false);
        }

        if (Application.loadedLevel == 2) {
            sentenceID = 0;
            deadPanelAnim = GameObject.Find("DeadAnim").GetComponent<Animator>();
            deadCamFade = GameObject.Find("deadCamFade").GetComponent<TweenAlpha>();
            deadCamFadeCollider = GameObject.Find("deadCamFade").GetComponent<Collider>();
            diolog.text = "Welcome to surviving cross. Defend zombie attack n survive ALAP! Don't let Mr.Z touch ur ass! Close this Dialog when u're READY!";
            isPlayerinTrainingField = false;
            rotateJoystick.enable = false;
            moveJoystick.enable = false;
            deadCamFadeCollider.enabled = false;
            if (DataBase.weaponSet == true) {
                //playerUI.playerAudio.volume = 0;
                playerUI.gameObject.SetActive(false);
                DioAnim.SetBool("hide", false);
                UICamera3D.gameObject.SetActive(false);
                weaponInventory.gameObject.SetActive(false);
            }
            else {
                playerUI.gameObject.SetActive(false);
                DataBase.currentWeaponID = 0;
                setWeaponInventoryIcon();
                UICamera3D.gameObject.SetActive(true);
                moveJoystick.enable = false;
                rotateJoystick.enable = false;
                weaponInventory.gameObject.SetActive(true);
                StartCoroutine(delayPlayerUI());
            }
            dioFrameBtn.gameObject.SetActive(false);
            closeFrameBtn.gameObject.SetActive(true);
            armorLevel = GameObject.Find("armorLevel").GetComponent<UISlider>();
            healthLevel = GameObject.Find("healthLevel").GetComponent<UISlider>();
            zombieBlocker0Health = GameObject.Find("zombieBlocker0Health").GetComponent<UISlider>();
            zombieBlocker1Health = GameObject.Find("zombieBlocker1Health").GetComponent<UISlider>();
            zombieBlocker2Health = GameObject.Find("zombieBlocker2Health").GetComponent<UISlider>();
            zombieBlocker3Health = GameObject.Find("zombieBlocker3Health").GetComponent<UISlider>();
        }
    }

    void Start() {
        StartCoroutine(camFadeIn());
        rotateJoystick.enable = isPlayerinTrainingField;
        moveJoystick.enable = isPlayerMove;
        bulletCount.text = DataBase.bullets.ToString();
        magCapacity.text = "/" + DataBase.bullets.ToString();
        killCount.text = DataBase.killedTarget.ToString();
        totalTarget.text = "/" + DataBase.totalTarget.ToString();
        gunIcon.spriteName = gunIconSpriteNameArray[DataBase.currentWeaponID];
        if (DataBase.weaponSet == false) {
            StartCoroutine(removeWeaponAnchor());
        }
    }

    public void triggerEnterEvent(string _name) {
        switch(_name){
            case "entranceTrigger":
                if (isPlayerEntered == false && sentenceID == 2) {
                    changeDiologContent();
                }
                isPlayerEntered = true;
                if (sentenceID < 6 || tutorialisDone == true) {
                    playActionBtnAnim(isPlayerEntered);
                }
                
                isPlayerinTrainingField = false;
                rotateJoystick.enable = false;
                zombieIconPositionAnimation.Play(isPlayerinTrainingField);
                break;
            case "startTrainingTrigger":
                if (isPlayerinTrainingField == false) {
                    if (DataBase.totalTarget <= 0) {
                        SendMessage("restrictArea");
                        SendMessage("spawnFakeTarget");
                    }
                    if (sentenceID == 3) {
                        changeDiologContent();
                    }
                }

                if (sentenceID >= 5) {
                    rotateJoystick.enable = true;
                }

                DataBase.totalTarget = 10;
                totalTarget.text = "/" + DataBase.totalTarget.ToString();
                isPlayerinTrainingField = true;
                zombieIconPositionAnimation.Play(isPlayerinTrainingField);
                break;
            case "exitTrigger":
                if (isPlayerinTrainingField == false && exitDoorIsOpen == false) {
                    isPlayerinTrainingField = true;
                    exitDoorIsOpen = true;
                    SendMessage("openExitDoor");
                }
                break;
            case "gateTrigger":
                if (isPlayerTrained == true && sentenceID == 6) {
                    changeDiologContent();
                }
                if (DataBase.hangZombieisDead == false && sentenceID >= 6) {
                    rotateJoystick.enable = true;
                    isPlayerinTrainingField = true;
                }
                else {
                    rotateJoystick.enable = false;
                    isPlayerinTrainingField = false;
                }

                if (sentenceID >= 12 && tutorialisDone == true) {
                    isReadytoOutside = true;
                    DioAnim.SetBool("hide", false);
                    closeFrameBtn.gameObject.SetActive(false);
                    diolog.text = "You wanna have a try outside? Press A at Right of the screen if you confirm to go. Or keep away from this Gate.";
                    playActionBtnAnim(true);
                }
                break;
            case "blockAreaTrigger":
                if (isPlayerinArea2 == false) {
                    SendMessage("restrictArea");
                    SendMessage("spawnMoveingFakeTarget");
                }
                isPlayerinArea2 = true;
                break;
            case "endTrainingTrigger":
                SendMessage("removeAreaRestrict");
                rotateJoystick.enable = false;
                isPlayerinTrainingField = false;
                isPlayerinArea2 = false;
                zombieIconPositionAnimation.Play(isPlayerinTrainingField);
                if (DataBase.killedTarget != 0) {
                    StartCoroutine(resetTargetNumer(0.5f));
                }
                
                break;
            default:
                break;
        }
    }

    public void triggerExitEvent(string _name) {
        switch (_name) {
            case "entranceTrigger":
                isPlayerEntered = false;
                if (sentenceID < 6 || tutorialisDone == true) {
                    actionButtonAnimation.Play(isPlayerEntered);
                }
                
                SendMessage("closeEntranceDoor");
                isPlayerinTrainingField = false;
                rotateJoystick.enable = false;
                break;
            case "startTrainingTrigger":
                if (isPlayerinTrainingField == false) {
                    if (DataBase.totalTarget <= 0) {
                        SendMessage("restrictArea");
                        SendMessage("spawnFakeTarget");
                    }
                    
                    if (sentenceID == 3) {
                        changeDiologContent();
                    }
                }

                if (sentenceID >= 5) {
                    rotateJoystick.enable = true;
                }

                DataBase.totalTarget = 10;
                totalTarget.text = "/" + DataBase.totalTarget.ToString();
                isPlayerinTrainingField = true;
                break;
            case "exitTrigger":
                if (isPlayerinTrainingField == false && exitDoorIsOpen == false) {
                    isPlayerinTrainingField = true;
                    exitDoorIsOpen = true;
                    SendMessage("openExitDoor");
                }
                break;
            case "gateTrigger":
                if (sentenceID >= 12 && tutorialisDone == true) {
                    actionButtonAnimation.Play(false);
                    DioAnim.SetBool("hide", true);
                    closeFrameBtn.gameObject.SetActive(false);
                    isReadytoOutside = false;
                }
                break;
            case "endTrainingTrigger":
                SendMessage("removeAreaRestrict");
                SendMessage("closeExitDoor");
                rotateJoystick.enable = false;
                isPlayerinTrainingField = false;
                isPlayerinArea2 = false;
                isPlayerTrained = true;
                zombieIconPositionAnimation.Play(isPlayerinTrainingField);
                if (DataBase.killedTarget != 0) {
                    StartCoroutine(resetTargetNumer(0.5f));
                }
                break;
            default:
                break;
        }
    }

    public void triggerStayEvent(string _name) {
        switch (_name) {
            case "entranceTrigger":
                isPlayerinTrainingField = false;
                rotateJoystick.enable = false;
                break;
            case "startTrainingTrigger":
                if (isPlayerinTrainingField == false) {
                    if (DataBase.totalTarget <= 0) {
                        SendMessage("restrictArea");
                        SendMessage("spawnFakeTarget");
                    }
                    if (sentenceID == 3) {
                        changeDiologContent();
                    }
                }

                if (sentenceID >= 5) {
                    rotateJoystick.enable = true;
                }

                DataBase.totalTarget = 10;
                totalTarget.text = "/" + DataBase.totalTarget.ToString();
                isPlayerinTrainingField = true;
                if (isPause == true) {
                    isPlayerinTrainingField = false;
                    rotateJoystick.enable = false;
                }
                break;
            default:
                break;
        }
    }

    public void disableActionButton() {
        actionButtonAnimation.Play(false);
    }

    public void actionBtnAction() {
        if (interact2Engineer == true) {
            DataBase.weaponSet = false;
            weaponInventoryAnim.SetBool("hide", false);
            DioAnim.SetBool("hide", true);
            inventoryOpened = true;
            moveJoystick.enable = false;
            StartCoroutine(showPlayerUI(1));
        }

        if (interact2Nurse == true) {
            DioAnim.SetBool("hide", false);
            closeFrameBtn.gameObject.SetActive(true);
            diolog.text = "Accessories from Nurse Vanessa are only available in Released Version. Demo only has Default Setting. Good Luck!";
        }

        if (isReadytoOutside == true) {
            DataBase.weaponSet = true;
            DataBase.levelID = 2;
            moveJoystick.enable = false;
            DioAnim.SetBool("hide", true);
            cameraFade.Play(false);
            SendMessage("openGate");
            StartCoroutine(loadNewScene(1));
        }

        if (interact2Engineer == false && interact2Nurse == false) {
            SendMessage("openEntranceDoor");
        }
    }

    public void changeDiologContent() {
        if (sentenceID < gameGuide.Length - 1) {
            diolog.text = gameGuide[++sentenceID];
        }else if (sentenceID >= gameGuide.Length - 1) {
            DioAnim.SetBool("hide", true);
        }
        if (UIButton.current == closeFrameBtn) {
            tutorialisDone = true;
        }
        changeDioController(sentenceID);
    }

    public void closeFrame() {
        isPlayerinTrainingField = true;
        rotateJoystick.enable = true;
        moveJoystick.enable = true;
        DioAnim.SetBool("hide", true);
        GameObjectAction.isSceneStarted = true;
    }

    private void changeDioController(int _int) {
        switch (_int) {
            case 1:
                isPlayerMove = true;
                dioFrameBtn.gameObject.SetActive(false);
                moveJoystick.enable = isPlayerMove;
                break;
            case 2:
                break;
            case 3:
                dioFrameBtn.gameObject.SetActive(false);
                break;
            case 4:
                dioFrameBtn.gameObject.SetActive(true);
                break;
            case 5:
                dioFrameBtn.gameObject.SetActive(false);

                if (isPlayerinTrainingField == false) {
                    rotateJoystick.enable = false;
                }
                else {
                    rotateJoystick.enable = true;
                }
                
                break;
            case 8:
                dioFrameBtn.gameObject.SetActive(true);
                isPlayerinTrainingField = false;
                rotateJoystick.enable = false;
                break;
            case 9:
                dioFrameBtn.gameObject.SetActive(false);
                break;
            case 10:
                dioFrameBtn.gameObject.SetActive(true);
                break;
            case 12:
                dioFrameBtn.gameObject.SetActive(false);
                closeFrameBtn.gameObject.SetActive(true);
                break;
            default:
                break;
        }
        //Debug.Log(sentenceID);
    }

    public void weaponInventoryBack() {
        if (Application.loadedLevel == 1) {
            moveJoystick.enable = true;

            if (sentenceID == 9) {
                changeDiologContent();
                DioAnim.SetBool("hide", false);
            }
        }
        
        inventoryOpened = false;
        weaponInventoryAnim.SetBool("hide", true);

        if (tutorialisDone == false) {
            DioAnim.SetBool("hide", false);
        }

        if (Application.loadedLevel == 2) {
            dioFrameBtn.gameObject.SetActive(false);
            closeFrameBtn.gameObject.SetActive(true);
            weaponInventoryAnim.SetBool("weaponSet", true);
            DioAnim.SetBool("hide", false);
            rotateJoystick.enable = false;
            moveJoystick.enable = false;
            playerUI.gameObject.SetActive(false);
            StartCoroutine(disableWeaponInventory());
        }
        DataBase.weaponSet = true;
        StartCoroutine(showPlayerUI(0));
    }

    public void setWeaponInventoryIcon() {
        for (int i = 0; i < weaponIconArray.Length; i++) {
            weaponIconArray[i].normalSprite = weaponNormalIconArray[i];
        }
        weaponIconArray[DataBase.currentWeaponID].normalSprite = weaponSelectIconArray[DataBase.currentWeaponID];
        weaponIntro.text = weaponIntroArray[DataBase.currentWeaponID];
    }

    public static void playActionBtnAnim(bool _bool) {
        if (initializedTweenPostion == false) {
            actionButtonAnimation.from.x = actionButton.transform.localPosition.x;
            actionButtonAnimation.to.x = actionButton.transform.localPosition.x - (actionButton.width * 1.5f);
            initializedTweenPostion = true;
        }
        actionButtonAnimation.Play(_bool);
    }

    public static void tweenWeaponLevel(float _damage, float _accuracy, float _fireRate) {
        damageLevelTween.to = new Vector3(_damage, 1, 1);
        accuracyLevelTween.to = new Vector3(_accuracy, 1, 1);
        fireRateLevelTween.to = new Vector3(_fireRate, 1, 1);
        damageLevelTween.Play(true);
        accuracyLevelTween.Play(true);
        fireRateLevelTween.Play(true);
    }

    public void pauseGame() {
        if (Time.timeScale == 1) {
            Time.timeScale = 0;
            cameraFade.from = 0;
            cameraFade.to = 0.6f;
            cameraFade.duration = 0.3f;
            cameraFade.ResetToBeginning();
            cameraFade.Play(true);
            moveJoystick.enable = false;
            rotateJoystick.enable = false;
            pausedFrameAnim.SetBool("hide", false);
            camFadeCollider.enabled = true;
            isPause = true;
        }
    }

    public void resumeGame() {
        if (Time.timeScale == 0) {
            cameraFade.from = 0.6f;
            cameraFade.to = 0;
            cameraFade.duration = 0.3f;
            cameraFade.ResetToBeginning();
            cameraFade.Play(true);
            pausedFrameAnim.SetBool("hide", true);
            camFadeCollider.enabled = false;
            if (Application.loadedLevel == 1) {
                if (sentenceID >= 1) {
                    moveJoystick.enable = true;
                }
                if (isPlayerinTrainingField == true && sentenceID >= 5) {
                    rotateJoystick.enable = true;
                }
            }
            else if (Application.loadedLevel == 2) {
                if (isPlayerinTrainingField == true) {
                    moveJoystick.enable = true;
                    rotateJoystick.enable = true;
                }
            }
            Time.timeScale = 1;
            isPause = false;
        }
    }

    public void exitGame() {
        if (Time.timeScale == 0) {
            Time.timeScale = 1;
        }
        cameraFade.from = 0.6f;
        cameraFade.to = 1;
        cameraFade.duration = 0.5f;
        cameraFade.ResetToBeginning();
        cameraFade.Play(true);
        pausedFrameAnim.SetBool("hide", true);
        DataBase.levelID = 3;
        moveJoystick.enable = false;
        rotateJoystick.enable = false;
        isPlayerinTrainingField = false;
        StartCoroutine(loadNewScene(0.5f));
    }

    public void openTheEnd() {
        deadCamFadeCollider.enabled = true;
        deadCamFade.Play(true);
        DataBase.weaponSet = false;
        DataBase.levelID = 5;
        StartCoroutine(loadNewScene(1.1f));
    }

    private IEnumerator resetTargetNumer(float _waitforSecond) {
        yield return new WaitForSeconds(_waitforSecond);
        DataBase.totalTarget = 0;
        DataBase.killedTarget = 0;
        totalTarget.text = "/" + DataBase.totalTarget.ToString();
        killCount.text = DataBase.killedTarget.ToString();
    }

    private IEnumerator showPlayerUI(float _second) {
        yield return new WaitForSeconds(_second);
        if (inventoryOpened == true) {
            if (DataBase.isMute == true) {
                playerUI.playerAudio.volume = 0;
            }
            else {
                playerUI.playerAudio.volume = 1;
            }
            UICamera3D.gameObject.SetActive(true);
        }
        else {
            playerUI.playerAudio.volume = 0;
            UICamera3D.gameObject.SetActive(false);
        }
        
    }

    private IEnumerator removeWeaponAnchor() {
        yield return new WaitForSeconds(0.05f);
        weaponAnchor.enabled = false;
    }

    private IEnumerator loadNewScene(float _second) {
        yield return new WaitForSeconds(_second);
        Application.LoadLevel(4);
    }

    private IEnumerator camFadeIn() {
        yield return new WaitForSeconds(0.1f);
        cameraFade.Play(true);
    }

    private IEnumerator disableWeaponInventory() {
        yield return new WaitForSeconds(0.75f);
        weaponInventoryAnim.gameObject.SetActive(false);
    }

    private IEnumerator delayPlayerUI() {
        yield return new WaitForSeconds(0.75f);
        playerUI.gameObject.SetActive(true);
    }
}
