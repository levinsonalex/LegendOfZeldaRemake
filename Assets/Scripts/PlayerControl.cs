﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, PUSHING, TRANSITIONING };

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;

    public bool pause = false;
    public GameObject inventoryScreen;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;
	public bool eastMostTypewriterOnSwitch = false;

	public int roomX = 0;
	public int roomY = 0;

	public int curHealth = 6;
	public int maxHealth = 6;
	public bool invincible = false;

	public GameObject selected_weapon_prefab;
    public bool hasBoomerang = false;
    public bool hasBow = false;
    public bool hasCompass = false;
    public bool hasMap = false;

	public float walking_velocity = 1.0f;
	public int rupee_count = 0;
	public int key_count = 0;
	public int bomb_count = 0;

	private bool doorTouch = false;
	private bool moveRoom = false;
	private GameObject firstTouch;

	private bool doorUnlocked0x0 = false;
	private bool doorUnlocked_1x2 = false;
	private bool doorUnlocked0x3 = false;

	public static PlayerControl instance;

	public Sprite[] mapSprites;

	void Awake()
	{
		mapSprites = Resources.LoadAll<Sprite>("map_sprites");
	}

	// Use this for initialization
	void Start () {
		if(instance != null)
		{
			Debug.LogError("Multiple Link objects detected!");
		}
		instance = this;

		animation_state_machine = new StateMachine();
		animation_state_machine.ChangeState(new StateIdleWithSprite(this, GetComponent<SpriteRenderer>(), link_run_down[0]));

		control_state_machine = new StateMachine();
		control_state_machine.ChangeState(new StateLinkNormalMovement(this));
	}

	// Update is called once per frame
	void Update () {
        //Pause
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (pause)
            {
                inventoryScreen.SetActive(false);
            }
            else
            {
                inventoryScreen.SetActive(true);
            }
            pause = !pause;
        }

        if (pause)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            return;
        }

        animation_state_machine.Update();
		control_state_machine.Update();

		if (control_state_machine.IsFinished())
		{
			control_state_machine.ChangeState(new StateLinkNormalMovement(this));
		}

		
        if (Input.GetKeyDown(KeyCode.I))
        { 
			if (invincible)
			{
				GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
			}
			else
			{
				GetComponent<SpriteRenderer>().color = new Color(0, 255, 0);
			}
			invincible = !invincible;
		}
	}

	void FixedUpdate()
	{
		moveRoom = false;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Rupee")
		{
			Destroy(coll.gameObject);
			rupee_count++;
		}
		else if(coll.gameObject.tag == "Key")
		{
			Destroy(coll.gameObject);
			key_count++;
		}
        else if(coll.gameObject.tag == "Map")
        {
            Destroy(coll.gameObject);
            hasMap = true;
        }
        else if(coll.gameObject.tag == "Compass")
        {
            Destroy(coll.gameObject);
            hasCompass = true;
        }
        else if(coll.gameObject.name == "080x049")
        {
            coll.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
		else
		{
			print(coll.name);
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		//Left Door
		if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_50")
		{
			if (!moveRoom)
			{
				control_state_machine.ChangeState(new StateLinkTransition(this, Direction.WEST));
				moveRoom = true;
			}
			else
			{
				moveRoom = false;
			}
		}
		//Right Door
		else if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_49")
		{
			if (!moveRoom)
			{
				control_state_machine.ChangeState(new StateLinkTransition(this, Direction.EAST));
				moveRoom = true;
			}
			else 
			{
				moveRoom = false;
			}
		}
		//Up Door
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_91")
		{
			if (!moveRoom)
			{
				control_state_machine.ChangeState(new StateLinkTransition(this, Direction.NORTH));
				moveRoom = true;
			}
			else
			{
				moveRoom = false;
			}
		}
		//Down Door
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_10")
		{
			if (!moveRoom)
			{
				control_state_machine.ChangeState(new StateLinkTransition(this, Direction.SOUTH));
				moveRoom = true;
			} 
			else
			{
				moveRoom = false;
			}
		}
		//Locked Door Right
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_101")
		{
			if(key_count > 0)
			{
				coll.gameObject.GetComponent<SpriteRenderer>().sprite = mapSprites[48];
				coll.gameObject.GetComponent<BoxCollider>().isTrigger = true;
				key_count--;

				if (roomX == 0 && roomY == 3)
				{
					doorUnlocked0x3 = true;
				}
			}
		}
		//Locked Door Left
		else if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_106")
		{
			if (key_count > 0)
			{
				coll.gameObject.GetComponent<SpriteRenderer>().sprite = mapSprites[51];
				coll.gameObject.GetComponent<BoxCollider>().isTrigger = true;
				key_count--;
			}
		}
		//Locked Door Up
		else if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_80" || coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_81")
		{
			if (doorTouch)
			{
				if(key_count > 0)
				{
					if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_80")
					{
						coll.gameObject.GetComponent<SpriteRenderer>().sprite = mapSprites[92];
						firstTouch.GetComponent<SpriteRenderer>().sprite = mapSprites[93];
					}
					else
					{
						coll.gameObject.GetComponent<SpriteRenderer>().sprite = mapSprites[93];
						firstTouch.GetComponent<SpriteRenderer>().sprite = mapSprites[92];
					}
					coll.gameObject.GetComponent<BoxCollider>().isTrigger = true;
					firstTouch.GetComponent<BoxCollider>().isTrigger = true;

					if(roomX == 0 && roomY == 0)
					{
						doorUnlocked0x0 = true;
					}
					else if(roomX == -1 && roomY == 2)
					{
						doorUnlocked_1x2 = true;
					}

					firstTouch = null;
					doorTouch = false;
					key_count--;
				}
				else
				{
					print("GET A KEY!");
				}
			}
			doorTouch = true;
			firstTouch = coll.gameObject;
		}
		//Pushable Blocks
		else if (coll.gameObject.CompareTag("Pushable"))
		{
			if (coll.gameObject.name == "023x038" && coll.gameObject.transform.position.x > 22)
			{
				if(coll.gameObject.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x < 0 &&
					Mathf.Abs(coll.gameObject.GetComponent<Transform>().position.y - GetComponent<Transform>().position.y) < .25)
				{
					control_state_machine.ChangeState(new StateLinkPush(this, coll.gameObject, GameObject.Find("017x038").gameObject));
				}
			}
			if(coll.gameObject.name == "022x060" && Mathf.Abs(coll.gameObject.transform.position.y-60) < 1)
			{
				if (Mathf.Abs(coll.gameObject.GetComponent<Transform>().position.x - GetComponent<Transform>().position.x) < .25)
				{
					if (coll.gameObject.GetComponent<Transform>().position.y - GetComponent<Transform>().position.y < 0)
					{
						control_state_machine.ChangeState(new StateLinkPush(this, coll.gameObject));
					}
					else
					{
						control_state_machine.ChangeState(new StateLinkPush(this, coll.gameObject));
					}
				}
			}
		}
		else
		{
			print(coll.collider.name);
			doorTouch = false;
			firstTouch = null;
			moveRoom = false;
		}
	}

	public void roomHandle(int x, int y)
	{
		if (doorUnlocked0x0)
		{
			GameObject leftUpDoor = GameObject.Find("039x009");
			GameObject rightUpDoor = GameObject.Find("040x009");
			if (leftUpDoor != null && rightUpDoor != null)
			{
				leftUpDoor.GetComponent<SpriteRenderer>().sprite = mapSprites[92];
				rightUpDoor.GetComponent<SpriteRenderer>().sprite = mapSprites[93];

				leftUpDoor.GetComponent<BoxCollider>().isTrigger = true;
				rightUpDoor.GetComponent<BoxCollider>().isTrigger = true;
			}
		}

		if (doorUnlocked_1x2)
		{
			GameObject leftUpDoor = GameObject.Find("023x031");
			GameObject rightUpDoor = GameObject.Find("024x031");
			if (leftUpDoor != null && rightUpDoor != null)
			{
				leftUpDoor.GetComponent<SpriteRenderer>().sprite = mapSprites[92];
				rightUpDoor.GetComponent<SpriteRenderer>().sprite = mapSprites[93];

				leftUpDoor.GetComponent<BoxCollider>().isTrigger = true;
				rightUpDoor.GetComponent<BoxCollider>().isTrigger = true;
			}
		}

		if (doorUnlocked0x3)
		{
			GameObject door = GameObject.Find("046x038");
			if (door != null)
			{
				door.GetComponent<SpriteRenderer>().sprite = mapSprites[48];
				door.GetComponent<BoxCollider>().isTrigger = true;
			}
		}

		if (x == -1 && y == 3)
		{
			eastMostTypewriterOnSwitch = false;
		}
		else if (x == -2 && y == 3)
		{
			eastMostTypewriterOnSwitch = true;
		}
	}
}
