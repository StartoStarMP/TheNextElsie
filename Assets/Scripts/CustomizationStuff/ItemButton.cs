using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public ItemInfo itemInfo;
    public Image itemImage;
    //public Text itemName;
    //public Text itemBudgetCost;
    public GameObject disableOverlay;
    public Text disableText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetDetails(ItemInfo newItemInfo)
    {
        itemInfo = newItemInfo;

        if (itemInfo.customType == CustomizationType.Item)
        {
            //itemName.text = itemInfo.name;
            //itemBudgetCost.text = itemInfo.cost.ToString();
            itemImage.sprite = itemInfo.itemPrefab[0].GetComponent<SpriteRenderer>().sprite;
        }
        else if (itemInfo.customType == CustomizationType.Sprite)
        {
            itemImage.sprite = itemInfo.itemSprite;
        }

        GetComponent<Button>().interactable = true;
        disableOverlay.SetActive(false);
    }

    public void DisableButton(string disablePrompt)
    {
        GetComponent<Button>().interactable = false;
        disableOverlay.SetActive(true);
        disableText.text = disablePrompt;
    }

    /*public void ShowDetails()
    {
        itemName.gameObject.SetActive(true);
        itemBudgetCost.gameObject.SetActive(true);
    }

    public void HideDetails()
    {
        itemName.gameObject.SetActive(false);
        itemBudgetCost.gameObject.SetActive(false);
    }*/
}
