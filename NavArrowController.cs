using UnityEngine;
using System.Collections;

public class NavArrowController : MonoBehaviour {
    public Transform scalePivot;
    public Animator navArrowAnim;
    public Transform[] locationPoint;

    private Transform arrowRotationPivot;
    private float targetDistance;

    void Awake() {
        arrowRotationPivot = this.transform;
    }

	// Use this for initialization
	void Start () {
        scalePivot.localScale = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        switch (UIManager.sentenceID) {
            case 2:
                point2Me(0);
                break;
            case 3:
                point2Me(0);
                break;
            case 4:
                hideArrow(true);
                break;
            case 6:
                point2Me(1);
                break;
            case 7:
                point2Me(1);
                break;
            case 8:
                hideArrow(true);
                break;
            case 9:
                point2Me(2);
                break;
            case 10:
                hideArrow(true);
                break;
            default:
                break;
        }
	}

    private void point2Me(int _targetIndex) {
        targetDistance = Vector3.Distance(arrowRotationPivot.position, locationPoint[_targetIndex].position);
        smoothLookAt(arrowRotationPivot, locationPoint[_targetIndex], 10);
        if (targetDistance <= 2) {
            hideArrow(true);
        }
        else {
            hideArrow(false);
        }
    }

    private void smoothLookAt(Transform _trans, Transform _target, float _speed){
        Quaternion _targetRotation = Quaternion.LookRotation(_target.position - _trans.position);
        _trans.rotation = Quaternion.Slerp(_trans.rotation, _targetRotation, _speed * Time.deltaTime);
    }

    private void hideArrow(bool _bool){
        if (_bool == true) {
            iTween.ScaleTo(scalePivot.gameObject, iTween.Hash("scale", new Vector3(0, 0, 0), "time", 0.3f, "easetype", iTween.EaseType.easeOutCubic));
            navArrowAnim.SetBool("isImpulse", false);
        }
        else {
            iTween.ScaleTo(scalePivot.gameObject, iTween.Hash("scale", new Vector3(1, 1, 1), "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "oncomplete", "onTweenComplete", "oncompletetarget", arrowRotationPivot.gameObject));
            
        }
    }

    private void onTweenComplete() {
        navArrowAnim.SetBool("isImpulse", true);
    }
}
