using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControl : MonoBehaviour {

	public enum WaveState{
		Out,
		Rising,
		In,
		Falling
	}
	public const int zVal = -2;
	public Transform m_player;
	public float m_highTarget;
	public float m_lowTarget;
	public float m_waveTime;

	public WaveState m_waveState {get; private set;}
	float m_waveTimeRemaining;

	public void setPlayer(PlayerMoveControl p){
		m_player = p.transform;
	}

	// Use this for initialization
	void Start () {
		m_waveState = WaveState.Out;
		m_waveTimeRemaining = 0;
	}

	public void setWaterMarks(float high, float low){
		m_highTarget = high;
		m_lowTarget = low;
	}
	
	// Update is called once per frame
	void Update () {
		updateTime();
		switch(m_waveState)
		{
		case WaveState.Out:
			if(timeUp()){
				resetTimer();
				m_waveState = WaveState.Rising;
			}
			break;
		case WaveState.Rising:
			transform.position = Vector3.Lerp(new Vector3(transform.position.x, m_lowTarget, zVal),new Vector3(transform.position.x, m_highTarget, zVal), m_waveTimeRemaining/ m_waveTime );
			if(timeUp()){
				resetTimer();
				m_waveState = WaveState.In;
			}
			break;
		case WaveState.In:
			if(timeUp()){
				resetTimer();
				m_waveState = WaveState.Falling;
			}
			break;
		case WaveState.Falling:
			transform.position = Vector3.Lerp(new Vector3(transform.position.x, m_highTarget, zVal),new Vector3(transform.position.x, m_lowTarget, zVal), m_waveTimeRemaining/ m_waveTime );
			if(timeUp()){
				resetTimer();
				m_waveState = WaveState.Out;
			}
			break;
		default:
			break;
		}
		transform.position = new Vector3(m_player.position.x, transform.position.y, zVal);
	}

	void updateTime(){
		m_waveTimeRemaining += Time.deltaTime;
	}

	bool timeUp(){
		return m_waveTimeRemaining >= m_waveTime;
	}

	void resetTimer(float time = 0){
		m_waveTimeRemaining = 0;
	}
}
