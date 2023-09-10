using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.ComponentModel.DataAnnotations;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemInfo : ScriptableObject
{
    public CustomizationType customType;
    public ItemType itemType;

    [ShowIf("customType", CustomizationType.Item)]
    public GameObject[] itemPrefab;
    [ShowIf("customType", CustomizationType.Item)]
    public string targetLayer;
    [ShowIf("customType", CustomizationType.Sprite)]
    public Sprite itemSprite;
    public int cost;
    public ColorType[] colors;
    public ThemeType[] themes;
    public CategoryType[] categoryTypes;
}

public enum ItemType
{
    WallObject, FloorObject, RugObject, Wallpaper, Flooring
}

public enum CustomizationType
{
    Item,
    Sprite
}

public enum ColorType 
{
    none,
    [Display(Name = "Red")]
    red,
    [Display(Name = "Orange")]
    orange,
    [Display(Name = "Yellow")]
    yellow,
    [Display(Name = "Green")]
    green,
    [Display(Name = "Blue")]
    blue,
    [Display(Name = "Purple")]
    purple,
    [Display(Name = "White")]
    white,
    [Display(Name = "Black")]
    black,
    [Display(Name = "Gray")]
    gray,
    [Display(Name = "Brown")]
    brown 
}

public enum ThemeType
{
    Modern,
    Floral
}

public enum CategoryType 
{
    None,
    Chair, 
    Table, 
    Armchair, 
    Bed, 
    Bench,
    [Display(Name = "Coffee Table")]
    CoffeeTable, 
    Dresser, 
    Lamp, 
    Painting, 
    Plant, 
    Rug, 
    Sconce, 
    Shelf, 
    Sofa, 
    Stand, 
    [Display(Name = "Wall Decoration")]
    WallDecoration, 
    Light 
}