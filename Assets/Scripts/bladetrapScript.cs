using UnityEngine;
using System.Collections;

public class bladetrapScript : MonoBehaviour {

	public float positionX;
	public float positionY;

	// Use this for initialization
	void Start () {
		positionX = transform.position.x;
		positionY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if(Mathf.Abs(PlayerControl.instance.transform.position.x - positionX) < 1)
		{

		}
		else if(Mathf.Abs(PlayerControl.instance.transform.position.y - positionY) < 1)
		{

		}
		
	}
}

//public class StateTrapTriggered : State
//{
//	PlayerControl pc;
//	GameObject bladeTrap;
//	float cooldown = 0.0f;

//	public StateLinkAttack(PlayerControl pc, GameObject weapon_prefab, int cooldown)
//	{
//		this.pc = pc;
//		this.weapon_prefab = weapon_prefab;
//		this.cooldown = cooldown;
//	}

//	public override void OnStart()
//	{

//		if (pc == null) //Boomerang catch workaround.
//		{
//			PlayerControl.instance.GetComponent<Rigidbody>().velocity = Vector3.zero;
//			return;
//		}
//		pc.current_state = EntityState.ATTACKING;
//		pc.GetComponent<Rigidbody>().velocity = Vector3.zero;


//		Vector3 direction_offset = Vector3.zero;
//		Vector3 direction_eulerAngle = Vector3.zero;

//		if (pc.current_direction == Direction.NORTH)
//		{
//			direction_offset = new Vector3(0, 1, 0);
//			direction_eulerAngle = new Vector3(0, 0, 90);
//		}
//		else if (pc.current_direction == Direction.EAST)
//		{
//			direction_offset = new Vector3(1, 0, 0);
//			direction_eulerAngle = new Vector3(0, 0, 0);
//		}
//		else if (pc.current_direction == Direction.SOUTH)
//		{
//			direction_offset = new Vector3(0, -1, 0);
//			direction_eulerAngle = new Vector3(0, 0, 270);
//		}
//		else if (pc.current_direction == Direction.WEST)
//		{
//			direction_offset = new Vector3(-1, 0, 0);
//			direction_eulerAngle = new Vector3(0, 0, 180);
//		}

//		Quaternion new_weapon_rotation = new Quaternion();
//		new_weapon_rotation.eulerAngles = direction_eulerAngle;

//		if (weapon_prefab == pc.sword_prefab || (weapon_prefab == pc.bow_prefab && GameObject.FindGameObjectWithTag("Arrow") == null))
//		{
//			weapon_instance = Object.Instantiate(weapon_prefab, pc.transform.position, Quaternion.identity) as GameObject;
//			weapon_instance.transform.position += direction_offset;
//			weapon_instance.transform.rotation = new_weapon_rotation;
//		}
//		if (pc.== pc.maxHealth && weapon_prefab.gameObject.tag == "Sword" && GameObject.FindGameObjectWithTag("Beam") == null)
//		{
//			beam = Object.Instantiate(pc.beam_prefab, pc.transform.position, Quaternion.identity) as GameObject;
//			beam.transform.position += direction_offset;
//			beam.transform.rotation = new_weapon_rotation;

//			beam.GetComponent<Rigidbody>().velocity = direction_offset * pc.projectile_velocity;
//		}
//		else if (weapon_prefab == pc.bow_prefab && GameObject.FindGameObjectWithTag("Arrow") == null)
//		{
//			GameObject arrow = Object.Instantiate(pc.arrow_prefab, pc.transform.position, Quaternion.identity) as GameObject;
//			arrow.transform.position += direction_offset;
//			arrow.transform.rotation = new_weapon_rotation;

//			arrow.GetComponent<Rigidbody>().velocity = direction_offset * pc.projectile_velocity;
//		}
//		else if (weapon_prefab == pc.boomerang_prefab && GameObject.FindGameObjectWithTag("Boomerang") == null)
//		{
//			GameObject boomerang = Object.Instantiate(pc.boomerang_prefab, pc.transform.position, Quaternion.identity) as GameObject;
//			boomerang.transform.position += direction_offset;
//			boomerang.transform.rotation = new_weapon_rotation;

//			boomerang.GetComponent<Rigidbody>().velocity = direction_offset * pc.boomerang_velocity;
//		}
//		else if (weapon_prefab == pc.bomb_prefab && pc.bomb_count > 0)
//		{
//			pc.bomb_count--;
//			GameObject bomb = Object.Instantiate(pc.bomb_prefab, pc.transform.position, Quaternion.identity) as GameObject;
//			bomb.transform.position += direction_offset;

//			if (pc.bomb_count <= 0)
//			{
//				Hud.instance.cursorLocations.Remove(0);
//				Hud.instance.Bomb_Inv.GetComponent<Image>().color = new Color(64f / 255f, 64f / 255f, 64f / 255f, 1);

//				float localCursorLoc = pc.getNextCursorLocation();
//				if (Hud.instance.cursorLocations.Count != 0)
//				{
//					Hud.instance.Cursor_Inv.transform.localPosition = new Vector3(localCursorLoc, Hud.instance.Cursor_Inv.transform.localPosition.y, 0);
//				}
//				else
//				{
//					Hud.instance.Cursor_Inv.GetComponent<Image>().enabled = false;
//					pc.selected_weapon_prefab = null;
//				}
//			}
//		}
//	}

//	public override void OnUpdate(float time_delta_fraction)
//	{
//		cooldown -= time_delta_fraction;
//		if (cooldown <= 0)
//		{
//			ConcludeState();
//		}
//	}

//	public override void OnFinish()
//	{
//		if (pc == null) //Boomerang catch workaround
//		{
//			return;
//		}
//		pc.current_state = EntityState.NORMAL;
//		if (weapon_instance != null && weapon_instance.gameObject.name != "wooden_boomerang(Clone)")
//		{
//			Object.Destroy(weapon_instance);
//		}
//	}
//}