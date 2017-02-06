using UnityEngine;
using System.Collections;

public class NurseController : MonoBehaviour {
    public float rotationSpeed = 5;

    private Transform nurse;
    private CharacterAnimation nurseAnimation;
    private int animationStateID = 0;
    private int lastStateID = 0;
    private bool isPlayerCome = false;
    private bool actionBtnAnimPlayed = false;
    private float animationTimer = 5;
    private float animationInterval = 5;

    private Quaternion originalRoation;

    private Transform player;

    void Awake() {
        nurse = this.gameObject.transform;
        originalRoation = nurse.rotation;
        nurseAnimation = this.GetComponent<CharacterAnimation>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        actionBtnAnimPlayed = false;
    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        animationTimer -= Time.deltaTime;
        lastStateID = animationStateID;
        if (animationTimer <= 0 && isPlayerCome == false) {
            animationStateID = makeRandomStatID();

            if (animationStateID == lastStateID && animationStateID == 4) {
                animationTimer = 1;
                for (int i = 0; i <= 20; i++) {
                    animationStateID = makeRandomStatID();
                    if (animationStateID != 4) {
                        break;
                    }
                }
            } else {
                animationTimer = animationInterval;
            }
            nurseAnimation.SetAnimatorInt("stateID", animationStateID);
        }

        if (Vector3.Distance(nurse.transform.position, player.transform.position) <= 2) {
            isPlayerCome = true;
            nurseAnimation.SetAnimatorInt("stateID", 0);
            nurseAnimation.SetAnimatorBool("isPlayerCome", isPlayerCome);
            smoothRotation(nurse, player);
            if (isPlayerCome == true && nurseAnimation.animationController.IsInTransition(0)) {
                nurseAnimation.SetAnimatorBool("isRepeatAnystate", false);
            }
            if (actionBtnAnimPlayed == false && UIManager.sentenceID >= 12) {
                UIManager.interact2Nurse = isPlayerCome;
                UIManager.playActionBtnAnim(isPlayerCome);
                actionBtnAnimPlayed = true;
            }
        } else {
            isPlayerCome = false;
            nurseAnimation.SetAnimatorInt("stateID", animationStateID);
            nurseAnimation.SetAnimatorBool("isPlayerCome", isPlayerCome);
            nurse.rotation = Quaternion.Slerp(nurse.rotation, originalRoation, rotationSpeed * Time.deltaTime);
            nurseAnimation.SetAnimatorBool("isRepeatAnystate", true);
            if (actionBtnAnimPlayed == true && UIManager.sentenceID >= 12) {
                UIManager.interact2Nurse = isPlayerCome;
                UIManager.actionButtonAnimation.Play(isPlayerCome);
                actionBtnAnimPlayed = false;
            }
        }

    }

    private int makeRandomStatID() {
        return ((int)Random.Range(1.0f, 5.0f));
    }

    private void smoothRotation(Transform _selfTransform, Transform _target) {
        Quaternion _targetRotation = Quaternion.LookRotation(_target.position - _selfTransform.position);
        _selfTransform.rotation = Quaternion.Slerp(_selfTransform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
    }
}
