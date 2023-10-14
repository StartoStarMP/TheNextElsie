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

    public void SetDetails(ItemInfo newItemInfo, bool required = false)
    {
        itemInfo = newItemInfo;
        itemImage.sprite = itemInfo.GetItemSpriteToDisplay();

        GetComponent<Button>().interactable = true;

        if (required)
        {
            GetComponent<Image>().color = new Color(1,1,1,1);
            GetComponent<Outline>().effectColor = new Color(1,0,0.5f,1);
        }
        else
        {
            GetComponent<Image>().color = new Color(1,1,1,0);
            GetComponent<Outline>().effectColor = new Color(0, 0, 0, 1);
        }
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
