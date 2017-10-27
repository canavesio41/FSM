using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowGold : MonoBehaviour
{
	private Text objText;
	public GameObject reference;
	public Canvas canvas;

	// Use this for initialization
	void Start ()
	{
		GameObject GO = Instantiate(reference) as GameObject;
		objText = GO.GetComponent<Text> ();
		objText.transform.SetParent (canvas.transform, false);

		GO.name = "ShowGold";
	}
	
	// Update is called once per frame
	void Update ()
	{
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if(hit.collider != null && hit.collider.tag != "Node")
		{
			objText.gameObject.SetActive (true);	

			if(hit.collider.gameObject.GetComponent<Mine>() != null)
			{
				objText.text = hit.collider.gameObject.GetComponent<Mine> ().GoldInMine.ToString();
			}

			else if(hit.collider.name == "Worker")
			{
				objText.text = hit.collider.gameObject.GetComponent<Worker> ().GoldInBag.ToString();
			}

			else if(hit.collider.name == "Warehouse")
			{
				objText.text = hit.collider.gameObject.GetComponent<House> ().GoldInHouse.ToString();
			}


			Vector3 newPos = hit.transform.position;
			newPos.z = -1;
			objText.transform.position = Camera.main.WorldToScreenPoint(newPos);		
		}
		else
		{
			objText.gameObject.SetActive (false);	
		}
		
	}
}

