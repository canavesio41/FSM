using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum states
{
	idle,
	GoMine,
	GoHouse,
	Mining,
	DropGold
};

public enum events
{
	GotoMining,
	NoMoreGold,
	DropingGold,
	ReachMine,
	ReachHouse,
	MaxGold,
	Ocupped
};

public class FSM : MonoBehaviour
{
	public int[,] fsm;
	public int currentState = 0;
	public int currentEvent = 0;

	public void Init(int s, int e)
	{
		fsm = new int[s, e];
		for (int i = 0; i < s; i++)
		{
			for (int j = 0; j < e; j++)
			{
				fsm [i, j] = -1;
			}
		}
	}

	public void setRelation(int s, int e, int to)
	{
		fsm [s, e] = to;
	}

	public void SetEvent(int e)
	{
		int stateToSet = fsm [currentState == -1 ? 0 : currentState , e];

		if(stateToSet != -1)
		{
			currentState = stateToSet;	
			currentEvent = e;
		}
	}


	public int GetState()
	{
		return currentState;
	}

}
