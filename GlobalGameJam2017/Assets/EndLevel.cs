using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour {

	void OnTriggerEnter2D(){
		SceneManager.LoadScene("End");
	}
}
