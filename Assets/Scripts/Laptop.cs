using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laptop : Interactable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnMouseUpAsButton()
    {
        //base.OnMouseUpAsButton();

        MissionManager.current.SetMissionsActive(true);
    }
}
