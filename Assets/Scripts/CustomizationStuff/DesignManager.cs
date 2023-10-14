using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DesignManager : MonoBehaviour
{
    public static DesignManager current;
    public bool inEditMode = false;
    public GameObject designUI;

    [Header("Budget")]
    public Slider budgetSlider;
    public Text budgetText;
    public int currentBudget;
    public int maxBudget;

    [Header("Sets")]
    public Transform setsPool;
    private List<Text> sets = new List<Text>();
    public List<ColorType> colorTypes;
    public List<ThemeType> themeTypes;

    [Header("Categories")]
    public Animator categoryWheel;
    public Text categoryName;
    public Image categoryImage;
    public int currentCategory = 0;
    public Image[] categoryIcons;
    public Sprite[] categorySprites;
    public string[] categoryStrings;

    [Header("Item Row")]
    //public RectTransform items;
    public GameObject[] categoryRows;
    public GameObject[] categoryTabs;
    private List<GameObject> wallButtons = new List<GameObject>();
    private List<GameObject> floorButtons = new List<GameObject>();
    private List<GameObject> rugButtons = new List<GameObject>();
    private List<GameObject> wallTileButtons = new List<GameObject>();
    private List<GameObject> floorTileButtons = new List<GameObject>();

    [Header("World Terrain")]
    public SpriteRenderer wall;
    public SpriteRenderer floor;
    public Button selectedWallTile;
    public Button selectedFloorTile;

    [Header("Item Placement")]
    public PlacementTool placementTool;
    public Text placementText;

    [Header("Item Selection")]
    public Item selectedItem;
    public GameObject itemContextPopup;
    public Image itemContextImage;
    public Text itemContextName;
    public Text itemContextCost;
    public Button[] styleButtons;
    public Image[] styleButtonImages;
    public GameObject[] styleLockOverlays;

    [Header("Color Wheel")]
    public Image[] wheelSlices;
    public Slider[] colorSliders;
    public RectTransform colorDropdown;
    public bool showSliders = false;

    private void Awake()
    {
        current = this;
    }

    public void Start()
    {
        if (!inEditMode)
        {
            return;
        }

        List<ItemInfo> requiredItems = new List<ItemInfo>();
        foreach (Requirement affixEntry in GameManager.current.currentMission.affixes)
        {
            if (affixEntry.reqType == RequirementType.Item)
            {
                for(int i = 0; i < affixEntry.itemCount; i++)
                {
                    requiredItems.Add(affixEntry.item);
                }
            }
        }

        budgetSlider.maxValue = maxBudget;
        budgetSlider.value = maxBudget;
        budgetText.text = maxBudget.ToString();

        for (int i = 0; i < setsPool.childCount; i++)
        {
            sets.Add(setsPool.GetChild(i).GetComponent<Text>());
        }

        //SETTING WALL ITEMS
        for (int i = 0; i < categoryRows[0].transform.GetChild(0).childCount; i++)
        {
            wallButtons.Add(categoryRows[0].transform.GetChild(0).GetChild(i).gameObject);
        }

        //List<ItemInfo> currentItemsToSet = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.WallObject });
        List<ItemInfo> currentItemsToSet = new List<ItemInfo>();
        foreach(ItemInfo itemInfo in requiredItems)
        {
            if (itemInfo.itemType == ItemType.WallObject && !currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }
        foreach (ItemInfo itemInfo in GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.WallObject }))
        {
            if (!currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }

        for (int i = 0; i < currentItemsToSet.Count; i++)
        {
            ItemInfo wallItem = currentItemsToSet[i];
            wallButtons[i].transform.GetComponent<ItemButton>().SetDetails(wallItem, requiredItems.Contains(wallItem));
            wallButtons[i].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(wallItem); });
            wallButtons[i].gameObject.SetActive(true);
        }

        //SETTING FLOOR ITEMS
        for (int i = 0; i < categoryRows[1].transform.GetChild(0).childCount; i++)
        {
            floorButtons.Add(categoryRows[1].transform.GetChild(0).GetChild(i).gameObject);
        }

        //currentItemsToSet = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.FloorObject });
        currentItemsToSet.Clear();
        foreach (ItemInfo itemInfo in requiredItems)
        {
            if (itemInfo.itemType == ItemType.FloorObject && !currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }
        foreach (ItemInfo itemInfo in GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.FloorObject }))
        {
            if (!currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }

        for (int i = 0; i < currentItemsToSet.Count; i++)
        {
            int x;
            x = i;
            ItemInfo floorItem = currentItemsToSet[x];
            floorButtons[x].transform.GetComponent<ItemButton>().SetDetails(currentItemsToSet[x], requiredItems.Contains(floorItem));
            floorButtons[x].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(floorItem); });

            /*EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { StartPlacementTool(floorItem); });
            floorButtons[x].GetComponent<EventTrigger>().triggers.Add(entry);

            EventTrigger.Entry entry2 = new EventTrigger.Entry();
            entry2.eventID = EventTriggerType.PointerDown;
            entry2.callback.AddListener((data) => { floorButtons[x].GetComponent<ItemButton>().ToggleStyles(true); });
            floorButtons[x].GetComponent<EventTrigger>().triggers.Add(entry2);

            EventTrigger.Entry entry3 = new EventTrigger.Entry();
            entry3.eventID = EventTriggerType.PointerUp;
            entry3.callback.AddListener((data) => { floorButtons[x].GetComponent<ItemButton>().ToggleStyles(false); });
            floorButtons[x].GetComponent<EventTrigger>().triggers.Add(entry3);
            */

            floorButtons[x].gameObject.SetActive(true);
        }

        //SETTING RUG ITEMS
        for (int i = 0; i < categoryRows[2].transform.GetChild(0).childCount; i++)
        {
            rugButtons.Add(categoryRows[2].transform.GetChild(0).GetChild(i).gameObject);
        }

        //currentItemsToSet = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.RugObject });
        currentItemsToSet.Clear();
        foreach (ItemInfo itemInfo in requiredItems)
        {
            if (itemInfo.itemType == ItemType.RugObject && !currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }
        foreach (ItemInfo itemInfo in GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.RugObject }))
        {
            if (!currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }

        for (int i = 0; i < currentItemsToSet.Count; i++)
        {
            ItemInfo rugItem = currentItemsToSet[i];
            rugButtons[i].transform.GetComponent<ItemButton>().SetDetails(currentItemsToSet[i], requiredItems.Contains(rugItem));
            rugButtons[i].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(rugItem); });
            rugButtons[i].gameObject.SetActive(true);
        }

        //SETTING WALLPAPER ITEMS
        for (int i = 0; i < categoryRows[3].transform.GetChild(0).childCount; i++)
        {
            wallTileButtons.Add(categoryRows[3].transform.GetChild(0).GetChild(i).gameObject);
        }

        //currentItemsToSet = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.Wallpaper });
        currentItemsToSet.Clear();
        foreach (ItemInfo itemInfo in requiredItems)
        {
            if (itemInfo.itemType == ItemType.Wallpaper && !currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }
        foreach (ItemInfo itemInfo in GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.Wallpaper }))
        {
            if (!currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }

        for (int i = 0; i < currentItemsToSet.Count; i++)
        {
            int x;
            x = i;
            ItemInfo wallpaperItem = currentItemsToSet[i];
            wallTileButtons[i].transform.GetComponent<ItemButton>().SetDetails(currentItemsToSet[i], requiredItems.Contains(wallpaperItem));
            wallTileButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ChangeWallTiles(wallTileButtons[x].GetComponent<Button>(), wallpaperItem); });
            wallTileButtons[i].gameObject.SetActive(true);
        }

        //SETTING FLOORING ITEMS
        for (int i = 0; i < categoryRows[4].transform.GetChild(0).childCount; i++)
        {
            floorTileButtons.Add(categoryRows[4].transform.GetChild(0).GetChild(i).gameObject);
        }

        //currentItemsToSet = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.Flooring });
        currentItemsToSet.Clear();
        foreach (ItemInfo itemInfo in requiredItems)
        {
            if (itemInfo.itemType == ItemType.Flooring && !currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }
        foreach (ItemInfo itemInfo in GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.Flooring }))
        {
            if (!currentItemsToSet.Contains(itemInfo))
            {
                currentItemsToSet.Add(itemInfo);
            }
        }

        for (int i = 0; i < currentItemsToSet.Count; i++)
        {
            int x;
            x = i;
            ItemInfo flooringItem = currentItemsToSet[i];
            floorTileButtons[i].transform.GetComponent<ItemButton>().SetDetails(currentItemsToSet[i], requiredItems.Contains(flooringItem));
            floorTileButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ChangeFloorTiles(floorTileButtons[x].GetComponent<Button>(), flooringItem); });
            floorTileButtons[i].gameObject.SetActive(true);
        }

        //selectedWallTile.GetComponent<Outline>().enabled = true;
        //selectedFloorTile.GetComponent<Outline>().enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (placementTool.gameObject.activeInHierarchy)
            {
                placementTool.gameObject.SetActive(false);
                placementText.gameObject.SetActive(false);
            }

            if (selectedItem != null)
            {
                DeselectItem();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollCategory("left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollCategory("right");
        }

        /*if (itemContextPopup.activeInHierarchy)
        {
            itemContextPopup.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x + 10, Input.mousePosition.y - 10, 0);
        }*/
    }

    public void Filter(string category = "")
    {
        List<ItemButton> itemButtons = new List<ItemButton>();
        foreach (GameObject button in wallButtons)
        {
            if (button.GetComponent<ItemButton>().itemInfo != null)
            {
                itemButtons.Add(button.GetComponent<ItemButton>());
            }
        }
        foreach (GameObject button in floorButtons)
        {
            if (button.GetComponent<ItemButton>().itemInfo != null)
            {
                itemButtons.Add(button.GetComponent<ItemButton>());
            }
        }
        foreach (GameObject button in rugButtons)
        {
            if (button.GetComponent<ItemButton>().itemInfo != null)
            {
                itemButtons.Add(button.GetComponent<ItemButton>());
            }
        }

        if (category != "")
        {
            foreach (ItemButton itemButton in itemButtons)
            {
                if (!itemButton.itemInfo.categoryTypes.Contains((CategoryType)Enum.Parse(typeof(CategoryType), category)))
                {
                    itemButton.gameObject.SetActive(false);
                }
                else
                {
                    itemButton.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (ItemButton itemButton in itemButtons)
            {
                itemButton.gameObject.SetActive(true);
            }
        }
    }

    public void ScrollCategory(string type)
    {
        if (type == "left")
        {
            categoryWheel.SetTrigger("scrollLeft");
            currentCategory += 1;
            if (currentCategory > categoryStrings.Length - 1)
            {
                currentCategory = 0;
            }
        }
        else if (type == "right")
        {
            categoryWheel.SetTrigger("scrollRight");
            currentCategory -= 1;
            if (currentCategory < 0)
            {
                currentCategory = categoryStrings.Length - 1;
            }
        }
        categoryIcons[0].sprite = categorySprites[currentCategory];
        categoryIcons[3].sprite = categorySprites[ListOffset(currentCategory, categorySprites.Length, -2)];
        categoryIcons[4].sprite = categorySprites[ListOffset(currentCategory, categorySprites.Length, -1)];
        categoryIcons[1].sprite = categorySprites[ListOffset(currentCategory, categorySprites.Length, 1)];
        categoryIcons[2].sprite = categorySprites[ListOffset(currentCategory, categorySprites.Length, 2)];
        categoryName.text = categoryStrings[currentCategory];
        categoryImage.sprite = categorySprites[currentCategory];
        SelectCategory(currentCategory);
    }

    public void StartPlacementTool(ItemInfo itemInfo, int styleIdx = 0, int rotationIdx = 0, bool limitedUse = false)
    {
        DeselectItem();

        placementTool.gameObject.SetActive(true);
        placementTool.gameObject.GetComponent<PlacementTool>().limitedUse = limitedUse;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        placementTool.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 1);
        placementTool.SetSelectedItem(itemInfo, styleIdx, rotationIdx);
        placementText.gameObject.SetActive(true);
        StartCoroutine(placementTool.GetComponent<PlacementTool>().DelayPlacement());
    }

    public void SelectItem(Item item)
    {
        DeselectItem();

        selectedItem = item;
        selectedItem.Highlight(Color.white);
        selectedItem.selected = true;
        ShowItemContext(item);
    }

    public void DeselectItem()
    {
        if (selectedItem == null)
        {
            return;
        }

        selectedItem.Highlight(Color.clear);
        selectedItem.selected = false;
        selectedItem = null;

        if (itemContextPopup.activeInHierarchy)
        {
            itemContextPopup.SetActive(false);
        }
    }

    public void PickUpItem(Item item)
    {
        if (placementTool.gameObject.activeInHierarchy)
        {
            return;
        }

        //itemContextPopup.SetActive(true);
        //items.gameObject.SetActive(false);
        //itemContextImage.sprite = item.itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        //itemContextName.text = item.itemInfo.name;
        //itemContextCost.text = "Costs: " + item.itemInfo.cost;

        if (item.itemsOnSurface.Count != 0)
        {
            Debug.Log("cannot remove, items on surface");
            return;
        }
        
        if (item.surface != null)
        {
            item.surface.itemsOnSurface.Remove(item);
        }

        StartPlacementTool(item.itemInfo, item.style, item.rotation, true);
        RemoveItem(item);

        AudioManager.current.PlaySoundEffect("coin-Stardew");
    }

    public void RemoveItem(Item item)
    {
        //MissionUIManager.current.DecreaseCriteriaProgress(item.itemInfo);
        AdjustBudget(item.itemInfo.cost);
        foreach (ColorType colorType in item.itemInfo.colors)
        {
            AdjustColors(colorType, "remove");
        }
        foreach (ThemeType themeType in item.itemInfo.themes)
        {
            AdjustThemes(themeType, "remove");
        }
        Destroy(item.gameObject);
        itemContextPopup.SetActive(false);
        //items.gameObject.SetActive(true);
    }

    public void SelectCategory(int idx)
    {
        for (int i = 0; i < categoryStrings.Length; i++)
        {
            if (i == idx)
            {
                categoryRows[i].SetActive(true);
                //categoryTabs[i].SetActive(true);
            }
            else
            {
                categoryRows[i].SetActive(false);
                //categoryTabs[i].SetActive(false);
            }
        }
        Filter();
    }

    public void ChangeWallTiles(Button button, ItemInfo itemInfo, int style = 0)
    {
        DeselectItem();

        if (selectedWallTile != null)
        {
            selectedWallTile.GetComponent<Outline>().enabled = false;
        }
        selectedWallTile = button;
        selectedWallTile.GetComponent<Outline>().enabled = true;
        wall.sprite = itemInfo.GetItemSpriteToDisplay();

        wall.GetComponent<Item>().itemInfo = itemInfo;
        wall.GetComponent<Item>().style = style;
    }

    public void ChangeFloorTiles(Button button, ItemInfo itemInfo, int style = 0)
    {
        DeselectItem();

        if (selectedFloorTile != null)
        {
            selectedFloorTile.GetComponent<Outline>().enabled = false;
        }
        selectedFloorTile = button;
        selectedFloorTile.GetComponent<Outline>().enabled = true;
        floor.sprite = itemInfo.GetItemSpriteToDisplay();

        floor.GetComponent<Item>().itemInfo = itemInfo;
        floor.GetComponent<Item>().style = style;
    }

    public void AdjustBudget(int amount)
    {
        currentBudget += amount;
        budgetSlider.value = currentBudget;
        budgetText.text = currentBudget.ToString();
    }

    public void AdjustColors(ColorType colorType, string type)
    {
        if (type == "add")
        {
            if (!colorTypes.Contains(colorType))
            {
                colorTypes.Add(colorType);
            }
            else
            {
                colorTypes.Add(colorType);
            }
        }
        else if (type == "remove")
        {
            if (colorTypes.Contains(colorType))
            {
                colorTypes.Remove(colorType);
            }
            else
            {
                colorTypes.Remove(colorType);
            }
        }

        List<ColorType> colorsPresent = new List<ColorType>();
        List<int> values = new List<int>();

        foreach (ColorType colorType1 in colorTypes)
        {
            if (!colorsPresent.Contains(colorType1))
            {
                colorsPresent.Add(colorType1);
                values.Add(1);
            }
            else
            {
                values[colorsPresent.IndexOf(colorType1)] += 1;
            }
        }

        SetColors(colorsPresent, values);
    }

    public void AdjustThemes(ThemeType themeType, string type)
    {
        if (type == "add")
        {
            if (!themeTypes.Contains(themeType))
            {
                themeTypes.Add(themeType);
            }
            else
            {
                themeTypes.Add(themeType);
            }
        }
        else if (type == "remove")
        {
            if (themeTypes.Contains(themeType))
            {
                themeTypes.Remove(themeType);
            }
            else
            {
                themeTypes.Remove(themeType);
            }
        }
    }

    public void SetColors(List<ColorType> colorTypes, List<int> values)
    {
        List<Color> colors = new List<Color>() { Color.red, new Color(1, 0.5f, 0), Color.yellow, Color.green, Color.blue, new Color(0.5f, 0, 1), Color.white, Color.black, Color.gray, new Color(0.5f, 0.25f, 0) };
        List<ColorType> colorTypesOrder = new List<ColorType>() { ColorType.red, ColorType.orange, ColorType.yellow, ColorType.green, ColorType.blue, ColorType.purple, ColorType.white, ColorType.black, ColorType.gray, ColorType.brown };

        colorDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(colorDropdown.GetComponent<RectTransform>().sizeDelta.x, 0);

        foreach (Image wheelSlice in wheelSlices)
        {
            wheelSlice.gameObject.SetActive(false);
        }

        foreach (Image slice in wheelSlices)
        {
            slice.gameObject.SetActive(false);
        }

        foreach (Slider colorSlider in colorSliders)
        {
            colorSlider.gameObject.SetActive(false);
        }

        int totalValues = 0;
        foreach (int value in values)
        {
            totalValues += value;
        }

        float filledSlice = 0;
        for (int i = colorTypes.Count - 1; i >= 0; i--)
        {
            //Debug.Log(i);
            wheelSlices[i].gameObject.SetActive(true);
            wheelSlices[i].color = colors[colorTypesOrder.IndexOf(colorTypes[i])];
            wheelSlices[i].fillAmount = (float)values[i] / totalValues + filledSlice;
            filledSlice += (float)values[i] / totalValues;
            Debug.Log(colorTypesOrder.IndexOf(colorTypes[i]));

            colorSliders[colorTypesOrder.IndexOf(colorTypes[i])].gameObject.SetActive(true);
            colorSliders[colorTypesOrder.IndexOf(colorTypes[i])].maxValue = totalValues;
            colorSliders[colorTypesOrder.IndexOf(colorTypes[i])].value = values[i];
            //colorSliders[i].transform.SetSiblingIndex(Mathf.Abs(i - colorTypes.Count + 1));
            if (showSliders)
            {
                colorDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(colorDropdown.GetComponent<RectTransform>().sizeDelta.x, colorDropdown.GetComponent<RectTransform>().sizeDelta.y + 60);
            }
        }
    }

    public void ToggleSliderDropdown()
    {
        showSliders = !showSliders;

        if (showSliders)
        {
            foreach (Image wheelSlice in wheelSlices)
            {
                if (wheelSlice.gameObject.activeInHierarchy)
                {
                    colorDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(colorDropdown.GetComponent<RectTransform>().sizeDelta.x, colorDropdown.GetComponent<RectTransform>().sizeDelta.y + 60);
                }
            }
        }
        else
        {
            colorDropdown.GetComponent<RectTransform>().sizeDelta = new Vector2(colorDropdown.GetComponent<RectTransform>().sizeDelta.x, 0);
        }
    }

    public void ShowItemContext(Item item)
    {
        itemContextPopup.SetActive(true);
        itemContextImage.sprite = item.itemInfo.itemStyles[item.style].sprites[0];
        itemContextName.text = item.itemInfo.name;
        itemContextCost.text = "Costs: " + item.itemInfo.cost;

        for (int i = 0; i < styleButtons.Length; i++)
        {
            styleLockOverlays[i].SetActive(true);
            styleButtons[i].interactable = false;
            styleButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < item.itemInfo.itemStyles.Count; i++)
        {
            styleButtons[i].gameObject.SetActive(true);
            styleButtonImages[i].sprite = item.itemInfo.itemStyles[i].sprites[0];

            //HIGHLIGHT SELECTED STYLE
            if (i == item.style) 
            {
                styleButtons[i].image.color = Color.yellow;
            }
            else
            {
                styleButtons[i].image.color = Color.white;
            }

            if (ItemStatsManager.current.GetUnlockedStyles(item.itemInfo).Contains(i))
            {
                styleLockOverlays[i].SetActive(false);
                styleButtons[i].interactable = true;
            }
        }
    }

    public void SetStyleOfSelectedItem(int styleIdx)
    {
        if (selectedItem == null)
        {
            Debug.LogError("No selected item, but tried to set style of selected item.");
        }
        else
        {
            selectedItem.SelectStyle(styleIdx);
            ShowItemContext(selectedItem);
        }
    }

    public void HideItemContext()
    {
        itemContextPopup.SetActive(false);
    }

    public int ListOffset(int idx, int listLength, int offset)
    {
        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                idx += 1;
                if (idx >= listLength)
                {
                    idx = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i > offset; i--)
            {
                idx -= 1;
                if (idx < 0)
                {
                    idx = listLength - 1;
                }
            }
        }
        return idx;
    }

    public void Finish()
    {
        inEditMode = false;

        StartCoroutine(Timer(x => designUI.SetActive(false), 0.5f));
        StartCoroutine(Timer(x => EvaluationManager.current.StartCoroutine("GatherPointsBreakdown"), 0.5f));
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
