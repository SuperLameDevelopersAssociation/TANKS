using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TrueSync;

public class SelectType : TrueSyncBehaviour 
{
	public string componentName;
	public string description;
	public int positionInList;
	public StoreCustomizationVariables customizationManager; 

	public enum ComponentType
	{
		Hull,
		Weapon,
		Ability,
		ArmorLevel,
	}

	public ComponentType currentComponent = ComponentType.Hull;

	public Text hullInfo;
	public Text weaponInfo;
	public Text abilityInfo;
	public Text abilityDescription;
	public Text armorLevel;
	public Text armorLevelDescription;

	void Start()
	{
		customizationManager = GameObject.Find ("CustomizationManager").GetComponent<StoreCustomizationVariables> ();
		switch (currentComponent) 
		{
		case ComponentType.Hull:
			hullInfo.text = "Hull: " + componentName;
			break;
		case ComponentType.Weapon:
			weaponInfo.text = "Weapon: " + componentName;
			break;
		case ComponentType.Ability:
			abilityInfo.text = "Ability: " + componentName;
			abilityDescription.text = description;
			break;
		case ComponentType.ArmorLevel:
			armorLevel.text = "Armor Level: " + componentName;
			armorLevelDescription.text = description;
			break;
		}
	}

	public void AdjustInfo()
	{
		customizationManager.CollectInfo (currentComponent, positionInList);

		switch (currentComponent) 
		{
			case ComponentType.Hull:
				hullInfo.text = "Hull: " + componentName;
				break;
			case ComponentType.Weapon:
				weaponInfo.text = "Weapon: " + componentName;
				break;
			case ComponentType.Ability:
				abilityInfo.text = "Ability: " + componentName;
				abilityDescription.text = description;
				break;
			case ComponentType.ArmorLevel:
				armorLevel.text = "Armor Level: " + componentName;
				armorLevelDescription.text = description;
				break;
		}
	}
}
