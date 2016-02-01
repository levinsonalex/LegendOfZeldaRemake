using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    static Sprite[] spriteArray;

    public Texture2D spriteTexture;
    public int x, y;
    public int tileNum;
    private BoxCollider bc;
    private Material mat;

    private SpriteRenderer sprend;

    public

    void Awake() {
        if (spriteArray == null) {
            spriteArray = Resources.LoadAll<Sprite>(spriteTexture.name);
        }

        bc = GetComponent<BoxCollider>();

        sprend = GetComponent<SpriteRenderer>();
        //Renderer rend = gameObject.GetComponent<Renderer>();
        //mat = rend.material;
    }

    public void SetTile(int eX, int eY, int eTileNum = -1) {

        if (x == eX && y == eY) return; // Don't move this if you don't have to. - JB

        x = eX;
        y = eY;
        transform.localPosition = new Vector3(x, y, 0);
        gameObject.name = x.ToString("D3") + "x" + y.ToString("D3");

        tileNum = eTileNum;
        if (tileNum == -1 && ShowMapOnCamera.S != null) {
            tileNum = ShowMapOnCamera.MAP[x, y];
            if (tileNum == 0) {
                ShowMapOnCamera.PushTile(this);
            }
        }

        sprend.sprite = spriteArray[tileNum];

        if (ShowMapOnCamera.S != null) SetCollider();
        //TODO: Add something for destructibility - JB

        gameObject.SetActive(true);
        if (ShowMapOnCamera.S != null) {
            if (ShowMapOnCamera.MAP_TILES[x, y] != null) {
                if (ShowMapOnCamera.MAP_TILES[x, y] != this) {
                    ShowMapOnCamera.PushTile(ShowMapOnCamera.MAP_TILES[x, y]);
                }
            } else {
                ShowMapOnCamera.MAP_TILES[x, y] = this;
            }
        }
    }


    // Arrange the collider for this tile
    void SetCollider() {
        // Collider info from collisionData
        bc.enabled = true;
        char c = ShowMapOnCamera.S.collisionS[tileNum];
        if (!PlayerControl.customMap &&
            (bc.gameObject.name == "023x038" ||
            bc.gameObject.name == "022x060"))
        {
            setPushableCollider(ref bc);
        }
        else if (PlayerControl.customMap &&
            (bc.gameObject.name == "025x005" || //R
             bc.gameObject.name == "022x005" || //L
             bc.gameObject.name == "038x024" || //R
             bc.gameObject.name == "036x030" || //L
             bc.gameObject.name == "041x030" || //L
             bc.gameObject.name == "043x024" || //R
             bc.gameObject.name == "056x027" || //L
             bc.gameObject.name == "021x036" || //L
             bc.gameObject.name == "025x036" || //L
             bc.gameObject.name == "021x040" || //R
             bc.gameObject.name == "026x040" || //R
             bc.gameObject.name == "019x038" || //U
             bc.gameObject.name == "028x038" || //D
             bc.gameObject.name == "035x038" || //R <--
             bc.gameObject.name == "035x036" || //D
             bc.gameObject.name == "037x036" || //R
             bc.gameObject.name == "037x038" || //U
             bc.gameObject.name == "039x038" || //R
             bc.gameObject.name == "039x036" || //D
             bc.gameObject.name == "041x036" || //R
             bc.gameObject.name == "041x038" || //U
             bc.gameObject.name == "043x038" || //R
             bc.gameObject.name == "043x040" || //U
             bc.gameObject.name == "041x040" //L
             )) //bc.gameObject.name == "x" ||
        {
            setPushableCollider(ref bc);
        }
        else if(bc.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_99")
		{
			bc.gameObject.tag = "Untagged";
			bc.enabled = false;
		}
        else if(c == 'F')
        {
            bc.center = Vector3.zero;
            bc.size = Vector3.one;
            bc.gameObject.tag = "Flyable";
            bc.gameObject.layer = 10;
        }
        else if (c == 'S') // Solid
        {
            bc.center = Vector3.zero;
            bc.size = Vector3.one;
            bc.gameObject.tag = "Untagged";
        }
        else
        {
            bc.gameObject.tag = "Untagged";
            bc.enabled = false;
		}
	}
    
    public void setPushableCollider(ref BoxCollider BC)
    {
        BC.center = Vector3.zero;
        BC.size = Vector3.one;
        BC.gameObject.tag = "Pushable";
        BC.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
        BC.gameObject.AddComponent<Rigidbody>();
        BC.gameObject.GetComponent<Rigidbody>().useGravity = false;
        BC.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        BC.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        BC.gameObject.layer = 10;
    }	
}