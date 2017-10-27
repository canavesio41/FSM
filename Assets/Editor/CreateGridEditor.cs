using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Grid))]
public class CreateGridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();
		Grid gridScript = (Grid)target;
		if(GUILayout.Button("Generate Grid"))
		{
			gridScript.MakeGrid (gridScript.horCells, gridScript.verCells);
		}

	}

}
