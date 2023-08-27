using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AffixEntry : MonoBehaviour
{
    public Requirement affix;
    public Text affixNameText;
    public Text affixCriteriaText;
    //public Text affixProgress;

    public void Awake()
    {
        
    }

    public void SetupAffixEntry(Requirement affix)
    {
        if (affix.reqType == RequirementType.ItemType)
        {
            affixNameText.text = "Required Item Types";
            affixCriteriaText.text = affix.itemType.GetDisplayName();
        }
        else if (affix.reqType == RequirementType.Item)
        {
            affixNameText.text = "Required Items";
            affixCriteriaText.text = affix.item.name;
        }
        else if (affix.reqType == RequirementType.Color)
        {
            affixNameText.text = "Preferred Color";
            affixCriteriaText.text = affix.color.GetDisplayName();
        }
        else if (affix.reqType == RequirementType.RoomType)
        {
            affixNameText.text = "Room Type";
            affixCriteriaText.text = affix.roomType.GetDisplayName();
        }
        else if (affix.reqType == RequirementType.Theme)
        {
            affixNameText.text = "Preferred Theme";
            affixCriteriaText.text = affix.theme.GetDisplayName();
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
