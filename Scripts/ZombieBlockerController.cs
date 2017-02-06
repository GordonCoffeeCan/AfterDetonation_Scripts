using UnityEngine;
using System.Collections;

public class ZombieBlockerController : MonoBehaviour {
    public float health = 1000;
    public Transform blockerParts;
    private float explosionPower = 8.5f;

    private float fullHealth;

	// Use this for initialization
	void Start () {
        fullHealth = health;
	}
	
	// Update is called once per frame
	void Update () {
        switch (this.name) {
            case "zombieBlocker0":
                UIManager.zombieBlocker0Health.value = health / fullHealth;
                break;
            case "zombieBlocker1":
                UIManager.zombieBlocker1Health.value = health / fullHealth;
                break;
            case "zombieBlocker2":
                UIManager.zombieBlocker2Health.value = health / fullHealth;
                break;
            case "zombieBlocker3":
                UIManager.zombieBlocker3Health.value = health / fullHealth;
                break;
            default:
                break;
        }

        if (health <= 0) {
            Transform _blockParts = Instantiate(blockerParts, this.transform.position, this.transform.rotation) as Transform;
            switch (this.name) {
                case "zombieBlocker0":
                    foreach (Transform child in _blockParts) {
                        explosionParts(child, Vector3.back);
                    }
                    break;
                case "zombieBlocker1":
                    foreach (Transform child in _blockParts) {
                        explosionParts(child, Vector3.left);
                    }
                    break;
                case "zombieBlocker2":
                    foreach (Transform child in _blockParts) {
                        explosionParts(child, Vector3.forward);
                    }
                    break;
                case "zombieBlocker3":
                    foreach (Transform child in _blockParts) {
                        explosionParts(child, Vector3.right);
                    }
                    break;
                default:
                    break;
            }
            Destroy(this.gameObject);
        }
	}

    void explosionParts(Transform _transform, Vector3 _direction) {
        _transform.Rotate(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)));
        _transform.rigidbody.AddForce(_direction * explosionPower * Random.value, ForceMode.Impulse);
    }
}
