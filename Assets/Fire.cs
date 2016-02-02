using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {
	Rigidbody myRig;
	public int	speed = 5;

	// Use this for initialization
	void Start () {
		myRig = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.name == "FireF(Clone)") {
			myRig.velocity = speed * Vector3.left;
		} else if (gameObject.name == "FireU(Clone)") {
			myRig.velocity = (speed * Vector3.left) + (Vector3.up);
		} else if (gameObject.name == "FireD(Clone)") {
			myRig.velocity = (speed * Vector3.left) + (Vector3.down);
		}
	
	}

	void OnCollisionEnter(Collision coll){
		Destroy(gameObject);
	}
}
