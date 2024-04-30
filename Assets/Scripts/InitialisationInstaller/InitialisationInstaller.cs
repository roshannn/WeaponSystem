using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InitialisationInstaller : MonoInstaller
{
	public override void InstallBindings() {
        DiContainerAPI.Container = Container;
    }
}
