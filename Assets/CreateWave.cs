using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWave : MonoBehaviour
{
    const int m_numberOfWaves = 4;
    const int m_tilesPerWaves = 200;
    const float m_maxWaveY = 2;
    const float m_offsetY = -1;

    // X position of one tile
    static readonly float[] x_positions = new float[m_tilesPerWaves];
    // X position multiplied by the frequency used in the Sine function
    static readonly float[] x_positionsFrequencyAdjusted = new float[m_tilesPerWaves];

    public GameObject m_waveTile;

    public Vector3  m_globalPos;
    private const float m_amplitude = 0.13f;
    private const float m_frequency = 3.75f;

    public float m_speed = 0.25f;

    private GameObject[,] m_wave = new GameObject[m_numberOfWaves, m_tilesPerWaves];
    private float[] m_waveOffsetX = new float[m_numberOfWaves];
    private float[] m_waveTimers = new float[m_numberOfWaves];

    // /!\ We use this timer only to start the wave at a different time /!\
    // It will stop getting incremented at one point
    private float m_globalTimer = 0.0f;
    public Transform m_waveParent;
    // Start is called before the first frame update
    void Start()
    {
        // The x position of the tiles stay the same so we calculate them at the start
        for (int i = 0; i < m_tilesPerWaves; ++i)
        {
            x_positions[i] = (float)i / 8;
            x_positionsFrequencyAdjusted[i] = x_positions[i] * m_frequency;
        }

        for (int i = 0; i < m_numberOfWaves; ++i)
        {
            for (int j = 0; j < m_tilesPerWaves; ++j)
            {
                m_wave[i, j] = (GameObject)Instantiate(m_waveTile, m_waveParent);
            }
            m_waveTimers[i] = 0.0f;
            randomizeWaveOffset(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_numberOfWaves; ++i)
        {
            updateOneWave(i, m_waveOffsetX[i]);
            m_waveTimers[i] += Time.deltaTime;

            if (m_globalTimer < (3.0f * (i + 1)))
                break;
        }

        // We use this timer only to start the wave at a different time
        if (m_globalTimer < 1000)
            m_globalTimer += Time.deltaTime;
    }

    private void randomizeWaveOffset(int index)
    {
        m_waveOffsetX[index] = Random.Range(0.0f, 2.0f);
    }

    private void updateOneWave(int index, float offsetX)
    {
        float minY = m_amplitude;
        float yOffsetFromTime = m_waveTimers[index] * m_speed;
        float constantOffsetY = m_offsetY;

        for (int i = 0; i < m_tilesPerWaves; ++i)
        {
            float x = x_positions[i];
            float y = Mathf.Sin(x_positionsFrequencyAdjusted[i] + m_waveTimers[index]) * m_amplitude;

            // We want to know the lowest point of the wave            
            if (y < minY)
                minY = y;

            // Position for each tile with the y position adjusted with time to make the wave move up
            Vector3 newPosition = new Vector3(x, y + yOffsetFromTime, 0);

            newPosition.x += offsetX;
            newPosition.y += constantOffsetY;
            // Offset the position to keep the waves separated and shifted
            newPosition += m_globalPos;

            m_wave[index, i].transform.position = newPosition;
        }

        // If the wave is high enough (not in the sea anymore) we reposition it at the bottom
        if ((minY + yOffsetFromTime + constantOffsetY) >= m_maxWaveY)
        {
            m_waveTimers[index] = 0;
            randomizeWaveOffset(index);
        }
    }
}
