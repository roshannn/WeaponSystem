using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class KeyBindPageView : MonoBehaviour {
    [Inject] private IAccessPlayerActions _accessPlayerActions;
    [Inject] private DiContainer _container;
    [Inject] private SignalBus _signalBus;

    private Dictionary<PlayerActions, PlayerInputData> currData;
    private Dictionary<KeyCode, List<KeyBindController>> keyConflicts = new Dictionary<KeyCode, List<KeyBindController>>();
    private Dictionary<PlayerActions, ActionInputContainerView> playerActionToContainerMap;

    [SerializeField] private GenericPoolFactory<ActionInputContainerView> actionInputContainerPool;
    [SerializeField] private GameObject content;

    private void OnEnable() {
        SubscribeEvents();
        InitializeData();
    }

    private void SubscribeEvents() {
        _signalBus.Subscribe<KeyBindingSignals.KeyBindConflictCheckSignal>(OnKeyBindUpdate);
    }

    private void InitializeData() {
        currData = _accessPlayerActions.GetPlayerInputSettings();
        playerActionToContainerMap = new Dictionary<PlayerActions, ActionInputContainerView>();

        _container.Inject(actionInputContainerPool);
        actionInputContainerPool.Initialize(this.transform);
    }

    [Button("RenderView")]
    public void RenderView() {
        foreach (var data in currData) {
            var actionInputContainer = actionInputContainerPool.GetNewInstance();
            actionInputContainer.RenderContainer(data.Value);
            actionInputContainer.transform.SetParent(content.transform, false);
            playerActionToContainerMap.Add(data.Key, actionInputContainer);
        }
        CheckForConflicts();
    }

    private void CheckForConflicts() {
        foreach (var entry in playerActionToContainerMap) {
            AddOrUpdateConflict(entry.Value.keyBind1.CurrKeyCode, entry.Value.keyBind1);
            AddOrUpdateConflict(entry.Value.keyBind2.CurrKeyCode, entry.Value.keyBind2);
        }
        HighlightConflicts();
    }

    private void AddOrUpdateConflict(KeyCode keyCode, KeyBindController controller) {
        if (keyCode != KeyCode.None) {
            if (!keyConflicts.TryGetValue(keyCode, out var list)) {
                list = new List<KeyBindController>();
                keyConflicts[keyCode] = list;
            }
            list.Add(controller);
        }
    }

    private void HighlightConflicts() {
        foreach (var conflict in keyConflicts) {
            bool isConflict = conflict.Value.Count > 1;
            foreach (var controller in conflict.Value) {
                controller.SetConflict(isConflict);
            }
        }
    }

    private void OnKeyBindUpdate(KeyBindingSignals.KeyBindConflictCheckSignal signalData) {
        if (signalData.prevKeyBind != KeyCode.None) {
            UpdateConflictList(signalData.prevKeyBind, signalData.controller, false);
        }
        if (signalData.newkeyBind != KeyCode.None) {
            UpdateConflictList(signalData.newkeyBind, signalData.controller, true);
        }
        HighlightConflicts();
    }

    private void UpdateConflictList(KeyCode keyCode, KeyBindController controller, bool add) {
        if (keyConflicts.TryGetValue(keyCode, out var list)) {
            if (add) {
                list.Add(controller);
            } else {
                list.Remove(controller);
            }
        } else if (add) {
            keyConflicts[keyCode] = new List<KeyBindController> { controller };
        }
    }

    private void OnDisable() {
        _signalBus.TryUnsubscribe<KeyBindingSignals.KeyBindConflictCheckSignal>(OnKeyBindUpdate);
        actionInputContainerPool.ReturnAllInstances();
    }
}
