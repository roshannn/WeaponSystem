using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using Zenject;

public class ActionInputContainerView : MonoBehaviour {
    [Inject] DiContainer _container;

    [SerializeField] private TMP_Text actionName;
    public KeyBindController keyBind1;
    public KeyBindController keyBind2;

    private PlayerActions currKeyAction;

    public void RenderContainer(PlayerInputData data) {
        currKeyAction = data.PlayerAction;
        actionName.text = GameUtility.AddSpacesToCamelCase(data.PlayerAction.ToString());
        _container.Inject(keyBind1);
        keyBind1.InitializeKeyBind(data, KeyBindType.KeyBind1);
        _container.Inject(keyBind2);
        keyBind2.InitializeKeyBind(data, KeyBindType.KeyBind2);
    }
}
