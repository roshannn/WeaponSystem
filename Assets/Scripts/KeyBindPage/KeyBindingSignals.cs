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
}
