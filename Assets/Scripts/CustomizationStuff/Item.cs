using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemInfo itemInfo;
    public int rotation = 0;
    public int style = 0;
    public Item surface;
    public List<Item> itemsOnSurface = new List<Item>();
    public float itemPickupProgress = 0;
    public bool selected = false;

    private void Start()
    {
        GetComponent<Renderer>().material.SetFloat("_Thickness", 0.01f);
        GetComponent<Renderer>().material.SetColor("_Color", Color.clear);
    }

    public virtual void OnMouseDown()
    {
        if (!GameManager.current.inEditMode) { return; }

        if (!UIHoverListener.current.isUIOverride)
        {
            StartFilling();
            DesignManager.current.SelectItem(this);
            Highlight(Color.white);
            selected = true;
        }
    }

    public virtual void OnMouseUp()
    {
        if (!GameManager.current.inEditMode) { return; }

        if (!UIHoverListener.current.isUIOverride)
        {
            StopFilling();
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!GameManager.current.inEditMode) { return; }

        Debug.Log("entered" + gameObject.name);
        if (!DesignManager.current.placementTool.gameObject.activeInHierarchy)
        {
            //DesignManager.current.ShowItemContext(itemInfo);
            if (!selected)
            {
                Highlight(Color.yellow);
            }
        }
    }

    public virtual void OnMouseExit()
    {
        if (!GameManager.current.inEditMode) { return; }

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
        if (rotation == 1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
        Debug.Log("ya");
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
        Debug.Log("no");
        while (itemPickupProgress > 0)
        {
            itemPickupProgress -= 0.05f;
            GetComponent<Renderer>().material.SetFloat("_FillAmount", itemPickupProgress);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Highlight(Color outlineColor)
    {
        Debug.Log("ran");
        GetComponent<Renderer>().material.SetFloat("_Thickness", 0.01f);
        GetComponent<Renderer>().material.SetColor("_Color", outlineColor);
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
