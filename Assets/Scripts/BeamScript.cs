using UnityEngine;
using System.Collections;

public class BeamScript : MonoBehaviour {

    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision coll)
    {
        GameObject explosion = Object.Instantiate(explosionPrefab);
        explosion.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
