using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFollowParabol : MonoBehaviour
{
    private float duration = 0;
    private float maxHeight = 0f;
    private Vector3 startPos;
    private Vector3 EndPos;

    public Animator animator;
    private int jmpHash;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();

        jmpHash = Animator.StringToHash("Jump");
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

        transform.LookAt(EndPos, Vector3.up);
    }


    private float currentTime = 0;

    public float JumpTime = 0.2f;
    private float jumpingTimeClock = 0;

    private Vector3 startPosJump;
    private Vector3 endPosJump;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(jmpHash);
            animator.speed = 25f;
            jumpingTimeClock = JumpTime;

            startPosJump = transform.position;
            endPosJump = startPosJump + transform.forward * 2f;
            maxHeight = 2f;
        }
    }


    public void Move()
    {
        if (hasData)
        {
            currentTime += Time.deltaTime;

            if (jumpingTimeClock > 0)
            {
                jumpingTimeClock -= Time.deltaTime;
                transform.position = MathParabola.Parabola(startPosJump, endPosJump, maxHeight, JumpTime - jumpingTimeClock / JumpTime);

                if (jumpingTimeClock <= 0)
                {
                    startPos = transform.position;
                }
            }
            else
            {
                animator.speed = 1f;
                transform.position = MathParabola.Line(startPos, EndPos, currentTime / duration);
            }

//            transform.position = MathParabola.Parabola(startPos, EndPos, maxHeight, currentTime / duration);
        }
    }
}
