using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverListener : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIHoverListener current;
    public bool isUIOverride { get; private set; }

    private void Start()
    {
        current = this;
    }

    void Update()
    {
        // It will turn true if hovering any UI Elements
        isUIOverride = EventSystem.current.IsPointerOverGameObject();
    }
}
