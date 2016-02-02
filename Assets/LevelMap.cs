using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMap : MonoBehaviour {

	public Image 			greenLight;

	Vector2 				oldLinksRoom;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControl.instance.roomX != oldLinksRoom.x || PlayerControl.instance.roomY != oldLinksRoom.y) {
			Vector3 newGreenLightLoc = greenLight.transform.position;
			if (PlayerControl.instance.roomX > oldLinksRoom.x) {
				newGreenLightLoc.x += 11.5f;
			} else if(PlayerControl.instance.roomX < oldLinksRoom.x) {
				newGreenLightLoc.x -= 11.5f;
			} else if(PlayerControl.instance.roomY > oldLinksRoom.y){
				newGreenLightLoc.y += 5f;
			} else if (PlayerControl.instance.roomY < oldLinksRoom.y){
				newGreenLightLoc.y -= 5f;
			}
			oldLinksRoom.x = PlayerControl.instance.roomX;
			oldLinksRoom.y = PlayerControl.instance.roomY;
			greenLight.transform.position = newGreenLightLoc;
		}
	}
}
