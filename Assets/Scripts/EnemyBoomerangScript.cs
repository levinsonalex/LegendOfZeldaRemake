using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyBoomerangScript : MonoBehaviour
{
    public Vector3 initialLocation;
    public GoriyaScript thrower;
    public bool turnAround = false;

    // Use this for initialization
    void Start()
    {
        initialLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(thrower == null)
        {
            Destroy(gameObject);
        }
        else if (!turnAround && Vector3.Distance(initialLocation, transform.position) > 4)
        {
            turnAround = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (turnAround)
        {
            if (Vector3.Distance(thrower.transform.position, transform.position) < .1)
            {
                thrower.catchBoomerang();
                Destroy(gameObject);
            }
            Vector3 returnVec = thrower.transform.position - transform.position;
            returnVec.Normalize();
            GetComponent<Rigidbody>().velocity = returnVec * PlayerControl.instance.boomerang_velocity;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        turnAround = true;
    }
}
