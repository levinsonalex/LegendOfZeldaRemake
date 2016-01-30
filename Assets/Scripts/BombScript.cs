using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    private float startTime;
    public float fuseTime = 1;
    public GameObject BombExplosion;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime >= fuseTime)
        {
            Explode();
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        GameObject explosion = Object.Instantiate(BombExplosion, transform.position, Quaternion.identity) as GameObject;
    }
}
