using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour {
    public GenericPoolFactory<Bullet> BulletPool;
    [SerializeField]private Transform releasePoint;
    protected virtual void Awake() {
        BulletPool.Initialize(this.transform);
    }
    public void Fire() {
        if (BulletPool != null) {
            Bullet bullet = BulletPool.GetNewInstance();
            bullet.transform.position = releasePoint.position;

        }
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
    }
}

