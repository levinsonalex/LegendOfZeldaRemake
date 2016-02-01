using UnityEngine;
using System.Collections;


public class EnemyScript : MonoBehaviour {

    public GameObject rupee;
    public GameObject heart;
    public GameObject bomb;

    public EntityState current_state = EntityState.NORMAL;
    public float move_velocity = 1f;
    public float knockback_velocity = 10f;
    public StateMachine control_state_machine;

    public bool invincible = false;
 
    public int health = 2;

    // Use this for initialization
    public virtual void Start () {
        control_state_machine = new StateMachine();
        control_state_machine.ChangeState(new EnemyMoveTile(this, (Direction)Random.Range(0, 4), move_velocity));
    }
	
	// Update is called once per frame
	public virtual void Update () {
        control_state_machine.Update();

        if (control_state_machine.IsFinished())
        {
            control_state_machine.ChangeState(new EnemyMoveTile(this, (Direction)Random.Range(0, 4), move_velocity));
        }
    }

    public virtual void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Sword")
        {
            Damage(1, PlayerControl.instance.gameObject);
        }
    }

    public virtual void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Beam")
        {
            Damage(1, PlayerControl.instance.gameObject);
        }
        else if (coll.gameObject.tag == "Boomerang")
        {

        }
        else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
            Direction moveDir = (Direction)Random.Range(0, 4);
            control_state_machine.ChangeState(new EnemyMoveTile(this, moveDir, move_velocity));
        }
    }

    public virtual void OnCollisionStay(Collision coll)
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
        Direction moveDir = (Direction)Random.Range(0, 4);
        control_state_machine.ChangeState(new EnemyMoveTile(this, moveDir, move_velocity));
    }

    public virtual void Damage(int dmg, GameObject damageFrom)
    {
        control_state_machine.ChangeState(new StateEnemyDamaged(this, damageFrom, 1));
        if(health <= 0)
        {
            if (Random.Range(0, 2) == 1)
            {
                int itemDrop = Random.Range(0, 3);
                switch (itemDrop)
                {
                    case 0:
                        Instantiate(rupee, transform.position, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(heart, transform.position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(bomb, transform.position, Quaternion.identity);
                        break;
                    default:
                        Debug.Log("Weird stuff is happening.");
                        break;
                }
            }
            Destroy(gameObject);
        }
    }

    public void invincibleOn()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
        invincible = true;
    }

    public void invincibleOff()
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        invincible = false;
    }
}