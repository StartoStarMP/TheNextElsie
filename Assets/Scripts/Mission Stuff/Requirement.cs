using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

[System.Serializable]
public class Requirement
{
    //have some of item type, ex: needs 5 chairs
    //have some with specific color
    //have some of specific item

    [Header("Requirement Parameters")]
    public RequirementType reqType;

    [ShowIf("reqType", RequirementType.RoomType)]
    public RoomType roomType;

    [ShowIf("reqType", RequirementType.Item)]
    public ItemInfo item;
    [ShowIf("reqType", RequirementType.Item)]
    public int itemCount;

    [ShowIf("reqType", RequirementType.Color)]
    public ColorType color;
    [ShowIf("reqType", RequirementType.Color)]
    public float colorRatio;

    [ShowIf("reqType", RequirementType.Theme)]
    public ThemeType theme;
    [ShowIf("reqType", RequirementType.Theme)]
    public float themeRatio;

    [ShowIf("reqType", RequirementType.CategoryType)]
    public CategoryType categoryType;
    [ShowIf("reqType", RequirementType.CategoryType)]
    public int categoryTypeCount;

    [ShowIf("reqType", RequirementType.Unique)]
    public UniqueConditionType uniqueCondition;
}

[System.Serializable]
public enum RequirementType
{
    RoomType, Item, Color, Theme, CategoryType, Unique
}

[System.Serializable]
public enum RoomType
{
    Bedroom, Office
}

[System.Serializable]
public enum UniqueConditionType
{
    OpenSpace
}

/*
public class RequirementListAttributeProcessor : OdinAttributeProcessor<ColorRequirement>
{
    public override bool CanProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member)
    {
        return typeof(IList).IsAssignableFrom(parentProperty.ParentType);
    }

    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
    {
        attributes.Clear();

        switch (member.Name)
        {
            case "count":
                attributes.Add(new HorizontalGroupAttribute("Split", width: 70));
                attributes.Add(new BoxGroupAttribute("Split/Stats", true));
                attributes.Add(new HorizontalGroupAttribute("Split/Stats"));
                attributes.Add(new LabelWidthAttribute(40));
                break;
            case "color":
                attributes.Add(new BoxGroupAttribute("Split/Stats", true));
                attributes.Add(new HorizontalGroupAttribute("Split/Stats/Color"));
                attributes.Add(new LabelWidthAttribute(40));
                break;
            default:
                attributes.Add(new FoldoutGroupAttribute("Split/Color/Vertical/Properties", expanded: false));
                attributes.Add(new LabelWidthAttribute(60));
                break;
        }

        /*
        switch (member.GetType().ToString())
        {
            case "ColorRequirement":
                Debug.Log("ColorRequirement");
                attributes.Add(new BoxGroupAttribute("Split/$Name", true));
                break;
            case "ItemRequirement":
                Debug.Log("ItemRequirement");
                break;
            default:
                Debug.Log("N/A");
                break;
        }

        
        switch (member.Name)
        {
            case "Icon":
                attributes.Add(new HorizontalGroupAttribute("Split", width: 70));
                attributes.Add(new PreviewFieldAttribute(70, Sirenix.OdinInspector.ObjectFieldAlignment.Left));
                attributes.Add(new PropertyOrderAttribute(-5));
                attributes.Add(new HideLabelAttribute());
                break;

            case "Name":
            case "Id":
                attributes.Add(new BoxGroupAttribute("Split/$Name", true));
                attributes.Add(new VerticalGroupAttribute("Split/$Name/Vertical"));
                attributes.Add(new HorizontalGroupAttribute("Split/$Name/Vertical/NameId"));
                attributes.Add(new LabelWidthAttribute(40));
                break;

            default:
                attributes.Add(new FoldoutGroupAttribute("Split/$Name/Vertical/Properties", expanded: false));
                attributes.Add(new LabelWidthAttribute(60));
                break;
        }
    }
}*/
