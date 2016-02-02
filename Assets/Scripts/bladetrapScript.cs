using UnityEngine;
using System.Collections;

public class bladetrapScript : MonoBehaviour {
	
	
	Rigidbody myRig;
	
	public int speedForward = 6;
	public int speedBack = 3;
	
	public bool goHome = false;
	
	Vector3 startPosition;
	
	// Use this for initialization
	void Start () {
		startPosition.x = transform.position.x;
		startPosition.y = transform.position.y;
		myRig = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (goHome) {
			Vector3 returnVec = startPosition - transform.position;
			returnVec.Normalize ();
			myRig.velocity = returnVec * speedBack;
			//			Vector3.Distance(
			if(Vector3.Distance(startPosition,transform.position) < .5){
				transform.position = startPosition;
				goHome = false;
				myRig.velocity = Vector3.zero;
			}
		} else {
			if (Mathf.Abs (PlayerControl.instance.transform.position.x - startPosition.x) < 1) {
				if (PlayerControl.instance.transform.position.y > startPosition.y) {
					myRig.velocity = speedForward * Vector3.up;
				} else {
					myRig.velocity = speedForward * Vector3.down;
				}
			} else if (Mathf.Abs (PlayerControl.instance.transform.position.y - startPosition.y) < 1) {
				if (PlayerControl.instance.transform.position.x > startPosition.x) {
					myRig.velocity = speedForward * Vector3.right;
				} else {
					myRig.velocity = speedForward * Vector3.left;
				}
			}
			
		}
	}
	
	void OnCollisionEnter(Collision coll){
		Debug.Log ("in here");
		goHome = true;
	}
}
