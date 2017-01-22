using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveControl : MonoBehaviour {

	public float m_walkSpeed;
	public float m_jumpVelocity;
	public float m_damageBounceVelocity;
	public Transform m_groundCheck;
	public Transform m_leftWallCheck;
	public Transform m_rightWallCheck;
	//public Transform m_ceilingCheck;
	public float m_width;
	public float m_height;
	public float m_knockbackTime;
	public float m_invulnerableTime;
	public Text m_debugText;

	public float m_airGravity;
	public float m_waterGravity;
	public float m_risingWaterGravity;
	public float m_fallingWaterGravity;

	public int m_walkDirection { get; private set; }// = 1;
	Rigidbody2D m_rb;
	bool m_touchDetected;
	bool m_grounded;
	bool m_cielinged;
	bool m_damaged;
	bool m_invulnerable = false;
	Vector3 m_touchVector;
	float m_knockbackTimeRemaining = 0;
	float m_invulnerabilityTimeRemaining = 0;
	Animator m_animator;
	Transform m_waveTransform;
	WaveControl m_waveControl;

	Collider2D m_groundCollider;
	Collider2D m_leftCollider;
	Collider2D m_rightCollider;

	List<Collider2D> m_allPlatforms;

	const float ALMOST_NOTHING = 0.0001f;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody2D>();
		m_animator = GetComponent<Animator>();
		m_walkDirection = 1;
		m_waveControl = FindObjectOfType<WaveControl>();
		m_waveTransform = m_waveControl.GetComponent<Transform>();
		//m_rb.gravityScale = m_airGravity;



		Collider2D col = GetComponent<Collider2D>();
		m_width = col.bounds.extents.x - 10*ALMOST_NOTHING;
		m_height = col.bounds.extents.y - 0.5f;

		m_groundCollider = m_groundCheck.GetComponent<Collider2D>();
		m_leftCollider = m_groundCheck.GetComponent<Collider2D>();
		m_rightCollider = m_groundCheck.GetComponent<Collider2D>();


		GameObject[] allPlatforms = GameObject.FindGameObjectsWithTag("Platform");

		m_allPlatforms = new List<Collider2D>();
		foreach(GameObject p in allPlatforms){
			m_allPlatforms.Add(p.GetComponent<Collider2D>());
		}

	}

	// Update is called once per frame
	void Update () {

		if(m_waveTransform.position.y > transform.position.y){
			//Underwater
			if(m_waveControl.m_waveState == WaveControl.WaveState.Rising){
				m_rb.gravityScale = m_risingWaterGravity;
			}else if(m_waveControl.m_waveState == WaveControl.WaveState.Falling){
				m_rb.gravityScale = m_fallingWaterGravity;
			}else{
				m_rb.gravityScale = m_waterGravity;
			}
		}else{
			//in air
			m_rb.gravityScale = m_airGravity;
		}

		//Physics2D.OverlapArea(new Vector2(1, 0), new Vector2(0, 1));

//		Collider2D groundCheck = Physics2D.OverlapBox(m_groundCheck.position, new Vector2(m_width*2, 0.0001f), 0.0f);
//
//		m_grounded = groundCheck != null && groundCheck.tag == "Platform";
		bool groundedBefore = m_grounded;
		m_grounded = checkForGround();

		//m_debugText.text = "Grounded: "+m_grounded;

		if(groundedBefore == m_grounded){
			if(checkForWalls()){
				m_walkDirection = -m_walkDirection;
			}
		}else{
			if(m_grounded)
				m_animator.SetTrigger("Landed");
		}
//
		if(m_invulnerable){
			m_invulnerabilityTimeRemaining -= Time.deltaTime;
			if(m_invulnerabilityTimeRemaining <= 0){
				m_invulnerable = false;
			}
		}

		if(m_damaged){
			m_knockbackTimeRemaining -= Time.deltaTime;
			if(m_knockbackTimeRemaining <= 0){
				m_damaged = false;
			}
		}else{

			m_rb.velocity = new Vector3( m_walkSpeed * m_walkDirection, m_rb.velocity.y);

			if(Input.GetMouseButtonDown(0)){
				//Add processing to react to down press
					m_touchVector = Input.mousePosition;
			}

			if(Input.GetMouseButtonUp(0)){
				

				Vector3 diff = Input.mousePosition - m_touchVector;

				if(Mathf.Abs(diff.x) > Mathf.Abs(diff.y) && diff.magnitude > 100){
					m_walkDirection = (int)(diff.x/Mathf.Abs(diff.x));
				}else{
					if(m_grounded){
						m_rb.velocity = new Vector3( m_rb.velocity.x, m_jumpVelocity);
						m_animator.SetTrigger("Jumped");
					}
				}
			}
		}

	}

