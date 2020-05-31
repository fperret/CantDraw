using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum Direction {
        LEFT,
        UP,
        RIGHT,
        BOTTOM
    }

    // Simulate the direction like a 3D object would have
    public Vector2 m_direction;

    public Sprite m_sandDeep;
    private float m_sandMarkTimer = 0.0f;
    private bool m_sandMarkOK = true;

    public GameObject m_sandMark;

    [SerializeField]
    private Transform m_center;

    private GameObject m_groundTile = null;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            if (m_sandMarkOK)
            {
                GameObject sandMark = (GameObject)Instantiate(m_sandMark, transform.position, Quaternion.identity);
                m_sandMarkOK = false;
            }
        }
        m_sandMarkTimer += Time.deltaTime;
        if (m_sandMarkTimer >= 0.2f)
        {
            m_sandMarkOK = true;
            m_sandMarkTimer = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //other.
        if (other.tag == "Ground")
        {
            m_groundTile = other.gameObject;

            Color newColor = other.GetComponent<SpriteRenderer>().color;
            newColor.r *= 0.9f;
            newColor.g *= 0.9f;
            newColor.b *= 0.9f;
            //other.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    public void direction(Vector2 val) {
        m_direction = val;
    }

    public Vector2 direction() {
        return m_direction;
    }

    public Transform center() {
        return m_center;
    }

}


