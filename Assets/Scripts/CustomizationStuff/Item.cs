using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemInfo itemInfo;

    public virtual void OnMouseDown()
    {
        if (!GameManager.current.inEditMode) { return; }

        if (!UIHoverListener.current.isUIOverride)
        {
            DesignManager.current.SelectItem(this);
        }
    }

    public virtual void OnMouseEnter()
    {
        if (!GameManager.current.inEditMode) { return; }

        Debug.Log("entered" + gameObject.name);
        if (!DesignManager.current.placementTool.gameObject.activeInHierarchy)
        {
            DesignManager.current.ShowItemContext(itemInfo);
        }
    }

    public virtual void OnMouseExit()
    {
        if (!GameManager.current.inEditMode) { return; }

        Debug.Log("exit" + gameObject.name);
        if (!DesignManager.current.placementTool.gameObject.activeInHierarchy)
        {
            DesignManager.current.HideItemContext();
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
