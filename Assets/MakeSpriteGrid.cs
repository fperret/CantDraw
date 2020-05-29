using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpriteGrid : MonoBehaviour
{
    public GameObject m_basicGroundTile;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < Constants.g_gridSizeLength; ++y)
        {
            for (int x = 0; x < Constants.g_gridSizeLength; ++x)
            {
                GameObject tile = (GameObject)Instantiate(m_basicGroundTile, transform);
                tile.transform.localPosition = new Vector2(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
