using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    //private Vector2 velocity = Vector2.zero;
    public Camera currentShopCamera;
    public Camera formerShopCamera;
    public RawImage currentShopCameraView;
    public RawImage formerShopCameraView;
    public Coroutine focusingCamera;

    [Header("Shop Displays")]
    public Button[] shopCategoryButtons;
    public GameObject purchasesDisplay;
    public GameObject reputationDisplay;
    public GameObject investmentsDisplay;

    [Header("Shop Details")]
    public Image shopLogo;
    public Image shopLogoBg;
    public Image shopViewBg;
    public ItemType[] shopTypes;
    public Sprite[] shopLogos;
    public Sprite[] shopLogoBgs;
    public Sprite[] shopViewBgs;
    public Sprite[] shopItemBgs;
    public int currentShop = 0;
    public Transform[] shopLocations;
    public Transform[] shopItemPreviewPos;

    [Header("Purchase Display")]
    public Transform purchaseButtonsPool;
    public List<ItemButton> purchaseButtons;
    public List<ItemInfo> availableItems;

    [Header("Reputation Display")]
    public Transform repMilestonePool;
    public List<RepMilestone> repMilestones;

    [Header("Investments Display")]
    public Transform investmentButtonsPool;
    public List<ItemButton> investmentButtons;

    [Header("Item Display")]
    public GameObject itemDisplayView;
    public GameObject itemDetailsDisplay;
    public GameObject itemUpgradesDisplay;
    public ItemInfo displayedItemInfo;
    public Text itemName;
    public List<Button> rotationViews;
    public Image itemImage;
    public Text itemBudgetCost;
    public Text itemUnlockCost;
    public List<GameObject> attributesList;

    [Header("Item Preview")]
    public Sprite defaultWallpaper;
    public Sprite defaultFlooring;
    public SpriteRenderer wallpaperPreview;
    public SpriteRenderer flooringPreview;
    public GameObject currentItemPreview;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < purchaseButtonsPool.childCount; i++)
        {
            purchaseButtons.Add(purchaseButtonsPool.GetChild(i).GetComponent<ItemButton>());
        }
        for (int i = 0; i < repMilestonePool.childCount; i++)
        {
            repMilestones.Add(repMilestonePool.GetChild(i).GetComponent<RepMilestone>());
        }
        for (int i = 0; i < investmentButtonsPool.childCount; i++)
        {
            investmentButtons.Add(investmentButtonsPool.GetChild(i).GetComponent<ItemButton>());
        }

        SelectShop(0, 0);
        DisplayPurchases();

        //UpdateAvailableItems();
        //displayedItemInfo = availableItems[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ScrollShop("left");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ScrollShop("right");
        }
    }

    public void ScrollShop(string type)
    {
        if (type == "left")
        {
            currentShop -= 1;
            if (currentShop < 0)
            {
                currentShop = shopTypes.Length - 1;
            }
            SelectShop(currentShop, -1);
        }
        else if (type == "right")
        {
            currentShop += 1;
            if (currentShop > shopTypes.Length - 1)
            {
                currentShop = 0;
            }
            SelectShop(currentShop, 1);
        }
    }

    public IEnumerator SwitchShopCamera(Transform focusTarget, int direction)
    {
        formerShopCameraView.color = new Color(1,1,1,1);
        currentShopCameraView.color = new Color(1, 1, 1, 0);

        formerShopCamera.transform.position = new Vector3(currentShopCamera.transform.position.x, currentShopCamera.transform.position.y, -10);
        currentShopCamera.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, -10);

        int i = 0;

        if (direction == 1)
        {
            Debug.Log("right");
            while (i <= 25)
            {
                formerShopCameraView.color = new Color(1, 1, 1, 1 - i / 25f);
                currentShopCameraView.color = new Color(1, 1, 1, i / 25f);
                formerShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-i * 2, 0, 0);
                currentShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 - i * 2, 0, 0);
                i += 1;
                yield return new WaitForSeconds(0.005f);
            }
        }
        else if (direction == -1)
        {
            Debug.Log("left");
            while (i <= 25)
            {
                formerShopCameraView.color = new Color(1, 1, 1, 1 - i / 25f);
                currentShopCameraView.color = new Color(1, 1, 1, i / 25f);
                formerShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 2, 0, 0);
                currentShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(i * 2 - 50, 0, 0);
                i += 1;
                yield return new WaitForSeconds(0.005f);
            }
        }
        else if (direction == 0)
        {
            formerShopCameraView.color = new Color(1, 1, 1, 0);
            currentShopCameraView.color = new Color(1, 1, 1, 1);
            currentShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    /*public IEnumerator FocusCamera(Transform focusTarget)
    {
        while (Vector2.Distance(focusTarget.position, currentShopCamera.transform.position) > 0.1f)
        {
            Vector3 newPos = Vector2.SmoothDamp(currentShopCamera.transform.position, focusTarget.position, ref velocity, 0.2f);
            currentShopCamera.transform.position = new Vector3(newPos.x, newPos.y, -10);
            yield return new WaitForFixedUpdate();
        }

        currentShopCamera.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, -10);
    }*/

    public void SelectShop(int idx, int direction)
    {
        itemDisplayView.SetActive(false);

        if (focusingCamera != null)
        {
            StopCoroutine(focusingCamera);
        }
        focusingCamera = StartCoroutine(SwitchShopCamera(shopLocations[idx], direction));
        shopLogo.sprite = shopLogos[idx];
        shopLogoBg.sprite = shopLogoBgs[idx];
        shopViewBg.sprite = shopViewBgs[idx];

        //RESET WALLPAPER AND FLOORING
        wallpaperPreview.sprite = defaultWallpaper;
        flooringPreview.sprite = defaultFlooring;

        //SET UP DISPLAYS
        SetupPurchasesDisplay();
        SetupReputationDisplay();
        SetupInvestmentsDisplay();

        if (currentItemPreview != null)
        {
            Destroy(currentItemPreview);
        }
    }

    public void SetupPurchasesDisplay()
    {
        List<ItemInfo> shopItems = new List<ItemInfo>();

        for (int i = 0; i < availableItems.Count; i++)
        {
            if (availableItems[i].itemType == shopTypes[currentShop])
            {
                shopItems.Add(availableItems[i]);
            }
        }

        foreach (ItemButton purchaseButton in purchaseButtons)
        {
            purchaseButton.GetComponent<Image>().sprite = shopItemBgs[currentShop];
            purchaseButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < shopItems.Count; i++)
        {
            purchaseButtons[i].gameObject.SetActive(true);
            purchaseButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            purchaseButtons[i].SetDetails(shopItems[i]);

            int x = new int();
            x = i;
            purchaseButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ViewPurchasableItem(shopItems[x]); });
        }
    }

    public void SetupInvestmentsDisplay()
    {
        List<ItemInfo> upgradableItems = new List<ItemInfo>();

        for (int i = 0; i < GameManager.current.unlockedItems.Count; i++)
        {
            if (GameManager.current.unlockedItems[i].itemType == shopTypes[currentShop])
            {
                upgradableItems.Add(GameManager.current.unlockedItems[i]);
            }
        }

        foreach (ItemButton investmentButton in investmentButtons)
        {
            investmentButton.GetComponent<Image>().sprite = shopItemBgs[currentShop];
            investmentButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < upgradableItems.Count; i++)
        {
            investmentButtons[i].gameObject.SetActive(true);
            investmentButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            investmentButtons[i].SetDetails(upgradableItems[i]);

            int x = new int();
            x = i;
            investmentButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ViewUpgradeableItem(upgradableItems[x]); });
        }
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

    public void ViewPurchasableItem(ItemInfo itemInfo)
    {
        itemDisplayView.SetActive(true);
        itemDetailsDisplay.SetActive(true);
        itemUpgradesDisplay.SetActive(false);

        displayedItemInfo = itemInfo;

        itemName.text = itemInfo.name;
        if (itemInfo.customType == CustomizationType.Item)
        {
            itemImage.sprite = itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        }
        else if (itemInfo.customType == CustomizationType.Sprite)
        {
            itemImage.sprite = itemInfo.itemSprite;
        }
        itemBudgetCost.text = itemInfo.cost.ToString();
        itemUnlockCost.text = itemInfo.cost.ToString();

        //CLEAR ALL ROTATION TOGGLES
        foreach (Button button in rotationViews)
        {
            button.gameObject.SetActive(false);
        }

        //ACTIVATE ROTATION TOGGLES
        for (int i = 0; i < itemInfo.itemPrefab.Length; i++)
        {
            rotationViews[i].gameObject.SetActive(true);
        }

        //CLEAR ALL ATTRIBUTES
        foreach (GameObject attribute in attributesList)
        {
            attribute.SetActive(false);
        }

        //FIND ATTRIBUTES
        List<string> attributes = new List<string>();
        foreach (ColorType colorType in itemInfo.colors)
        {
            attributes.Add(colorType.ToString());
        }
        foreach (ThemeType themeType in itemInfo.themes)
        {
            attributes.Add(themeType.ToString());
        }
        foreach (CategoryType categoryType in itemInfo.categoryTypes)
        {
            attributes.Add(categoryType.ToString());
        }

        //ADD ATTRIBUTES
        for (int i = 0; i < attributes.Count; i++)
        {
            attributesList[i].SetActive(true);
            attributesList[i].GetComponentInChildren<Text>().text = attributes[i];
        }

        //SPAWN ITEM PREVIEW
        if (itemInfo.customType == CustomizationType.Item)
        {
            if (currentItemPreview != null)
            {
                Destroy(currentItemPreview);
            }
            GameObject spawnedItem = Instantiate(itemInfo.itemPrefab[0]) as GameObject;
            currentItemPreview = spawnedItem;
            spawnedItem.transform.parent = shopLocations[currentShop];
            spawnedItem.transform.position = shopItemPreviewPos[currentShop].position;
        }
        else if (itemInfo.customType == CustomizationType.Sprite)
        {
            if (itemInfo.itemType == ItemType.Wallpaper)
            {
                wallpaperPreview.sprite = itemInfo.itemSprite;
            }
            else if (itemInfo.itemType == ItemType.Flooring)
            {
                flooringPreview.sprite = itemInfo.itemSprite;
            }
        }
    }

    public void ViewUpgradeableItem(ItemInfo itemInfo)
    {
        itemDisplayView.SetActive(true);
        itemDetailsDisplay.SetActive(false);
        itemUpgradesDisplay.SetActive(true);

        displayedItemInfo = itemInfo;

        itemName.text = itemInfo.name;

        //SPAWN ITEM PREVIEW
        if (itemInfo.customType == CustomizationType.Item)
        {
            if (currentItemPreview != null)
            {
                Destroy(currentItemPreview);
            }
            GameObject spawnedItem = Instantiate(itemInfo.itemPrefab[0]) as GameObject;
            currentItemPreview = spawnedItem;
            spawnedItem.transform.parent = shopLocations[currentShop];
            spawnedItem.transform.position = shopItemPreviewPos[currentShop].position;
        }
        else if (itemInfo.customType == CustomizationType.Sprite)
        {
            if (itemInfo.itemType == ItemType.Wallpaper)
            {
                wallpaperPreview.sprite = itemInfo.itemSprite;
            }
            else if (itemInfo.itemType == ItemType.Flooring)
            {
                flooringPreview.sprite = itemInfo.itemSprite;
            }
        }
    }

    public void RotateItemDisplay(int view)
    {
        itemImage.sprite = displayedItemInfo.itemPrefab[view].GetComponent<SpriteRenderer>().sprite;
        if (view == 3)
        {
            itemImage.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            itemImage.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }

        if (currentItemPreview != null)
        {
            Destroy(currentItemPreview);
        }
        GameObject spawnedItem = Instantiate(displayedItemInfo.itemPrefab[view]);
        spawnedItem.transform.parent = shopLocations[currentShop];
        spawnedItem.transform.position = shopItemPreviewPos[currentShop].position;
        currentItemPreview = spawnedItem;
    }

    public void BuyItem()
    {
        if (GameManager.current.playerMoney >= displayedItemInfo.cost)
        {
            GameManager.current.ChangePlayerMoney(-displayedItemInfo.cost);
            GameManager.current.unlockedItems.Add(displayedItemInfo);

            //DISABLE SHOP BUTTON
            foreach (ItemButton purchaseButton in purchaseButtons)
            {
                if (purchaseButton.itemInfo == displayedItemInfo)
                {
                    purchaseButton.DisableButton("Sold!");
                }
            }

            if (availableItems.Contains(displayedItemInfo))
            {
                availableItems.Remove(displayedItemInfo);
            }
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    public void DisplayPurchases()
    {
        reputationDisplay.SetActive(false);
        investmentsDisplay.SetActive(false);
        purchasesDisplay.SetActive(true);
        shopCategoryButtons[0].image.color = Color.white;
        shopCategoryButtons[1].image.color = Color.gray;
        shopCategoryButtons[2].image.color = Color.gray;
        SetupPurchasesDisplay();
    }

    public void DisplayReputation()
    {
        purchasesDisplay.SetActive(false);
        investmentsDisplay.SetActive(false);
        reputationDisplay.SetActive(true);
        shopCategoryButtons[0].image.color = Color.gray;
        shopCategoryButtons[1].image.color = Color.white;
        shopCategoryButtons[2].image.color = Color.gray;
        SetupReputationDisplay();

        itemDisplayView.SetActive(false);
    }

    public void DisplayInvestments()
    {
        purchasesDisplay.SetActive(false);
        reputationDisplay.SetActive(false);
        investmentsDisplay.SetActive(true);
        shopCategoryButtons[0].image.color = Color.gray;
        shopCategoryButtons[1].image.color = Color.gray;
        shopCategoryButtons[2].image.color = Color.white;
        SetupInvestmentsDisplay();
    }

    public void SetupReputationDisplay()
    {
        for (int i = 0; i < repMilestones.Count; i++)
        {
            repMilestones[i].repNumBg.sprite = shopLogoBgs[currentShop];
            repMilestones[i].repMilestoneBg.sprite = shopViewBgs[currentShop];
            repMilestones[i].repNum.text = (i + 1).ToString();

            if (i + 1 < GameManager.current.npcReps[currentShop] / 100)
            {
                repMilestones[i].repFillAmount.fillAmount = 0;
                repMilestones[i].repNumBg.color = new Color(1,1,1,1f);
                repMilestones[i].repNum.color = Color.yellow;
                repMilestones[i].repMilestoneBg.color = new Color(1,1,1,1f);
                repMilestones[i].repUnlockText.color = Color.white;
            }
            else if (i + 1 == GameManager.current.npcReps[currentShop] / 100)
            {
                repMilestones[i].repFillAmount.fillAmount = 1 - GameManager.current.npcReps[currentShop] % 100 / 100f;
                repMilestones[i].repNumBg.color = new Color(0.75f, 0.75f, 0.75f, 1);
                repMilestones[i].repNum.color = Color.white;
                repMilestones[i].repMilestoneBg.color = new Color(1, 1, 1, 1f);
                repMilestones[i].repUnlockText.color = Color.white;
            }
            else if (i + 1 > GameManager.current.npcReps[currentShop] / 100)
            {
                repMilestones[i].repFillAmount.fillAmount = 1;
                repMilestones[i].repNumBg.color = new Color(0.5f, 0.5f, 0.5f, 1);
                repMilestones[i].repNum.color = Color.white;
                repMilestones[i].repMilestoneBg.color = new Color(0.5f, 0.5f, 0.5f, 1f);
                repMilestones[i].repUnlockText.color = Color.gray;
            }
        }
    }
}
