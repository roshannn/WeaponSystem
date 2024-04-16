using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindModifier : MonoBehaviour {
	private KeyBindController currkeyBindController;
    private bool listenForKeyPress;
    public void StartKeyBindSetting(KeyBindController keyBindController) {
        if(currkeyBindController != null) {
            currkeyBindController.ResetKeyBind();
        }
        currkeyBindController = keyBindController;
        listenForKeyPress = true;
    }

    private void Update() {
        if (listenForKeyPress) {
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode))) {
                if (Input.GetKeyDown(keyCode)) {
                    currkeyBindController.HandleKeyBindUpdation(keyCode);
                    listenForKeyPress = false;
                    currkeyBindController = null;
                }
            }
        }
    }
}
