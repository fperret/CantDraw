using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float m_speed = 5.0f;

    private Vector2 m_movement;

    private Animator    m_animator;
    private Rigidbody2D m_rigidBody;

    private Player m_player;
    private OrientationManagement m_orientationManagement;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_player = GetComponent<Player>();
        m_orientationManagement = GetComponent<OrientationManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        m_movement.x = Input.GetAxisRaw("Horizontal");
        m_movement.y = Input.GetAxisRaw("Vertical");

        // Update the internal direction if there is a player movement input
        if (!(m_movement.x == 0 && m_movement.y == 0))
        {

            m_animator.SetFloat("Horizontal", m_movement.x);
            m_animator.SetFloat("Vertical", m_movement.y);

            m_player.direction(m_movement);

            m_orientationManagement.setOrientation(m_movement);
        }

    }

    private void FixedUpdate() {
        m_rigidBody.MovePosition(m_rigidBody.position + m_movement * m_speed * Time.fixedDeltaTime);    
    }
}
