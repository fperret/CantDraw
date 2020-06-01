using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    // Simulate the direction like a 3D object would have
    public Vector2 m_direction;

    public float m_footPrintsPeriod = 0.3f;
    public int m_maxNbFootPrints = 40;

    private float m_footPrintsTimer = 0.0f;
    private bool m_spawnFootPrint = true;
    private List<GameObject> m_activeFootPrints = new List<GameObject>();    


    public Transform m_footPrintsParent;
    public GameObject m_footPrintsVertical;
    public GameObject m_footPrintsHorizontal;

    [SerializeField]
    private Transform m_center;

    private GameObject m_groundTile = null;


    private OrientationManagement m_orientationManagement;

    // Start is called before the first frame update
    void Start()
    {
        m_orientationManagement = GetComponent<OrientationManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.E))
        //{
        if (m_direction.sqrMagnitude != 0)
        {
            if (m_spawnFootPrint)
            {
                if (m_orientationManagement.m_orientation == OrientationManagement.Orientation.UP || m_orientationManagement.m_orientation == OrientationManagement.Orientation.DOWN)
                    m_activeFootPrints.Add((GameObject)Instantiate(m_footPrintsVertical, m_center.position, Quaternion.identity, m_footPrintsParent));
                else if (m_orientationManagement.m_orientation == OrientationManagement.Orientation.LEFT || m_orientationManagement.m_orientation == OrientationManagement.Orientation.RIGHT)
                    m_activeFootPrints.Add((GameObject)Instantiate(m_footPrintsHorizontal, m_center.position, Quaternion.identity, m_footPrintsParent));
                m_spawnFootPrint = false;
            }
        //}
        m_footPrintsTimer += Time.deltaTime;
        if (m_footPrintsTimer >= 0.3f)
        {
            m_spawnFootPrint = true;
            m_footPrintsTimer = 0;
        }
        }

        // Optimize ?
        while (m_activeFootPrints.Count > m_maxNbFootPrints)
        {
            GameObject.Destroy(m_activeFootPrints[0]);
            m_activeFootPrints.RemoveAt(0);
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


