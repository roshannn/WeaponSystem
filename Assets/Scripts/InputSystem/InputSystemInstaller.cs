using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputSystemInstaller : MonoInstaller {
    public KeyBindModifier keyBindModifier;
    public override void InstallBindings() {
        Container.Bind<IAccessPlayerActions>().To<InputSystemHandler>().AsSingle();
        Container.BindInstance<KeyBindModifier>(keyBindModifier);
        Container.DeclareSignal<KeyBindingSignals.KeyBindUpdationSignal>().OptionalSubscriber();
    }
}
