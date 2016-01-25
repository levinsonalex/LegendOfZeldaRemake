using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    static Sprite[]         spriteArray;

    public Texture2D        spriteTexture;
	public int				x, y;
	public int				tileNum;
	private BoxCollider		bc;
    private Material        mat;

    private SpriteRenderer  sprend;

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
        gameObject.name = x.ToString("D3")+"x"+y.ToString("D3");

		tileNum = eTileNum;
		if (tileNum == -1 && ShowMapOnCamera.S != null) {
			tileNum = ShowMapOnCamera.MAP[x,y];
			if (tileNum == 0) {
				ShowMapOnCamera.PushTile(this);
			}
		}

        sprend.sprite = spriteArray[tileNum];

		if (ShowMapOnCamera.S != null) SetCollider();
        //TODO: Add something for destructibility - JB

        gameObject.SetActive(true);
		if (ShowMapOnCamera.S != null) {
			if (ShowMapOnCamera.MAP_TILES[x,y] != null) {
				if (ShowMapOnCamera.MAP_TILES[x,y] != this) {
					ShowMapOnCamera.PushTile( ShowMapOnCamera.MAP_TILES[x,y] );
				}
			} else {
				ShowMapOnCamera.MAP_TILES[x,y] = this;
			}
		}
	}


	// Arrange the collider for this tile
	void SetCollider() {
		// Collider info from collisionData
		bc.enabled = true;
		char c = ShowMapOnCamera.S.collisionS[tileNum];
		if (bc.gameObject.name == "023x038")
        {
            bc.center = Vector3.zero;
            bc.size = Vector3.one;
            bc.gameObject.tag = "Pushable";
			bc.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
			bc.gameObject.AddComponent<Rigidbody>();
			bc.gameObject.GetComponent<Rigidbody>().useGravity = false;
			bc.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
		else if(bc.gameObject.name == "022x060")
		{
			bc.center = Vector3.zero;
			bc.size = Vector3.one;
			bc.gameObject.tag = "Pushable";
			bc.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
			bc.gameObject.AddComponent<Rigidbody>();
			bc.gameObject.GetComponent<Rigidbody>().useGravity = false;
			bc.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
		}
		else if(bc.gameObject.GetComponent<SpriteRenderer>().sprite.name == "spriteMap_99")
		{
			bc.gameObject.tag = "Untagged";
			bc.enabled = false;
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
}