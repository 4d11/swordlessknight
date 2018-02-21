using UnityEngine;
using System.Collections;

public class Enemy : MovingObject {

	public int playerDamage;
//	public AudioClip enemyAttack1;
//	public AudioClip enemyAttack2;

	private Animator animator;
	private Transform target;
	private bool skipMove; // as enemy moes every other turn

	// Use this for initialization
	protected override void Start () 
	{
		GameManager.instance.AddEnemyToList (this); // have enemy script add itself to list of enemies
		animator = GetComponent<Animator> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}

	// Update is called once per frame
	void Update () {

	}

	protected override void AttemptMove<T>(int xDir, int yDir)
	{
		if (skipMove) {
			skipMove = false;
			return;
		}

		base.AttemptMove<T> (xDir, yDir);

		skipMove = true;
	}

	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;

		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) // if coordinates if roughly the same ie in the same column
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;  //

		AttemptMove<Player> (xDir, yDir);
	}

	protected override void OnCantMove<T>(T component)
	{
		Player hitPlayer = component as Player;

		hitPlayer.LoseFood(playerDamage);

		animator.SetTrigger("enemyAttack");

		// SoundManager.instance.RandomizeSfx (enemyAttack1, enemyAttack2);
	}
}