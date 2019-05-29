using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    public Animator animator;
    public Transform cube;

    private int jumptriggerhash = 0;

    private int speedhash = 0;
    // Start is called before the first frame update
    void Start()
    {
        jumptriggerhash = Animator.StringToHash("jump");
        speedhash = Animator.StringToHash("speed");

        animator.SetFloat(speedhash, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger(jumptriggerhash);

            animator.MatchTarget(cube.position, cube.rotation, AvatarTarget.RightHand,
                new MatchTargetWeightMask(Vector3.one, 1f), 0.41f, 0.58f);
        }
    }
}
