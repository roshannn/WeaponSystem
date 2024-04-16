using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingSignals
{
	public struct KeyBindUpdationSignal {
        public PlayerActions currKeyAction;
        public KeyCode keyBind;
        public KeyBindType keyBindType;
    }

    public struct KeyBindConflictCheckSignal {
        public KeyBindController controller;
        public KeyCode prevKeyBind;
        public KeyCode newkeyBind;
    }
}
