using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour {

	public PlayerMoveControl m_player;
	public Image m_life;


	public void setPLayer(PlayerMoveControl player){
		m_player = player;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(transform.childCount == m_player.m_currentHealth)
			return;

		if(transform.childCount > m_player.m_currentHealth && transform.childCount>0){
			Debug.Log("removing a child");
			Destroy(transform.GetChild(0).gameObject);
		}

		while(transform.childCount < m_player.m_currentHealth){
			RectTransform life = Instantiate(m_life).rectTransform;
			life.SetParent(transform);
			//life.localPosition = new Vector3(0,0,0);

		}
	}

}
