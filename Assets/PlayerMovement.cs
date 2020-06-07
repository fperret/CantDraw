using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float m_speed = 5.0f;

    public Vector2 m_movement;

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
        if ((m_movement.x = Input.GetAxisRaw("Horizontal")) != 0)
        {
            m_movement.y = 0;
        }
        else
        {
            m_movement.y = Input.GetAxisRaw("Vertical");
        }

        // Update the internal direction if there is a player movement input
        if (m_movement != Vector2.zero)
        {
            m_animator.SetFloat("Horizontal", m_movement.x);
            m_animator.SetFloat("Vertical", m_movement.y);

            m_orientationManagement.setOrientation(m_movement);
            m_player.direction(m_movement);
        }
    }

    void FixedUpdate() {
        Vector2 oldLocation = m_rigidBody.position;
        Vector2 moveVector = m_movement * m_speed * Time.fixedDeltaTime;

        oldLocation = PixelPerfectClamp(oldLocation, 8);
        moveVector = PixelPerfectClamp(moveVector, 8);
        m_rigidBody.MovePosition(oldLocation + moveVector);
    }

    private Vector2 PixelPerfectClamp(Vector2 moveVector, float pixelsPerUnit)
    {
        Vector2 vectorInPixels = new Vector2(Mathf.RoundToInt(moveVector.x * pixelsPerUnit), Mathf.RoundToInt(moveVector.y * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}
