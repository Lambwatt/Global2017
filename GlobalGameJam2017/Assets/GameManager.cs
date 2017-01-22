using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public CameraControl m_mainCamera;
	public PlayerMoveControl m_player;
	public Vector3 m_checkPoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void setCheckPoint(Vector3 checkPoint){
		m_checkPoint = checkPoint;
	}

	public void respawnPlayer(){
		PlayerMoveControl p = Instantiate(m_player, m_checkPoint, Quaternion.identity);
		m_mainCamera.setNewPlayer(p);
	}
}
