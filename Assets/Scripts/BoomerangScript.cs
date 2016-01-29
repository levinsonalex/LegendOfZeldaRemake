using UnityEngine;
using System.Collections;

public class BoomerangScript : MonoBehaviour {

    public Vector3 initialLocation;
    public bool turnAround = false;

	// Use this for initialization
	void Start () {
        initialLocation = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(!turnAround && Vector3.Distance(initialLocation, transform.position) > 4)
        {
            Debug.Log("YO TURN AROUND BOY.");
            turnAround = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } 
        else if(turnAround)
        {
            Vector3 returnVec = PlayerControl.instance.transform.position - transform.position;
            returnVec.Normalize();
            GetComponent<Rigidbody>().velocity = returnVec * PlayerControl.instance.boomerang_velocity;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            PlayerControl.instance.catchBoomerang();
        }
        else if (!turnAround)
        {
            turnAround = true;
        }
    }
}
