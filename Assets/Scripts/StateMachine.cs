using UnityEngine;

// State Machines are responsible for processing states, notifying them when they're about to begin or conclude, etc.
public class StateMachine
{
	private State _current_state;
	
	public void ChangeState(State new_state)
	{
		if(_current_state != null)
		{
			_current_state.OnFinish();
		}
		
		_current_state = new_state;
		// States sometimes need to reset their machine. 
		// This reference makes that possible.
		_current_state.state_machine = this;
		_current_state.OnStart();
	}
	
	public void Reset()
	{
		if(_current_state != null)
			_current_state.OnFinish();
		_current_state = null;
	}
	
	public void Update()
	{
		if(_current_state != null)
		{
			float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
			_current_state.OnUpdate(time_delta_fraction);
		}
	}

	public bool IsFinished()
	{
		return _current_state == null;
	}
}

// A State is merely a bundle of behavior listening to specific events, such as...
// OnUpdate -- Fired every frame of the game.
// OnStart -- Fired once when the state is transitioned to.
// OnFinish -- Fired as the state concludes.
// State Constructors often store data that will be used during the execution of the State.
public class State
{
	// A reference to the State Machine processing the state.
	public StateMachine state_machine;
	
	public virtual void OnStart() {}
	public virtual void OnUpdate(float time_delta_fraction) {} // time_delta_fraction is a float near 1.0 indicating how much more / less time this frame took than expected.
	public virtual void OnFinish() {}
	
	// States may call ConcludeState on themselves to end their processing.
	public void ConcludeState() { state_machine.Reset(); }
}

// A State that takes a renderer and a sprite, and implements idling behavior.
// The state is capable of transitioning to a walking state upon key press.
public class StateIdleWithSprite : State
{
	PlayerControl pc;
	SpriteRenderer renderer;
	Sprite sprite;
	
