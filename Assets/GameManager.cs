using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager   Instance;

    [SerializeField]
    public UIManager m_uiManager;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void win()
    {
        Time.timeScale = 0;
        m_uiManager.screenWin();
    }
}
