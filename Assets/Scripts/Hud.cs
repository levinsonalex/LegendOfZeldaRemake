
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int num_player_rupees = PlayerControl.instance.rupee_count;
		Rupee_Text.text = " × " + num_player_rupees;

		int num_player_keys = PlayerControl.instance.key_count;
		Key_Text.text = " × " + num_player_keys;

		int num_player_hearts = PlayerControl.instance.curHealth;
		Heart_Text.text = " × " + num_player_hearts;

        if (PlayerControl.instance.hasCompass)
        {
            Compass_Inv.GetComponent<Image>().color = new Color(255, 255, 255);
        }
        if (PlayerControl.instance.hasMap)
        {
            Map_Inv.GetComponent<Image>().color = new Color(255, 255, 255);
        }
    }
}
