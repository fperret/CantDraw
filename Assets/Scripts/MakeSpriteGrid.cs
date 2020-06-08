using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSpriteGrid : MonoBehaviour
{
    private enum WaterTiles {
        WATER_SAND_ALL = 0,
        WATER_SAND_BOTTOM,
        WATER_SAND_LEFT,
        WATER_SAND_TOP,
        WATER_SAND_RIGHT,
        WATER_SAND_BOTTOM_LEFT,
        WATER_SAND_TOP_LEFT,
        WATER_SAND_TOP_RIGHT,
        WATER_SAND_BOTTOM_RIGHT,
        WATER_SAND_BOTTOM_TOP,
        WATER_SAND_LEFT_RIGHT,
        WATER_SAND_BOTTOM_TOP_LEFT,
        WATER_SAND_TOP_LEFT_RIGHT,
        WATER_SAND_BOTTOM_TOP_RIGHT,
        WATER_SAND_BOTTOM_LEFT_RIGHT,
        WATER_SAND_NONE
    }

    private GameObject[,] m_grid = new GameObject[Constants.g_gridSizeLength, Constants.g_gridSizeLength];

    public GameObject m_basicGroundTile;
    public GameObject m_sandTilePimped;

    public GameObject[] m_waterTiles = new GameObject[14];

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
                m_grid[y, x] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getTileAtPos(int x, int y)
    {
        if (x >= 0 && y >= 0)
        {
            if (x < Constants.g_gridSizeLength && y < Constants.g_gridSizeLength)
            {
                return m_grid[y, x];
            }
        }
        return null;
    }

    public bool replaceTileAtPos(int x, int y, GameObject newTile)
    {
        if (x >= 0 && y >= 0)
        {
            if (x < Constants.g_gridSizeLength && y < Constants.g_gridSizeLength)
            {
                Destroy(m_grid[y, x]);
                GameObject tile = (GameObject)Instantiate(newTile, transform);
                tile.transform.localPosition = new Vector2(x, y);
                m_grid[y, x] = tile;
                return true;
            }
        }
        return false;
    }

    public bool switchWaterAndSandAtPos(int x, int y)
    {
        if (x >= 1 && y >= 1)
        {
            if (x < Constants.g_gridSizeLengthMinusOne && y < Constants.g_gridSizeLengthMinusOne)
            {
                WaterTiles tileStatus = getStatusFromSurroundingTiles(x, y);
                Debug.Log(tileStatus);
                string oldTag = m_grid[y, x].tag;
                Destroy(m_grid[y, x]);

                GameObject newTile = null;
                if (oldTag == "Ground")
                    newTile = (GameObject)Instantiate(m_waterTiles[(int)getStatusFromSurroundingTiles(x, y)], transform);
                else if (oldTag == "Water")
                    newTile = (GameObject)Instantiate(m_basicGroundTile, transform);
                else
                    return false;
                
                newTile.transform.localPosition = new Vector2(x, y);
                m_grid[y, x] = newTile;
                return true;
            }
        }
        return false;        
    }

    private bool checkBit(pos val, pos check)
    {
        return ((val & check) != 0);
    }

    private enum pos {
        BOTTOM = 1,
        LEFT = 2,
        TOP = 4,
        RIGHT = 8
    }

    private WaterTiles getStatusFromSurroundingTiles(int x, int y)
    {
        pos val = 0;
        // Bottom
        if (m_grid[y - 1, x].tag == "Ground")
            val |= pos.BOTTOM;
        // Left
        if (m_grid[y, x - 1].tag == "Ground")
            val |= pos.LEFT;
        // Top
        if (m_grid[y + 1, x].tag == "Ground")
            val |= pos.TOP;
        // Right
        if (m_grid[y, x + 1].tag == "Ground")
            val |= pos.RIGHT;
        
        if (checkBit(val, pos.RIGHT))
        {
            if (checkBit(val, pos.TOP))
            {
                if (checkBit(val, pos.LEFT))
                {
                    if (checkBit(val, pos.BOTTOM))
                        return WaterTiles.WATER_SAND_ALL;
                    else
                        return WaterTiles.WATER_SAND_TOP_LEFT_RIGHT;
                }
                else                                    // 8 & 4 & !2
                {
                    if (checkBit(val, pos.BOTTOM))      // 8 & 4 & !2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM_TOP_RIGHT;
                    else                                // 8 & 4 & !2 & !1
                        return WaterTiles.WATER_SAND_TOP_RIGHT;
                }
            }
            else                                        // 8 & !4
            {
                if (checkBit(val, pos.LEFT))            // 8 & !4 & 2
                {
                    if (checkBit(val, pos.BOTTOM))      // 8 & !4 & 2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM_LEFT_RIGHT;
                    else                                // 8 & !4 & 2 & !1
                        return WaterTiles.WATER_SAND_LEFT_RIGHT;
                }
                else                                    // 8 & !4 & !2
                {
                    if (checkBit(val, pos.BOTTOM))      // 8 & !4 & !2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM_RIGHT;
                    else                                // 8 & !4 & !2 & !1
                        return WaterTiles.WATER_SAND_RIGHT;
                }
            }
        }
        else
        {
            if (checkBit(val, pos.TOP))
            {
                if (checkBit(val, pos.LEFT))
                {
                    if (checkBit(val, pos.BOTTOM))
                        return WaterTiles.WATER_SAND_BOTTOM_TOP_LEFT;
                    else
                        return WaterTiles.WATER_SAND_TOP_LEFT;
                }
                else                                    // !8 & 4 & !2
                {
                    if (checkBit(val, pos.BOTTOM))      // !8 & 4 & !2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM_TOP;
                    else                                // !8 & 4 & !2 & !1
                        return WaterTiles.WATER_SAND_TOP;
                }
            }
            else                                        // !8 & !4
            {
                if (checkBit(val, pos.LEFT))            // !8 & !4 & 2
                {
                    if (checkBit(val, pos.BOTTOM))      // !8 & !4 & 2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM_LEFT;
                    else                                // !8 & !4 & 2 & !1
                        return WaterTiles.WATER_SAND_LEFT;
                }
                else                                    // !8 & !4 & !2
                {
                    if (checkBit(val, pos.BOTTOM))      // !8 & !4 & !2 & 1
                        return WaterTiles.WATER_SAND_BOTTOM;
                    else                                // !8 & !4 & !2 & !1
                        return WaterTiles.WATER_SAND_NONE;
                }
            }
        }


    }
}
