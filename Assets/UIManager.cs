using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject  m_successPanel;

    // Start is called before the first frame update
    void Start()
    {
        m_successPanel = transform.Find("Success Panel").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void screenWin()
    {
        m_successPanel.SetActive(true);
    }
}
