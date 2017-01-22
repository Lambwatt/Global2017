using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int m_checkPointNumber;
	public Transform m_highWater;
	public Transform m_lowWater;
	public Sprite activated;

	public void OnTriggerEnter2D(){
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		r.sprite = activated;
		//change sprite state.
	}

//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
