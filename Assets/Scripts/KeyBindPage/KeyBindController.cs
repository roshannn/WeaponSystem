using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class KeyBindController : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler {
    private KeyBindModifier _modifier;
    private SignalBus _signalBus;

    [Inject]
    private void Construct(KeyBindModifier modifier, SignalBus signalBus) {
        _modifier = modifier;
        _signalBus = signalBus;
    }

    [SerializeField]
    private Button keyBindButton;
    [SerializeField]
    private TMP_Text keyBindText;

    [SerializeField]
    private Button removeBindingButton;

    private PlayerActions currKeyAction;
    private KeyBindType currKeyBindType;
    private KeyCode currKeyCode;
    public KeyCode CurrKeyCode => currKeyCode;


    public void InitializeKeyBind(PlayerInputData data, KeyBindType keyBindType) {
        currKeyBindType = keyBindType;
        currKeyCode = currKeyBindType == KeyBindType.KeyBind1 ? data.keyBind1 : data.keyBind2;
        CacheCurrentKeyAction(data.PlayerAction);
        RenderKeyBind(currKeyCode);
        SetKeyBindButton();
        ToggleRemoveBindingButton(false);
    }

    private void ToggleRemoveBindingButton(bool state) {
        removeBindingButton.gameObject.SetActive(state);
    }

    public void CacheCurrentKeyAction(PlayerActions currKeyAction) {
        this.currKeyAction = currKeyAction;
    }
    public void RenderKeyBind(KeyCode keyBind) {
        if (keyBind.Equals(KeyCode.None)) {
            keyBindText.text = "-";
        } else {
            keyBindText.text = GameUtility.AddSpacesToCamelCase(keyBind.ToString());
        }
    }
    public void SetKeyBindButton() {
        keyBindButton.onClick.RemoveAllListeners();
        keyBindButton.onClick.AddListener(UpdateKeyBindDelegate);
        removeBindingButton.onClick.RemoveAllListeners();
        removeBindingButton.onClick.AddListener(RemoveBinding);
    }

    private void RemoveBinding() {
        HandleKeyBindUpdation(KeyCode.None);
    }

    public void UpdateKeyBindDelegate() {
        SetKeyBindButtonColor("141414");
        _modifier.StartKeyBindSetting(this);
    }

    public void SetKeyBindButtonColor(string hex) {
        Color currentButtonColor = keyBindButton.image.color;
        Color toSet = GameUtility.HexToColor(hex);
        toSet.a = currentButtonColor.a;
        keyBindButton.image.color = toSet;
    }

    public void HandleKeyBindUpdation(KeyCode keyCode) {
        RenderKeyBind(keyCode);
        SetKeyBindButtonColor("000000");
        _signalBus.Fire(new KeyBindingSignals.KeyBindUpdationSignal() {
            currKeyAction = currKeyAction, keyBind = keyCode, keyBindType = currKeyBindType
        });
        _signalBus.Fire(new KeyBindingSignals.KeyBindConflictCheckSignal() {
            controller = this, prevKeyBind = currKeyCode, newkeyBind = keyCode
        });
        currKeyCode = keyCode;

    }
    public void ResetKeyBind() {
        SetKeyBindButtonColor("000000");
        RenderKeyBind(currKeyCode);
    }

    public void SetConflict(bool state) {
        SetKeyBindButtonColor(state ? "E80000" : "000000");
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ToggleRemoveBindingButton(true);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ToggleRemoveBindingButton(false);
    }
}
