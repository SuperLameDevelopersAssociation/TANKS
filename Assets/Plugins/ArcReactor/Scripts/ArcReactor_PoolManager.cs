using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Arc Reactor Rays/Managers/Pooling manager")]
public class ArcReactor_PoolManager : MonoBehaviour {

	//Singleton
	public static ArcReactor_PoolManager Instance { get; private set;}

	public Dictionary<GameObject,List<ArcReactor_Arc>> freeEntities;
	public Dictionary<ArcReactor_Arc,GameObject> activeEntities;


	protected void Awake()
	{
		if (ArcReactor_PoolManager.Instance == null)
		{
			Instance = this;
			freeEntities = new Dictionary<GameObject, List<ArcReactor_Arc>>();
			activeEntities = new Dictionary<ArcReactor_Arc, GameObject>();
		}
		else
		{
			Debug.LogError("More than one instance of ArcReactor_PoolManager is active. Disabling additional instance");
			this.enabled = false;
		}
	}

	public GameObject GetFreeEntity(GameObject originalPrefab)
	{
		if (freeEntities.ContainsKey(originalPrefab))
		{
			List<ArcReactor_Arc> entitiesList = freeEntities[originalPrefab];
			if (entitiesList.Count == 0)
			{
				GameObject newEntity = GameObject.Instantiate(originalPrefab);
				activeEntities.Add(newEntity.GetComponent<ArcReactor_Arc>(),originalPrefab);
				return newEntity;
			}
			else
			{
				ArcReactor_Arc arc = entitiesList[entitiesList.Count-1];
				entitiesList.RemoveAt(entitiesList.Count-1);
				arc.EnableArc();
				arc.currentlyInPool = false;
				arc.elapsedTime = 0;
				arc.playBackward = false;
				arc.Initialize();
				activeEntities.Add(arc,originalPrefab);
				return arc.gameObject;
			}
		}
		else
		{
			GameObject newEntity = GameObject.Instantiate(originalPrefab);
			activeEntities.Add(newEntity.GetComponent<ArcReactor_Arc>(),originalPrefab);
			freeEntities.Add(originalPrefab,new List<ArcReactor_Arc>());
			return newEntity;
		}
	}

	public void SetEntityAsFree(ArcReactor_Arc arc)
	{
		if (activeEntities.ContainsKey(arc))
		{			
			arc.DisableArc();
			arc.currentlyInPool = true;
			freeEntities[activeEntities[arc]].Add(arc);
			activeEntities.Remove(arc);
		}
		else
		{
			arc.DestroyArc();
		}
	}
}
