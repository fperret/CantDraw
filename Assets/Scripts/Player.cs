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

    public Transform m_center;

    //private Direction m_direction = Direction.BOTTOM;

    private float m_speed = 3.0f;
    private float m_raycastLength = 3.0f;

    private Vector2 m_movement;
    // Simulate the direction like a 3D object would have
    private Vector2 m_direction;
    private Rigidbody2D m_rigidBody;
    private Animator    m_animator;


    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_movement.x = Input.GetAxisRaw("Horizontal");
        m_movement.y = Input.GetAxisRaw("Vertical");

        m_animator.SetFloat("Horizontal", m_movement.x);
        m_animator.SetFloat("Vertical", m_movement.y);

        // Update the internal direction if there is a player movement input
        if (!(m_movement.x == 0 && m_movement.y == 0))
        {
            m_direction = m_movement;
        }
    }

    private void FixedUpdate() {
        m_rigidBody.MovePosition(m_rigidBody.position + m_movement * m_speed * Time.fixedDeltaTime);    

        RaycastHit2D hit = Physics2D.Raycast(m_center.position, m_direction, m_raycastLength);
        Debug.DrawLine(m_center.position, m_center.position + (new Vector3(m_direction.x, m_direction.y) * m_raycastLength), Color.magenta);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Collider2D[] myColliders = new Collider2D[2];
        int nbColliders = other.GetContacts(myColliders);
        bool feetCollider = false;
        for (int i = 0; i < nbColliders; ++i)
        {
            Debug.Log(myColliders[i].name);
            if (myColliders[i].name == "Feet")
                feetCollider = true;
        }
        if (feetCollider)
        {
            Color newColor = other.GetComponent<SpriteRenderer>().color;
            newColor.r *= 0.9f;
            newColor.g *= 0.9f;
            newColor.b *= 0.9f;
            other.GetComponent<SpriteRenderer>().color = newColor;
         //   other.GetComponent<SpriteRenderer>().sprite = m_darkerSand;
        }        
    }

}


