using UnityEngine;
using System.Collections;

public enum Hand {LEFT, RIGHT};

public class WallmasterScript : EnemyScript {

    public Vector3 startLocation;
    public Hand handType;

    public WallmasterScript()
    {

    }

    // Update is called once per frame
    override public void Update()
    {
        control_state_machine.Update();

        if (control_state_machine.IsFinished())
        {
            control_state_machine.ChangeState(new StateIdle());
        }
    }

    public override void Damage(int dmg, GameObject damageFrom, bool knockBack = true)
    {
        base.Damage(dmg, damageFrom, false);
    }

    public override void OnCollisionEnter(Collision coll)
    {

    }

    public override void OnCollisionStay(Collision coll)
    {

    }

    public override void OnTriggerEnter(Collider coll)
    {

    }
}
