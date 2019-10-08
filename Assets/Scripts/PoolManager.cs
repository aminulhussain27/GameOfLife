using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	//Keeping one list of all the objects which are on pool
	public List<GameObject> objectsToPool;

	public List<int> numberOfObjectsToPool;

	Dictionary <string, Stack<GameObject>> pool;

	//Singleton
	public static PoolManager instance = null;

	void Awake ()
	{
		if (instance == null) 
		{
			instance = this;
		}
		else if (instance != this) 
		{
			Destroy (gameObject);
		}

		InitializePoolManager ();
	}


	// Initialize pool manager.
	void InitializePoolManager ()
	{
		pool = new Dictionary<string, Stack<GameObject>> ();
	
		for (int i = 0; i < objectsToPool.Count; i++)
		{
			pool.Add (objectsToPool [i].name, new Stack<GameObject> ());
	
			for (int j = 0; j < numberOfObjectsToPool [i]; j++)
			{
				GameObject go = Instantiate (objectsToPool [i]);
			
				go.transform.SetParent (transform);
			
				go.name = objectsToPool [i].name;

				go.gameObject.SetActive (false);

				pool [objectsToPool [i].name].Push (go);
			}
		}
	}

	//Spawn, or displace an existing hidden object at a particular position or rotation.
	public Cell Spawn (string objectName, Vector3 newPosition, Quaternion newRotation)
	{
		if (!pool.ContainsKey (objectName)) 
		{
			return null;
		}
	
		if (pool [objectName].Count > 0) 
		{
			GameObject go = pool [objectName].Pop ();

			go.transform.position = newPosition;

			go.transform.rotation = newRotation;

			go.SetActive (true);

			return go.GetComponent<Cell> ();

		} 
		return null;
	}
		
	public void Despawn (GameObject objectToDespawn)
	{
		objectToDespawn.SetActive (false);
		pool [objectToDespawn.name].Push (objectToDespawn);
	}
}
