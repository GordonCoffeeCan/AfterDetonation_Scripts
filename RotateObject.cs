using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {
    public float rotationSpeed = 10;

    private Transform myTransform;

    void Awake() {
        myTransform = this.transform;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        myTransform.Rotate(Vector3.up, rotationSpeed * 25 * Time.deltaTime, Space.World);
	}
}
