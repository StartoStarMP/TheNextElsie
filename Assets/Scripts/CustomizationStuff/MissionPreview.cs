using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionPreview : MonoBehaviour
{
    public static MissionPreview current;
    public Text missionPreviewStatus;

    public MissionInfo currentMissionInfo;
    public Text missionNameText;
    public NPCDisplay npcDisplay;
    public Text dimensionsText;
    public RoomConstructor roomConstructor;
    public Text moneyText;

    public Image test;
    public Texture2D currentCapture;

    private void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        roomConstructor = RoomConstructor.FindObjectOfType<RoomConstructor>();
    }

    public void UpdateMissionPreview(MissionInfo missionInfo)
    {
        currentMissionInfo = missionInfo;
        missionNameText.text = missionInfo.missionName;
        npcDisplay.CreateNPCDisplay(missionInfo.clientInfo);
        dimensionsText.text = "Dimensions:\n" + missionInfo.gridWidth + " x " + missionInfo.gridHeight;
        roomConstructor.CreateRoom(missionInfo.borderSprite, missionInfo.wallSprite, missionInfo.floorSprite, missionInfo.gridWidth, missionInfo.gridHeight);
        moneyText.text = missionInfo.missionMoney.ToString();
    }

    public void OpenMissionPreview(MissionInfo missionInfo)
    {
        missionPreviewStatus.text = "Opening...";
        GetComponent<Animator>().SetTrigger("open");
        UpdateMissionPreview(missionInfo);
        StartCoroutine(Timer(x => missionPreviewStatus.text = "Accept Job", 1.25f));
    }

    public void CloseMissionPreview()
    {
        missionPreviewStatus.text = "Closing...";
        GetComponent<Animator>().SetTrigger("close");
    }

    public void AcceptMission()
    {
        GetComponent<Animator>().SetTrigger("accept");

        StartCoroutine(Timer(x => GameManager.current.currentMission = currentMissionInfo, 1.25f));
        StartCoroutine(Timer(x => SceneLoader.current.LoadScene("MissionSceneNew", LoadingScreenType.StartingMission), 1.25f));
    }

    public void StartMission(MissionInfo missionInfo)
    {
        roomConstructor = RoomConstructor.FindObjectOfType<RoomConstructor>();
        missionPreviewStatus.text = "Starting...";
        UpdateMissionPreview(missionInfo);
        GetComponent<Animator>().SetTrigger("start");
    }

    IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            currentCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            currentCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            currentCapture.Apply();
            //Sprite sprite = Sprite.Create(currentCapture, new Rect(0, 0, Screen.width, Screen.height - (Screen.height * 200/1080)), new Vector2(0, 0));
            //test.sprite = sprite;

            SceneLoader.current.LoadScene("MissionSceneNew", LoadingScreenType.StartingMission);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Screen capture failed!");
            Debug.LogError(e.ToString());
        }
    }

    IEnumerator Timer(System.Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
