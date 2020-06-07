using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoGoZone : MonoBehaviour
{
    private List<GameObject> m_zonesList = new List<GameObject>();

    void updateZoneColor(GameObject zone, bool valid)
    {
        if (valid)
            zone.GetComponent<SpriteRenderer>().color = new Color(0, 0.87f, 0.87f, 1);
        else
            zone.GetComponent<SpriteRenderer>().color = new Color(0.87f, 0, 0.87f, 1);
    }

    public bool checkNoGoZones()
    {
        // globalCheck is used to know if any point is invalid
        bool globalCheck = true;
        foreach (GameObject zone in m_zonesList)
        {
            Collider2D[] overlaps = Physics2D.OverlapPointAll(zone.transform.position);
            // zoneCheck is used for the color
            bool zoneCheck = true;
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.tag != "Ground")
                {
                    zoneCheck = false;
                    globalCheck = false;
                    updateZoneColor(zone, false);
                }
            }
            if (zoneCheck)
                updateZoneColor(zone, true);
        }

        return globalCheck;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            m_zonesList.Add(child.gameObject);
            if (Goal.m_hideSprite)
                child.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checkNoGoZones();
    }
}
