using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public GameObject m_resourceToGive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void use(GameObject player)
    {
        GetComponent<Collider2D>().enabled = false;
        transform.parent = player.transform;
        transform.localPosition = player.GetComponent<Player>().center().localPosition + new Vector3(player.GetComponent<Player>().m_direction.x, player.GetComponent<Player>().m_direction.y);
    }

    public void drop()
    {
        transform.parent = null;
    }
}
