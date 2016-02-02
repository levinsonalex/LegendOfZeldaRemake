using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoomerangScript : MonoBehaviour
{

    public Vector3 initialLocation;
    public GameObject bombExplosion;
    private bool bombGrab = false;
    public bool turnAround = false;

    // Use this for initialization
    void Start()
    {
        initialLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!turnAround && Vector3.Distance(initialLocation, transform.position) > 4)
        {
            turnAround = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        else if (turnAround)
        {
            Vector3 returnVec = PlayerControl.instance.transform.position - transform.position;
            returnVec.Normalize();
            GetComponent<Rigidbody>().velocity = returnVec * PlayerControl.instance.boomerang_velocity;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (bombGrab)
            {
                Instantiate(bombExplosion, PlayerControl.instance.transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
            PlayerControl.instance.catchBoomerang();
        }
        else if (!turnAround)
        {
            turnAround = true;
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Rupee")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.rupee_count++;
        }
        else if (coll.gameObject.tag == "Key")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.key_count++;
        }
        else if (coll.gameObject.tag == "Heart")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.curHealth += 2;
            if (PlayerControl.instance.curHealth > PlayerControl.instance.maxHealth)
            {
                PlayerControl.instance.curHealth = PlayerControl.instance.maxHealth;
            }
        }
        else if (coll.gameObject.tag == "Map")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.hasMap = true;
            Hud.instance.Map_Inv.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else if (coll.gameObject.tag == "Compass")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.hasCompass = true;
            Hud.instance.Compass_Inv.GetComponent<Image>().color = new Color(1, 1, 1);
        }
        else if (coll.gameObject.tag == "Bow")
        {
            Destroy(coll.gameObject);
            PlayerControl.instance.hasBow = true;
            Hud.instance.Bow_Inv.GetComponent<Image>().color = new Color(1, 1, 1);
            Hud.instance.cursorLocations.Add(-50);
            if (!Hud.instance.Cursor_Inv.GetComponent<Image>().enabled)
            {
                Hud.instance.Cursor_Inv.GetComponent<Image>().enabled = true;
                Hud.instance.Cursor_Inv.transform.localPosition = new Vector3(-50, Hud.instance.Cursor_Inv.transform.localPosition.y, Hud.instance.Cursor_Inv.transform.localPosition.z);
            }
        }
        else if (coll.gameObject.tag == "Bomb")
        {
            Destroy(coll.gameObject);
            bombGrab = true;
            PlayerControl.instance.curHealth -= 1;
        }
    }
}
