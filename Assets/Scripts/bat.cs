using UnityEngine;
using System.Collections;
public enum nineDir{
	NW, N, NE, E, SE, S, SW, W
};

public class bat : MonoBehaviour {



	public bool				foundLocX = true;
	public bool				foundLocY = true;
	public int				targetY;
	public int				targetX;
	
	public int 				movingSpeed = 2;// = 2;
	public int 				attackedSpeed;// = 3;
	public int				hitCount = 1;// = 2;
	
	public nineDir 			curDir;
	public int 				curSpeed;
	public Mode				curMode;
	
	public Rigidbody		rig;
	
	// Use this for initialization
	void Start () {
		curSpeed = movingSpeed;
		rig = GetComponent<Rigidbody> ();
		//change later
		curDir = nineDir.N;
		curMode = Mode.walking;
	}


	// Update is called once per frame
	void Update(){
		//still
		if (curMode == Mode.attacked) {
			print ("still");
		} else if (curMode == Mode.walking) {
			if(foundLocX && foundLocY)
			{
				targetX = Random.Range (34, 45);
				targetY = Random.Range (2, 9);
				print ("TargetX is " + targetX);
				print ("TargetY is " + targetY);
				foundLocX = false;
				foundLocY = false;
			}

			Vector3 curLoc = transform.position;
			if (Mathf.Round((curLoc.x * 10) % 5) != 0 && Mathf.Round((curLoc.x * 10) % 5) != 5)
			{
				float vertical_input = .75f;
				if (Mathf.Round((curLoc.x * 10) % 5) < 2.5)
				{
					vertical_input = -vertical_input;
				}
				curLoc.x += vertical_input;
			}

			if (curLoc.y < targetY && !foundLocY) {
				curDir = nineDir.N;
			} 
			else if (curLoc.y > targetY && !foundLocY) {
				curDir = nineDir.S;
			} 
			else if (curLoc.x < targetX && !foundLocX) {
				curDir = nineDir.E;
			} 
			else if (curLoc.x > targetX && !foundLocX) {
				curDir = nineDir.W;
			} 
			else if (curLoc.x == targetX){
				foundLocX = true;
			} 
			else if (curLoc.y == targetY){
				foundLocY = true;
			}


		


			Vector3 newVel = Vector3.zero;
			if (curMode == Mode.walking) { 
				if (curDir == nineDir.N) {
					newVel = curSpeed * Vector3.up;
				} else if (curDir == nineDir.NE) {
					newVel = curSpeed * (Vector3.up + Vector3.right);
				} else if (curDir == nineDir.E) {
					newVel = curSpeed * Vector3.right;
				} else if (curDir == nineDir.SE) {
					newVel = curSpeed * (Vector3.down + Vector3.right);
				} else if (curDir == nineDir.S) {
					newVel = curSpeed * Vector3.down;
				} else if (curDir == nineDir.SW) {
					newVel = curSpeed * (Vector3.down + Vector3.left);
				} else if (curDir == nineDir.W) {
					newVel = curSpeed * Vector3.left;
				} else if (curDir == nineDir.NW) {
					newVel = curSpeed * (Vector3.up + Vector3.left);
				}
			}
	
			rig.velocity = newVel;
		}
	}

}
