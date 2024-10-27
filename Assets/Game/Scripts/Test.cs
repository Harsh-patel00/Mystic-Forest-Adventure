using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private void OnEnable()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        
        
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        float pos = 0;
        while (pos <= 5)
        {
            pos += Time.deltaTime * 2f;
            lineRenderer.SetPosition(1, new Vector3(pos, 0, 0));
            yield return new WaitForEndOfFrame();
        }

    }
}
