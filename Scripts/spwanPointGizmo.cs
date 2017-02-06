using UnityEngine;
using System.Collections;

public class spwanPointGizmo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "spawnPointGizmo.jpg", true);
    }
}
