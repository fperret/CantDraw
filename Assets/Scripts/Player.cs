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

    private GameObject m_currentTarget = null;
    private Color      m_saveTargetColor;

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

    // Update the game object of the new target (highlight, etc...)
    private void updateNewTargetGameObject(GameObject newCurrentTarget)
    {
        m_currentTarget = newCurrentTarget;
        m_saveTargetColor = m_currentTarget.GetComponent<SpriteRenderer>().color;

        Color newColor = m_saveTargetColor;
        newColor.r *= 1.3f;
        newColor.g *= 1.3f;
        newColor.b *= 1.3f;
        m_currentTarget.GetComponent<SpriteRenderer>().color = newColor;
    }

    // Restore the game object of the previous target as it was before targetting.
    // Should do the opposite of updateNewTargetGameObject()
    private void restorePreviousTargetGameObject()
    {
        m_currentTarget.GetComponent<SpriteRenderer>().color = m_saveTargetColor;
    }

    // Get a target from a raycast in front of the player
    private void target()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(m_center.position, m_direction, m_raycastLength);
        GameObject target = null;

        Debug.DrawLine(m_center.position, m_center.position + (new Vector3(m_direction.x, m_direction.y) * m_raycastLength), Color.magenta);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.tag == "Interactable")
            {
                // Colliders are sorted in order of distance so we can stop at the first one
                target = hit.collider.gameObject;
                break;
            }
        }

        // No object to target
        if (target == null)
        {
            // Restore the previous object we had as a target
            if (m_currentTarget != null)
                restorePreviousTargetGameObject();
            m_currentTarget = null;
        }

        // We have possibly an objet to target
        if (target != null)
        {
            // If no current target, use the new one
            if (m_currentTarget == null)
                updateNewTargetGameObject(target);
            // Check if the target is different from currentTarget
            else if (target.GetInstanceID() != m_currentTarget.GetInstanceID())
            {
                restorePreviousTargetGameObject();
                updateNewTargetGameObject(target);
            }
        }

    }

    private void FixedUpdate() {
        m_rigidBody.MovePosition(m_rigidBody.position + m_movement * m_speed * Time.fixedDeltaTime);    

        target();

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
        }        
    }

}


