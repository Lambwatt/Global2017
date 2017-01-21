using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public PlayerMoveControl m_player;
	public Transform m_target;
	public float m_margin;

	public float m_cameraHeight;

	// Use this for initialization
	void Start () {
		//m_source = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.position = Vector3.Lerp(transform.position, m_target.position + new Vector3(m_margin * m_player.m_walkDirection, m_cameraHeight), Time.deltaTime*2);
		transform.position += new Vector3(0,0,-10);
	}
}
