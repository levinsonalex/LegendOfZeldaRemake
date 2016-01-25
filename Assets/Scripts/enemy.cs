using UnityEngine;
using System.Collections;

public enum Mode{
	walking, attacked
};

public class enemy : MonoBehaviour {

	Random random = new Random();

	public int 				movingSpeed;// = 2;
	public int 				attackedSpeed;// = 3;
	public int				hitCount;// = 2;

	public Direction 		curDirection;
	public int 				curSpeed;
	public Mode				curMode;

	public Rigidbody		rig;

	// Use this for initialization
	void Start () {
		curSpeed = movingSpeed;
		rig = GetComponent<Rigidbody> ();
		//change later
		curDirection = Direction.NORTH;
		curMode = Mode.walking;
	
	}
	
	// Update is called once per frame
	void Update () {


//		if(horizontal_input != 0.0f)
//		{
//			if (Mathf.Round((this.GetComponent<Transform>().position.y * 10) % 5) != 0 && Mathf.Round((this.GetComponent<Transform>().position.y * 10) % 5) != 5)
//			{
//				//Debug.Log("Not Y aligned! - " + Mathf.Round((pc.GetComponent<Transform>().position.y * 10) % 5));
//				vertical_input = .75f;
//				if (Mathf.Round((this.GetComponent<Transform>().position.y * 10) % 5) < 2.5)
//				{
//					vertical_input = -vertical_input;
//				}
//			}
//			else
//			{
//				vertical_input = 0.0f;
//			}
//		}
//		else if(vertical_input != 0.0f)
//		{
//			if (Mathf.Round((this.GetComponent<Transform>().position.x * 10) % 5) != 0 && Mathf.Round((this.GetComponent<Transform>().position.x * 10) % 5) != 5)
//			{
//				//Debug.Log("Not H aligned! - " + Mathf.Round((pc.GetComponent<Transform>().position.x * 10) % 5));
//				horizontal_input = .75f;
//				if (Mathf.Round((this.GetComponent<Transform>().position.x * 10) % 5) < 2.5)
//				{
//					horizontal_input = -horizontal_input;
//				}
//			}
//		}
//		
//		this.GetComponent<Rigidbody>().velocity = new Vector3(horizontal_input, vertical_input, 0) * this.curSpeed * time_delta_fraction;
//

		Vector3 newVel = Vector3.zero;
		if (curMode == Mode.walking) { 
			if (curDirection == Direction.NORTH) {
				newVel = curSpeed * Vector3.up;
			} else if (curDirection == Direction.EAST) {
				newVel = curSpeed * Vector3.right;
			} else if (curDirection == Direction.SOUTH) {
				newVel = curSpeed * Vector3.down;
			} else if (curDirection == Direction.WEST) {
				newVel = curSpeed * Vector3.left;
			}
		} else if (curMode == Mode.attacked) {
			if (curDirection == Direction.NORTH) {
				newVel = attackedSpeed * Vector3.down;
			} else if (curDirection == Direction.EAST) {
				newVel = attackedSpeed * Vector3.left;
			} else if (curDirection == Direction.SOUTH) {
				newVel = attackedSpeed * Vector3.up;
			} else if (curDirection == Direction.WEST) {
				newVel = attackedSpeed * Vector3.right;
			}
		}

		rig.velocity = newVel;

	
	}

	void OnCollisionEnter(Collision coll){
		print ("Touched " + coll.transform.parent.name);
		if (coll.gameObject.tag == "link") {
			//take away a heart from link
		} else if (coll.gameObject.tag == "weapon") {
			//hit by the sword
			hitCount--;
			curMode = Mode.attacked;   
		} else if (coll.transform.parent.name == "MapAnchor") {
			print ("Anchor");
			//switchDirections(curDirection);

		}
	}

	void switchDirections(Direction oldDir){
		while (curDirection == oldDir) {
			curDirection = (Direction)Random.Range (0, 3);
		}
	}
}
