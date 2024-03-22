using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseGun : MonoBehaviour {
    public GenericPoolFactory<Bullet> BulletPool;
    [SerializeField]private Transform releasePoint;

    public WeaponSO currWeapon;
    public float FireRate => currWeapon.weaponData.FireRate;
    public float RecoilRate => currWeapon.weaponData.RecoilRate;
    public Quaternion InitialRotation;
    public bool applyingRecoil;
    protected virtual void Awake() {
        BulletPool.Initialize(this.transform);
        InitialRotation = transform.rotation;
        prevRotation = transform.rotation;
    }

    public ShootHandler shoot;

    public float currTimer = 0;
    private float recoilRecoverySpeed = 2f;

    Quaternion prevRotation;
    public void Fire() {
        currTimer += Time.deltaTime;
        if(currTimer >= currWeapon.weaponData.FireRate) {
            if (BulletPool != null) {
                InstantiateAndFireBullet();
                StartCoroutine(ApplyRecoil(FireRate));
            }
            currTimer = 0;
        }
    }

    private void ReturnToInitialRotation() {
        if (!applyingRecoil) {
            transform.rotation = Quaternion.Lerp(transform.rotation, InitialRotation, Time.deltaTime * recoilRecoverySpeed);
        }
    }

    private IEnumerator ApplyRecoil(float time) {
        applyingRecoil = true;
        Quaternion recoilRotation = Quaternion.Euler(RecoilRate, RecoilRate, 0);
        Quaternion reqRotation = prevRotation * recoilRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, reqRotation, Time.deltaTime * FireRate);
        yield return null;
    }

    private void InstantiateAndFireBullet() {
        Bullet bullet = BulletPool.GetNewInstance();
        bullet.transform.position = releasePoint.position;
        bullet.transform.rotation = releasePoint.rotation;
        bullet.Fire(releasePoint,()=>BulletPool.ReturnInstance(bullet));
    }

    private void Update() {
        if (Input.GetMouseButton(0)) {
            Fire();
        }
        ReturnToInitialRotation();
    }

}