//	void OnCollisionExit2D(Collision2D col){
//		if(checkForWalls()){
//			m_walkDirection = -m_walkDirection;
//		}
//	}

//	bool checkForGround(){
//		//string allCollidersChecked = "";
//		foreach(Collider2D c in m_allPlatforms){
//			//allCollidersChecked+=c.name;
//			if(m_groundCollider.IsTouching(c)){
//				//Debug.Log(allCollidersChecked);
//				return true;
//			}
//		}
//		//Debug.Log(allCollidersChecked);
//		return false;
//		//return m_groundCheck.GetComponent<Collider2D>().IsTouching();
//	}

//	bool checkForWalls(){
//
////		foreach(Collider2D c in m_allPlatforms){
////			if(m_leftCollider.IsTouching(c) || m_rightCollider.IsTouching(c))
////				return true;
////		}
//		return false;
////		return m_leftWallCheck.GetComponent<Collider2D>().IsTouching()
////			|| m_rightWallCheck.GetComponent<Collider2D>().IsTouching();
//	}

	bool checkForWalls(){
//		Debug.DrawLine(new Vector2(m_leftWallCheck.position.x - ALMOST_NOTHING, m_leftWallCheck.position.y - m_height), new Vector2(m_leftWallCheck.position.x + ALMOST_NOTHING, m_leftWallCheck.position.y + m_height));
//		Debug.DrawLine(new Vector2(m_rightWallCheck.position.x - ALMOST_NOTHING, m_rightWallCheck.position.y - m_height), new Vector2(m_rightWallCheck.position.x + ALMOST_NOTHING, m_rightWallCheck.position.y + m_height));
		Collider2D leftCheck = Physics2D.OverlapArea(
			new Vector2(m_leftWallCheck.position.x - ALMOST_NOTHING, m_leftWallCheck.position.y - m_height), new Vector2(m_leftWallCheck.position.x + ALMOST_NOTHING, m_leftWallCheck.position.y + m_height));
		Collider2D rightCheck = Physics2D.OverlapArea(
			new Vector2(m_rightWallCheck.position.x - ALMOST_NOTHING, m_rightWallCheck.position.y - m_height), new Vector2(m_rightWallCheck.position.x + ALMOST_NOTHING, m_rightWallCheck.position.y + m_height));
		return (leftCheck != null && leftCheck.tag == "Platform") || (rightCheck != null && rightCheck.tag == "Platform") ;
	}

	bool checkForGround(){
		m_debugText.text = "started ground check";
		Collider2D groundCheck = Physics2D.OverlapArea(
			new Vector2(m_groundCheck.position.x - m_width, m_groundCheck.position.y - 0.0001f), 
			new Vector2(m_groundCheck.position.x + m_width , m_groundCheck.position.y + 0.0001f));// = Physics2D.OverlapBox(m_groundCheck.position, new Vector2(m_width*2, 0.0001f), 0.0f);

		 //= "groundCheck != null: "+(groundCheck != null)+", groundCheck.tag == \"Platform\":"+(groundCheck.tag == "Platform");
		if(groundCheck != null){
			//m_debugText.text = ""+groundCheck.gameObject.tag;
			return groundCheck.CompareTag("Platform");//groundCheck.tag == "Platform";
		}
		return false; 
	}

	void checkForDamage(Collision2D col){
		if(col.collider.tag == "Damaging" && m_invulnerable == false){
			m_animator.SetTrigger("Damaged");
			m_damaged = true;
			m_knockbackTimeRemaining = m_knockbackTime;
			m_rb.velocity = new Vector2((m_damageBounceVelocity * m_walkDirection * (m_grounded ? -1 : 1)), m_damageBounceVelocity);	
			m_invulnerable = true;
			m_invulnerabilityTimeRemaining = m_invulnerableTime;
		}
	}

	void OnCollisionEnter2D(Collision2D col){

		checkForDamage(col);
	}


	void OnCollisionStay2D(Collision2D col){
		checkForDamage(col);
	}

	public static Collider2D OverlapBox2D_2(Vector2 min, Vector2 max) {
		Bounds bounds = new Bounds(new Vector3(min.x+(max.x - min.x)/2, min.y+(max.y - min.y)/2), new Vector3(max.x - min.x, max.y - min.y));
		Vector2 center = bounds.center;
		Vector2 onePoint = new Vector2(center.x + bounds.size.x / 2, center.y + bounds.size.y / 2);
		float radius = Vector2.Distance(center, onePoint);

		List<Collider2D> inBox = new List<Collider2D>();
		Debug.DrawLine(bounds.min, bounds.max);
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
		foreach (Collider2D col in hitColliders) {

			if (bounds.Intersects(col.bounds)) {
				// Inside box
				//inBox.Add(col);
				return col;
			}
		}

		return null;
	}

}
