
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Hud : MonoBehaviour {

	public Text Rupee_Text;
	public Text Key_Text;
	public Text Heart_Text;
    public GameObject Compass_Inv;
    public GameObject Map_Inv;
    public GameObject Sword_Inv;
    public GameObject Bow_Inv;
    public GameObject Boomerang_Inv;
	public GameObject LHeartPrefab;
	public GameObject RHeartPrefab;
    public GameObject Cursor_Inv;
    public List<float> cursorLocations;
    public int cursorIndex = 0;

    public static Hud instance;

    // Use this for initialization
    void Start () {
        if (instance != null)
        {
            Debug.LogError("Multiple Hud objects detected!");
        }
        instance = this;
		Cursor_Inv.GetComponent<Image>().enabled = false;
	}

	// Update is called once per frame
	void Update()
	{
		int num_player_rupees = PlayerControl.instance.rupee_count;
		Rupee_Text.text = " × " + num_player_rupees;

		int num_player_keys = PlayerControl.instance.key_count;
		Key_Text.text = " × " + num_player_keys;

		int num_player_hearts = PlayerControl.instance.curHealth;
		Heart_Text.text = " × " + num_player_hearts;

		if (Cursor_Inv.GetComponent<Image>().enabled)
		{
			if (cursorLocations[cursorIndex] == -25)
			{
				PlayerControl.instance.selected_weapon_prefab = PlayerControl.instance.bow_prefab;
			}
			else if (cursorLocations[cursorIndex] == 25)
			{
				PlayerControl.instance.selected_weapon_prefab = PlayerControl.instance.boomerang_prefab;
			}
			else
			{
				Debug.Log("Broken cursor.");
			}
		}
	}
}
