using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class ObjectPoolItem
{
	public GameObject objectToPool;

	public int amountToPool;
}

public class ObjectPooler : MonoBehaviour
{
	public static ObjectPooler SharedInstance;

	public List<ObjectPoolItem> itemsToPool;

	public List<GameObject> pooledObjects;

	void Awake ()
	{
		SharedInstance = this;
	}

	// Use this for initialization
	void Start ()
	{
		// List for the objects
		pooledObjects = new List<GameObject> ();

		foreach (ObjectPoolItem item in itemsToPool) 
		{
			//Th required amount of objects are created here
			for (int i = 0; i < item.amountToPool; i++) 
			{
				GameObject obj = (GameObject)Instantiate (item.objectToPool);

				//Default keeping the false
				obj.SetActive (false);

				//Adding all the objects in the list
				pooledObjects.Add (obj);
			}
		}
	}
}
