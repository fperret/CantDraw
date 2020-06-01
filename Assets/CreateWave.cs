using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWave : MonoBehaviour
{
    const int m_numberOfWaves = 3;
    const int m_tilesPerWaves = 200;
    const float m_maxWaveY = 2;
    const float m_spawnOffsetY = -1;

    public GameObject m_waveTile;

    public Vector3  m_globalPos;
    public float m_amplitude;
    public float m_frequency;

    public float m_speed = 0.25f;

    private GameObject[,] m_wave = new GameObject[m_numberOfWaves, m_tilesPerWaves];
    private Vector3[] m_waveOffset = new Vector3[m_numberOfWaves];
    public float[] m_waveTimers = new float[m_numberOfWaves];
    public Transform m_waveParent;
    // Start is called before the first frame update
    void Start()
    {
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
            updateOneWave(i, m_waveOffset[i]);
            m_waveTimers[i] += Time.deltaTime;
        }
    }

    private void randomizeWaveOffset(int index)
    {
        m_waveOffset[index] = new Vector3(Random.Range(0.0f, 2.0f), m_spawnOffsetY - index, 0);
    }

    private void updateOneWave(int index, Vector3 offset)
    {
        float minY = m_amplitude;
        float yOffsetFromTime = m_waveTimers[index] * m_speed;

        for (int i = 0; i < m_tilesPerWaves; ++i)
        {
            float x = (float)i / 8;
            float y = Mathf.Sin(x * m_frequency + m_waveTimers[index]) * m_amplitude;
            if (y < minY)
                minY = y;
            Vector3 newPosition = new Vector3(x, y + yOffsetFromTime, 0);
            newPosition += m_globalPos + offset;

            m_wave[index, i].transform.position = newPosition;
        }

        // If the wave is high enough (not in the sea anymore) we reposition it at the bottom
        if ((minY + yOffsetFromTime + offset.y) >= m_maxWaveY)
        {
            m_waveTimers[index] = 0;
            randomizeWaveOffset(index);
        }
    }
}
