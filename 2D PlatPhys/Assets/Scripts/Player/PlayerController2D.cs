using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour {
	[SerializeField] LayerMask collisionMask;
	[SerializeField] int verticalRayCount = 4;
	[SerializeField] int horizontalRayCount = 4;
	float verticalRaySpacing;
	float horizontalRaySpacing;

	const float skinWidth = .015f;

	public BoxCollider2D collider;
	RayCastOrigins rayCastOrigins;
	public CollisionInfo collisions;
	[HideInInspector] public Vector2 playerInput;

	void Awake () 
	{
		collider = GetComponent<BoxCollider2D>();
		CalculateRaySpacing();
	}

	public void Move(Vector2 velocity)
	{
		UpdateRaycastOrigins();
		collisions.Reset();

		VerticalCollisions(ref velocity); 
		HorizontalCollisions(ref velocity);
		if(!collisions.right || !collisions.left || !collisions.below || !collisions.above && velocity.x != 0 && velocity.y != 0)
		{
			DiagonalCollisions(ref velocity); 
		}


		transform.Translate(velocity);
	}

	void DiagonalCollisions(ref Vector2 velocity)
	{
		bool isGoingUp = velocity.y > 0;
		bool isGoingRight = velocity.x > 0;

		Vector2 rayOrigin = rayCastOrigins.topRight;
		if(isGoingRight && isGoingUp)
		{
			rayOrigin = rayCastOrigins.topRight;
		}
		else if(!isGoingRight && !isGoingUp)
		{
			rayOrigin = rayCastOrigins.bottomLeft;
		}
		else if(!isGoingRight && isGoingUp)
		{
			rayOrigin = rayCastOrigins.topLeft;
		}
		else if(isGoingRight && !isGoingUp)
		{
			rayOrigin = rayCastOrigins.bottomRight;
		}
		RaycastHit2D hit = Physics2D.Raycast(rayOrigin, velocity, velocity.magnitude, collisionMask);
		Debug.DrawRay(rayOrigin, velocity, Color.red); 
		if(hit)
		{

			float xSkinAdjustment = isGoingRight ? -skinWidth : skinWidth;
			float ySkinAdjustment = isGoingUp ? -skinWidth : skinWidth;

			velocity.y = hit.point.y - rayOrigin.x + ySkinAdjustment;
			velocity.x = hit.point.x - rayOrigin.y + xSkinAdjustment;
		}

	}

	void VerticalCollisions(ref Vector2 velocity)
	{
		float directionY = Mathf.Sign(velocity.y);
		float rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++) 
		{
			Vector2 rayOrigin = (directionY == -1)? rayCastOrigins.bottomLeft:rayCastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i); //+ velocity.x
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if(hit)
			{
				velocity.y = (hit.distance- skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	void HorizontalCollisions(ref Vector2 velocity)
	{
		float directionX = Mathf.Sign(velocity.x);
		float rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) 
		{
			Vector2 rayOrigin = (directionX == -1)? rayCastOrigins.bottomLeft:rayCastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

			if(hit)
			{
				velocity.x = (hit.distance- skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}

			//if(hit.
		}
	}

	void UpdateRaycastOrigins()
	{
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		rayCastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		rayCastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		rayCastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		rayCastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	}

	void CalculateRaySpacing()
	{	
		Bounds bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

		verticalRayCount = Mathf.Clamp(verticalRayCount, 4, int.MaxValue);
		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 4, int.MaxValue);

		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
	}

	struct RayCastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;

		public void Reset()
		{
			above = below = false;
			left = right = false;
		}
	}
}
