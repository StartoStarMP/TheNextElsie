using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public ItemInfo itemInfo;
    public Text itemName;
    public Text itemBudgetCost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetDetails(ItemInfo newItemInfo)
    {
        itemInfo = newItemInfo;
        itemName.text = itemInfo.name;
        itemBudgetCost.text = itemInfo.cost.ToString();
    }

    public void ShowDetails()
    {
        itemName.gameObject.SetActive(true);
        itemBudgetCost.gameObject.SetActive(true);
    }

    public void HideDetails()
    {
        itemName.gameObject.SetActive(false);
        itemBudgetCost.gameObject.SetActive(false);
    }
}
