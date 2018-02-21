using System;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
	public GameObject gameManager;     

	public MainMenu ()
	{
	}

	public void StartGame()
	{
		if (GameManager.instance == null)
			//Instantiate gameManager prefab
			Instantiate(gameManager);
	}
}


