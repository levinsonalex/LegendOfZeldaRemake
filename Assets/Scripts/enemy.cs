using UnityEngine;
using System.Collections;

public enum Mode{
	walking, attacked
};

public class enemy : MonoBehaviour {

	Random random = new Random();
	
	public int 				movingSpeed = 2;
	public int				hitCount = 1;
	
	public Direction 		curDirection;
	public int 				curSpeed;
	public Mode				curMode;
	
	public Rigidbody		rig;
	
	public bool 			foundLocX = true;
	public bool				foundLocY = true;
	public int 				targetX;
	public int				targetY;
	
	public float					t;
	public bool 					getTime = true;
	public int						numSpaces;
	public bool						getSpeed = true;
	Vector3 oldPos = Vector3.zero;
	Vector3 newPos = Vector3.zero;
	public float						timeDelay = 10;
	
	public int oldX;
	public int oldY;
	public bool move = true;


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
		if (curMode == Mode.walking) {	
			dirMovement (transform.position);
		}
	}
	bool checkNextTo(float x, float y){
		return ShowMapOnCamera.MAP [Mathf.RoundToInt (x), Mathf.FloorToInt (y)] == 29;
	}
	
	void dirMovement(Vector3 curPos){
		
		if (curDirection == Direction.NORTH) {
			if (checkNextTo(Mathf.RoundToInt(curPos.x),Mathf.FloorToInt(curPos.y + 1))) {
//				Debug.Log ("Can move north");
				moveFunc (Vector3.up, curPos);
			} else {
//				Debug.Log ("Can't move North");
				rig.velocity = Vector3.zero;
				curPos.y = Mathf.Floor (curPos.y);
				transform.position = curPos;
				switchDirections(curDirection);
			}
		} else if (curDirection == Direction.EAST) {
			if (checkNextTo(Mathf.FloorToInt(curPos.x + 1), Mathf.RoundToInt(curPos.y))) {
//				Debug.Log ("Can move east");
				moveFunc (Vector3.right, curPos);
			} else {
//				Debug.Log (" Can't move east");
				rig.velocity = Vector3.zero;
				curPos.x = Mathf.Floor (curPos.x);
				transform.position = curPos;
				switchDirections(curDirection);
				
			}
		} else if (curDirection == Direction.SOUTH) {
			if (checkNextTo(Mathf.RoundToInt(curPos.x),Mathf.CeilToInt(curPos.y - 1))) {
//				Debug.Log ("Can move south");
				moveFunc (Vector3.down, curPos);
			} else {
//				Debug.Log ("can't move south");
				rig.velocity = Vector3.zero;
				curPos.y = Mathf.Ceil (curPos.y);
				transform.position = curPos;
				switchDirections(curDirection);
			}
		} else if (curDirection == Direction.WEST) {
			if (checkNextTo(Mathf.CeilToInt(curPos.x - 1),Mathf.RoundToInt(curPos.y))) {
//				Debug.Log ("Can move west");
				moveFunc (Vector3.left, curPos);
			} else {
//				Debug.Log ("can't move west");
				rig.velocity = Vector3.zero;
				curPos.x = Mathf.Ceil (curPos.x);
				transform.position = curPos;
				switchDirections(curDirection);
			}
		}
	}
	
	
	
	void moveFunc(Vector3 newVelo, Vector3 curLoca){
		//Checks to see if the enemy should be moving
		//if it should then record the old position
		if (move) {
			oldPos = transform.position;
			move = false;
		} else {
			if (curDirection == Direction.EAST || curDirection == Direction.SOUTH) {
				if (oldPos.x == Mathf.Floor (curLoca.x)) {
					rig.velocity = newVelo;
				} else { 
					curLoca.x = Mathf.Floor (curLoca.x);
					transform.position = curLoca;
					rig.velocity = Vector3.zero;
					move = true;
				}
			}else if (curDirection == Direction.WEST || curDirection == Direction.NORTH) {
				if (oldPos.x == Mathf.Ceil (curLoca.x)) {
					rig.velocity = newVelo;
				} else { 
					curLoca.x = Mathf.Ceil (curLoca.x);
					transform.position = curLoca;
					rig.velocity = Vector3.zero;
					move = true;
				}
			}
		}
	}
	
	
	void switchDirections(Direction oldDir){
		while (curDirection == oldDir) {
			curDirection = (Direction)Random.Range (0, 4);
		}
	}

	void OnTriggerEnter(Collider other){
		print (other.gameObject.name);
	}
	
	
}