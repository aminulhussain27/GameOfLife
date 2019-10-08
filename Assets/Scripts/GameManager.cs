using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Cell cellPrefab;

	public float updateInterval = 0.1f;
    float counter;

    // Declaring a matrix for cells, allows neighbour checking!
    public Cell[,] cells;

	public bool isPlaying = false;

	public int generation;
	public Text genText;

	//Singleton
	private static GameManager instance = null;

	public static GameManager Instance()
	{
		if(instance == null)
		{
			instance = FindObjectOfType<GameManager> ();
		}
		return instance;
	}

	void Start ()
	{
		InitGrid ();

		SoundManager.Instance ().playSound (SoundManager.SOUND_ID.LOOP_BACKGROUND, 1, true);
    }

	void Update ()
	{
		if(isPlaying)
		{
			counter += Time.deltaTime;
			if(counter >= updateInterval)
			{
				counter = 0.0f;
				UpdateCells();
			}
		}
	}

	public void UpdateCells()
	{
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				cells[i,j].CellUpdate();
			}
		}
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				cells[i,j].ApplyCellUpdate();
			}
		}

		generation++;
		genText.text = generation.ToString("000");
	}

	public void RemoveGrid ()
	{
		// Checks if there are cells in the grid or not.
		if (cells == null) return;

		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
                PoolManager.instance.Despawn (cells [i, j].gameObject);
			}
		}
	}


	//Initializing the map or Grid(I kept fixed size 6x6 as per the assignment)
	public void InitGrid ()
	{
		generation = 0;

		genText.text = generation.ToString("000");

		//Create cells
		cells = new Cell[6, 6];

		for (int i = 0; i < 6; i++)// 6cell size in X direction
		{
			for (int j = 0; j < 6; j++)//6 cells in Y directions
			{
				// Creating a cell into the scene.

				Cell c = PoolManager.instance.Spawn ("CellSprite", new Vector3 (i * 4.5f, j * 4.5f, 0.0f), Quaternion.identity);

				c.InitCell (i, j);

				cells [i, j] = c;
			}
		}
	}

	public void ResetCells ()
	{
		// Checks if there are cells in the grid or not.
		if (cells == null) return;

		generation = 0;
		genText.text = generation.ToString("000");

		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 6; j++)
			{
				cells [i, j].ClearCell ();
            }
		}
	}

	public void Run ()
	{
		if (!isPlaying)
		{
			isPlaying = true;
			counter = 0.0f;
        }
	}

	public void Stop ()
	{
		if (isPlaying)
		{
			isPlaying = false;
			counter = 0.0f;
		}
    }
}
