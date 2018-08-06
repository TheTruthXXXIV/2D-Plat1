using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectThrownAtPlayer : MonoBehaviour {

	public Rigidbody2D projectile;
	Transform player;

	public float h = 10f;
	public float gravity = -18f;

	bool hitPlayer;

	void Awake()
	{
		player = FindObjectOfType<Player>().transform;
	}

	void Start () 
	{	
		float playerHeight = player.position.y - projectile.position.y;
		float newH = (player.position.y - projectile.position.y) + 3f;
		if (playerHeight > h)
		{
			h = newH;
		}
		Launch();
	}

	void Update()
	{
		Destroy(this.gameObject, 3.5f);
	}

	void Launch () 
	{
		Physics2D.gravity = Vector2.up * gravity;
		projectile.velocity = CalculateVelocity();
	}

	Vector2 CalculateVelocity()
	{	
		float displacementY = player.position.y - projectile.position.y;
		float displacementX = player.position.x - projectile.position.x;

		float velocityY = Mathf.Sqrt(-2 * gravity * h);
		float velocityX = displacementX / (Mathf.Sqrt(-2 * h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity));

		Vector2 Velocity = new Vector2(velocityX, velocityY);
		return Velocity;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Player")
		{
			print("Hit Player");
		}
	}
}
