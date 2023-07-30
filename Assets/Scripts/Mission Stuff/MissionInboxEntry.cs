using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionInboxEntry : Button
{
    public Mission mission;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        //Display mission data from this Entry's Mission var.
        MissionInboxManager.current.ToggleMissionInfoPanel();
        MissionInboxManager.current.MissionInfoPopulate(mission);
    }
}

public class MissionInboxRelation
{
    public Mission mission;
    public MissionInboxEntry inboxEntry;
}
