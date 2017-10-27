using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
public class Worker : MonoBehaviour
{
	public int GoldInBag = 0; 
	public GameObject _mine;
	public Mine actualMine;
	public House _house;
	private FSM fsm;
	public float Speed;
	public GameObject[] PathToFollow;
	[HideInInspector]
	public int targetIndex = 0;
	private void Awake ()
	{
		this.fsm = gameObject.AddComponent<FSM> ();

		this.fsm.Init (System.Enum.GetNames (typeof(states)).Length, System.Enum.GetNames (typeof(events)).Length);
	
		this.fsm.setRelation ((int)states.idle, (int)events.GotoMining, (int)states.GoMine);
		this.fsm.setRelation ((int)states.idle, (int)events.NoMoreGold, (int)states.idle);

		this.fsm.setRelation ((int)states.GoMine, (int)events.ReachMine, (int)states.Mining);

		this.fsm.setRelation ((int)states.Mining, (int)events.MaxGold, (int)states.GoHouse);
		this.fsm.setRelation ((int)states.Mining, (int)events.NoMoreGold, (int)states.GoHouse);

		this.fsm.setRelation ((int)states.GoHouse, (int)events.ReachHouse, (int)states.DropGold);
	
		this.fsm.setRelation ((int)states.DropGold, (int)events.GotoMining, (int)states.GoMine);
		this.fsm.setRelation ((int)states.DropGold, (int)events.NoMoreGold, (int)states.idle);
		this.fsm.setRelation ((int)states.DropGold, (int)events.Ocupped, (int)states.idle);


	}
	void Start()
	{
		_mine = FindClosestMine (MinesManager.sharedInstance.activateMines);
		actualMine = _mine.GetComponent<Mine> ();
	}

	public void OnPathFound(GameObject[] newPath, bool pathSuccessful)
	{
		if (pathSuccessful) 
		{
			this.PathToFollow = newPath;
			this.targetIndex = 0;
			StopCoroutine("FollowPath");
			StartCoroutine("FollowPath");
		}
	}

	private void Update ()
	{
		if (actualMine != _mine.GetComponent<Mine> ())
		{
			actualMine = _mine.GetComponent<Mine> ();
		}

		switch (this.fsm.GetState())
		{
			case (int)states.idle:

			if(this._mine != null)
				{
					if(this.actualMine.GoldInMine > 0)
					{
							this.fsm.SetEvent ((int)events.GotoMining);
							Debug.Log ("Idle To Work");
					}

					else
					{
					this.fsm.SetEvent ((int)events.NoMoreGold);
						Debug.Log ("Idle");	
					}
				}
				break;
		
		case (int)states.GoMine:
			Debug.Log ("GoToMine");

			if(actualMine.WorkerIntoTheMine.Count == 0 && MinesManager.sharedInstance.activateMines.Count > 0 && this.actualMine.GoldInMine > 0)
			{
				actualMine.AddWorker (this);
				PathRequestManager.RequestPath(this.gameObject, this._mine.gameObject, OnPathFound);
			}

			break;
					
		case (int)states.Mining:
			Debug.Log ("Mining");

			if (this.GoldInBag <= 100 && this.actualMine.GoldInMine > 0) // and the gold in mine is bigger to 0
				{
					this.GoldInBag += 1;
					this.actualMine.GoldInMine -= 1;
				}

				if (this.GoldInBag == 100)
				{	
					actualMine.RemoveWorker (this);
					this.fsm.SetEvent ((int)events.MaxGold);
				}
				
				break;

		case (int)states.GoHouse:

			Debug.Log ("GoToHouse");
			PathRequestManager.RequestPath(this.gameObject, _house.gameObject, OnPathFound);


			if (!this._mine.gameObject.activeInHierarchy) 
			{
				if (MinesManager.sharedInstance.activateMines.Count > 0)
					_mine = FindClosestMine (MinesManager.sharedInstance.activateMines);
			}

			break;

		case (int)states.DropGold:
			
			if(this.GoldInBag >  0)
			{
				this.GoldInBag -= 1;
				this._house.GoldInHouse += 1;
			}

			if(this._mine == null)
			{
				this.fsm.SetEvent ((int)events.NoMoreGold);
			}
				
			if(this.actualMine.GoldInMine > 0 && GoldInBag <= 0)
			{
				this.fsm.SetEvent ((int)events.GotoMining);
			}

			break;
		}
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Mine") 
		{
			this.fsm.SetEvent ((int)events.ReachMine);
			Debug.Log ("Reach Mine");
		}

		if (other.name == "Warehouse") 
		{
			this.fsm.SetEvent ((int)events.ReachHouse);
			Debug.Log ("Reach House");
		}
	}

	IEnumerator FollowPath()
	{
		Vector3 currentWaypoint = this.PathToFollow[0].transform.position;
		while (true)
		{
			if (this.transform.position == currentWaypoint) 
			{
				this.targetIndex ++;
				if (this.targetIndex >= this.PathToFollow.Length) 
				{
					yield break;
				}
				currentWaypoint = this.PathToFollow [targetIndex].transform.position;
			}

			this.transform.position = Vector3.MoveTowards(transform.position,currentWaypoint, Speed * Time.deltaTime);
			yield return null;

		}
	}

	private GameObject FindClosestMine(List<GameObject> _gameObjects)
	{
		GameObject Closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject GO in _gameObjects)
		{
			Vector3 diff = GO.transform.position - position;
			float currentDistance = diff.sqrMagnitude;
			if (currentDistance < distance) 
			{
				Closest = GO;
				distance = currentDistance;
			}
		}
		return Closest;
	}

}
