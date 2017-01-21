using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour {

	public float m_walkSpeed;
	public float m_jumpVelocity;

	int m_walkDirection = 1;
	Rigidbody2D m_rb;
	bool m_touchDetected;
	Vector3 m_touchVector;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		m_rb.velocity = new Vector3( m_walkSpeed * m_walkDirection, m_rb.velocity.y);

//		if(Input.GetKeyDown(0)){
//			
//		}


		if(Input.GetMouseButtonDown(0)){
			 		

				m_touchVector = Input.mousePosition;
				//m_touchDetected = true;
				Debug.Log("press");

		}

		if(Input.GetMouseButtonUp(0)){
			
			Debug.Log("release");
			//Touch touch = Input.GetTouch(0);
			Vector3 diff = Input.mousePosition - m_touchVector;
			Debug.Log("magnitude: "+diff.magnitude+" , x: "+Mathf.Abs(diff.x)+", y: "+ Mathf.Abs(diff.y));
			if(Mathf.Abs(diff.x) > Mathf.Abs(diff.y) && diff.magnitude > 100){
				m_walkDirection = (int)(diff.x/Mathf.Abs(diff.x));
			}else{
				m_rb.velocity = new Vector3( m_rb.velocity.x, m_jumpVelocity);
			}
				//m_touchDetected = false;
			
		}

	}
}
