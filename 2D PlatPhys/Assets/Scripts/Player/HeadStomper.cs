using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadStomper : MonoBehaviour {

	Player player;

	void Start()
	{	
		player = transform.parent.GetComponent<Player>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Enemy" || other.tag == "Bat")
		{
			StartCoroutine(BounceOnEnemy(other));
		}
	}

	IEnumerator BounceOnEnemy(Collider2D other)
	{
		player.bouncedOnHead = true;
		Destroy(other.gameObject);
		yield return new WaitForSeconds(0f);
		player.bouncedOnHead = false;
	}

}
