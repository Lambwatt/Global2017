using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public CameraControl m_mainCamera;
	public PlayerMoveControl m_player;
	public Vector3 m_checkPoint;
	public WaveControl m_waveControl;
	public HealthDisplay m_healthDisplay;

	int m_checkPointNumber;

	// Use this for initialization
	void Start () {
		m_checkPointNumber = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void setCheckPoint(Checkpoint c){
		if(c.m_checkPointNumber > m_checkPointNumber){
			m_checkPoint = c.transform.position;
			m_waveControl.setWaterMarks(c.m_highWater.transform.position.y, c.m_lowWater.transform.position.y);
		}

	}

	public void respawnPlayer(){
		PlayerMoveControl p = Instantiate(m_player, m_checkPoint, Quaternion.identity);
		m_mainCamera.setNewPlayer(p);
		m_waveControl.setPlayer(p);
		m_healthDisplay.setPLayer(p);
	}
}
