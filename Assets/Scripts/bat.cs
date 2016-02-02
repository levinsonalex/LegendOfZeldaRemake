using UnityEngine;
using System.Collections;
public enum nineDir{
	NW, N, NE, E, SE, S, SW, W
};
public enum batState{
	stopped, go_up, flying, slowDown
};

public class bat : MonoBehaviour
{



    public bool foundLocX = true;
    public bool foundLocY = true;
    public int targetY;
    public int targetX;

    public int movingSpeed = 2;// = 2;
    public int attackedSpeed;// = 3;
    public int hitCount = 1;// = 2;

    public int numLocation = 0;

    public nineDir curDir;
    public float curSpeed;

    public int roomXMin;
    public int roomXMax;
    public int roomYMin;
    public int roomYMax;

    public float t;
    public float startTime;
    bool starting = true;
    public float timeDelay = 1.5f;
    public float flightTime = 100;
    public float restTime = 5.0f;
    public float maxSpeed = 5.0f;
    public float endWaitTime = 5.0f;
    public float startWaitTime;
    public float startFlightTime;
    public float endFlightTime;
    public batState curState;


    public Rigidbody rig;

    // Use this for initialization
    void Start()
    {
        curSpeed = movingSpeed;
        rig = GetComponent<Rigidbody>();
        //change later
        curDir = nineDir.N;
    }


    // Update is called once per frame
    void Update()
    {
        //still
        if (numLocation >= 500  /*Time.time - startTime > flightTime*/)
        {
            //			print ("still");
            numLocation = 0;
            rig.velocity = Vector3.zero;
            t = Time.time;
            starting = true;

        }
        else if (Time.time - t > restTime)
        {
            if (starting)
            {
                startTime = Time.time;
                starting = false;
            }
            if (foundLocX && foundLocY)
            {
                int cornerX = Mathf.FloorToInt(Camera.main.transform.position.x - 4);
                int cornerY = Mathf.FloorToInt(Camera.main.transform.position.y + 2);
                targetX = Random.Range(cornerX, cornerX + 11);//change these to room limits
                targetY = Random.Range(cornerY, cornerY - 6); // change these to room limits
                                                              //				print ("TargetX is " + targetX);
                                                              //				print ("TargetY is " + targetY);
                foundLocX = false;
                foundLocY = false;
                numLocation++;
            }

            Vector3 curLoc = transform.position;

            if (curLoc.y < targetY && curLoc.x < targetX && !foundLocY && !foundLocX)
            {
                curDir = nineDir.NE;
            }
            else if (curLoc.y < targetY && curLoc.x > targetX && !foundLocX && !foundLocY)
            {
                curDir = nineDir.NW;
            }
            else if (curLoc.y > targetY && curLoc.x > targetX && !foundLocX && !foundLocY)
            {
                curDir = nineDir.SW;
            }
            else if (curLoc.y > targetY && curLoc.x < targetX && !foundLocX && !foundLocY)
            {
                curDir = nineDir.SE;
            }
            else if (curLoc.y < targetY && !foundLocY)
            {
                curDir = nineDir.N;
            }
            else if (curLoc.y >= (targetY + 1) && !foundLocY)
            {
                curDir = nineDir.S;
            }
            else if (curLoc.x < targetX && !foundLocX)
            {
                curDir = nineDir.E;
            }
            else if (curLoc.x >= (targetX + 1) && !foundLocX)
            {
                curDir = nineDir.W;
            }
            if (curLoc.x >= targetX && curLoc.x < targetX + 1 && !foundLocX)
            {
                foundLocX = true;
            }
            if (curLoc.y >= targetY && curLoc.y < targetY + 1 && !foundLocY)
            {
                foundLocY = true;
            }

            //Handles acc in the begining and the end of the flight
            if (curState == batState.go_up)
            {
                curSpeed += .1f;
                if (curSpeed >= maxSpeed)
                {
                    curState = batState.flying;
                    curSpeed = maxSpeed;
                    startFlightTime = Time.time;
                    endFlightTime = Random.Range(3, 15);
                }
            }
            else if (curState == batState.flying)
            {
                if (Time.time - startFlightTime > endFlightTime)
                {
                    curState = batState.slowDown;
                }
            }
            else if (curState == batState.slowDown)
            {
                curSpeed -= .1f;
                if (curSpeed <= 0)
                {
                    curSpeed = 0;
                    curState = batState.stopped;
                    startWaitTime = Time.time;
                }
            }
            else if (curState == batState.stopped)
            {
                if (Time.time - startWaitTime > endWaitTime)
                {
                    curState = batState.go_up;
                }
            }


            Vector3 newVel = Vector3.zero;

            if (curDir == nineDir.N)
            {
                newVel = curSpeed * Vector3.up;
            }
            else if (curDir == nineDir.NE)
            {
                newVel = curSpeed * (Vector3.up + Vector3.right);
            }
            else if (curDir == nineDir.E)
            {
                newVel = curSpeed * Vector3.right;
            }
            else if (curDir == nineDir.SE)
            {
                newVel = curSpeed * (Vector3.down + Vector3.right);
            }
            else if (curDir == nineDir.S)
            {
                newVel = curSpeed * Vector3.down;
            }
            else if (curDir == nineDir.SW)
            {
                newVel = curSpeed * (Vector3.down + Vector3.left);
            }
            else if (curDir == nineDir.W)
            {
                newVel = curSpeed * Vector3.left;
            }
            else if (curDir == nineDir.NW)
            {
                newVel = curSpeed * (Vector3.up + Vector3.left);
            }

            rig.velocity = newVel;
        }
    }

    public void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Sword")
        {
            Damage(1, PlayerControl.instance.gameObject);
        }
    }

    public void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Beam")
        {
            Damage(1, PlayerControl.instance.gameObject);
        }
        else if (coll.gameObject.tag == "Boomerang")
        {
            Damage(1, PlayerControl.instance.gameObject);
        }

    }

    public virtual void Damage(int dmg, GameObject damageFrom)
    {
        GetComponentInParent<RoomScript>().enemiesList.Remove(gameObject);
        Destroy(gameObject);
    }
}
