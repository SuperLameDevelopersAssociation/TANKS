using UnityEngine;
using System.Collections;
using TrueSync;

public class CustomizationManager : TrueSyncBehaviour 
{
	public GameObject[] hulls;

	CloakingAbility cloak;
	TeleportAbility teleport;

	public static int[] hullChosen;
	public static int[] weaponChosen;
    public static int[] abilityChosen;
    public static int[] armorLevelChosen;

	Shooting shooting;

	void Start()
	{

	}

	public override void OnSyncedStart()
	{		
	}

    public void CustomizeMyTank()
    {
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
