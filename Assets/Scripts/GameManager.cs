using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public float levelStartDelay = 2f; // time to wait before starting levels (s)
	public float turnDelay = .1f;

	public BoardManager boardScript;
	public int playerFoodPoints = 100;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;   // display current level/day
	private GameObject levelImage;
	private GameObject restartButton;
	public int level = 1; // 5 to test w enemies
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;

	// Use this for initialization
	void Awake()
	{
		if (instance == null)
			instance = this;
		// else if (instance != this)
		// Destroy (gameObject);
		if (instance != this)
			Destroy (this);

		DontDestroyOnLoad (gameObject);
		// this is because when one loads a new scene, all objects are destoryed. Want to use kegame amaner to keep taacof score between screens
		enemies = new List<Enemy>();
		boardScript = GetComponent<BoardManager>();

		InitGame ();
	}


	/* UNITY UPDATES
	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode
		mode)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}
	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to
//		start listening for a scene change event as soon as
//			this script is enabled.
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop
//		listening for a scene change event as soon as this
//			script is disabled.
			//Remember to always have an unsubscription for every
//			delegate you subscribe to!
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	 *//*END UNITY UPDATES */


	/*
	the OnLevelWasLoaded
	function has been deprecated. It has been replaced with
	the SceneManager.sceneLoaded event. In order use the
	sceneLoaded event we must add a delegate to get notifications
	when a scene has been loaded. For

	private void OnLevelWasLoaded(int index)
	{
		level++;

		InitGame ();

		static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			instance.level++;
			instance.InitGame();
		}
	}
	*/


	void OnLevelWasLoaded(int index)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}


	void InitGame()
	{
		doingSetup = true; // to prevet use from moving while moving

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		restartButton = GameObject.Find ("RestartButton");
		restartButton.SetActive (false);

		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay); // display then wait 2 s before turning it off

		enemies.Clear ();
		boardScript.SetupScene (level);
	}


	public void RestartGame(){
		playerFoodPoints = 100;
		level = 1;
		SceneManager.LoadScene (0);
		//SceneManager.LoadScene( SceneManager.GetActiveScene().name);
	}

	private void HideLevelImage()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}

	public void GameOver()
	{
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		restartButton.SetActive (true);
		enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}

		StartCoroutine (MoveEnemies ());
	}

	public void AddEnemyToList(Enemy script)
	{
		enemies.Add (script);
	}

	IEnumerator MoveEnemies ()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds(turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay); // wait even if there are no enemies (for first 2 rounds)
		}

		for (int i = 0; i < enemies.Count; i++) 
		{
			enemies[i].MoveEnemy();
			yield return new WaitForSeconds(enemies[i].moveTime); 
		}

		playersTurn = true;
		enemiesMoving = false;
	}
}