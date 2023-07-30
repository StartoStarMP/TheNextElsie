using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RequirementEntry : MonoBehaviour
{
    public Requirement requirement;
    [Tooltip("The player's current progress on this Requirement, if applicable.")]
    public int progress = 0;

    [Header("References")]
    public TextMeshProUGUI criteriaText;

    [Tooltip("The unchanging requirement text without the progress count.")]
    string setCriteriaText;

    public void Awake()
    {
        criteriaText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetEntryRequirement(Requirement req)
    {
        requirement = req;
        SetEntryText(requirement.reqType);
    }

    public void SetEntryText(RequirementType reqType)
    {
        if (reqType == RequirementType.ItemType)
        {
            string requirementText;
            requirementText = requirement.itemType.GetDisplayName();
            setCriteriaText = requirementText;
            UpdateEntryText();
        }
        else if (reqType == RequirementType.Item)
        {
            string requirementText;
            requirementText = requirement.item.name;
            setCriteriaText = requirementText;
            UpdateEntryText();
        }
        else if (reqType == RequirementType.Color)
        {
            string requirementText;
            requirementText = requirement.color.GetDisplayName();
            setCriteriaText = requirementText;
            UpdateEntryText();
        }
    }

    public void IncreaseProgress(ItemInfo item)
    {
        Debug.Log(item);
        if (item == requirement.item && requirement.item != null)
        {
            progress++;
            UpdateEntryText();
        }

        foreach (ItemType tag in item.itemTypes)
        {
            if (item.itemTypes.Length == 0)
            {
                break;
            }
            if (tag == requirement.itemType)
            {
                progress++;
                UpdateEntryText();
            }
        }

        foreach (ColorType color in item.colors)
        {
            if (item.colors.Length == 0)
            {
                break;
            }
            if (color == requirement.color)
            {
                progress++;
                UpdateEntryText();
            }
        }
    }

    public void DecreaseProgress(ItemInfo item)
    {
        if (item == requirement.item && requirement.item != null)
        {
            progress--;
            UpdateEntryText();
        }

        foreach (ItemType tag in item.itemTypes)
        {
            if (item.itemTypes.Length == 0)
            {
                break;
            }
            if (tag == requirement.itemType)
            {
                progress--;
                UpdateEntryText();
            }
        }

        foreach (ColorType color in item.colors)
        {
            if (item.colors.Length == 0)
            {
                break;
            }
            if (color == requirement.color)
            {
                progress--;
                UpdateEntryText();
            }
        }
    }

    public void UpdateEntryText()
    {
        if (progress < requirement.count)
        {
            criteriaText.text = setCriteriaText + " <color=#FFFFFF>(" + progress + "/" + requirement.count.ToString() + ")</color>";
        }
        else 
        {
            criteriaText.text = setCriteriaText + " <color=#00C730>(" + progress + "/" + requirement.count.ToString() + ")</color>";
        }
    }
}
