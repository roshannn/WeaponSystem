using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private Rigidbody bulletRigidBody;
    public void Fire() {
        if(bulletRigidBody != null) {
            bulletRigidBody.AddForce(transform.forward * 500f, ForceMode.Force);
        }
    }
}