	public StateIdleWithSprite(PlayerControl pc, SpriteRenderer renderer, Sprite sprite)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.sprite = sprite;
	}
	
	public override void OnStart()
	{
		renderer.sprite = sprite;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		// Transition to walking animations on key press.
		if(Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
		if(Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
		if(Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));
		if(Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
	}
}

// A State for playing an animation until a particular key is released.
// Good for animations such as walking.
public class StatePlayAnimationForHeldKey : State
{
	PlayerControl pc;
	SpriteRenderer renderer;
	KeyCode key;
	Sprite[] animation;
	int animation_length;
	float animation_progression;
	float animation_start_time;
	int fps;
	
	public StatePlayAnimationForHeldKey(PlayerControl pc, SpriteRenderer renderer, Sprite[] animation, int fps, KeyCode key)
	{
		this.pc = pc;
		this.renderer = renderer;
		this.key = key;
		this.animation = animation;
		this.animation_length = animation.Length;
		this.fps = fps;
		
		if(this.animation_length <= 0)
			Debug.LogError("Empty animation submitted to state machine!");
	}
	
	public override void OnStart()
	{
		animation_start_time = Time.time;
	}
	
	public override void OnUpdate(float time_delta_fraction)
	{
		if(pc.current_state == EntityState.ATTACKING)
			return;

		if(this.animation_length <= 0)
		{
			Debug.LogError("Empty animation submitted to state machine!");
			return;
		}
		
		// Modulus is necessary so we don't overshoot the length of the animation.
		int current_frame_index = ((int)((Time.time - animation_start_time) / (1.0 / fps)) % animation_length);
		renderer.sprite = animation[current_frame_index];
		
		// If another key is pressed, we need to transition to a different walking animation.
		if(Input.GetKeyDown(KeyCode.DownArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_down, 6, KeyCode.DownArrow));
		else if(Input.GetKeyDown(KeyCode.UpArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_up, 6, KeyCode.UpArrow));
		else if(Input.GetKeyDown(KeyCode.RightArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_right, 6, KeyCode.RightArrow));
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
			state_machine.ChangeState(new StatePlayAnimationForHeldKey(pc, renderer, pc.link_run_left, 6, KeyCode.LeftArrow));
		
		// If we detect the specified key has been released, return to the idle state.
		else if(!Input.GetKey(key))
			state_machine.ChangeState(new StateIdleWithSprite(pc, renderer, animation[1]));
	}
}

public class StateLinkNormalMovement : State
{
	PlayerControl pc;
	
	public StateLinkNormalMovement(PlayerControl pc)
	{
		this.pc = pc;
	}

	public override void OnUpdate(float time_delta_fraction)
	{
		float horizontal_input = Input.GetAxis("Horizontal");
		float vertical_input = Input.GetAxis("Vertical");

		if(horizontal_input != 0.0f)
		{
			if (Mathf.Round((pc.GetComponent<Transform>().position.y * 10) % 5) != 0 && Mathf.Round((pc.GetComponent<Transform>().position.y * 10) % 5) != 5)
			{
				//Debug.Log("Not Y aligned! - " + Mathf.Round((pc.GetComponent<Transform>().position.y * 10) % 5));
				vertical_input = .75f;
				if (Mathf.Round((pc.GetComponent<Transform>().position.y * 10) % 5) < 2.5)
				{
					vertical_input = -vertical_input;
				}
			}
			else
			{
				vertical_input = 0.0f;
			}
		}
		else if(vertical_input != 0.0f)
		{
			if (Mathf.Round((pc.GetComponent<Transform>().position.x * 10) % 5) != 0 && Mathf.Round((pc.GetComponent<Transform>().position.x * 10) % 5) != 5)
			{
				//Debug.Log("Not H aligned! - " + Mathf.Round((pc.GetComponent<Transform>().position.x * 10) % 5));
				horizontal_input = .75f;
				if (Mathf.Round((pc.GetComponent<Transform>().position.x * 10) % 5) < 2.5)
				{
					horizontal_input = -horizontal_input;
				}
			}
		}

		pc.GetComponent<Rigidbody>().velocity = new Vector3(horizontal_input, vertical_input, 0) * pc.walking_velocity * time_delta_fraction;

		if(horizontal_input > 0.0f)
		{
			pc.current_direction = Direction.EAST;
		}
		else if(horizontal_input < 0.0f)
		{
			pc.current_direction = Direction.WEST;
		}
		else if (vertical_input > 0.0f)
		{
			pc.current_direction = Direction.NORTH;
		}
		else if (vertical_input < 0.0f)
		{
			pc.current_direction = Direction.SOUTH;
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			state_machine.ChangeState(new StateLinkAttack(pc, pc.selected_weapon_prefab, 15));
		}
	}
}

public class StateLinkAttack : State
{
	PlayerControl pc;
	GameObject weapon_prefab;
	GameObject weapon_instance;
	float cooldown = 0.0f;

	public StateLinkAttack(PlayerControl pc, GameObject weapon_prefab, int cooldown)
	{
		this.pc = pc;
		this.weapon_prefab = weapon_prefab;
		this.cooldown = cooldown;
	}

	public override void OnStart()
	{
		pc.current_state = EntityState.ATTACKING;

		pc.GetComponent<Rigidbody>().velocity = Vector3.zero;

		weapon_instance = Object.Instantiate(weapon_prefab, pc.transform.position, Quaternion.identity) as GameObject;

		Vector3 direction_offset = Vector3.zero;
		Vector3 direction_eulerAngle = Vector3.zero;

		if(pc.current_direction == Direction.NORTH)
		{
			direction_offset = new Vector3(0, 1, 0);
			direction_eulerAngle = new Vector3(0, 0, 90);
		}
		else if (pc.current_direction == Direction.EAST)
		{
			direction_offset = new Vector3(1, 0, 0);
			direction_eulerAngle = new Vector3(0, 0, 0);
		}
		else if (pc.current_direction == Direction.SOUTH)
		{
			direction_offset = new Vector3(0, -1, 0);
			direction_eulerAngle = new Vector3(0, 0, 270);
		}
		else if (pc.current_direction == Direction.WEST)
		{
			direction_offset = new Vector3(-1, 0, 0);
			direction_eulerAngle = new Vector3(0, 0, 180);
		}

		weapon_instance.transform.position += direction_offset;
		Quaternion new_weapon_rotation = new Quaternion();
		new_weapon_rotation.eulerAngles = direction_eulerAngle;
		weapon_instance.transform.rotation = new_weapon_rotation;
	}

	public override void OnUpdate(float time_delta_fraction)
	{
		cooldown -= time_delta_fraction;
		if(cooldown <= 0)
		{
			ConcludeState();
		}
	}

	public override void OnFinish()
	{
		pc.current_state = EntityState.NORMAL;
		Object.Destroy(weapon_instance);
	}
}

public class StateLinkTransition : State
{
	PlayerControl pc;
	Direction dir;
	Vector3 camInitialPos;
	Vector3 camFinalPos;
	Vector3 camTranslation;
	Vector3 playerInitialLocation;
	Vector3 doorExitLocation;
	Vector3 vel;

	public StateLinkTransition(PlayerControl pc, Direction dir)
	{
		this.pc = pc;
		this.dir = dir;
		playerInitialLocation = pc.transform.position;
		camInitialPos = ShowMapOnCamera.S.transform.position;
	}

	public override void OnStart()
	{
		pc.current_state = EntityState.TRANSITIONING;
		pc.GetComponent<SpriteRenderer>().sortingOrder = -1;

		if (dir == Direction.EAST)
		{
			pc.roomX++;
			camTranslation = new Vector3(ShowMapOnCamera.S.screenSize.x, 0, 0);
		}
		else if (dir == Direction.WEST)
		{
			pc.roomX--;
			camTranslation = new Vector3(-ShowMapOnCamera.S.screenSize.x, 0, 0);
		}
		else if (dir == Direction.NORTH)
		{
			pc.roomY++;
			camTranslation = new Vector3(0, ShowMapOnCamera.S.screenSize.y - ShowMapOnCamera.S.tileClearOverage, 0);
		}
		else //dir == Direction.SOUTH
		{
			pc.roomY--;
			camTranslation = new Vector3(0, -ShowMapOnCamera.S.screenSize.y + ShowMapOnCamera.S.tileClearOverage, 0);
		}

		camFinalPos = camInitialPos + camTranslation;
	}

	public override void OnUpdate(float time_delta_fraction)
	{
		if (dir == Direction.EAST && ShowMapOnCamera.S.transform.position.x < camFinalPos.x)
		{
			vel = new Vector3(1f, 0, 0);
			camTranslation = new Vector3(ShowMapOnCamera.S.screenSize.x, 0, 0);
			ShowMapOnCamera.S.GetComponent<Rigidbody>().velocity = vel * pc.walking_velocity * time_delta_fraction;
			doorExitLocation = new Vector3(camFinalPos.x - 5.5f, playerInitialLocation.y, playerInitialLocation.z);
		}
		else if (dir == Direction.WEST && ShowMapOnCamera.S.transform.position.x > camFinalPos.x)
		{
			vel = new Vector3(-1f, 0, 0);
			camTranslation = new Vector3(-ShowMapOnCamera.S.screenSize.x, 0, 0);
			ShowMapOnCamera.S.GetComponent<Rigidbody>().velocity = vel * pc.walking_velocity * time_delta_fraction;
			doorExitLocation = new Vector3(camFinalPos.x + 5.5f, playerInitialLocation.y, playerInitialLocation.z);
		}
		else if (dir == Direction.NORTH && ShowMapOnCamera.S.transform.position.y < camFinalPos.y)
		{
			vel = new Vector3(0, 1f, 0);
			camTranslation = new Vector3(0, ShowMapOnCamera.S.screenSize.y - ShowMapOnCamera.S.tileClearOverage, 0);
			ShowMapOnCamera.S.GetComponent<Rigidbody>().velocity = vel * pc.walking_velocity * time_delta_fraction;
			doorExitLocation = new Vector3(playerInitialLocation.x, camFinalPos.y - 5f, playerInitialLocation.z);
		}
		else if (dir == Direction.SOUTH && ShowMapOnCamera.S.transform.position.y > camFinalPos.y)
		{
			vel = new Vector3(0, -1f, 0);
			camTranslation = new Vector3(0, -ShowMapOnCamera.S.screenSize.y + ShowMapOnCamera.S.tileClearOverage, 0);
			ShowMapOnCamera.S.GetComponent<Rigidbody>().velocity = vel * pc.walking_velocity * time_delta_fraction;
			doorExitLocation = new Vector3(playerInitialLocation.x, camFinalPos.y + 2f, playerInitialLocation.z);
		}
		else
		{
			ConcludeState();
		}
	}

	public override void OnFinish()
	{
		pc.GetComponent<SpriteRenderer>().sortingOrder = 10;
		pc.GetComponent<Transform>().position = doorExitLocation;

		pc.transform.position = new Vector3(pc.transform.position.x, pc.transform.position.y, 0);
		ShowMapOnCamera.S.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ShowMapOnCamera.S.transform.position = camFinalPos;

		Debug.Log("X:" + pc.roomX + " Y:" + pc.roomY);
		pc.roomHandle(pc.roomX, pc.roomY);
		pc.current_state = EntityState.NORMAL;
	}
}

public class StateLinkPush : State
{
	PlayerControl pc;
	GameObject pushBlock;
	GameObject doorBlock;
	Vector3 initialLocation;
	Direction dir;

	public StateLinkPush(PlayerControl pc, GameObject pushBlock, GameObject doorBlock = null)
	{
		this.pc = pc;
		this.pushBlock = pushBlock;
		this.doorBlock = doorBlock;
		initialLocation = pushBlock.transform.position;
		dir = pc.current_direction;
	}

	public override void OnStart()
	{
		pc.current_state = EntityState.PUSHING;

		pc.GetComponent<Rigidbody>().velocity = Vector3.zero;

		RigidbodyConstraints posFreeze = RigidbodyConstraints.None;
		if(dir == Direction.NORTH || dir == Direction.SOUTH)
		{
			posFreeze = RigidbodyConstraints.FreezePositionX;
		}
		else
		{
			posFreeze = RigidbodyConstraints.FreezePositionY;
		}
		pushBlock.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | posFreeze;
	}

	public override void OnUpdate(float time_delta_fraction)
	{
		if (!isOver())
		{
			Vector3 vel;
			if(dir == Direction.NORTH)
			{
				vel = new Vector3(0, 1f, 0);
			}
			else if(dir == Direction.SOUTH)
			{
				vel = new Vector3(0, -1f, 0);
			}
			else if(dir == Direction.EAST)
			{
				vel = new Vector3(1f, 0, 0);
			}
			else
			{
				vel = new Vector3(-1f, 0, 0);
			}
			pc.GetComponent<Rigidbody>().velocity = vel;
			pushBlock.GetComponent<Rigidbody>().velocity = vel;
		}
		else
		{
			ConcludeState();
		}
	}

	public override void OnFinish()
	{
		pushBlock.GetComponent<Rigidbody>().velocity = Vector3.zero;
		pushBlock.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		if (doorBlock != null)
		{
			doorBlock.gameObject.GetComponent<SpriteRenderer>().sprite = pc.mapSprites[51];
			doorBlock.gameObject.GetComponent<BoxCollider>().isTrigger = true;
		}
		pc.current_state = EntityState.NORMAL;
	}

	private bool isOver()
	{
		if (dir == Direction.WEST || dir == Direction.EAST) {
			if (Mathf.Abs(pushBlock.GetComponent<Rigidbody>().position.x - initialLocation.x) >= 1)
			{
				return true;
			}
		}
		else if(dir == Direction.NORTH || dir == Direction.SOUTH)
		{
			if (Mathf.Abs(pushBlock.GetComponent<Rigidbody>().position.y - initialLocation.y) >= 1)
			{
				return true;
			}
		}
		return false;
	}
}
// Additional recommended states:
// StateDeath
// StateDamaged
// StateWeaponSwing
// StateVictory

// Additional control states:
// LinkStunnedState.




