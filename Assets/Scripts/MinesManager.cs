using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinesManager : MonoBehaviour
{
	public List<GameObject> deactivateMines = new List<GameObject>();
	public List<GameObject> activateMines = new List<GameObject>();
	public static MinesManager sharedInstance; 

	void Awake()
	{
		sharedInstance = this;
		activateMines = FindMines();
	}


	private List<GameObject> FindMines()
	{
		GameObject[] _gameObjects;
		_gameObjects = GameObject.FindGameObjectsWithTag("Mine");

		List<GameObject> Mines = new List<GameObject>();
			
		foreach (GameObject GO in _gameObjects)
		{
			Mines.Add (GO);
		}
		return Mines;
	}

}


