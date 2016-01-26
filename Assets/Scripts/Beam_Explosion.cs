using UnityEngine;
using System.Collections;

public class Beam_Explosion : MonoBehaviour {

    public GameObject TL;
    public GameObject TR;
    public GameObject BL;
    public GameObject BR;
    public float explosion_speed = .25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Mathf.Abs(TL.transform.localPosition.x) > 1)
        {
            Destroy(gameObject);
        }
        TL.transform.Translate(new Vector3(-1, 1, 0) * explosion_speed * Time.deltaTime);
        TR.transform.Translate(new Vector3(1, 1, 0) * explosion_speed * Time.deltaTime);
        BL.transform.Translate(new Vector3(-1, -1, 0) * explosion_speed * Time.deltaTime);
        BR.transform.Translate(new Vector3(1, -1, 0) * explosion_speed * Time.deltaTime);
    }
}
