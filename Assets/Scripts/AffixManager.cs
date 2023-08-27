using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AffixManager : MonoBehaviour
{
    public List<Requirement> missionAffixes;

    public List<AffixEntry> affixEntries;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentMissionAffixes(GameManager.current.currentMission.affixes);
        PopulateAffixDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentMissionAffixes(List<Requirement> affixes)
    {
        missionAffixes = affixes;
    }

    public void PopulateAffixDisplay()
    {
        foreach (AffixEntry affixEntry in affixEntries)
        {
            affixEntry.gameObject.SetActive(false);
        }

        for (int i = 0; i < missionAffixes.Count; i++)
        {
            affixEntries[i].gameObject.SetActive(true);
            affixEntries[i].SetupAffixEntry(missionAffixes[i]);
        }
    }

    public bool CheckAffixStatus(Requirement requirement)
    {
        //PREFERED COLOR
        if (requirement.reqType == RequirementType.Color)
        {
            if (DesignManager.current.colorTypes.Count(n => n == requirement.color)/(float)DesignManager.current.colorTypes.Count > requirement.colorRatio)
            {
                return true;
            }
        }
        //REQUIRED ITEM
        else if (requirement.reqType == RequirementType.Item)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<ItemInfo> placedItemInfos = new List<ItemInfo>();

            foreach(Item item in placedItems)
            {
                placedItemInfos.Add(item.itemInfo);
            }

            if (placedItemInfos.Count(n => n == requirement.item) > requirement.itemCount)
            {
                return true;
            }
        }
        //REQUIRED ITEM TYPE
        else if (requirement.reqType == RequirementType.ItemType)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<ItemType> placedItemTypes = new List<ItemType>();

            foreach (Item item in placedItems)
            {
                foreach (ItemType itemType in item.itemInfo.itemTypes)
                {
                    placedItemTypes.Add(itemType);
                }
            }

            if (placedItemTypes.Count(n => n == requirement.itemType) > requirement.itemTypeCount)
            {
                return true;
            }
        }
        //SPECIFIED ROOM TYPE
        else if (requirement.reqType == RequirementType.RoomType)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<ItemType> placedItemTypes = new List<ItemType>();

            foreach (Item item in placedItems)
            {
                foreach (ItemType itemType in item.itemInfo.itemTypes)
                {
                    placedItemTypes.Add(itemType);
                }
            }

            if (requirement.roomType == RoomType.Bedroom)
            {
                if (placedItemTypes.Contains(ItemType.Bed))
                {
                    return true;
                }
            }
            else if (requirement.roomType == RoomType.Office)
            {
                if (placedItemTypes.Contains(ItemType.Table) && placedItemTypes.Contains(ItemType.Chair))
                {
                    return true;
                }
            }
        }
        //PREFERED THEME
        else if (requirement.reqType == RequirementType.Theme)
        {
            if (DesignManager.current.themeTypes.Count(n => n == requirement.theme) / (float)DesignManager.current.themeTypes.Count > requirement.themeRatio)
            {
                return true;
            }
        }
        else if (requirement.reqType == RequirementType.Unique)
        {
            if (requirement.uniqueCondition == UniqueConditionType.OpenSpace)
            {
                if (EvaluationManager.current.GetRoomDensity() < 0.5f)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void ToggleAffixDisplay()
    {
        if (GetComponent<RectTransform>().anchoredPosition.x != 0)
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(0, GetComponent<RectTransform>().anchoredPosition.y, 0);
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(430, GetComponent<RectTransform>().anchoredPosition.y, 0);
        }
    }
}
