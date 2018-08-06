using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[HideInInspector] public Vector2 velocity;
	PlayerController2D controller;
	Animator anim;

	[SerializeField] float timeToJumpApex = .5f;
	[SerializeField] float maxJumpHeight = 4f;
	[SerializeField] float minJumpHeight = 1f;

	float gravity;
	public float maxJumpVelocity;
	float minJumpVelocity;
	[SerializeField] float moveSpeed = 6f;

	//bool canDoubleJump;
	public bool bouncedOnHead;

	void Start () 
	{
		controller = GetComponent<PlayerController2D>();
		anim = GetComponentInChildren<Animator>();
		CalculateGravityAndJumpVelocity();
	}

	void Update () 
	{
		StopAccumulatingGravity();
		Jumping();
		Moving();
		BounceOnEnemyHead();
	}

	void Jumping ()
	{
		if (Input.GetButtonDown ("Jump") && controller.collisions.below) 
		{
			velocity.y = 0;
			velocity.y = maxJumpVelocity;
			//print(maxJumpVelocity);
			//canDoubleJump = true;
		}
		if(Input.GetButtonUp("Jump"))
		{
			if(velocity.y > minJumpVelocity)
			{
				velocity.y = minJumpVelocity;
			}
		}
		/*else if (Input.GetButtonDown ("Jump") && !controller.collisions.below && canDoubleJump) 
		{
			velocity.y = 0;
			velocity.y = maxJumpVelocity;
			canDoubleJump = false;
		}*/
	}

	void Moving()
	{
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		velocity.y += gravity * Time.deltaTime;
		velocity.x = input.x * moveSpeed;
		FlipSprite(input);
		AnimateRunning(input);
		controller.Move(velocity * Time.deltaTime);
	}

	void StopAccumulatingGravity()
	{
		if(controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}
	}

	void CalculateGravityAndJumpVelocity()
	{
		gravity = -((maxJumpHeight * 2) / Mathf.Pow(timeToJumpApex, 2));
		maxJumpVelocity = Mathf.Abs(gravity) * (timeToJumpApex);
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
		//print(gravity + "," + maxJumpVelocity);
	}

	void FlipSprite(Vector2 input)
	{
		bool playerHasHorizontalSpeed = Mathf.Abs(input.x) > Mathf.Epsilon;
		if(playerHasHorizontalSpeed)
		{
			transform.localScale = new Vector2(Mathf.Sign(input.x), 1f);
		}
	}

	void AnimateRunning(Vector2 input)
	{
		bool playerHasHorizontalSpeed = Mathf.Abs(input.x) > Mathf.Epsilon;
		anim.SetBool("Running", playerHasHorizontalSpeed);
	}

	void BounceOnEnemyHead()
	{
		if(bouncedOnHead == true)
		{
			velocity.y = maxJumpVelocity;
		}
	}
}
