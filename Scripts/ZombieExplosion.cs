using UnityEngine;
using System.Collections;

public class ZombieExplosion : MonoBehaviour {
    private float explosionPower = 1.8f;
    private float destroyTimer = 4;

	// Use this for initialization
	void Start () {
        foreach(Transform child in this.transform) {
            explosion(child);
        }
	}

    void Update() {
        if(Application.loadedLevelName == "Level1_StreetCross"){
            destroyTimer -= Time.deltaTime;
            if (destroyTimer < 1) {
                foreach (Transform child in this.transform) {
                    child.collider.enabled = false;
                }
            }

            if (destroyTimer <= 0) {
                Destroy(this.gameObject);
            }
        }
    }

    private void explosion(Transform _transform) {
        _transform.Rotate(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)));
        _transform.rigidbody.AddForce(new Vector3(Random.Range(-explosionPower, explosionPower), Random.Range(-1.0f, explosionPower), Random.Range(-explosionPower, explosionPower)), ForceMode.Impulse);
    }
}
