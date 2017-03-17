using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrueSync;

public class StoreCustomizationVariables : MonoBehaviour
{
    List<int> hullChosen;
	List<int> weaponChosen;
	List<int> abilityChosen;
	List<int> armorLevelChosen;

	void Start()
	{
		print ((TrueSyncManager.allPlayers.Count));
		hullChosen = new List<int>(PhotonNetwork.playerList.Length);
		weaponChosen = new List<int>(PhotonNetwork.playerList.Length);
		abilityChosen = new List<int>(PhotonNetwork.playerList.Length);
		armorLevelChosen = new List<int>(PhotonNetwork.playerList.Length);

		for (int i = 0; i < PhotonNetwork.playerList.Length ; i++) 
		{
			hullChosen.Add (0);
			weaponChosen.Add (0);
			abilityChosen.Add (0);
			armorLevelChosen.Add (0);
		}

		print ("hullchosen " + hullChosen.Capacity);
		print ("hullchosen " + weaponChosen.Capacity);
		print ("hullchosen " + abilityChosen.Capacity);
		print ("hullchosen " + armorLevelChosen.Capacity);

		print ("hullchosen " + hullChosen.Count);
		print ("hullchosen " + weaponChosen.Count);
		print ("hullchosen " + abilityChosen.Count);
		print ("hullchosen " + armorLevelChosen.Count);
	}

    public void CollectInfo(SelectType.ComponentType component, int listPosition)
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if (PhotonNetwork.player == PhotonNetwork.playerList[i])
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
		CustomizationManager.UpdateList (hullChosen, weaponChosen, abilityChosen, armorLevelChosen);
    }
}