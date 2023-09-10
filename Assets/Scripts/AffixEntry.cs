using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AffixEntry : MonoBehaviour
{
    public Requirement currentAffix;
    public Text affixNameText;
    public Text affixCriteriaText;
    //public Text affixProgress;
    public float currentProgress;
    public float maxProgress;

    public void Awake()
    {
        
    }

    public void SetupAffixEntry(Requirement affix)
    {
        currentProgress = 0;
        currentAffix = affix;

        if (affix.reqType == RequirementType.CategoryType)
        {
            affixNameText.text = "Required Category";
            affixCriteriaText.text = affix.categoryType.GetDisplayName() + " (" + affix.categoryTypeCount + ")";

            maxProgress = affix.categoryTypeCount;
        }
        else if (affix.reqType == RequirementType.Item)
        {
            affixNameText.text = "Required Items";
            affixCriteriaText.text = affix.item.name + " (" + affix.itemCount + ")";

            maxProgress = affix.itemCount;
        }
        else if (affix.reqType == RequirementType.Color)
        {
            affixNameText.text = "Preferred Color";
            affixCriteriaText.text = "Likes: " + affix.color.GetDisplayName();

            maxProgress = affix.colorRatio;
        }
        else if (affix.reqType == RequirementType.RoomType)
        {
            affixNameText.text = "Room Type";
            affixCriteriaText.text = affix.roomType.GetDisplayName();

            maxProgress = 1;
        }
        else if (affix.reqType == RequirementType.Theme)
        {
            affixNameText.text = "Preferred Theme";
            affixCriteriaText.text = affix.theme.GetDisplayName();

            maxProgress = affix.themeRatio;
        }
    }

    public void UpdateAffixStatus()
    {
        //PREFERED COLOR
        if (currentAffix.reqType == RequirementType.Color)
        {
            currentProgress = DesignManager.current.colorTypes.Count(n => n == currentAffix.color) / (float)DesignManager.current.colorTypes.Count;
        }
        //REQUIRED ITEM
        else if (currentAffix.reqType == RequirementType.Item)
        {
            Item[] placedItems = FindObjectsOfType<Item>();
            List<ItemInfo> placedItemInfos = new List<ItemInfo>();

            foreach (Item item in placedItems)
            {
                placedItemInfos.Add(item.itemInfo);
            }

            currentProgress = placedItemInfos.Count(n => n == currentAffix.item);
        }
        //REQUIRED CATEGORY
        else if (currentAffix.reqType == RequirementType.CategoryType)
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

            currentProgress = placedItemTypes.Count(n => n == currentAffix.categoryType);
        }
        //SPECIFIED ROOM TYPE
        else if (currentAffix.reqType == RequirementType.RoomType)
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

            List<CategoryType> requiredTypes = new List<CategoryType>();

            if (currentAffix.roomType == RoomType.Bedroom)
            {
                requiredTypes.Add(CategoryType.Bed);
            }
            else if (currentAffix.roomType == RoomType.Office)
            {
                requiredTypes.Add(CategoryType.Table);
                requiredTypes.Add(CategoryType.Chair);
            }

            int amountFulfilled = 0;

            foreach (CategoryType requiredType in requiredTypes)
            {
                if (placedItemTypes.Contains(requiredType))
                {
                    amountFulfilled += 1;
                }
            }

            currentProgress = amountFulfilled / (float)requiredTypes.Count;
        }
        //PREFERED THEME
        else if (currentAffix.reqType == RequirementType.Theme)
        {
            currentProgress = DesignManager.current.themeTypes.Count(n => n == currentAffix.theme) / (float)DesignManager.current.themeTypes.Count;
        }
        else if (currentAffix.reqType == RequirementType.Unique)
        {
            if (currentAffix.uniqueCondition == UniqueConditionType.OpenSpace)
            {
                currentProgress = EvaluationManager.current.GetRoomDensity();
            }
        }
    }

    /*public void IncreaseProgress(ItemInfo item)
    {
        Debug.Log(item);
        if (item == affix.item && affix.item != null)
        {
            progress++;
            //UpdateEntryText();
        }

        foreach (ItemType tag in item.itemTypes)
        {
            if (item.itemTypes.Length == 0)
            {
                break;
            }
            if (tag == affix.itemType)
            {
                progress++;
                //UpdateEntryText();
            }
        }

        foreach (ColorType color in item.colors)
        {
            if (item.colors.Length == 0)
            {
                break;
            }
            if (color == affix.color)
            {
                progress++;
                //UpdateEntryText();
            }
        }
    }

    public void DecreaseProgress(ItemInfo item)
    {
        if (item == affix.item && affix.item != null)
        {
            progress--;
            //UpdateEntryText();
        }

        foreach (ItemType tag in item.itemTypes)
        {
            if (item.itemTypes.Length == 0)
            {
                break;
            }
            if (tag == affix.itemType)
            {
                progress--;
                //UpdateEntryText();
            }
        }

        foreach (ColorType color in item.colors)
        {
            if (item.colors.Length == 0)
            {
                break;
            }
            if (color == affix.color)
            {
                progress--;
                //UpdateEntryText();
            }
        }
    }*/

    /*public void UpdateEntryText()
    {
        if (progress < requirement.count)
        {
            criteriaText.text = setCriteriaText + " <color=#FFFFFF>(" + progress + "/" + requirement.count.ToString() + ")</color>";
        }
        else 
        {
            criteriaText.text = setCriteriaText + " <color=#00C730>(" + progress + "/" + requirement.count.ToString() + ")</color>";
        }
    }*/
}
