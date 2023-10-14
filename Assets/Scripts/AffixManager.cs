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
            if (DesignManager.current.colorTypes.Count(n => n == requirement.color) / (float)DesignManager.current.colorTypes.Count > requirement.colorRatio)
            {
                return true;
            }
        }
        //REQUIRED ITEM
        else if (requirement.reqType == RequirementType.Item)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<ItemInfo> placedItemInfos = new List<ItemInfo>();

            foreach (Item item in placedItems)
            {
                placedItemInfos.Add(item.itemInfo);
            }

            if (placedItemInfos.Count(n => n == requirement.item) > requirement.itemCount)
            {
                return true;
            }
        }
        //REQUIRED CATEGORY
        else if (requirement.reqType == RequirementType.CategoryType)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<CategoryType> placedItemTypes = new List<CategoryType>();

            foreach (Item item in placedItems)
            {
                foreach (CategoryType itemType in item.itemInfo.categoryTypes)
                {
                    placedItemTypes.Add(itemType);
                }
            }

            if (placedItemTypes.Count(n => n == requirement.categoryType) > requirement.categoryTypeCount)
            {
                return true;
            }
        }
        //SPECIFIED ROOM TYPE
        else if (requirement.reqType == RequirementType.RoomType)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<CategoryType> placedItemTypes = new List<CategoryType>();

            foreach (Item item in placedItems)
            {
                foreach (CategoryType itemType in item.itemInfo.categoryTypes)
                {
                    placedItemTypes.Add(itemType);
                }
            }

            if (requirement.roomType == RoomType.Bedroom)
            {
                if (placedItemTypes.Contains(CategoryType.Bed))
                {
                    return true;
                }
            }
            else if (requirement.roomType == RoomType.Office)
            {
                if (placedItemTypes.Contains(CategoryType.Table) && placedItemTypes.Contains(CategoryType.Chair))
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
            GetComponent<RectTransform>().anchoredPosition = new Vector3(500, GetComponent<RectTransform>().anchoredPosition.y, 0);
        }
        GetCurrentAffixesProgress();
    }

    public void GetCurrentAffixesProgress()
    {
        string affixesProgress = "";
        foreach (AffixEntry affixEntry in affixEntries)
        {
            if (affixEntry.gameObject.activeInHierarchy)
            {
                affixEntry.UpdateAffixStatus();
                affixesProgress += affixEntry.affixNameText.text + ": " + affixEntry.currentProgress + "/" + affixEntry.maxProgress + "\n";
            }
        }
        Debug.Log(affixesProgress);
    }

    public float GetTotalAffixCompletionStat()
    {
        List<float> affixCompletionStats = new List<float>();

        foreach (AffixEntry affixEntry in affixEntries)
        {
            if (affixEntry.gameObject.activeInHierarchy)
            {
                affixCompletionStats.Add(affixEntry.currentProgress / affixEntry.maxProgress);
            }
        }

        if (affixCompletionStats.Count == 0)
        {
            return 1f;
        }
        else
        {
            return affixCompletionStats.Sum() / affixCompletionStats.Count;
        }
    }
}