﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {

	public void StartFirstLevel()
	{
		SceneManager.LoadScene(1);
	}

	public void QuitGame()
	{
		Debug.Log("Quit");
	}

}
