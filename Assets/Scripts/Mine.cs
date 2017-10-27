using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mine : MonoBehaviour
{
	public int GoldInMine = 1000;
	public bool check = false;
	public List<Worker> WorkerIntoTheMine = new List<Worker>();
	public int maxWorkersNumbers;
	
	void Update ()
	{
		if(GoldInMine <=  0)
		{
			Die ();	
		}

	}

	public void Die()
	{
		if(check == false)
		{
			MinesManager.sharedInstance.deactivateMines.Add (this.gameObject);
			MinesManager.sharedInstance.activateMines.Remove (this.gameObject);
			check = true;
			gameObject.SetActive (false);
		}
	}

	public void AddWorker(Worker go)
	{
		WorkerIntoTheMine.Add (go);
	}

	public void RemoveWorker(Worker go)
	{
		WorkerIntoTheMine.Remove (go);
	}


}

