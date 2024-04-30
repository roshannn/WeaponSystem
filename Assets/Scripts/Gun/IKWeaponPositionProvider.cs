using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKWeaponPositionProvider : MonoBehaviour {
    public Transform LeftHandPosition;
    public Transform RightHandPosition;

    public Transform GetHandTransform(Hand hand) {
        if (hand == Hand.Left) {
            return LeftHandPosition;
        } else {
            return RightHandPosition;
        }
    }

    public enum Hand {
        Left, Right
    }
}
