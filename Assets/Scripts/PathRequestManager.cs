using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PathRequestManager : MonoBehaviour 
{

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
	PathRequest currentPathRequest;
	static PathRequestManager instance;
	AStar pathfinding;
	bool isProcessingPath;


	void Awake() 
	{
		instance = this;
		pathfinding = GetComponent<AStar>();
	}

	public static void RequestPath(GameObject GOroot, GameObject GOgoal, Action<GameObject[], bool> callback)
	{
		Nodo pathStart = RaycastAboveNode(GOroot).GetComponent<Nodo>();
		Nodo pathEnd = RaycastAboveNode(GOgoal).GetComponent<Nodo>();
		PathRequest newRequest = new PathRequest(pathStart,pathEnd,callback);
		instance.pathRequestQueue.Enqueue(newRequest);
		instance.TryProcessNext();
	}

	private void TryProcessNext() 
	{
		if (!isProcessingPath && pathRequestQueue.Count > 0) 
		{
			currentPathRequest = pathRequestQueue.Dequeue();
			isProcessingPath = true;
			pathfinding.GetPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		}
	}

	private static GameObject RaycastAboveNode(GameObject GO)
	{

		Vector2 v = GO.transform.position;
		Collider2D[] col = Physics2D.OverlapPointAll(v);

		if(col.Length > 0)
		{
			foreach(Collider2D c in col)
			{
				if(c.tag == "Node")
				{
					return c.GetComponent<Collider2D>().gameObject;
				}
			}
		}
		return null;
	}

	public void FinishedProcessingPath(GameObject[] path, bool success)
	{
		currentPathRequest.callback(path,success);
		isProcessingPath = false;
		TryProcessNext();
	}

	struct PathRequest
	{
		public Nodo pathStart;
		public Nodo pathEnd;
		public Action<GameObject[], bool> callback;

		public PathRequest(Nodo _start, Nodo _end, Action<GameObject[], bool> _callback) 
		{
			pathStart = _start;
			pathEnd = _end;
			callback = _callback;
		}

	}
}
