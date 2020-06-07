using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject m_resourceToGive;

    public bool        m_isInteractable = true;
    private bool         m_isValidated = false;

    [SerializeField]
    private string m_name;

    private SpriteRenderer m_spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void use(GameObject player)
    {
        if (m_isInteractable)
        {
            GetComponent<Collider2D>().enabled = false;
            transform.parent = player.transform;
            transform.localPosition = player.GetComponent<Player>().center().localPosition + new Vector3(player.GetComponent<Player>().m_direction.x, player.GetComponent<Player>().m_direction.y);
        }
    }

    public void drop()
    {
        GetComponent<Collider2D>().enabled = true;
        transform.parent = null;
    }

    [ExposeMethodInEditor]
    public void validatePosition()
    {
        if (!m_isValidated)
        {
            m_isValidated = true;
            m_isInteractable = false;
            StartCoroutine("validateAnimation");
        }
    }

    public string getName()
    {
        return m_name;
    }

    private IEnumerator validateAnimation()
    {
        Color saveColor = m_spriteRenderer.color;
        Color newColor = m_spriteRenderer.color;

        // Darken the sprite for a little while
        newColor.r *= 0.4f;
        newColor.g *= 0.4f;
        newColor.b *= 0.4f;
        m_spriteRenderer.color = newColor;
        yield return new WaitForSeconds(0.3f);

        // Restore the original sprite color
        m_spriteRenderer.color = saveColor;
        GetComponent<ParticleSystem>().Play();

        Camera.main.GetComponent<AudioSource>().Play();
    }
}
