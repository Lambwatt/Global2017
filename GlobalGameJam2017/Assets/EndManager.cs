using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

	public void EndGame(){
		SceneManager.LoadScene("Start");
	}
}