using UnityEngine;
using System.Collections;

public class GelScript : EnemyScript {

    // Update is called once per frame
    override public void Update()
    {
        control_state_machine.Update();

        if (control_state_machine.IsFinished())
        {
            if (Random.Range(0, 4) == 0)
            {
                control_state_machine.ChangeState(new StateEnemyStunned(this));
            }
            else
            {
                control_state_machine.ChangeState(new EnemyMoveTile(this, getRandomDirection(), move_velocity));
            }
        }
    }

    override public void BoomerangHit()
    {
        Damage(1, PlayerControl.instance.gameObject);
    }
}
