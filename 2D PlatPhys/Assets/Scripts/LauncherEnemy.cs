using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherEnemy : MonoBehaviour {

	public LayerMask playerLayer;
	public GameObject projectilePrefab;
	public float throwRange = 3f;
	public float timeBetweenThrows = 2f;
	bool playerInRange;
	bool firstTimeSeeingPlayer = true;

	public BoxCollider2D collider;
	Bounds bounds;

	float waitTime;


	void Start () 
	{
		waitTime = timeBetweenThrows;
		bounds = collider.bounds;
	}

	void Update () 	
	{
		playerInRange = Physics2D.OverlapCircle(transform.position, throwRange, playerLayer);

		if(playerInRange)
		{
			ThrowProjectile();
		}
		if(playerInRange && firstTimeSeeingPlayer)
		{
			
			CreateFirstProjectile();
			firstTimeSeeingPlayer = false;
		}

	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, throwRange);
	}

	void ThrowProjectile()
	{	
		Vector2 topOfLauncher = new Vector2(bounds.center.x, bounds.max.y);
		Vector3 throwSpot = new Vector3(topOfLauncher.x,topOfLauncher.y, transform.position.z);
		waitTime -= Time.deltaTime;
		if(waitTime <= 0)
		{
			GameObject projectile = Instantiate(projectilePrefab, throwSpot, Quaternion.identity) as GameObject;
			Vector3 projectileScale = new Vector3(.25f, .25f, .25f);
			projectile.transform.SetParent(GameObject.Find("ProjectilesHolder").transform);
			projectile.transform.localScale = projectileScale;
			waitTime = timeBetweenThrows;
		}
	}

	void CreateFirstProjectile()
	{
		Vector2 topOfLauncher = new Vector2(bounds.center.x, bounds.max.y);
		Vector3 throwSpot = new Vector3(topOfLauncher.x,topOfLauncher.y, transform.position.z);

		GameObject projectile = Instantiate(projectilePrefab, throwSpot, Quaternion.identity) as GameObject;
		Vector3 projectileScale = new Vector3(.25f, .25f, .25f);
		projectile.transform.SetParent(GameObject.Find("ProjectilesHolder").transform);
		projectile.transform.localScale = projectileScale; 
	}


}
