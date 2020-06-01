using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSign : MonoBehaviour
{
    public GameObject  m_goalSpeechBubble;

    // Start is called before the first frame update
    void Start()
    {
        m_goalSpeechBubble = transform.Find("GoalSpeechBubble").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_goalSpeechBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            m_goalSpeechBubble.SetActive(false);
        }
    }
}
