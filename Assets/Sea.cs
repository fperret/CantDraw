using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine("animate");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -0.1f * Time.deltaTime, 0);
    }

    private IEnumerator animate()
    {
        while (true)
        {
            transform.Translate(0, -0.1f * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
