using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Settings/PlayerInputClass",fileName = "PlayerInputClass")]
public class PlayerInputClass : ScriptableObject
{
	public PlayerInputDataContainer PlayerInputDataContainer;

	[Button("Populate List")]
    public void PopulateList() {
        if (PlayerInputDataContainer.playerInputData.Count > 0) {
            Debug.Log("Cannot Create Now, InputData is not empty");
            return;
        }
        foreach (var x in System.Enum.GetValues(typeof(PlayerActions))) {
            PlayerInputDataContainer.playerInputData.Add(new PlayerInputData() { PlayerAction = (PlayerActions)x });
        }
    }
}
