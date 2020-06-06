using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalSign : MonoBehaviour
{
    public GameObject  m_goalSpeechBubble;
    private GameObject m_goalModel;

    private SpriteRenderer  m_speechBubbleSpriteRenderer;
    private SpriteRenderer  m_modelSpriteRenderer;

    // These 3 booleans are used to :
    // - not trigger the fadeIn / fadeOut  multiple times at once
    // - bufferize a fadeIn / fadeOut if the player enters/exits the trigger while the other animation is running
    private bool m_coroutineRunning = false;
    private bool m_needToDisplay = false;
    private bool m_needToHide = false;

    // Start is called before the first frame update
    void Start()
    {
        m_goalSpeechBubble = transform.Find("GoalSpeechBubble").gameObject;
        m_goalModel = m_goalSpeechBubble.transform.Find("GoalModel").gameObject;

        m_speechBubbleSpriteRenderer = m_goalSpeechBubble.GetComponent<SpriteRenderer>();
        m_modelSpriteRenderer = m_goalModel.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_needToDisplay && !m_coroutineRunning)
        {
            m_needToDisplay = false;
            StartCoroutine("fadeIn");

        }
        else if (m_needToHide && !m_coroutineRunning)
        {
            m_needToHide = false;
            StartCoroutine("fadeOut");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other is CapsuleCollider2D)
        {
            m_needToDisplay = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && other is CapsuleCollider2D)
        {
            m_needToHide = true;
        }
    }

    // Make the goal speech bubble and model appear gradually
    private IEnumerator fadeIn()
    {
        m_coroutineRunning = true;

        Color startColor = m_speechBubbleSpriteRenderer.color;
        startColor.a = 0;

        m_speechBubbleSpriteRenderer.color = startColor;
        m_modelSpriteRenderer.color = startColor;

        m_goalSpeechBubble.SetActive(true);

        while (m_speechBubbleSpriteRenderer.color.a <= 1)
        {
            Color newColor = m_speechBubbleSpriteRenderer.color;
            newColor.a += 0.1f;

            m_speechBubbleSpriteRenderer.color = newColor;
            m_modelSpriteRenderer.color = newColor;

            yield return new WaitForSeconds(0.05f);
        }

        m_coroutineRunning = false;
    }

    // Make the goal speech bubble and model disappear gradually
    private IEnumerator fadeOut()
    {
        m_coroutineRunning = true;

        Color startColor = m_speechBubbleSpriteRenderer.color;
        startColor.a = 1;

        m_speechBubbleSpriteRenderer.color = startColor;
        m_modelSpriteRenderer.color = startColor;

        while (m_speechBubbleSpriteRenderer.color.a >= 0)
        {
            Color newColor = m_speechBubbleSpriteRenderer.color;
            newColor.a -= 0.1f;

            m_speechBubbleSpriteRenderer.color = newColor;
            m_modelSpriteRenderer.color = newColor;

            yield return new WaitForSeconds(0.05f);
        }
        m_goalSpeechBubble.SetActive(true);

        m_coroutineRunning = false;
    }

}
