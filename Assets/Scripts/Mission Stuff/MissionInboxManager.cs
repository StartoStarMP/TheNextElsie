using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles interactions with the Mission Inbox panel.
/// </summary>
public class MissionInboxManager : MonoBehaviour
{
    public static MissionInboxManager current;

    [Header("Mission Inbox Panel")]
    [SerializeField] Image missionInboxPanel;
    [SerializeField] Image missionInboxPanelBG;
    [SerializeField] GameObject missionInboxEntryTemplate;
    public RectTransform missionInboxContent;
    public List<GameObject> missionInboxEntries;

    [Header("Mission Info Panel")]
    [SerializeField] Image missionInfoPanel;
    [SerializeField] Image missionInfoPanelBG;
    [SerializeField] TextMeshProUGUI infoMissionTitle;
    [SerializeField] TextMeshProUGUI infoMissionClient;
    [SerializeField] TextMeshProUGUI infoMissionDesc;
    [SerializeField] TextMeshProUGUI infoMissionDescEnd;

    [Header("Mission Rewards Panel")]
    [SerializeField] Image infoMissionRewardsPanel;
    [SerializeField] Sprite rewardsButtonSelected_1;
    [SerializeField] Sprite rewardsButtonSelected_2;
    Sprite currentSelectedSprite;
    [SerializeField] Sprite coinSprite;
    [SerializeField] GameObject rewardsEntryTemplate;
    [SerializeField] RectTransform rewardsPanelContentParent;
    public List<GameObject> rewardsEntries;

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            current = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        missionInboxPanel.gameObject.SetActive(false);
        missionInboxPanelBG.gameObject.SetActive(false);
        missionInfoPanel.gameObject.SetActive(false);
        missionInfoPanelBG.gameObject.SetActive(false);
        infoMissionRewardsPanel.gameObject.SetActive(false);

        currentSelectedSprite = rewardsButtonSelected_1;
    }

    public void ShowMissionInboxPanel()
    {
        missionInboxPanel.gameObject.SetActive(true);
        missionInboxPanelBG.gameObject.SetActive(true);
    }

    public void HideMissionInboxPanel()
    {
        missionInboxPanel.gameObject.SetActive(false);
        missionInboxPanelBG.gameObject.SetActive(false);
    }

    public void ToggleMissionInboxPanel()
    {
        missionInboxPanel.gameObject.SetActive(!missionInboxPanel.gameObject.activeInHierarchy);
        missionInboxPanelBG.gameObject.SetActive(!missionInboxPanelBG.gameObject.activeInHierarchy);
    }

    public bool MissionInboxActive()
    {
        return missionInboxPanel.gameObject.activeInHierarchy;
    }

    // Mission Info Panel ------------------------------------------------------

    public void ToggleMissionInfoPanel()
    {
        missionInfoPanel.gameObject.SetActive(!missionInfoPanel.gameObject.activeInHierarchy);
        missionInfoPanelBG.gameObject.SetActive(!missionInfoPanelBG.gameObject.activeInHierarchy);
    }

    public bool MissionInfoActive()
    {
        return missionInfoPanel.gameObject.activeInHierarchy;
    }

    public void MissionInboxRefresh()
    {
        MissionManager.current.RefreshMissions();
    }

    public void MissionInboxPopulate()
    {
        // First, clear current inbox entries.
        foreach (GameObject entry in missionInboxEntries)
        {
            Destroy(entry.gameObject);
        }
        missionInboxEntries.Clear();

        // Second, instantiate new inbox entries, then add to missionInboxEntry list.
        float nextInboxEntryPos = 240f;
        /*foreach (Mission mission in MissionManager.current.missions)
        {
            GameObject newInboxEntry = Instantiate(missionInboxEntryTemplate, new Vector3(0, nextInboxEntryPos, 0), new Quaternion(0, 0, 0, 0), missionInboxContent);
            newInboxEntry.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, nextInboxEntryPos, 0);
            newInboxEntry.GetComponent<MissionInboxEntry>().mission = mission;
            newInboxEntry.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = mission.missionTitle;
            nextInboxEntryPos -= 90;
            missionInboxEntries.Add(newInboxEntry);
        }*/
    }

    /// <summary>
    /// Populates the mission info panel with Mission info.
    /// </summary>
    public void MissionInfoPopulate(Mission mission)
    {
        //MissionManager.current.currentMission = mission;

        infoMissionClient.text = mission.missionClient.clientName;
        infoMissionTitle.text = mission.missionTitle;
        infoMissionDesc.text = mission.missionDesc;
        infoMissionDescEnd.text = mission.missionClient.closingPhrase + ", " + mission.missionClient.clientName;

        int money = mission.missionMoney;
        List<Reward> rewards = mission.missionRewards;

        // First, clear current reward display entries.
        foreach (GameObject entry in rewardsEntries)
        {
            Destroy(entry.gameObject);
        }
        rewardsEntries.Clear();

        float nextRewardsEntryPos = 44f;
        // Populate rewards panel with mission rewards, if applicable.
        //  Money reward first (all missions have a money reward).
        if (money > 0)
        {
            Debug.Log("Assigning cash reward of " + money);
            GameObject newRewardsEntry;
            newRewardsEntry = Instantiate(rewardsEntryTemplate, new Vector3(0, nextRewardsEntryPos, 0), new Quaternion(0, 0, 0, 0), rewardsPanelContentParent);
            newRewardsEntry.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, nextRewardsEntryPos, 0);
            newRewardsEntry.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = coinSprite;
            newRewardsEntry.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = money.ToString();
            nextRewardsEntryPos -= 39;
            rewardsEntries.Add(newRewardsEntry);
        }
        //  Other rewards.
        if (rewards.Count > 0)
        {
            foreach (Reward r in rewards)
            {
                GameObject newRewardsEntry;
                newRewardsEntry = Instantiate(rewardsEntryTemplate, new Vector3(0, nextRewardsEntryPos, 0), new Quaternion(0, 0, 0, 0), rewardsPanelContentParent);
                newRewardsEntry.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, nextRewardsEntryPos, 0);
                newRewardsEntry.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = r.rewardIcon;
                newRewardsEntry.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = r.rewardName;
                nextRewardsEntryPos -= 39;
                rewardsEntries.Add(newRewardsEntry);
            }
        }
    }

    public void ToggleMissionRewardsPanel(Button rewardsButton = null)
    {
        infoMissionRewardsPanel.gameObject.SetActive(!infoMissionRewardsPanel.gameObject.activeInHierarchy);
        if (rewardsButton)
        {
            rewardsButton.image.sprite = currentSelectedSprite;

            if (currentSelectedSprite == rewardsButtonSelected_1) { currentSelectedSprite = rewardsButtonSelected_2; }
            else { currentSelectedSprite = rewardsButtonSelected_1; }
        }
    }

    public bool MissionRewardsPanelActive()
    {
        return infoMissionRewardsPanel.gameObject.activeInHierarchy;
    }


}