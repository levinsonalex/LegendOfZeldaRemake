using UnityEngine;
using System.Collections;

public class Typewriter : MonoBehaviour {

	private string str = "EASTMOST PENNINSULA \n IS THE SECRET.";
	public float tick = .25f;
	private int index = 0;
	private float count = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerControl.instance.eastMostTypewriterOnSwitch)
		{
			if (index < str.Length)
			{
				if (count >= tick)
				{
					gameObject.GetComponent<TextMesh>().text += str[index++];
					count = 0;
				}
				else
				{
					count += Time.deltaTime;
				}
			}
		}
		else
		{
			ResetText();
		}
	}

	void ResetText()
	{
		gameObject.GetComponent<TextMesh>().text = "";
		index = 0;
		count = 0;
	}
}
