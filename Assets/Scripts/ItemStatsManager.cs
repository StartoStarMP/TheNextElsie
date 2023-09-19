using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStatsManager : MonoBehaviour
{
    public static ItemStatsManager current;
    public ItemStat[] itemStats;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpgradeItemDiscountTier(ItemInfo itemInfo)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                itemStat.discountTier += 1;
                return;
            }
        }
    }

    public void UpgradeItemQualityTier(ItemInfo itemInfo)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                itemStat.qualityTier += 1;
                return;
            }
        }
    }

    public void UnlockStyle(ItemInfo itemInfo, int styleIdx)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                itemStat.stylesUnlocked.Add(styleIdx);
            }
        }
    }

    public int GetDiscountTier(ItemInfo itemInfo)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                return itemStat.discountTier;
            }
        }
        return 0;
    }

    public int GetQualityTier(ItemInfo itemInfo)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                return itemStat.qualityTier;
            }
        }
        return 0;
    }

    public List<int> GetUnlockedStyles(ItemInfo itemInfo)
    {
        foreach (ItemStat itemStat in itemStats)
        {
            if (itemStat.item = itemInfo)
            {
                return itemStat.stylesUnlocked;
            }
        }
        return null;
    }
}

[System.Serializable]
public class ItemStat
{
    public ItemInfo item;
    public int discountTier;
    public int qualityTier;
    public List<int> stylesUnlocked = new List<int> { 0 };
}
