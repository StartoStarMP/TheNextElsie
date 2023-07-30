using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesignManager : MonoBehaviour
{
    public static DesignManager current;
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
    public RectTransform items;
    public GameObject[] categoryRows;
    public GameObject[] categoryTabs;
    private List<GameObject> wallButtons = new List<GameObject>();
    public ItemInfo[] wallObjects;
    private List<GameObject> floorButtons = new List<GameObject>();
    public ItemInfo[] floorObjects;
    private List<GameObject> rugButtons = new List<GameObject>();
    public ItemInfo[] rugObjects;
    private List<GameObject> wallTileButtons = new List<GameObject>();
    public Sprite[] wallTileSprites;
    private List<GameObject> floorTileButtons = new List<GameObject>();
    public Sprite[] floorTileSprites;

    [Header("World Terrain")]
    public SpriteRenderer wall;
    public SpriteRenderer floor;
    public Button selectedWallTile;
    public Button selectedFloorTile;

    [Header("Item Placement")]
    public PlacementTool placementTool;
    public Text placementText;

    [Header("Item Selection")]
    public GameObject itemContextPopup;
    public Image itemContextImage;
    public Text itemContextName;
    public Text itemContextCost;

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
        GameManager.current.inEditMode = true;

        budgetSlider.maxValue = maxBudget;
        budgetSlider.value = maxBudget;
        budgetText.text = maxBudget.ToString();

        for (int i = 0; i < setsPool.childCount; i++)
        {
            sets.Add(setsPool.GetChild(i).GetComponent<Text>());
        }

        for (int i = 0; i < categoryRows[0].transform.GetChild(0).childCount; i++)
        {
            wallButtons.Add(categoryRows[0].transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < wallObjects.Length; i++)
        {
            int x = new int();
            x = i;
            wallButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = wallObjects[i].itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
            wallButtons[i].transform.GetComponent<ItemButton>().itemInfo = wallObjects[i];
            wallButtons[i].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(wallObjects[x]); });
            wallButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < categoryRows[1].transform.GetChild(0).childCount; i++)
        {
            floorButtons.Add(categoryRows[1].transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < floorObjects.Length; i++)
        {
            int x = new int();
            x = i;
            floorButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = floorObjects[i].itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
            floorButtons[i].transform.GetComponent<ItemButton>().itemInfo = floorObjects[i];
            floorButtons[i].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(floorObjects[x]); });
            floorButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < categoryRows[2].transform.GetChild(0).childCount; i++)
        {
            rugButtons.Add(categoryRows[2].transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < rugObjects.Length; i++)
        {
            int x = new int();
            x = i;
            rugButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = rugObjects[i].itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
            rugButtons[i].transform.GetComponent<ItemButton>().itemInfo = rugObjects[i];
            rugButtons[i].GetComponent<Button>().onClick.AddListener(delegate { StartPlacementTool(rugObjects[x]); });
            rugButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < categoryRows[3].transform.GetChild(0).childCount; i++)
        {
            wallTileButtons.Add(categoryRows[3].transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < wallTileSprites.Length; i++)
        {
            int x = new int();
            x = i;
            wallTileButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = wallTileSprites[i];
            wallTileButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ChangeWallTiles(wallTileButtons[x].GetComponent<Button>(), wallTileSprites[x]); });
            wallTileButtons[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < categoryRows[4].transform.GetChild(0).childCount; i++)
        {
            floorTileButtons.Add(categoryRows[4].transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < floorTileSprites.Length; i++)
        {
            int x = new int();
            x = i;
            floorTileButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = floorTileSprites[i];
            floorTileButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ChangeFloorTiles(floorTileButtons[x].GetComponent<Button>(), floorTileSprites[x]); });
            floorTileButtons[i].gameObject.SetActive(true);
        }

        selectedWallTile.GetComponent<Outline>().enabled = true;
        selectedFloorTile.GetComponent<Outline>().enabled = true;
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
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollCategory("left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollCategory("right");
        }

        if (itemContextPopup.activeInHierarchy)
        {
            itemContextPopup.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x + 10, Input.mousePosition.y - 10, 0);
        }
    }

    public void Filter(string subCategory = "")
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

        if (subCategory != "")
        {
            foreach (ItemButton itemButton in itemButtons)
            {
                if (!itemButton.itemInfo.subCategories.Contains(subCategory))
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

    public void StartPlacementTool(ItemInfo itemInfo, bool limitedUse = false)
    {
        placementTool.gameObject.SetActive(true);
        placementTool.gameObject.GetComponent<PlacementTool>().limitedUse = limitedUse;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        placementTool.transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, 1);
        placementTool.SetSelectedItem(itemInfo);
        placementText.gameObject.SetActive(true);
        StartCoroutine(placementTool.GetComponent<PlacementTool>().DelayPlacement());
    }

    public void SelectItem(Item item)
    {
        if (placementTool.gameObject.activeInHierarchy)
        {
            return;
        }

        itemContextPopup.SetActive(true);
        items.gameObject.SetActive(false);
        itemContextImage.sprite = item.itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        itemContextName.text = item.itemInfo.name;
        itemContextCost.text = "Costs: " + item.itemInfo.cost;
        StartPlacementTool(item.itemInfo, true);
        RemoveItem(item);
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
        items.gameObject.SetActive(true);
    }

    public void SelectCategory(int idx)
    {
        for (int i = 0; i < categoryStrings.Length; i++)
        {
            if (i == idx)
            {
                categoryRows[i].SetActive(true);
                categoryTabs[i].SetActive(true);
            }
            else
            {
                categoryRows[i].SetActive(false);
                categoryTabs[i].SetActive(false);
            }
        }
        Filter();
    }

    public void ChangeWallTiles(Button button, Sprite sprite)
    {
        selectedWallTile.GetComponent<Outline>().enabled = false;
        selectedWallTile = button;
        selectedWallTile.GetComponent<Outline>().enabled = true;
        wall.sprite = sprite;
    }

    public void ChangeFloorTiles(Button button, Sprite sprite)
    {
        selectedFloorTile.GetComponent<Outline>().enabled = false;
        selectedFloorTile = button;
        selectedFloorTile.GetComponent<Outline>().enabled = true;
        floor.sprite = sprite;
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
            Debug.Log(i);
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

    public void ShowItemContext(ItemInfo itemInfo)
    {
        itemContextPopup.SetActive(true);
        itemContextImage.sprite = itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        itemContextName.text = itemInfo.name;
        itemContextCost.text = "Costs: " + itemInfo.cost;
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
        GameManager.current.inEditMode = false;

        TransitionCanvas.current.Animate("blackFadeIn");
        StartCoroutine(Timer(x => designUI.SetActive(false), 0.5f));
        StartCoroutine(Timer(x => EvaluationManager.current.StartCoroutine("GatherPointsBreakdown"), 0.5f));
        StartCoroutine(Timer(x => TransitionCanvas.current.Animate("blackFadeOut"), 0.5f));
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
