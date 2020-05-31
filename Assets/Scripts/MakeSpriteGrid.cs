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

                GameObject tile2 = (GameObject)Instantiate(m_basicGroundTile, transform);
                tile2.transform.localPosition = new Vector2(x + 0.5f, y);

                GameObject tile3 = (GameObject)Instantiate(m_basicGroundTile, transform);
                tile3.transform.localPosition = new Vector2(x, y + 0.5f);

                GameObject tile4 = (GameObject)Instantiate(m_basicGroundTile, transform);
                tile4.transform.localPosition = new Vector2(x + 0.5f, y + 0.5f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
