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
		shooting = GetComponent<Shooting> ();
		cloak = GetComponent<CloakingAbility> ();
		teleport = GetComponent<TeleportAbility> ();
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
                    if (PhotonNetwork.playerList[i].ID == owner.Id)
                    {
                        //I need to get a referebce to the players gameobject
                    }

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
