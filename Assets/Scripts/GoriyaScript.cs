using UnityEngine;
using System.Collections;

public class GoriyaScript : EnemyScript {

    Animator animator;

    // Use this for initialization
    public override void Start()
    {
        animator = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        float h = GetComponent<Rigidbody>().velocity.x;
        float v = GetComponent<Rigidbody>().velocity.y;

        animator.SetFloat("Vertical_Velocity", v);
        animator.SetFloat("Horizontal_Velocity", h);
        animator.SetBool("Throwing", attacking);
        control_state_machine.Update();

        if (control_state_machine.IsFinished())
        {
            if (Random.Range(0, 10) == 0)
            {
                control_state_machine.ChangeState(new StateEnemyThrowBoomerang(this));
            }
            else
            {
                control_state_machine.ChangeState(new EnemyMoveTile(this, getRandomDirection(), move_velocity));
            }
        }
    }

    public void catchBoomerang()
    {
        attacking = false;
    }
}
