using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementTool : MonoBehaviour
{
    public int numOverlap = 0;
    public List<Item> possibleSurfaces = new List<Item>();
    public ItemInfo selectedItem;
    public bool placementReady = false;
    public int selectedStyle = 0;
    public int selectedRotation = 0;
    public bool limitedUse = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 0);

        if (numOverlap > 0)
        {
            if (CheckIfSurfaceAvailable())
            {
                GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.75f);
                HighlightPlacementTool(Color.green);
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.75f);
                HighlightPlacementTool(Color.red);
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.75f);
            HighlightPlacementTool(Color.green);
        }

        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
        if (CheckIfSurfaceAvailable())
        {
            GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * -10) + 50;
        }

        if (Input.GetMouseButtonDown(0) && !UIHoverListener.current.isUIOverride && (numOverlap == 0 || CheckIfSurfaceAvailable()) && selectedItem != null && placementReady)
        {
            if (selectedItem.targetLayer == "RugObjects")
            {
                GameObject newItem = Instantiate(selectedItem.itemPrefab[selectedRotation], new Vector3(transform.position.x, transform.position.y, 0.5f), transform.rotation) as GameObject;
                newItem.GetComponent<Item>().rotation = selectedRotation;
                newItem.GetComponent<Item>().SelectStyle(selectedStyle);
                newItem.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newItem.transform.position.y * -10);
                newItem.GetComponent<Item>().rotation = selectedRotation;
            }
            else if (CheckIfSurfaceAvailable())
            {
                GameObject newItem = Instantiate(selectedItem.itemPrefab[selectedRotation], new Vector3(transform.position.x, transform.position.y, -0.5f), transform.rotation) as GameObject;
                newItem.GetComponent<Item>().rotation = selectedRotation;
                newItem.GetComponent<Item>().SelectStyle(selectedStyle);
                newItem.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newItem.transform.position.y * -10) + 50;
                possibleSurfaces[0].itemsOnSurface.Add(newItem.GetComponent<Item>());
                newItem.GetComponent<Item>().surface = possibleSurfaces[0];
                newItem.GetComponent<Item>().rotation = selectedRotation;
            }
            else
            {
                GameObject newItem = Instantiate(selectedItem.itemPrefab[selectedRotation], new Vector3(transform.position.x, transform.position.y, 0), transform.rotation) as GameObject;
                newItem.GetComponent<Item>().rotation = selectedRotation;
                newItem.GetComponent<Item>().SelectStyle(selectedStyle);
                newItem.GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(newItem.transform.position.y * -10);
                newItem.GetComponent<Item>().rotation = selectedRotation;
            }

            //MissionUIManager.current.IncreaseCriteriaProgress(selectedItem);
            DesignManager.current.AdjustBudget(selectedItem.cost * -1);
            foreach (ColorType colorType in selectedItem.colors)
            {
                DesignManager.current.AdjustColors(colorType, "add");
            }
            foreach (ThemeType themeType in selectedItem.colors)
            {
                DesignManager.current.AdjustThemes(themeType, "add");
            }

            if (limitedUse && placementReady)
            {
                gameObject.SetActive(false);
                limitedUse = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }
    }

    public void RotateItem()
    {
        selectedRotation += 1;
        if (selectedRotation >= selectedItem.itemPrefab.Length)
        {
            selectedRotation = 0;
        }
        SetSelectedItem(selectedItem, selectedStyle, selectedRotation);
    }

    public void SetSelectedItem(ItemInfo itemInfo, int styleIdx = 0, int rotation = 0)
    {
        Debug.Log(itemInfo);
        selectedItem = itemInfo;
        selectedStyle = styleIdx;
        selectedRotation = rotation;
        //GetComponent<SpriteRenderer>().sprite = itemInfo.itemPrefab[rotation].GetComponent<SpriteRenderer>().sprite;
        GetComponent<SpriteRenderer>().sprite = itemInfo.itemStyles[styleIdx].sprites[rotation];
        GetComponent<SpriteRenderer>().flipX = itemInfo.itemPrefab[rotation].GetComponent<SpriteRenderer>().flipX;
        GetComponent<BoxCollider2D>().offset = itemInfo.itemPrefab[rotation].transform.GetChild(0).GetComponent< BoxCollider2D>().offset;
        GetComponent<BoxCollider2D>().size = itemInfo.itemPrefab[rotation].transform.GetChild(0).GetComponent<BoxCollider2D>().size;
        GetComponent<SpriteRenderer>().sortingLayerID = itemInfo.itemPrefab[rotation].GetComponent<SpriteRenderer>().sortingLayerID;
        gameObject.layer = LayerMask.NameToLayer(itemInfo.targetLayer);
    }

    public IEnumerator DelayPlacement()
    {
        placementReady = false;
        yield return new WaitForSeconds(0.1f);
        placementReady = true;
    }

    public bool CheckIfSurfaceAvailable()
    {
        foreach (Item item in possibleSurfaces)
        {
            if (CheckIfBoxContainsBox(GetComponent<BoxCollider2D>(), item.transform.GetChild(1).GetComponent<BoxCollider2D>()))
            {
                //Debug.Log("ya");
                return true;
            }
        }
        //Debug.Log("no");
        return false;
    }

    public bool CheckIfBoxContainsBox(BoxCollider2D smallBox, BoxCollider2D bigBox)
    {
        List<Vector3> smallBoxVertices = new List<Vector3>();
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x - (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y - (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x + (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y - (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x - (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y + (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x + (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y + (smallBox.size.y * 0.5f), 1));

        int i = 0;
        foreach (Vector3 vertex in smallBoxVertices)
        {
            i += 1;
            if (!bigBox.bounds.Contains(vertex))
            {
                if (i == 1)
                {
                    //Debug.Log(vertex + " is out of range, bottom left");
                }
                else if (i == 2)
                {
                    //Debug.Log(vertex + " is out of range, bottom right");
                }
                else if (i == 3)
                {
                    //Debug.Log(vertex + " is out of range, top left");
                }
                else if (i == 4)
                {
                    //Debug.Log(vertex + " is out of range, top right");
                }
                return false;
            }
        }
        return true;
    }

    public void HighlightPlacementTool(Color outlineColor)
    {
        GetComponent<Renderer>().material.SetFloat("_Thickness", 0.01f);
        GetComponent<Renderer>().material.SetColor("_Color", outlineColor);
    }

    /*public void OnCollisionEnter2D(Collision2D collision)
    {
        numOverlap += 1;
        if (collision.gameObject.tag == "SurfaceHitbox")
        {
            if (!possibleSurfaces.Contains(collision.gameObject.GetComponent<BoxCollider2D>()))
            {
                possibleSurfaces.Add(collision.gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        numOverlap -= 1;
        if (collision.gameObject.tag == "SurfaceHitbox")
        {
            if (possibleSurfaces.Contains(collision.gameObject.GetComponent<BoxCollider2D>()))
            {
                possibleSurfaces.Remove(collision.gameObject.GetComponent<BoxCollider2D>());
            }
        }
    }*/

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SurfaceHitbox")
        {
            if (!possibleSurfaces.Contains(collision.transform.parent.GetComponent<Item>()))
            {
                possibleSurfaces.Add(collision.transform.parent.GetComponent<Item>());
            }
        }
        else
        {
            numOverlap += 1;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SurfaceHitbox")
        {
            if (possibleSurfaces.Contains(collision.transform.parent.GetComponent<Item>()))
            {
                possibleSurfaces.Remove(collision.transform.parent.GetComponent<Item>());
            }
        }
        else
        {
            numOverlap -= 1;
        }
    }

    void OnDrawGizmos()
    {
        BoxCollider2D smallBox = GetComponent<BoxCollider2D>();

        List<Vector3> smallBoxVertices = new List<Vector3>();
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x - (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y - (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x + (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y - (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x - (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y + (smallBox.size.y * 0.5f), 1));
        smallBoxVertices.Add(new Vector3(smallBox.transform.position.x + smallBox.offset.x + (smallBox.size.x * 0.5f), smallBox.transform.position.y + smallBox.offset.y + (smallBox.size.y * 0.5f), 1));
        // Draw a yellow sphere at the transform's position

        /*foreach (Vector3 vertex in smallBoxVertices)
        {
            if (!possibleSurfaces[0].bounds.Contains(vertex))
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.green;
            }
            Gizmos.DrawSphere(vertex, 0.05f);
        }*/
    }
}
