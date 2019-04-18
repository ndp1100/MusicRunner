using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFollowParabol : MonoBehaviour
{
    private float duration = 0;
    private float maxHeight = 0f;
    private Vector3 startPos;
    private Vector3 EndPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool hasData = false;
    public void SetData(float _d, float _h, Vector3 sPos, Vector3 ePos)
    {
        hasData = true;

        currentTime = 0;

        duration = _d;
        maxHeight = _h;

        //todo
        sPos.y = 0;
        ePos.y = 0;

        startPos = sPos;
        EndPos = ePos;
    }


    private float currentTime = 0;
    // Update is called once per frame
    void Update()
    {
        
    }


    public void Move()
    {
        if (hasData)
        {
            currentTime += Time.deltaTime;
            transform.position = MathParabola.Parabola(startPos, EndPos, maxHeight, currentTime / duration);
        }
    }
}
