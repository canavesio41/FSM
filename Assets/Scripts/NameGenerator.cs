using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameGenerator : MonoBehaviour {

	public Canvas canvas;
	public Text objName;
	public GameObject reference;
	Vector2 Screenposition;

	// Use this for initialization
	void Start ()
	{
		GameObject GO = Instantiate(reference) as GameObject;
		GO.name = "UI - " + this.gameObject.name;
		objName = GO.GetComponent<Text> ();
		objName.text = this.gameObject.name;
		objName.transform.SetParent (canvas.transform, false);


	}

	// Update is called once per frame
	void Update () 
	{
		Screenposition  = new Vector2 (gameObject.transform.position.x , gameObject.transform.position.y + 0.2f);

		objName.transform.position = Camera.main.WorldToScreenPoint(Screenposition);

		if(gameObject.activeInHierarchy == false)
		{
			objName.gameObject.SetActive (false);
		}
	}
}
