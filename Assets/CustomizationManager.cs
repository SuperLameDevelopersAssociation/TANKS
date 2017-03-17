using UnityEngine;
using System.Collections;
using TrueSync;
using System.Collections.Generic;

public class CustomizationManager : TrueSyncBehaviour 
{
	public GameObject[] hulls;

	CloakingAbility cloak;
	TeleportAbility teleport;

	public static List<int> hullChosen;
	public static List<int> weaponChosen;
	public static List<int> abilityChosen;
	public static List<int> armorLevelChosen;

	Shooting shooting;

	void Start()
	{

	}

	public override void OnSyncedStart()
	{		
		hullChosen = new List<int>(PhotonNetwork.playerList.Length);
		weaponChosen = new List<int>(PhotonNetwork.playerList.Length);
		abilityChosen = new List<int>(PhotonNetwork.playerList.Length);
		armorLevelChosen = new List<int>(PhotonNetwork.playerList.Length);

		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			for (int i = 0; i < PhotonNetwork.playerList.Length; i++) {
				if (p == PhotonNetwork.playerList [i]) {
					hulls [0] = TrueSyncManager.allPlayers [i].transform.FindChild ("Hull 1").gameObject;
					hulls [1] = TrueSyncManager.allPlayers [i].transform.FindChild ("Hull 2").gameObject;
				}
			}
		}
	}

	public static void UpdateList(List<int> hulls, List<int> weapons, List<int> abilities, List<int> armor)
	{
		print ("Hull List Length is : " + hulls.Count);
		print ("weapons List Length is : " + weapons.Count);
		print ("abilities List Length is : " + abilities.Count);
		print ("armor List Length is : " + armor.Count);
		hullChosen = new List<int>(hulls.Count);
		weaponChosen = new List<int>(weapons.Count);
		abilityChosen = new List<int>(abilities.Count);
		armorLevelChosen = new List<int>(armor.Count);

		hullChosen = hulls;
		weaponChosen = weapons;
		abilityChosen = abilities;
		armorLevelChosen = armor;
	}

    public void CustomizeMyTank()
    {
		PhotonView[] photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();
		foreach (PhotonView view in photonViews)
		{
			var player = view.owner;
			//Objects in the scene don't have an owner, its means view.owner will be null
			if(player!=null){
				var playerPrefabObject = view.gameObject;
				//do works...
			}
		}

        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
            {
                if (p == PhotonNetwork.playerList[i])
                {
                    /// I need to set component based on the player object im talking about
                    /// 
                    shooting = TrueSyncManager.allPlayers[i].GetComponent<Shooting>();
                    cloak = TrueSyncManager.allPlayers[i].GetComponent<CloakingAbility>();
                    teleport = TrueSyncManager.allPlayers[i].GetComponent<TeleportAbility>();


                    hulls[0] = TrueSyncManager.allPlayers[i].transform.FindChild("Hull 1").gameObject;
                    hulls[1] = TrueSyncManager.allPlayers[i].transform.FindChild("Hull 2").gameObject;

                    hulls[hullChosen[i]].SetActive(true);   

                    switch (weaponChosen[i])
                    {
                        case 0:
                            shooting.currentWeapon = Shooting.CurrentWeapon.Projectile;
                            break;
                        case 1:
                            shooting.currentWeapon = Shooting.CurrentWeapon.Flamethrower;
                            break;
                        case 2:
                            shooting.currentWeapon = Shooting.CurrentWeapon.Laser;
                            break;
                    }

                    if (abilityChosen[i] == 0)
                        teleport.enabled = true;
                    else
                        cloak.enabled = true;


                    //Armor level chosen....
                }
            }
        }
    }
}
