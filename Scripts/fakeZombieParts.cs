using UnityEngine;
using System.Collections;

public class fakeZombieParts : MonoBehaviour {
    public Transform fzLeft;
    public Transform fzRight;
    public Transform fzMain;
    public Transform fzPole;

    private float lifeTimer = 5;

	// Use this for initialization
	void Start () {
        explosion(fzLeft);
        explosion(fzRight);
        explosion(fzMain);
        explosion(fzPole);
	}
	
	// Update is called once per frame
    void Update() {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 2) {
            fzLeft.collider.enabled = false;
            fzRight.collider.enabled = false;
            fzMain.collider.enabled = false;
            fzPole.collider.enabled = false;
        }
        if (lifeTimer <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void explosion(Transform _transform) {
        _transform.Rotate(new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f)));
        _transform.rigidbody.AddForce(new Vector3(Random.Range(-3.5f, 3.5f), Random.Range(-3.5f, 3.5f), Random.Range(-3.5f, 3.5f)), ForceMode.Impulse);
    }
}
