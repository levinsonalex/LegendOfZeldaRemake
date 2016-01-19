using UnityEngine;
using System.Collections;

public enum Direction {NORTH, EAST, SOUTH, WEST};
public enum EntityState {NORMAL, ATTACKING};

public class PlayerControl : MonoBehaviour {

	public Sprite[] link_run_down;
	public Sprite[] link_run_up;
	public Sprite[] link_run_right;
	public Sprite[] link_run_left;

	StateMachine animation_state_machine;
	StateMachine control_state_machine;
	
	public EntityState current_state = EntityState.NORMAL;
	public Direction current_direction = Direction.SOUTH;

	public GameObject selected_weapon_prefab;

	public float walking_velocity = 1.0f;
	public int rupee_count = 0;

	public static PlayerControl instance;

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
		//Left Door
		else if(coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_51")
		{
			Vector3 trans = new Vector3(- ShowMapOnCamera.S.screenSize.x, 0, 0);
			ShowMapOnCamera.S.transform.Translate(trans);
			Vector3 leftDoorExit = instance.GetComponent<Transform>().position;
			leftDoorExit.x = ShowMapOnCamera.S.transform.position.x + 5;
			instance.GetComponent<Transform>().position = leftDoorExit;
		}
		//Left Door
		else if (coll.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_48")
		{
			Vector3 trans = new Vector3(ShowMapOnCamera.S.screenSize.x, 0, 0);
			ShowMapOnCamera.S.transform.Translate(trans);
			Vector3 rightDoorExit = instance.GetComponent<Transform>().position;
			rightDoorExit.x = ShowMapOnCamera.S.transform.position.x - 5;
			instance.GetComponent<Transform>().position = rightDoorExit;
		}
		else
		{
			print(coll.name);
		}
	}
}
