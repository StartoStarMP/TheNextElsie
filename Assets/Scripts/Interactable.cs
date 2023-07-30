using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Basic class for Interactable objects.
/// </summary>
public class Interactable : MonoBehaviour
{
    [Header("Parameters")]
    [Tooltip("Whether this Interactable can be interacted with.")]
    public bool isEnabled = true;

    [Header("References")]
    [Tooltip("The highlight outline for this object.")]
    public GameObject highlightOutline;

    public virtual void OnMouseOver()
    {
        //Debug.Log("Mouse Enter: " + this.name);
        if (!highlightOutline || !isEnabled)
        {
            return;
        }
        /*
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }*/

        highlightOutline.SetActive(true);
    }

    public virtual void OnMouseExit()
    {
        //Debug.Log("Mouse Exit: " + this.name);
        if (!highlightOutline)
        {
            return;
        }

        highlightOutline.SetActive(false);
    }

    public virtual void OnMouseUpAsButton()
    {

    }
}
