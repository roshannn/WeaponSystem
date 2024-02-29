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
            InstantiateAndFireBullet();

        }
    }

    private void InstantiateAndFireBullet() {
        Bullet bullet = BulletPool.GetNewInstance();
        bullet.transform.position = releasePoint.position;
        bullet.transform.rotation = releasePoint.rotation;
        bullet.Fire(()=>BulletPool.ReturnInstance(bullet));
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Fire();
        }
    }
}

