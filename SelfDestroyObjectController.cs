using UnityEngine;
using System.Collections;

public class SelfDestroyObjectController : MonoBehaviour {
    public static float damage = 0;
    private Transform bullet;
    private Transform bulletShell;
    private Transform magazine;
    private float bulletSpeed = 35;
    private float destoryTimer;

	// Use this for initialization
	void Start () {
        if (this.transform.tag == "Bullet") {
            bullet = this.transform;
            switch (DataBase.currentWeaponID) {
                case 0:
                    destoryTimer = 0.1f;
                    break;
                case 1:
                    destoryTimer = 0.35f;
                    break;
                case 2:
                    destoryTimer = 0.125f;
                    break;
                default:
                    break;
            }
            damage = PlayerController.damage2Bullet;
        }
        else if (this.transform.tag == "bulletShell") {
            bulletShell = this.transform;
            bulletShell.Rotate(new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50)));
            destoryTimer = 3;
        }else if(this.transform.tag == "magazine"){
            magazine = this.transform;
            magazine.Rotate(new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
            destoryTimer = 5;
        }
        else if (this.transform.tag == "gunFire") {
            destoryTimer = 0.075f;
        }
        else if (this.transform.tag == "zombieBlockerParts") {
            destoryTimer = 8;
        }
        else if (this.transform.tag == "bloodSplash") {
            destoryTimer = 5;
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void FixedUpdate() {
        if (this.transform.tag == "Bullet") {
            bullet.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }

        if (this.transform.tag == "zombieBlockerParts") {
            if (destoryTimer <= 1.5f) {
                foreach (Transform child in this.transform) {
                    child.collider.enabled = false;
                }
            }
        }

        destoryTimer -= Time.deltaTime;
        if (destoryTimer <= 0) {
            destoryTimer = -1;
            Destroy(this.gameObject);
        }
    }
}
