using UnityEngine;
using System.Collections;

public class fakeZombieController : MonoBehaviour {
    public int id;
    public int health = 10;
    public Transform fakeTargetDebris;
    public Transform fakeZombieParts;

    private int partsCount = 0;

    public void targetisHit(int _damage) {
        health -= _damage;
        targetDead(health);
    }

    public void targetDead(int _health) {
        if (_health <= 0) {
            if (partsCount == 0) {
                partsCount++;
                GameObjectAction.targetCount--;
                DataBase.killedTarget++;
                UIManager.killCount.text = DataBase.killedTarget.ToString();
                Instantiate(fakeTargetDebris, new Vector3(this.transform.position.x, 1.5f, this.transform.position.z), Quaternion.identity);
                Instantiate(fakeZombieParts, this.transform.position, this.transform.rotation);
            }
            Destroy(this.gameObject);
        }
    }
}
