using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour 
{

	[SerializeField] protected int health;
	[SerializeField] protected float speed;
	[SerializeField] protected int gems;

	[SerializeField] protected Transform[] waypoints;

	protected Vector3 targetPosition;
	protected Animator anim;
	protected SpriteRenderer enemySprite;

	public virtual void Init()
	{
		anim = GetComponentInChildren<Animator>();
		enemySprite = GetComponentInChildren<SpriteRenderer>();
	}

	public virtual void Start()
	{	
		Init();
		targetPosition = waypoints[0].position; 
	}

	public virtual void Update()
	{
		Movement();
	}

	public virtual void Movement()
	{

		MoveOnPath();

		if(targetPosition.x > transform.position.x)
		{
			Vector3 tempScale = transform.localScale;
			tempScale.x = 1;
			transform.localScale = tempScale;
		}
		if(targetPosition.x < transform.position.x)
		{
			Vector3 tempScale = transform.localScale;
			tempScale.x = -1;
			transform.localScale = tempScale;
		}


		transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}

	public virtual void MoveOnPath()
	{
		for (int i = 0; i < waypoints.Length; i++)
		{	
			if(transform.position == waypoints[waypoints.Length - 1].position)
			{
					targetPosition = waypoints[0].position;
					i = 0;
					break;
			}
				
			else if(transform.position == waypoints[i].position)
			{
				int nextPositionInArray = i + 1;
				targetPosition = waypoints[nextPositionInArray].position;
			}
		}
	}

}
