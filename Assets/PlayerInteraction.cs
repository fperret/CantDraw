using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject m_currentTarget = null;
    private Color      m_saveTargetColor;

    private Player      m_player;

    private float m_raycastLength = 2.0f;

    public GameObject m_holdingObject = null;

    public MakeSpriteGrid   m_gridManager;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // TODO RMEOVE
            if (true || m_holdingObject.name == "Shovel")
            {
                //if (m_currentTarget != null)
                    dig();
            }
            else
            {
                if (m_holdingObject != null && m_holdingObject.GetComponent<Interactable>() != null)
                {
                    if (m_holdingObject.GetComponent<Interactable>().drop())
                    {
                        m_holdingObject = null;
                    }
                }

                if (m_currentTarget != null && m_currentTarget.GetComponent<Interactable>() != null)
                {
                    if (m_currentTarget.GetComponent<Interactable>().use(gameObject))
                    {
                        m_holdingObject = m_currentTarget;
                        GetComponent<OrientationManagement>().updateObjectHeld();
                    }
                }
            }
        }
    }

    // Update the game object of the new target (highlight, etc...)
    private void updateNewTargetGameObject(GameObject newCurrentTarget)
    {
        m_currentTarget = newCurrentTarget;
        m_saveTargetColor = m_currentTarget.GetComponent<SpriteRenderer>().color;

        Color newColor = m_saveTargetColor;
        newColor.r *= 1.5f;
        newColor.g *= 1.5f;
        newColor.b *= 1.5f;
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
        RaycastHit2D[] hits = Physics2D.RaycastAll(m_player.center().position, m_player.direction(), m_raycastLength);
        GameObject target = null;

        Debug.DrawLine(m_player.center().position, m_player.center().position + (new Vector3(m_player.direction().x, m_player.direction().y) * m_raycastLength), Color.magenta);
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

    private void dig()
    {
        GameObject tile = m_gridManager.getTileAtPos((int)transform.position.x, (int)transform.position.y);
        /*if (tile != null)
        {
            if (tile.tag == "Ground")
                m_gridManager.replaceTileAtPos((int)transform.position.x, (int)transform.position.y, m_waterTiles[(int)WaterTiles.WATER_SAND_ALL]);
            else if (tile.tag == "Water")
                m_gridManager.replaceTileAtPos((int)transform.position.x, (int)transform.position.y, m_waterTiles[(int)WaterTiles.WATER_SAND_ALL]);

            Debug.Log(tile.transform.position);
        }*/
        m_gridManager.switchWaterAndSandAtPos((int)transform.position.x, (int)transform.position.y);
        // Do a switch ?
    }

    // This one should be done once PlayerMovement() is done
    private void FixedUpdate() {
        target();
    }
}
