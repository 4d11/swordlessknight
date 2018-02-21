using System;
using UnityEngine;
using UnityEngine.SceneManagement;   

public class GameOver
{
	public GameOver ()
	{
	}

	public void RestartGame(){
		GameManager.instance.level = 0;
		SceneManager.LoadScene (0);
	}
}


