using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandPosition : MonoBehaviour
{
	public Transform handTransform;

    private void LateUpdate() {
        if(handTransform!= null) {
            transform.position = handTransform.position;
            transform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles);
        }
    }
}
