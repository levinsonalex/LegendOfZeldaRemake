using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING, PUSHING};

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;
	public bool eastMostTypewriterOnSwitch = false;

	public int roomX = 0;
	public int roomY = 0;

	public GameObject selected_weapon_prefab;

	public float walking_velocity = 1.0f;
	public int rupee_count = 0;
	public int key_count = 0;
	public int bomb_count = 0;

	private bool doorTouch = false;
	private GameObject firstTouch;

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
		animation_state_machine.Update();
		control_state_machine.Update();

		if (control_state_machine.IsFinished())
		{
			control_state_machine.ChangeState(new StateLinkNormalMovement(this));
		}
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
			if (!doorTouch)
			{
				Vector3 trans = new Vector3(-ShowMapOnCamera.S.screenSize.x, 0, 0);
				ShowMapOnCamera.S.transform.Translate(trans);
				Vector3 leftDoorExit = instance.GetComponent<Transform>().position;
				leftDoorExit.x = ShowMapOnCamera.S.transform.position.x + 5.5f;
				instance.GetComponent<Transform>().position = leftDoorExit;
				roomX--;
			}
			else
			{
				doorTouch = false;
			}
		}
		//Right Door
		else if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_49")
		{
			if (!doorTouch)
			{
				Vector3 trans = new Vector3(ShowMapOnCamera.S.screenSize.x, 0, 0);
				ShowMapOnCamera.S.transform.Translate(trans);
				Vector3 rightDoorExit = instance.GetComponent<Transform>().position;
				rightDoorExit.x = ShowMapOnCamera.S.transform.position.x - 5.5f;
				instance.GetComponent<Transform>().position = rightDoorExit;
				roomX++;
			}
			else 
			{
				doorTouch = false;
			}
		}
		//Up Door
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_91")
		{
			if (!doorTouch)
			{
				Vector3 trans = new Vector3(0, ShowMapOnCamera.S.screenSize.y - ShowMapOnCamera.S.tileClearOverage, 0);
				ShowMapOnCamera.S.transform.Translate(trans);
				Vector3 upDoorExit = instance.GetComponent<Transform>().position;
				upDoorExit.y = ShowMapOnCamera.S.transform.position.y - 5;
				instance.GetComponent<Transform>().position = upDoorExit;
				doorTouch = true;
				roomY++;
			}
			else
			{
				doorTouch = false;
			}
		}
		//Down Door
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_10")
		{
			if (!doorTouch)
			{
				Vector3 trans = new Vector3(0, - ShowMapOnCamera.S.screenSize.y + ShowMapOnCamera.S.tileClearOverage, 0);
				ShowMapOnCamera.S.transform.Translate(trans);
				Vector3 downDoorExit = instance.GetComponent<Transform>().position;
				downDoorExit.y = ShowMapOnCamera.S.transform.position.y + 2;
				instance.GetComponent<Transform>().position = downDoorExit;
				doorTouch = true;
				roomY--;
			} 
			else
			{
				doorTouch = false;
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
		}
		roomHandle(roomX, roomY);
		print("X:" + roomX + " Y:" + roomY);
	}

	void roomHandle(int x, int y)
	{
		if(x == -1 && y == 3)
		{
			eastMostTypewriterOnSwitch = false;
		}
		else if(x == -2 && y == 3)
		{
			eastMostTypewriterOnSwitch = true;
		}
	}
}
