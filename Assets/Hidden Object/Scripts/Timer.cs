using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text m_TimerText;
    [Range(0, 500)] public float m_ReverceTimeInSeconds = 150.0f;
	
	void Update ()
    {
        if (m_ReverceTimeInSeconds > 0)
        {
            m_TimerText.text = Mathf.Floor(m_ReverceTimeInSeconds / 60).ToString() + ':' + (m_ReverceTimeInSeconds % 60);
            m_ReverceTimeInSeconds -= Time.deltaTime;
        }
        else
        {
            GameManager.Inctance.m_Status = GameManager.Status.TimeUp;
            GameManager.Inctance.CheckStatus();

            m_ReverceTimeInSeconds = 0;
            m_TimerText.text = "0:0";
            enabled = false;
        }
	}
}