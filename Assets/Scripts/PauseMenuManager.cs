using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMenu;

    [Header("Item Catalog")]
    public ItemType[] categoryTypes;
    public Transform catalogButtonsPool;
    public List<Button> catalogButtons;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < catalogButtonsPool.childCount; i++)
        {
            catalogButtons.Add(catalogButtonsPool.GetChild(i).GetComponent<Button>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
    }

    //ITEM CATALOG
    public void UpdateItemCatalog(int categoryIdx)
    {
        //UPDATE BUTTONS
        foreach (Button catalogButton in catalogButtons)
        {
            catalogButton.gameObject.SetActive(false);
        }

        List<ItemInfo> catalogItems = new List<ItemInfo>();
        List<ItemInfo> availableItems = GameManager.current.GetAvailableItems(new List<ItemType>() { ItemType.WallObject, ItemType.FloorObject, ItemType.RugObject, ItemType.Wallpaper, ItemType.Flooring});

        for (int i = 0; i < availableItems.Count; i++)
        {
            if (availableItems[i].itemType == categoryTypes[categoryIdx])
            {
                catalogItems.Add(availableItems[i]);
            }
        }

        for (int i = 0; i < catalogItems.Count; i++)
        {
            catalogButtons[i].gameObject.SetActive(true);
            //catalogButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            catalogButtons[i].GetComponent<ItemButton>().SetDetails(catalogItems[i]);

            int x = new int();
            x = i;
            //shopButtons[i].GetComponent<Button>().onClick.AddListener(delegate { ViewItem(shopItems[x]); });
        }
    }
}
