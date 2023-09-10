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
    public Text moneyReward;

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

        moneyReward.text = missionInfo.missionMoney.ToString();

        //MissionInfo x;
        //x = newMissionInfo;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { SceneLoader.current.OpenMissionPreview(newMissionInfo); } );
    }
}
