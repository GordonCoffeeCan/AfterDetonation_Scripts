using UnityEngine;
using System.Collections;

public class CharacterAnimation : MonoBehaviour {
    public Animator animationController;

    public AnimatorStateInfo currentAnimationState {
        get {
            return (animationController.GetCurrentAnimatorStateInfo(0));
        }
    }

    void Awake() {

    }

	// Use this for initialization
	void Start () {
        if (animationController == null) {
            Debug.LogError("No Animation Container is assigned!");
            return;
        }
	}

    public void SetAnimatorBool(string _string, bool _bool) {
        animationController.SetBool(_string, _bool);
    }

    public void SetAnimatorFloat(string _string, float _floot) {
        animationController.SetFloat(_string, _floot);
    }

    public void SetAnimatorInt(string _string, int _int) {
        animationController.SetInteger(_string, _int);
    }

    public void SetLayerWeight(int _layerIndex, float _wight) {
        int layNum = animationController.layerCount;
        for (int i = 0; i < layNum; i++) {
            animationController.SetLayerWeight(i, 0);
            if (i == _layerIndex) {
                animationController.SetLayerWeight(_layerIndex, _wight);
            }
        }
    }
}
