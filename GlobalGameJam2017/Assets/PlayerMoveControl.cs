using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour {

	public float m_walkSpeed;
	public float m_jumpVelocity;
	public float m_damageBounceVelocity;
	public Transform m_groundCheck;
	public float m_width;
	public float m_knockbackTime;
	public float m_invulnerableTime;

	public int m_walkDirection { get; private set; }// = 1;
	Rigidbody2D m_rb;
	bool m_touchDetected;
	bool m_grounded;
	bool m_damaged;
	Vector3 m_touchVector;
	float m_knockbackTimeRemaining = 0;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody2D>();
		m_walkDirection = 1;
	}
	
	// Update is called once per frame
	void Update () {

		Collider2D groundCheck = Physics2D.OverlapBox(m_groundCheck.position, new Vector2(m_width*2, 0.0001f), 0.0f);

		m_grounded = groundCheck != null && groundCheck.tag == "Platform";


		if(m_damaged){
			m_knockbackTimeRemaining -= Time.deltaTime;
			if(m_knockbackTimeRemaining <= 0){
				m_damaged = false;
			}
		}else{

			m_rb.velocity = new Vector3( m_walkSpeed * m_walkDirection, m_rb.velocity.y);

			if(Input.GetMouseButtonDown(0)){

					m_touchVector = Input.mousePosition;
			}

			if(Input.GetMouseButtonUp(0)){
				

				Vector3 diff = Input.mousePosition - m_touchVector;

				if(Mathf.Abs(diff.x) > Mathf.Abs(diff.y) && diff.magnitude > 100){
					m_walkDirection = (int)(diff.x/Mathf.Abs(diff.x));
				}else{
					if(m_grounded){
						m_rb.velocity = new Vector3( m_rb.velocity.x, m_jumpVelocity);
					}
				}
			}
		}

	}

	void OnCollisionEnter2D(Collision2D col){
		Debug.Log("Hit "+col.collider.tag);
		if(col.collider.tag == "Damaging"){
			Debug.Log("Harmed");
			m_damaged = true;
			m_knockbackTimeRemaining = m_knockbackTime;
			m_rb.velocity = new Vector2((m_damageBounceVelocity * m_walkDirection * (m_grounded ? -1 : 1)), m_damageBounceVelocity);	
		}
	}
}
