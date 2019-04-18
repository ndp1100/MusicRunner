using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolTest : MonoBehaviour
{
    public Transform tran;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float currentTime;
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 5)
        {
            currentTime = 0;
        }

        tran.position = MathParabola.Parabola(Vector3.zero, Vector3.forward * 10f, 5f, currentTime / 5f);
    }
}
