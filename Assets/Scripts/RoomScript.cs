using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour {
    public List<GameObject> enemiesList;
    public GameObject drop;
    public GameObject unlock;

    public void Update()
    {
        if (drop != null)
        {
            if (enemiesList.Count == 0)
            {
                drop.SetActive(true);
            }
        }
        if (unlock != null)
        {
            if(enemiesList.Count > 0)
            {
                unlock.SetActive(true);
            }
            else
            {
                unlock.SetActive(false);
            }
        }
    }
}
