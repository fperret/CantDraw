﻿using System.Collections;
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

    // On pourrait utiliser des sprites plutot que des prefabs
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

    public bool switchWaterAndSandAtPos(int x, int y)
    {
        if (x >= 1 && y >= 1)
        {
            if (x < Constants.g_gridSizeLengthMinusOne && y < Constants.g_gridSizeLengthMinusOne)
            {
                string oldTag = m_grid[y, x].tag;
                Destroy(m_grid[y, x]);

                GameObject newTile = null;
                // If current tile is ground, replace it by water and remove footprints on it if any
                if (oldTag == "Ground")
                {                
                    newTile = (GameObject)Instantiate(m_waterTiles[(int)getStatusFromSurroundingTiles(x, y)], transform);
                    Collider2D[] collidersAtTile = Physics2D.OverlapPointAll(new Vector2(x, y));
                    foreach (Collider2D collider in collidersAtTile)
                    {
                        if (collider.tag == "FootPrints")
                            Destroy(collider.gameObject);
                    }

                }
                else if (oldTag == "Water")
                    newTile = (GameObject)Instantiate(m_basicGroundTile, transform);
                else
                    return false;
                
                newTile.transform.localPosition = new Vector2(x, y);
                m_grid[y, x] = newTile;

                updateNeighbors(y, x);
                return true;
            }
        }
        return false;        
    }

    // Should only be called by switchWaterAndSandAtPos()
    private void updateWaterTileAtPos(int x, int y)
    {
        if (m_grid[y, x].tag == "Water")
        {
            Destroy(m_grid[y, x]);
            m_grid[y, x] = (GameObject)Instantiate(m_waterTiles[(int)getStatusFromSurroundingTiles(x, y)], transform);
            m_grid[y, x].transform.localPosition = new Vector2(x, y);
        }
    }

    // ! Do not call for x and y too low / high to not hit an out of bound index !
    private void updateNeighbors(int x, int y)
    {
        updateWaterTileAtPos(y - 1, x);
        updateWaterTileAtPos(y, x - 1);
        updateWaterTileAtPos(y + 1, x);
        updateWaterTileAtPos(y, x + 1);
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
        /// On peut utiliser 4 booleens tout simplement ?
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
