using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {
    private AsyncOperation sceneLoaded;
    private UIButton muteBtn;
    private TweenAlpha camFade;
    private Collider camFadeCollider;
    private Animator alertFrame;
    private UILabel alertLabel;
    private UIPanel creditsPanel;
    private UIPanel termsPanel;

    // Use this for initialization
    void Start() {
        if (Application.loadedLevel == 0) {
            StartCoroutine(animationPlayTime(6.25f));
        }
        else if (Application.loadedLevel == 4) {
            if (DataBase.appIsFirstOpen == true) {
                DataBase.levelID = 3;
                DataBase.appIsFirstOpen = false;
                StartCoroutine(loadGameScene());
            }
            else {
                StartCoroutine(loadGameScene());
            }
        }
        else if (Application.loadedLevel == 3) {
            muteBtn = GameObject.Find("muteBtn").GetComponent<UIButton>();
            camFade = GameObject.Find("cameraFade").GetComponent<TweenAlpha>();
            camFadeCollider = GameObject.Find("cameraFade").GetComponent<Collider>();
            alertFrame = GameObject.Find("alertFrame").GetComponent<Animator>();
            alertLabel = GameObject.Find("alertLabel").GetComponent<UILabel>();
            if (DataBase.isMute == true) {
                muteBtn.normalSprite = "muteBtn2";
                muteBtn.hoverSprite = "muteBtn2";
                muteBtn.pressedSprite = "muteBtn2_pressed";
            }
            else {
                muteBtn.normalSprite = "muteBtn";
                muteBtn.hoverSprite = "muteBtn";
                muteBtn.pressedSprite = "muteBtn_pressed";
            }
            camFadeCollider.enabled = false;
            StartCoroutine(camFadeIn());
        }
        else if (Application.loadedLevel == 5) {
            camFade = GameObject.Find("cameraFade").GetComponent<TweenAlpha>();
            StartCoroutine(camFadeIn());
        }
        else if (Application.loadedLevel == 6) {
            creditsPanel = GameObject.Find("creditsPanel").GetComponent<UIPanel>();
            termsPanel = GameObject.Find("termsPanel").GetComponent<UIPanel>();
            camFade = GameObject.Find("cameraFade").GetComponent<TweenAlpha>();
            termsPanel.gameObject.SetActive(false);
            StartCoroutine(camFadeIn());
        }

        if (DataBase.isMute == true) {
            AudioListener.volume = 0;
        }
        else {
            AudioListener.volume = 1;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void loadLevel0() {
        DataBase.weaponSet = false;
        DataBase.levelID = 1;
        Application.LoadLevel(4);
    }

    public void loadLevel1() {
        DataBase.weaponSet = false;
        DataBase.levelID = 2;
        Application.LoadLevel(4);
    }

    public void loadAboutPage() {
        DataBase.weaponSet = false;
        DataBase.levelID = 6;
        Application.LoadLevel(4);
    }

    public void openFacebook() {
        Application.OpenURL("https://www.facebook.com/gordon.lee.77128");
    }

    public void openTwitter() {
        Application.OpenURL("https://twitter.com/GordonCoffeeCan");
    }

    public void openLinkedin() {
        Application.OpenURL("https://www.linkedin.com/profile/view?id=251911920&trk=hp-identity-name");
    }

    public void openHybrible() {
        Application.OpenURL("http://www.wowgordon.com/hybrible");
    }

    public void setMute() {
        if (DataBase.isMute == false) {
            DataBase.isMute = true;
            UIButton.current.normalSprite = "muteBtn2";
            UIButton.current.hoverSprite = "muteBtn2";
            UIButton.current.pressedSprite = "muteBtn2_pressed";
        }
        else if(DataBase.isMute == true){
            DataBase.isMute = false;
            UIButton.current.normalSprite = "muteBtn";
            UIButton.current.hoverSprite = "muteBtn";
            UIButton.current.pressedSprite = "muteBtn_pressed";
        }
    }

    public void openChapterSelection() {
        openAlert("Chapter Selection is only available with Story Mode in Released Version. Demo only provides New Game n Survive Mode. Thank you!");
    }

    public void openSettingFrame() {
        openAlert("Setting Menu is available with Released version. Demo only provides default setting.");
    }

    public void closeAlertFrame() {
        camFade.from = 0.6f;
        camFade.to = 0;
        camFade.duration = 0.5f;
        camFade.ResetToBeginning();
        camFade.Play(true);
        camFadeCollider.enabled = false;
        alertFrame.SetBool("hide", true);
    }

    private void openAlert(string _string) {
        camFade.from = 0;
        camFade.to = 0.6f;
        camFade.duration = 0.5f;
        camFade.ResetToBeginning();
        camFade.Play(true);
        camFadeCollider.enabled = true;
        alertLabel.text = _string;
        alertFrame.SetBool("hide", false);
    }

    public void loadMainMenu() {
        DataBase.levelID = 3;
        StartCoroutine(animationPlayTime(0.65f));
        camFade.from = 0;
        camFade.to = 1;
        camFade.ResetToBeginning();
        camFade.Play(true);
    }

    public void changeAboutContent() {
        if(UIButton.current.name == "creditsBtn"){
            creditsPanel.gameObject.SetActive(true);
            termsPanel.gameObject.SetActive(false);
        }
        else if (UIButton.current.name == "termsBtn") {
            creditsPanel.gameObject.SetActive(false);
            termsPanel.gameObject.SetActive(true);
        }
    }

    private IEnumerator animationPlayTime(float _second) {
        yield return new WaitForSeconds(_second);
        Application.LoadLevel(4);
    }

    private IEnumerator loadGameScene() {
        System.GC.Collect();
        sceneLoaded = Application.LoadLevelAsync(DataBase.levelID);
        yield return sceneLoaded;
    }

    private IEnumerator camFadeIn() {
        yield return new WaitForSeconds(0.1f);
        camFade.Play(true);
    }
}
