using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public AudioClip[] shopMusic;
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

    [Header("Item Upgrades")]
    //DISCOUNTS
    public Text itemDiscountedCost;
    public EventTrigger[] discountLockButtons;
    //QUALITY
    public Image itemQuality;
    public Sprite[] qualitySprites;
    public EventTrigger[] qualityLockButtons;
    //STYLES
    public Text itemStyles;
    public GameObject[] styleButtons;
    public Image[] styleImages;
    public EventTrigger[] styleLockButtons;

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

        AudioManager.current.PlaySoundEffect("brush-Stardew");
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
            //Debug.Log("right");
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
            //Debug.Log("left");
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

        float currentTrackTime = AudioManager.current.musicPlayer.time;
        AudioManager.current.PlayMusic(shopMusic[idx].name, currentTrackTime);
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
        AudioManager.current.PlaySoundEffect("bigSelect-Stardew");

        itemDisplayView.SetActive(true);
        itemDetailsDisplay.SetActive(true);
        itemUpgradesDisplay.SetActive(false);

        displayedItemInfo = itemInfo;

        itemName.text = itemInfo.name;
        itemImage.sprite = itemInfo.GetItemSpriteToDisplay();
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
        //PLAY SOUND EFFECT ON PRESSING ITEM BUTTON

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

        UpdateItemUpgrades();
    }

    public void RotateItemDisplay(int view)
    {
        //PLAY SOUND EFFECT ROTATE ITEM DISPLAY

        if (displayedItemInfo.customType == CustomizationType.Item)
        {
            itemImage.sprite = displayedItemInfo.itemPrefab[view].GetComponent<SpriteRenderer>().sprite;
        }
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
            //PLAY SOUND EFFECT BUY ITEM

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
            //PLAY SOUND EFFECT NOT ENOUGH MONEY

            Debug.LogError("Not enough money.");
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

        //PLAY SOUND EFFECT SWITCH DISPLAY
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

        //PLAY SOUND EFFECT SWITCH DISPLAY
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

        //PLAY SOUND EFFECT SWITCH DISPLAY
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

    public void UpdateItemUpgrades()
    {
        //DISCOUNTS
        int discountTier = ItemStatsManager.current.GetDiscountTier(displayedItemInfo);

        if (discountTier != 0)
        {
            itemDiscountedCost.text = (displayedItemInfo.cost - (displayedItemInfo.cost * Mathf.Pow(2, (discountTier - 1)) * 0.05f)).ToString();
        }
        else
        {
            itemDiscountedCost.text = displayedItemInfo.cost.ToString();
        }

        for (int i = 0; i < discountLockButtons.Length; i++)
        {
            discountLockButtons[i].triggers.Clear();

            if (i < discountTier)
            {
                discountLockButtons[i].gameObject.SetActive(false);
            }
            else if (i == discountTier)
            {
                discountLockButtons[i].gameObject.SetActive(true);
                int x = i;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => 
                { 
                    StopAllCoroutines();
                    StartCoroutine(_Fill(discountLockButtons[x].GetComponent<Image>(), delegate
                    {
                        UpgradeDiscount();
                    }));
                });
                discountLockButtons[i].triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerUp;
                entry2.callback.AddListener((data) =>
                {
                    StopAllCoroutines();
                    StartCoroutine(_Unfill(discountLockButtons[x].GetComponent<Image>()));
                });
                discountLockButtons[x].triggers.Add(entry2);
            }
            else if (i > discountTier)
            {
                discountLockButtons[i].gameObject.SetActive(true);
            }
        }

        //QUALITY
        int qualityTier = ItemStatsManager.current.GetQualityTier(displayedItemInfo);

        itemQuality.sprite = qualitySprites[qualityTier];

        for (int i = 0; i < qualityLockButtons.Length; i++)
        {
            qualityLockButtons[i].triggers.Clear();

            if (i < qualityTier)
            {
                qualityLockButtons[i].gameObject.SetActive(false);
            }
            else if (i == qualityTier)
            {
                qualityLockButtons[i].gameObject.SetActive(true);

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                int x = i;
                entry.callback.AddListener((data) =>
                {
                    StopAllCoroutines();
                    StartCoroutine(_Fill(qualityLockButtons[x].GetComponent<Image>(), delegate
                    {
                        UpgradeQuality();
                    }));
                });
                qualityLockButtons[i].triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerUp;
                entry2.callback.AddListener((data) =>
                {
                    StopAllCoroutines();
                    StartCoroutine(_Unfill(qualityLockButtons[x].GetComponent<Image>()));
                });
                qualityLockButtons[x].triggers.Add(entry2);
            }
            else if (i > qualityTier)
            {
                qualityLockButtons[i].gameObject.SetActive(true);
            }
        }

        //STYLES
        List<int> stylesUnlocked = ItemStatsManager.current.GetUnlockedStyles(displayedItemInfo);
        
        itemStyles.text = "Styles (" + stylesUnlocked.Count + "/" + displayedItemInfo.itemStyles.Count + ")";
        foreach (GameObject styleButton in styleButtons)
        {
            styleButton.SetActive(false);
        }

        for (int i = 0; i < displayedItemInfo.itemStyles.Count; i++)
        {
            styleButtons[i].SetActive(true);
            styleLockButtons[i].triggers.Clear();
            styleImages[i].sprite = displayedItemInfo.itemStyles[i].sprites[0];
            if (stylesUnlocked.Contains(i))
            {
                styleLockButtons[i].gameObject.SetActive(false);
            }
            else
            {
                styleLockButtons[i].gameObject.SetActive(true);
                styleLockButtons[i].GetComponent<Image>().fillAmount = 1;
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                int x = i;
                entry.callback.AddListener((data) =>
                {
                    StopAllCoroutines();
                    StartCoroutine(_Fill(styleLockButtons[x].GetComponent<Image>(), delegate
                    {
                        UnlockStyle(x);
                    }));
                });
                styleLockButtons[x].triggers.Add(entry);

                EventTrigger.Entry entry2 = new EventTrigger.Entry();
                entry2.eventID = EventTriggerType.PointerUp;
                entry2.callback.AddListener((data) =>
                {
                    StopAllCoroutines();
                    StartCoroutine(_Unfill(styleLockButtons[x].GetComponent<Image>()));
                });
                styleLockButtons[x].triggers.Add(entry2);
            }
        }
    }

    public IEnumerator _Fill(Image lockOverlay, Action action)
    {
        //PLAY SOUND EFFECT FILL

        while (lockOverlay.fillAmount > 0)
        {
            lockOverlay.fillAmount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        action.Invoke();
    }

    public IEnumerator _Unfill(Image lockOverlay)
    {
        //STOP PLAYING SOUND EFFECT FILL

        while (lockOverlay.fillAmount < 1)
        {
            lockOverlay.fillAmount += 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void UpgradeDiscount()
    {
        //PLAY SOUND EFFECT UPGRADE

        ItemStatsManager.current.UpgradeItemDiscountTier(displayedItemInfo);
        UpdateItemUpgrades();
    }

    public void UpgradeQuality()
    {
        //PLAY SOUND EFFECT UPGRADE

        ItemStatsManager.current.UpgradeItemQualityTier(displayedItemInfo);
        UpdateItemUpgrades();
    }

    public void UnlockStyle(int styleIdx)
    {
        //PLAY SOUND EFFECT UPGRADE

        ItemStatsManager.current.UnlockStyle(displayedItemInfo, styleIdx);
        UpdateItemUpgrades();
    }
}
