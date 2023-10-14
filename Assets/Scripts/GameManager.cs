using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    public MissionInfo currentMission;

    public int playerXP;
    public int playerMoney;
    public List<int> npcReps;

    [Tooltip("If true, will enable MouseHover checks in Item script. False by default.")]
    //public bool inEditMode = false;

    [Header("Items")]
    public List<ItemInfo> unlockedItems;
    public List<ItemInfo> lockedItems;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<ItemInfo> GetAvailableItems(List<ItemType> itemTypes)
    {
        List<ItemInfo> availableItems = new List<ItemInfo>();

        foreach (ItemInfo item in unlockedItems)
        {
            if (itemTypes.Contains(item.itemType))
            {
                availableItems.Add(item);
            }
        }

        return availableItems;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<GameManager>() != null)
            return;

        Instantiate(Resources.Load("GameManager"));
    }

    public void ChangePlayerMoney(int moneyChange)
    {
        playerMoney = playerMoney + moneyChange;
        
        if (FindObjectOfType<PlayerMoneyCounter>() != null)
        {
            FindObjectOfType<PlayerMoneyCounter>().ChangeMoney(moneyChange);
        }
    }

    public void AddPlayerXP(int xpAdded)
    {
        playerXP = playerXP + xpAdded;

        if (FindObjectOfType<PlayerLevelSlider>() != null)
        {
            FindObjectOfType<PlayerLevelSlider>().AddXP(xpAdded);
        }
    }
}

public enum NPCVendor { Walter, Flo, Ruth }