using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConstructor : MonoBehaviour
{
    public Camera[] focusCameras;
    public SpriteRenderer roomBorder;
    public SpriteRenderer roomWall;
    public SpriteRenderer roomFloor;
    public BoxCollider2D[] voids;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    public void CreateRoom(Sprite borderSprite, Sprite wallSprite, Sprite floorSprite, int width, int height)
    {
        if (width >= height * (16f / 9))
        {
            foreach (Camera focusCamera in focusCameras)
            {
                focusCamera.orthographicSize = width / 3 + 2;
            }
        }
        else
        {
            foreach (Camera focusCamera in focusCameras)
            {
                focusCamera.orthographicSize = height / 2 + 2;
            }
        }

        roomBorder.sprite = borderSprite;
        roomWall.sprite = wallSprite;
        roomFloor.sprite = floorSprite;

        roomBorder.size = new Vector2(width + 1, height + 1);
        roomWall.transform.localPosition = new Vector3(0, (height-3)/2f, 0.5f);
        roomWall.size = new Vector2(width, 3);
        roomWall.GetComponent<BoxCollider2D>().size = new Vector2(width, 3);
        roomFloor.transform.localPosition = new Vector3(0, (-3)/2f, 0.5f);
        roomFloor.size = new Vector2(width, height - 3);
        roomFloor.GetComponent<BoxCollider2D>().size = new Vector2(width, height - 3);

        if (voids.Length != 0)
        {
            voids[0].offset = new Vector2(0, 10f + height/2f); //N
            voids[1].offset = new Vector2(22.5f + width/2f, 0); //E
            voids[2].offset = new Vector2(0, -10f - height / 2f); //S
            voids[3].offset = new Vector2(-22.5f - width/2f, 0); //W
        }
    }
}
