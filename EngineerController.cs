using UnityEngine;
using System.Collections;

public class EngineerController : MonoBehaviour {
    public float rotationSpeed = 5;

    private Transform enginner;
    private CharacterAnimation enginnerAnimation;
    private int animationStateID = 0;
    private int lastStateID = 0;
    private bool isPlayerCome = false;
    private bool actionBtnAnimPlayed = false;

    private float animationTimer = 1;
    private float animationInterval = 1.5f;

    private Quaternion originalRoation;

    private Transform player;

    void Awake() {
        enginner = this.gameObject.transform;
        originalRoation = enginner.rotation;
        enginnerAnimation = this.GetComponent<CharacterAnimation>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        actionBtnAnimPlayed = false;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        animationTimer -= Time.deltaTime;
        lastStateID = animationStateID;

        if (animationTimer <= 0 && isPlayerCome == false) {
            animationStateID = makeRandomStatID();
            
            if (animationStateID == lastStateID && animationStateID == 4) {
                for (int i = 0; i <= 20; i++) {
                    animationStateID = makeRandomStatID();
                    if (animationStateID != 4) {
                        break;
                    }
                }
            }
            enginnerAnimation.SetAnimatorInt("stateID", animationStateID);
            animationTimer = animationInterval;
        }

        if (Vector3.Distance(enginner.transform.position, player.transform.position) <= 2) {
            isPlayerCome = true;
            enginnerAnimation.SetAnimatorInt("stateID", 0);
            enginnerAnimation.SetAnimatorBool("isPlayerCome", isPlayerCome);
            smoothRotation(enginner, player);
            if (isPlayerCome == true && enginnerAnimation.animationController.IsInTransition(0)) {
                enginnerAnimation.SetAnimatorBool("isRepeatAnystate", false);
            }

            if (actionBtnAnimPlayed == false && UIManager.sentenceID >= 9) {
                UIManager.interact2Engineer = isPlayerCome;
                UIManager.playActionBtnAnim(isPlayerCome);
                actionBtnAnimPlayed = true;
            }

            if (UIManager.sentenceID == 9) {
                UIManager.DioAnim.SetBool("hide", true);
            }
        } else {
            isPlayerCome = false;
            enginnerAnimation.SetAnimatorInt("stateID", animationStateID);
            enginnerAnimation.SetAnimatorBool("isPlayerCome", isPlayerCome);
            enginner.rotation = Quaternion.Slerp(enginner.rotation, originalRoation, rotationSpeed * Time.deltaTime);
            enginnerAnimation.SetAnimatorBool("isRepeatAnystate", true);
            if (actionBtnAnimPlayed == true && UIManager.sentenceID >= 9) {
                UIManager.interact2Engineer = isPlayerCome;
                UIManager.actionButtonAnimation.Play(isPlayerCome);
                actionBtnAnimPlayed = false;
            }
        }
	}

    private int makeRandomStatID() {
        return((int)Random.Range(1.0f, 5.0f));
    }

    private void smoothRotation(Transform _selfTransform, Transform _target) {
        Quaternion _targetRotation = Quaternion.LookRotation(_target.position - _selfTransform.position);
        _selfTransform.rotation = Quaternion.Slerp(_selfTransform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
    }
}
