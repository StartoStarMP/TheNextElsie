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

    [Header("Shop Details")]
    public Image shopLogo;
    public Image shopLogoBg;
    public Image shopViewBg;
    public Sprite[] shopLogos;
    public Sprite[] shopLogoBgs;
    public Sprite[] shopViewBgs;
    public Sprite[] shopItemBgs;
    public int currentShop = 0;
    public Transform[] shopLocations;

    [Header("Shop Items")]
    public Transform shopButtonsPool;
    public List<Button> shopButtons;
    public List<ItemInfo> availableItems;

    [Header("Item Details")]
    public ItemInfo displayedItemInfo;
    public Text itemName;
    public Text dimensions;
    public List<Button> rotationViews;
    public Image itemImage;
    public Text itemBudgetCost;
    public Text itemUnlockCost;
    public List<GameObject> attributesList;

    [Header("Item Preview")]
    public GameObject currentItemPreview;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopButtonsPool.childCount; i++)
        {
            shopButtons.Add(shopButtonsPool.GetChild(i).GetComponent<Button>());
        }
        SelectShop(0, true);
        
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
                currentShop = shopLocations.Length - 1;
            }
            SelectShop(currentShop, false);
        }
        else if (type == "right")
        {
            currentShop += 1;
            if (currentShop > shopLocations.Length - 1)
            {
                currentShop = 0;
            }
            SelectShop(currentShop, true);
        }
    }

    public IEnumerator SwitchShopCamera(Transform focusTarget, bool right = true)
    {
        formerShopCameraView.color = new Color(1,1,1,1);
        currentShopCameraView.color = new Color(1, 1, 1, 0);

        formerShopCamera.transform.position = new Vector3(currentShopCamera.transform.position.x, currentShopCamera.transform.position.y, -10);
        currentShopCamera.transform.position = new Vector3(focusTarget.position.x, focusTarget.position.y, -10);

        int i = 0;

        if (right)
        {
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
        else
        {
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
        while (i <= 25)
        {
            formerShopCameraView.color = new Color(1, 1, 1, 1 - i/25f);
            currentShopCameraView.color = new Color(1, 1, 1, i / 25f);
            formerShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(-i * 2,0,0);
            currentShopCameraView.GetComponent<RectTransform>().anchoredPosition = new Vector3(50 - i * 2, 0, 0);
            i += 1;
            Debug.Log(i);
            yield return new WaitForSeconds(0.005f);
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

    public void SelectShop(int idx, bool right)
    {
        if (focusingCamera != null)
        {
            StopCoroutine(focusingCamera);
        }
        focusingCamera = StartCoroutine(SwitchShopCamera(shopLocations[idx], right));
        shopLogo.sprite = shopLogos[idx];
        shopLogoBg.sprite = shopLogoBgs[idx];
        shopViewBg.sprite = shopViewBgs[idx];

        foreach (Button shopButton in shopButtons)
        {
            shopButton.GetComponent<Image>().sprite = shopItemBgs[idx];
        }

        //UPDATE BUTTONS
        foreach (Button shopItemButton in shopButtons)
        {
            shopItemButton.gameObject.SetActive(false);
        }

        for (int i = 0; i < availableItems.Count; i++)
        {
            shopButtons[i].gameObject.SetActive(true);
            shopButtons[i].GetComponent<ItemButton>().itemInfo = availableItems[i];
            shopButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = availableItems[i].itemPrefab[0].GetComponent<SpriteRenderer>().sprite;

            int x = new int();
            x = i;
            shopButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ViewItem(availableItems[x]); });
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

    public void ViewItem(ItemInfo itemInfo)
    {
        displayedItemInfo = itemInfo;

        itemName.text = itemInfo.name;
        itemImage.sprite = itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        itemBudgetCost.text = itemInfo.cost.ToString();

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
        foreach (ItemType itemType in itemInfo.itemTypes)
        {
            attributes.Add(itemType.ToString());
        }

        //ADD ATTRIBUTES
        for (int i = 0; i < attributes.Count; i++)
        {
            attributesList[i].SetActive(true);
            attributesList[i].GetComponentInChildren<Text>().text = attributes[i];
        }

        if (currentItemPreview != null)
        {
            Destroy(currentItemPreview);
        }
        GameObject spawnedItem = Instantiate(itemInfo.itemPrefab[0]) as GameObject;
        spawnedItem.transform.parent = shopLocations[currentShop];
        spawnedItem.transform.localPosition = new Vector3(4, 0, 0);
        currentItemPreview = spawnedItem;
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
        GameObject spawnedItem = Instantiate(displayedItemInfo.itemPrefab[view]) as GameObject;
        spawnedItem.transform.parent = shopLocations[currentShop];
        spawnedItem.transform.localPosition = new Vector3(0, 0, 0);
        currentItemPreview = spawnedItem;
    }
}
