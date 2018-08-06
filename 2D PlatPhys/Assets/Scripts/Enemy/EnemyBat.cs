using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : Enemy 
{	

	[SerializeField] float attackSpeed = 4f;

	public LayerMask playerLayer;
	public Transform target;

	Vector3 attackPos;
	float playerRange;
	float xStrikePos;
	float flyDirection;

	bool playerInRange;
	bool attacking;
	bool movingToStrike;
	bool enemyAtPlayer;

	public override void Start()
	{
		base.Start();
	}

	public override void Update()
	{
		if(!attacking)
		{
			base.Update();
		}

		FlippingSpriteForAttacking();

		playerInRange = Physics2D.OverlapCircle(transform.position, playerRange, playerLayer);
		enemyAtPlayer = Physics2D.OverlapCircle(transform.position, .15f, playerLayer);


		if(playerInRange)
		{
			attacking = true;

			if(!enemyAtPlayer && !movingToStrike)
			{
				attackPos = new Vector3(target.transform.position.x,target.transform.position.y - .3f, target.transform.position.z);
				xStrikePos = transform.localScale.x;

				transform.position = Vector3.MoveTowards(transform.position, attackPos, attackSpeed * Time.deltaTime);
			}
			else if(enemyAtPlayer || movingToStrike)
			{		 
				StartCoroutine(MoveToStrikePosition());
			}
		}
		else
		{
			attacking = false;
		}
		if(attacking)
		{
			playerRange = 6.5f;
		}
		else
		{
			playerRange = 4f;
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, playerRange);
		Gizmos.DrawWireSphere(transform.position, .15f);
	}

	IEnumerator MoveToStrikePosition()
	{
		if(attacking == false)
		{
			yield break;
		}

		Vector3 moveDirection = (target.transform.position - transform.position).normalized;

		Vector3 strikePosition = new Vector3 (transform.position.x + .4f*(xStrikePos), transform.position.y + .2f, transform.position.z );

		movingToStrike = true;
		transform.position = Vector3.MoveTowards(transform.position, strikePosition, attackSpeed * Time.deltaTime);

		yield return new WaitForSeconds(.75f);

		movingToStrike = false;
	}

	void FlippingSpriteForAttacking()
	{
		if(attacking)
		{
			if(target.transform.position.x > transform.position.x)
			{
				Vector3 tempScale = transform.localScale;
				tempScale.x = 1;
				transform.localScale = tempScale;
			}
			if(target.transform.position.x  < transform.position.x)
			{
				Vector3 tempScale = transform.localScale;
				tempScale.x = -1;
				transform.localScale = tempScale;
			}
		}
	}
}
