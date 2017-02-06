using UnityEngine;
using System.Collections;

public class ZombieArea : MonoBehaviour {
    public Transform[] zombieShape;

	// Use this for initialization
	void Start () {
        if (zombieShape == null) {
            Debug.LogError("No EnmeyShape assigned! Please assign an EnemyShape!");
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider _collider){
        if (_collider.gameObject.tag == "Player") {
            for (var i = 0; i < zombieShape.Length; i++) {
                if(zombieShape[i] != null){
                    zombieShape[i].GetComponent<ZombieController>().isNav = true;
                }
                else {
                    return;
                }
            }
        }
    }
}
