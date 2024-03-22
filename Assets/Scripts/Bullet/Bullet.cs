using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private Rigidbody bulletRigidBody;
    public void Fire(Transform ReleasePoint, Action ResetBullet = null) {
        if (bulletRigidBody != null) {
            bulletRigidBody.AddForce(ReleasePoint.transform.forward * 150f, ForceMode.Impulse);
            StartCoroutine(this.ResetBullet(ResetBullet));
        }
    }

    private IEnumerator ResetBullet(Action ResetBullet) {
        yield return null;
        yield return new WaitForSeconds(3);
        bulletRigidBody.velocity= Vector3.zero;
        ResetBullet?.Invoke();
    }
}
