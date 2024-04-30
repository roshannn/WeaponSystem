using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CharacterController : MonoBehaviour
{
    //make private and assign when changing weapons
    public IKWeaponPositionProvider weaponPositionProvider;

    public WeaponHandPosition leftHandWeaponPosition;
    public WeaponHandPosition rightHandWeaponPosition;

    private void Awake() {
        
    }

    private void SetHandPostion(IKWeaponPositionProvider weaponPositionProvider) {
        leftHandWeaponPosition.handTransform = weaponPositionProvider.GetHandTransform(IKWeaponPositionProvider.Hand.Left);
        rightHandWeaponPosition.handTransform = weaponPositionProvider.GetHandTransform(IKWeaponPositionProvider.Hand.Right);
    }
}
