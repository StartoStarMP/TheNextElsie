using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionButton : MonoBehaviour
{
    public MissionInfo missionInfo;

    public NPCDisplay npcDisplay;
    public GridDisplay gridDisplay;
    public Text missionName;
    public Text missionAffixes;
    public Text missionMoney;
    public Image missionBlueprintIcon;
    public Text missionBlueprintText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMission(MissionInfo newMissionInfo)
    {
        missionInfo = newMissionInfo;
        npcDisplay.CreateNPCDisplay(missionInfo.clientInfo);
        gridDisplay.CreateGrid(missionInfo.borderSprite, missionInfo.wallSprite, missionInfo.floorSprite, missionInfo.gridWidth, missionInfo.gridHeight);
        missionName.text = missionInfo.missionName;
        missionAffixes.text = "";
        for(int i = 0; i < missionInfo.affixes.Count; i++)
        {
            if (missionInfo.affixes[i].reqType == RequirementType.CategoryType)
            {
                missionAffixes.text += "Required Category Type";
            }
            else if (missionInfo.affixes[i].reqType == RequirementType.Item)
            {
                missionAffixes.text += "Required Items";
            }
            else if (missionInfo.affixes[i].reqType == RequirementType.Color)
            {
                missionAffixes.text += "Preferred Color";
            }
            else if (missionInfo.affixes[i].reqType == RequirementType.RoomType)
            {
                missionAffixes.text += "Room Type";
            }
            else if (missionInfo.affixes[i].reqType == RequirementType.Theme)
            {
                missionAffixes.text += "Preferred Theme";
            }

            if (i != missionInfo.affixes.Count - 1)
            {
                missionAffixes.text += "\n";
            }
        }

        missionMoney.text = missionInfo.missionMoney.ToString();
        if (missionInfo.itemBlueprint != null)
        {
            missionBlueprintIcon.gameObject.SetActive(true);
            missionBlueprintIcon.sprite = missionInfo.itemBlueprint.GetItemSpriteToDisplay();
            missionBlueprintText.text = missionInfo.itemBlueprint.name;
        }
        else
        {
            missionBlueprintIcon.gameObject.SetActive(false);
            missionBlueprintIcon.sprite = null;
            missionBlueprintText.text = "";
        }

        //MissionInfo x;
        //x = newMissionInfo;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { SceneLoader.current.OpenMissionPreview(newMissionInfo); } );
    }
}
