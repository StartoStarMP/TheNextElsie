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
        moneyReward.text = missionInfo.missionMoney.ToString();

        //MissionInfo x;
        //x = newMissionInfo;
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(delegate { MissionPreview.current.OpenMissionPreview(newMissionInfo); } );
    }
}
