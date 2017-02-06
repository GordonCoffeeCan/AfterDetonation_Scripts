using UnityEngine;
using System.Collections;

public class TriggerInfo : MonoBehaviour {
    private Collider trigger;
    private UIManager uIManager;

    void Awake() {
        trigger = this.collider;
        uIManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        if (!trigger.isTrigger) {
            Debug.LogError("The Collider belong to this Object is not setted as a trigger!");
            return;
        }
    }

    private void OnTriggerEnter(Collider _collider) {
        if (_collider.tag == "Player") {
            uIManager.triggerEnterEvent(trigger.name);
        }
        else if (_collider.tag == "Bullet" && this.tag == "trainingTarget") {
            this.animation.Play("fakeZombie@Hit");
            SendMessageUpwards("targetisHit", (int)PlayerController.damage2Bullet);
            Destroy(_collider.gameObject);
        }
        else if (_collider.tag == "Bullet") {
            Destroy(_collider.gameObject);
        }
    }

    private void OnTriggerStay(Collider _collider) {
        if (_collider.tag == "Bullet") {
            Destroy(_collider.gameObject);
        }
        if (_collider.tag == "Player") {
            uIManager.triggerStayEvent(trigger.name);
        }
    }

    private void OnTriggerExit(Collider _collider) {
        if (_collider.tag == "Player") {
            uIManager.triggerExitEvent(trigger.name);
        }
    }
}
