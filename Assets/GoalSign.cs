using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSign : MonoBehaviour
{
    public GameObject  m_goalSpeechBubble;
    private GameObject m_goalModel;

    // Start is called before the first frame update
    void Start()
    {
        m_goalSpeechBubble = transform.Find("GoalSpeechBubble").gameObject;
        m_goalModel = m_goalSpeechBubble.transform.Find("GoalModel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Enter sign zone");
            m_goalSpeechBubble.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //GetComponent<Animation>().Play();
            GetComponent<Animator>().Play("GoalFade");
            //m_goalSpeechBubble.GetComponent<Animator>().Play("GoalFade");
            //m_goalModel.GetComponent<Animator>().Play("Goal");
            //m_goalSpeechBubble.SetActive(false);
        }
    }
}
