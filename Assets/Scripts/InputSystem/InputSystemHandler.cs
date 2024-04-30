using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Zenject;

public class InputSystemHandler : IInitializable, IDisposable, IAccessPlayerActions {
    private PlayerInputDataContainer playerInputDataContainer;
    public Dictionary<PlayerActions, PlayerInputData> PlayerActionsToInputMap;


    [Inject]
    private SignalBus _signalBus;

    public InputSystemHandler() {
        PopulateData();
    }

    public void Initialize() {
        _signalBus.Subscribe<KeyBindingSignals.KeyBindUpdationSignal>(UpdateKeyBind);
    }
    public void Dispose() {
        _signalBus.TryUnsubscribe<KeyBindingSignals.KeyBindUpdationSignal>(UpdateKeyBind);
    }


    public void PopulateData() {
        playerInputDataContainer = AssetDatabase.LoadAssetAtPath<PlayerInputClass>("Assets/Scripts/InputSystem/PlayerInputData.asset").PlayerInputDataContainer;
        PlayerActionsToInputMap = new Dictionary<PlayerActions, PlayerInputData>();
        foreach (PlayerInputData playerInputData in playerInputDataContainer.playerInputData) {
            PlayerActionsToInputMap.Add(playerInputData.PlayerAction, playerInputData);
        }
    }

    public void UpdateKeyBind(KeyBindingSignals.KeyBindUpdationSignal signalData) {
        if (PlayerActionsToInputMap.TryGetValue(signalData.currKeyAction, out PlayerInputData playerInputData)) {
            if (signalData.keyBindType == KeyBindType.KeyBind1) {
                playerInputData.keyBind1 = signalData.keyBind;
            }
            if (signalData.keyBindType == KeyBindType.KeyBind2) {
                playerInputData.keyBind2 = signalData.keyBind;
            }
        }
        //SaveKeyBinds();
    }

    public Dictionary<PlayerActions, PlayerInputData> GetPlayerInputSettings() {
        return PlayerActionsToInputMap;
    }
}
