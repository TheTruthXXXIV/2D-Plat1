using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		print("Hi");
		if(other.gameObject.tag == "Player")
		{
			StartCoroutine(LoadNextLevel());
		}
	}

	IEnumerator LoadNextLevel()
	{
		yield return new WaitForSeconds(1f);
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex + 1);
	}

}
