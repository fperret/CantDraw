using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpriteGrid : MonoBehaviour
{
    public GameObject m_basicGroundTile;
    public GameObject m_sandTilePimped;

    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < Constants.g_gridSizeLength; ++y)
        {
            for (int x = 0; x < Constants.g_gridSizeLength; ++x)
            {
                GameObject tileToUse = m_basicGroundTile;
                if (Random.Range(1, 10) > 7)
                    tileToUse = m_sandTilePimped;

                GameObject tile = (GameObject)Instantiate(tileToUse, transform);
                tile.transform.localPosition = new Vector2(x, y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
