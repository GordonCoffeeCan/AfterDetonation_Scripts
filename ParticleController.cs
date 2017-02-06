using UnityEngine;
using System.Collections;

public class ParticleController : MonoBehaviour {
    private float lifeTime;

	// Use this for initialization
	void Start () {
        lifeTime = this.particleSystem.startLifetime;
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) {
            Destroy(this.gameObject);
        }
	}
}
