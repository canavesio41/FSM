using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AStar : MonoBehaviour
{

	private PathRequestManager requestManager;


	public void Awake()
	{
		requestManager = GetComponent<PathRequestManager>();
	}
		


	public void OpenNode (List<Nodo> open, Nodo n)
	{
		open.Add(n);
	}

	public void CloseNode(List<Nodo> closed,Nodo n)
	{
		closed.Add(n);
	}
		
	public Nodo getNode(List<Nodo> open)
	{
		return open [0];
	}

	public int GetDistance(Nodo a, Nodo b)
	{
		int distX = Mathf.Abs (a.Row - b.Row);
		int distY = Mathf.Abs (a.Col - b.Col);

		if (distX > distY) 
		{
			return Grid.sharedInstance.horCells * distY + Grid.sharedInstance.verCells * (distX - distY);
		}
		return Grid.sharedInstance.horCells * distX + Grid.sharedInstance.verCells * (distY - distX);
	}

	private GameObject[] RetracePath(Nodo startNode, Nodo endNode)
	{
		List<Nodo> path = new List<Nodo>();
		Nodo currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.Padre;
		}

		GameObject[] waypoints = ObtainGO(path);
		Array.Reverse(waypoints);
		return waypoints;
	}

	public GameObject[] ObtainGO(List<Nodo> path)
	{
		List<GameObject> newPath = new List<GameObject> ();

		for(int i = 0; i < path.Count; i++)
		{
			newPath.Add(path [i].gameObject);
		}
		return newPath.ToArray();
	}


	public void GetPath (Nodo root, Nodo goal)
	{
		GameObject[] waypoints = new GameObject[0];
		List<Nodo> closed = new List<Nodo> ();
		List<Nodo> open = new List<Nodo>();

		bool pathSuccess = false;

		OpenNode(open ,root);
		while (open.Count > 0) 
		{
			Nodo currentNode = getNode (open);
			for (int i = 1; i < open.Count; i++) 
			{	
				if (open [i].Weight < currentNode.Weight || open[i].Weight == currentNode.Weight)
				{
					if(open[i].h < currentNode.h)
					{
						currentNode = open[i];
					}
				}
			}

			open.Remove (currentNode);
			CloseNode (closed, currentNode);

			if (currentNode == goal) 
			{
				pathSuccess = true;
				break;
			}		

			for (int i = 0; i < currentNode.Adj.Count; i++) 
			{
				Nodo n = currentNode.Adj [i];
				if (!n.walkable || closed.Contains (n))
				{
					continue;
				}
				int newCostToNeighbour = n.Weight + GetDistance(currentNode, n);
				if (newCostToNeighbour < n.Weight || !open.Contains (n)) 
				{
					n.Padre = currentNode;
					n.Weight = newCostToNeighbour;
					n.h = GetDistance (n, goal);

					if (!open.Contains (n)) 
					{
						OpenNode(open, n);
					}
				}
			}
		}
		if (pathSuccess) 
		{
			waypoints = RetracePath(root,goal);
		}
		requestManager.FinishedProcessingPath(waypoints,pathSuccess);
	}


}


