using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid: MonoBehaviour
{
	public GameObject cube;
	public int horCells = 6;
	public int verCells = 4;
	public Vector3 startPos = new Vector3(0f, 0f, 0f);
	public float spacingX = 1f;
	public float spacingY = 1f;
	public  Nodo[,] cellsArray;
	public List<List<Nodo>> nodos = new List<List<Nodo>> ();
	public List<Nodo> fields = new List<Nodo> ();
	public List<Nodo> cells = new List<Nodo> ();
	public static Grid sharedInstance;
	public float nodeRadius;
	private float nodeDiameter;
	public GameObject grid;
	public Vector2 gridWorldSize;

	void Awake()
	{
		sharedInstance = this;
		cellsArray = new Nodo[horCells, verCells];
		nodeDiameter = nodeRadius *2;

	}

	void Update()
	{
		fields = cells;


		for (int x = 0; x < horCells; x++) 
		{
			for (int y = 0; y < verCells; y++)
			{
				foreach(var n in fields)
				{
					cellsArray[x, y] = n;
					cellsArray [x, y].transform.localScale = Vector3.one * (nodeDiameter -0.1f); //new Vector3 (nodeDiameter, nodeDiameter, 0);
				}
			} 
		}

	}

	public void MakeGrid(int hor, int vert)
	{
		cellsArray = new Nodo[horCells,verCells];
	
		Debug.Log (cellsArray.Length);
		GameObject clone;
		Vector3 clonePos;

		grid = new GameObject("Grid");


		for(int x = 0; x < hor; x++)
		{
			for(int y = 0; y < vert; y++)
			{

				clonePos = new Vector3(startPos.x + (x * -spacingX), startPos.y + (y * -spacingY), startPos.z);
				clone = Instantiate(cube, clonePos, Quaternion.identity) as GameObject;
				clone.name = (y /*+ 1*/) + "x" + (x /*+ 1*/);
				clone.tag = "Node";
				clone.AddComponent <Nodo>();
				clone.GetComponent<Nodo> ().Col = y;///0 + 1;
				clone.GetComponent<Nodo> ().Row = x;// + 1;
				clone.GetComponent<Nodo> ().pos = clone.transform.position;
				clone.transform.SetParent (grid.transform);
				cellsArray[x,y] = clone.GetComponent<Nodo>();
				cells.Add (cellsArray [x, y].GetComponent<Nodo>());

			}
		}
		//Agrego adyacentes
		for (int x = 0; x < hor; x++) 
		{
			for (int y = 0; y < vert; y++) 
			{
				cellsArray[x,y].GetComponent<Nodo> ().Adj = GetAdj (cellsArray[x,y].GetComponent<Nodo> ());
			}
		}
		grid.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 ( Screen.width *1.8f, Screen.height/2, 1));
	}


	public List<Nodo> GetAdj(Nodo current)
	{
		List<Nodo> neighbours = new List<Nodo> ();

		for (int ix = current.Row - 1; ix <= current.Row + 1; ix++) {
			for (int iy = current.Col - 1; iy <= current.Col + 1; iy++) {
				if (ix >= 0 && ix < cellsArray.GetLength (0) && iy >= 0 && iy < cellsArray.GetLength (1))// && ix != current.Row && iy != current.Col)
					neighbours.Add (cellsArray [ix, iy]);
			}
		}

		for (int ix = 0; ix < neighbours.Count; ix++) {
			if(neighbours[ix] == current){
				neighbours.Remove (current);
				break;
			}
		}
		//debug only
		string s = string.Empty;

		for (int i = 0; i < neighbours.Count; ++i) {
			s += neighbours[i].name + "\n";
		}
		//Debug.LogFormat ("Adyacentes de {0}: {1}", current.name, s);
		//

		return neighbours;
	}
		
	public Nodo NodeFromWorldPoint(Vector3 worldPosition) 
	{
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y/2) / gridWorldSize.y;

		percentX = Mathf.Clamp01(percentX);
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridWorldSize.x) * percentX);
		int y = Mathf.RoundToInt((gridWorldSize.y) * percentY);

		return cellsArray[x,y];
	}

	void OnDrawGizmos() 
	{
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x ,gridWorldSize.y, 0));
	}

}