using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

	public Text Rupee_Text;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int num_player_rupees = PlayerControl.instance.rupee_count;
		Rupee_Text.text = " × " + num_player_rupees;
	}
}
