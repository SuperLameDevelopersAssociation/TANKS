using UnityEngine;
using System.Collections;
using TrueSync;

public class CustomizationManager : TrueSyncBehaviour 
{
	public GameObject[] hulls;

	CloakingAbility cloak;
	TeleportAbility teleport;

	int hullChosen;
	int weaponChosen;
	int abilityChosen;
	int armorLevelChosen;

	Shooting shooting;

	void Start()
	{
		shooting = GetComponent<Shooting> ();
		cloak = GetComponent<CloakingAbility> ();
		teleport = GetComponent<TeleportAbility> ();
	}

	public override void OnSyncedStart()
	{		
		hullChosen = 0;
		weaponChosen = 0;
		abilityChosen = 0;
		armorLevelChosen = 1;
	}
	public void CollectInfo(SelectType.ComponentType component, int listPosition)
	{
		switch (component) 
		{
			case SelectType.ComponentType.Hull:
			hullChosen = listPosition;
			break;

			case SelectType.ComponentType.Weapon:
			weaponChosen = listPosition;
			break;

			case SelectType.ComponentType.Ability:
			abilityChosen = listPosition;
			break;

			case SelectType.ComponentType.ArmorLevel:
			armorLevelChosen = listPosition;
			break;

		}
	}

	public void CustomizeTank()
	{
		hulls [hullChosen].SetActive (true);

		switch (weaponChosen) 
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

		if (abilityChosen == 0)
			teleport.enabled = true;
		else
			cloak.enabled = true;


		//Armor level chosen....
	}
}
