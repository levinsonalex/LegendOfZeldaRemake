using UnityEngine;
using System.Collections;

public class BossScript : EnemyScript {

	public GameObject fireF;
	public GameObject fireU;
	public GameObject fireD;

	void Update(){
		if(Input.GetKeyDown(KeyCode.P)){
			Vector3 fireStart = transform.position;
			Instantiate (fireF,fireStart,Quaternion.identity);
			Instantiate (fireU, fireStart, Quaternion.identity);
			Instantiate (fireD, fireStart, Quaternion.identity);

		}
	}


}
