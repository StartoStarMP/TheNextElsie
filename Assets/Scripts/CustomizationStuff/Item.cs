using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Item : MonoBehaviour
{
    public CustomizationType customType = CustomizationType.Item;

    public ItemInfo itemInfo;
    [ShowIf("customType", CustomizationType.Item)]
    public int rotation = 0;
    public int style = 0;
    [ShowIf("customType", CustomizationType.Item)]
    public Item surface;
    [ShowIf("customType", CustomizationType.Item)]
    public List<Item> itemsOnSurface = new List<Item>();
    [ShowIf("customType", CustomizationType.Item)]
    public float itemPickupProgress = 0;
    public bool selected = false;

    private void Start()
    {
        if (customType == CustomizationType.Item)
        {
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0.01f);
            GetComponent<Renderer>().material.SetColor("_Color", Color.clear);
        }
    }

    public virtual void OnMouseDown()
    {
        if (!DesignManager.current.inEditMode) { return; }

        if (!UIHoverListener.current.isUIOverride)
        {
            DesignManager.current.SelectItem(this);
            foreach (Item item in Item.FindObjectsOfType<Item>())
            {
                item.Highlight(Color.clear);
            }

            if (customType == CustomizationType.Item)
            {
                StartFilling();
                Highlight(Color.white);
            }
            selected = true;
        }
    }

    public virtual void OnMouseUp()
    {
        if (!DesignManager.current.inEditMode || customType != CustomizationType.Item) { return; }

        if (!UIHoverListener.current.isUIOverride)
        {
            StopFilling();
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!DesignManager.current.inEditMode || customType != CustomizationType.Item) { return; }

        Debug.Log("entered" + gameObject.name);
        if (!DesignManager.current.placementTool.gameObject.activeInHierarchy)
        {
            //DesignManager.current.ShowItemContext(itemInfo);
            if (!selected)
            {
                Highlight(Color.yellow);
                AudioManager.current.PlaySoundEffect("pickUpItem-Stardew");
            }
        }
    }

    public virtual void OnMouseExit()
    {
        if (!DesignManager.current.inEditMode || customType != CustomizationType.Item) { return; }

        Debug.Log("exit" + gameObject.name);
        if (!DesignManager.current.placementTool.gameObject.activeInHierarchy)
        {
            //DesignManager.current.HideItemContext();
            if (!selected)
            {
                Highlight(Color.clear);
            }
        }

        if (itemPickupProgress > 0)
        {
            StopFilling();
        }
    }

    public void SelectStyle(int newStyle)
    {
        style = newStyle;
        GetComponent<SpriteRenderer>().sprite = itemInfo.itemStyles[newStyle].sprites[rotation];
        if (customType == CustomizationType.Item)
        {
            if (rotation == 1)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    public void StartFilling()
    {
        StopAllCoroutines();
        StartCoroutine(_Fill());
    }

    public void StopFilling()
    {
        StopAllCoroutines();
        StartCoroutine(_Unfill());
    }

    public IEnumerator _Fill()
    {
        while (itemPickupProgress < 1)
        {
            itemPickupProgress += 0.015f;
            GetComponent<Renderer>().material.SetFloat("_FillAmount", itemPickupProgress);
            yield return new WaitForSeconds(0.01f);
        }
        DesignManager.current.PickUpItem(this);
    }

    public IEnumerator _Unfill()
    {
        while (itemPickupProgress > 0)
        {
            itemPickupProgress -= 0.05f;
            GetComponent<Renderer>().material.SetFloat("_FillAmount", itemPickupProgress);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Highlight(Color outlineColor)
    {
        if (customType == CustomizationType.Item)
        {
            GetComponent<Renderer>().material.SetFloat("_Thickness", 0.01f);
            GetComponent<Renderer>().material.SetColor("_Color", outlineColor);
        }
    }

    /*public IEnumerator Highlight()
    {
        bool highlightOn = true;
        while (true)
        {
            if (highlightOn)
            {
                float red = GetComponent<SpriteRenderer>().color.g;
                red += 0.1f;
                GetComponent<SpriteRenderer>().color = new Color(1, red, red);
                if (red >= 0.9f)
                {
                    highlightOn = false;
                }
                yield return new WaitForSeconds(0.05f);
            }
            else
            {
                float red = GetComponent<SpriteRenderer>().color.g;
                red -= 0.1f;
                GetComponent<SpriteRenderer>().color = new Color(1, red, red);
                if (red <= 0.1f)
                {
                    highlightOn = true;
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }*/
}
