using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManagement : MonoBehaviour
{
    private enum Orientation {
        LEFT,
        UP,
        RIGHT,
        DOWN
    }

    private Orientation m_orientation;

    // We need to update the collider depending on the sprite used by the animator
    private BoxCollider2D   m_collider;

    private Transform       m_center;

    private readonly Vector2   m_colliderOffsetForLeft = new Vector2(0.7573f, 0.7505f);
    private readonly Vector2         m_colliderSizeForLeft = new Vector2(0.7613f, 0.6450f);

    private readonly Vector2         m_colliderOffsetForRight = new Vector2(0.8247f, 0.7505f);
    private readonly Vector2         m_colliderSizeForRight = new Vector2(0.7375f, 0.6450f);

    private readonly Vector2         m_colliderOffsetForUp = new Vector2(1.3083f, 0.7584f);
    private readonly Vector2         m_colliderSizeForUp = new Vector2(0.7217f, 0.7661f);

    private readonly Vector2         m_colliderOffsetForDown = new Vector2(1.4827f, 0.806f);
    private readonly Vector2         m_colliderSizeForDown = new Vector2(0.8168f, 0.7560f);


    private readonly Vector3         m_centerPositionForUp = new Vector3(1.3f, 0.5f, 0);
    private readonly Vector3         m_centerPositionForDown = new Vector3(1.43f, 0.55f, 0);

    private readonly Vector3         m_centerPositionForLeft = new Vector3(0.78f, 0.58f, 0);
    private readonly Vector3         m_centerPositionForRight = new Vector3(0.91f, 0.58f, 0);



    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider2D>();
        m_center = transform.Find("Center");
    }

    public void setOrientation(Vector2 movement)
    {
        if (movement.x > 0)
            m_orientation = Orientation.RIGHT;
        else if (movement.x < 0)
            m_orientation = Orientation.LEFT;
        else if (movement.y > 0)
            m_orientation = Orientation.UP;
        else
            m_orientation = Orientation.DOWN;

        updateCollider();
        updateCenter();
    }

    private void updateCollider()
    {
        switch (m_orientation)
        {
            case Orientation.RIGHT:
                m_collider.offset = m_colliderOffsetForRight;
                m_collider.size = m_colliderSizeForRight;
                break;

            case Orientation.LEFT:
                m_collider.offset = m_colliderOffsetForLeft;
                m_collider.size = m_colliderSizeForLeft;
                break;

            case Orientation.UP:
                m_collider.offset = m_colliderOffsetForUp;
                m_collider.size = m_colliderSizeForUp;
                break;

            case Orientation.DOWN:
                m_collider.offset = m_colliderOffsetForDown;
                m_collider.size = m_colliderSizeForDown;
                break;
        }
    }

    private void updateCenter()
    {
        switch (m_orientation)
        {
            case Orientation.RIGHT:
                m_center.localPosition = m_centerPositionForRight;
                break;

            case Orientation.LEFT:
                m_center.localPosition = m_centerPositionForLeft;
                break;

            case Orientation.UP:
                m_center.localPosition = m_centerPositionForUp;
                break;

            case Orientation.DOWN:
                m_center.localPosition = m_centerPositionForDown;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
