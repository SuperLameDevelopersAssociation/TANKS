using UnityEngine;
using System.Collections;

public class StoreCustomizationVariables : MonoBehaviour 
{
	int[] hullChosen;
	int[] weaponChosen;
	int[] abilityChosen;
	int[] armorLevelChosen;

	public void CollectInfo(SelectType.ComponentType component, int listPosition)
	{
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++) 
		{
			if (PhotonNetwork.player == PhotonNetwork.playerList [i]) 
			{
				switch (component) 
				{
				case SelectType.ComponentType.Hull:
					hullChosen[i] = listPosition;
					break;

				case SelectType.ComponentType.Weapon:
					weaponChosen[i] = listPosition;
					break;

				case SelectType.ComponentType.Ability:
					abilityChosen[i] = listPosition;
					break;

				case SelectType.ComponentType.ArmorLevel:
					armorLevelChosen[i] = listPosition;
					break;

				}
			}
		}

		CustomizationManager.hullChosen = hullChosen;
		CustomizationManager.weaponChosen = weaponChosen;
		CustomizationManager.abilityChosen = abilityChosen;
		CustomizationManager.armorLevelChosen = armorLevelChosen;
	}
}
