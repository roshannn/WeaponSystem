using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;
public class KeyBindPageView : MonoBehaviour {

    private IAccessPlayerActions _accessPlayerActions;
    private DiContainer _container;

    [Inject]
    void Construct(IAccessPlayerActions accessPlayerActions, DiContainer container) {
        _accessPlayerActions = accessPlayerActions;
        _container = container;
    }

    private Dictionary<PlayerActions, PlayerInputData> currData;
    private Dictionary<KeyCode, List<KeyBindController>> keyConflicts;
    private Dictionary<PlayerActions, ActionInputContainerView> playerActionToContainerMap;

    [SerializeField]
    private GenericPoolFactory<ActionInputContainerView> actionInputContainerPool;
    [SerializeField]
    private GameObject content;

    private void OnEnable() {
        playerActionToContainerMap = new Dictionary<PlayerActions, ActionInputContainerView>();
        currData = _accessPlayerActions.GetPlayerInputSettings();
        _container.Inject(actionInputContainerPool);
        actionInputContainerPool.Initialize(this.transform);
    }


    [Button("RenderView")]
    public void RenderView() {
        foreach (var data in currData) {
            ActionInputContainerView actionInputContainer = actionInputContainerPool.GetNewInstance();
            actionInputContainer.RenderContainer(data.Value);
            playerActionToContainerMap.Add(data.Key, actionInputContainer);
            actionInputContainer.transform.SetParent(content.transform, false);
        }
        CheckForConflicts();
    }

    private void CheckForConflicts() {
        keyConflicts ??= new Dictionary<KeyCode, List<KeyBindController>>();
        foreach (var kvp in playerActionToContainerMap) {
            KeyCode keyBind1 = kvp.Value.keyBind1.CurrKeyCode;
            KeyCode keyBind2 = kvp.Value.keyBind2.CurrKeyCode;
            if (keyBind1 != KeyCode.None) {
                if (keyConflicts.ContainsKey(keyBind1)) {
                    keyConflicts[keyBind1].Add(kvp.Value.keyBind1);
                } else {
                    keyConflicts.Add(keyBind1, new List<KeyBindController> { kvp.Value.keyBind1 });
                }
            }
            if (keyBind2 != KeyCode.None) {
                if (keyConflicts.ContainsKey(keyBind2)) {
                    keyConflicts[keyBind2].Add(kvp.Value.keyBind2);
                } else {
                    keyConflicts.Add(keyBind2, new List<KeyBindController> { kvp.Value.keyBind2 });
                }
            }
        }
        foreach(var kvp in keyConflicts) {
            if (kvp.Value.Count > 1) {
                foreach (var keyBind in kvp.Value) {
                    keyBind.SetConflict(true);
                }
            }
        }
    }

    public void OnDisable() {
        actionInputContainerPool.ReturnAllInstances();
    }


}
