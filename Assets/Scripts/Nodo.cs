using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour
{
	public Nodo Padre;
	public List<Nodo> Adj;
	public int Col;
	public int Row;
	public Vector3 pos;
	public bool walkable = true;

	public Renderer render;
	public int Weight;
	public int h;


	public void Start()
	{
		render = GetComponent<Renderer> ();
		Weight = Random.Range (1, 10);
	}

}
