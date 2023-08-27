using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles UI references and updating in Mission scene.
/// </summary>
public class MissionUIManager : MonoBehaviour
{
    public static MissionUIManager current;

    public TextMeshProUGUI missionNameDisplay;
    public TextMeshProUGUI clientNameDisplay;

    public Transform criteriaContentArea;
    public AffixEntry criteriaEntryTemplate;

    List<AffixEntry> criteriaEntries = new List<AffixEntry>();
    public Mission mission;

    private void Awake()
    {
        current = this;
    }

    public void Start()
    {
        //mission = MissionManager.current.currentMission;
        if (mission == null)
        {
            return;
        }
        MissionInfoPopulate();
    }

    public void MissionInfoPopulate()
    {
        clientNameDisplay.text = mission.missionClient.clientName;
        missionNameDisplay.text = mission.missionTitle;
        //infoMissionDesc.text = mission.missionDesc;
        //infoMissionDescEnd.text = mission.missionClient.closingPhrase + ", " + mission.missionClient.clientName;

        int money = mission.missionMoney;
        List<Requirement> criteria = new List<Requirement>(mission.requirements);

        float nextCriteriaEntryPos = -15f;

        // Populate mission criteria list with mission requirements.
        if (criteria.Count > 0)
        {
            foreach (Requirement r in criteria)
            {
                GameObject newCriteriaEntry;
                AffixEntry entry;
                newCriteriaEntry = Instantiate(criteriaEntryTemplate.gameObject, new Vector3(0, nextCriteriaEntryPos, 0), new Quaternion(0, 0, 0, 0), criteriaContentArea);
                newCriteriaEntry.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, nextCriteriaEntryPos, 0);
                entry = newCriteriaEntry.GetComponent<AffixEntry>();

                //entry.SetEntryRequirement(r);
                criteriaEntries.Add(entry);

                nextCriteriaEntryPos -= 50;
            }
        }
    }

    /*public void IncreaseCriteriaProgress(ItemInfo item)
    {
        foreach (AffixEntry r in criteriaEntries)
        {
            r.IncreaseProgress(item);
        }
    }

    public void DecreaseCriteriaProgress(ItemInfo item)
    {
        foreach (AffixEntry r in criteriaEntries)
        {
            r.DecreaseProgress(item);
        }
    }*/
}
