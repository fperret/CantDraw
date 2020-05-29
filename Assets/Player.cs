using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite m_darkerSand;

    private float m_speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.Translate(Vector2.left * Time.deltaTime * m_speed);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.Translate(Vector2.right * Time.deltaTime * m_speed);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            transform.Translate(Vector2.down * Time.deltaTime * m_speed);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            transform.Translate(Vector2.up * Time.deltaTime * m_speed);
        }
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
            //DebugUtils.debugColor(newColor);
            newColor.r *= 0.9f;
            newColor.g *= 0.9f;
            newColor.b *= 0.9f;
            other.GetComponent<SpriteRenderer>().color = newColor;
         //   other.GetComponent<SpriteRenderer>().sprite = m_darkerSand;
        }        
    }

}


