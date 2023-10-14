using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles scene loading and the loading screen.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader current;
    public LoadingScreenType currentLoadingScreenType = LoadingScreenType.None;

    [Header("Mission Preview")]
    public MissionInfo currentMissionInfo;
    public Text missionPreviewStatus;
    public Text missionNameText;
    public NPCDisplay npcDisplay;
    public AffixManager affixesDisplay;
    public Text dimensionsText;
    public Text moneyText;
    public Image itemBlueprintIcon;
    public Text itemBlueprintName;

    [Header("Location Change")]
    public bool locationChangeActive = false;
    public int selectedLocationIdx = 0;
    public Image nextLocationImage;
    public Text nextLocationName;
    public List<Image> locationIcons;
    public List<string> locationNames;
    public List<Sprite> locationImageSprites;
    public List<Sprite> locationIconSprites;

    //[Header("Black Screen")]
    

    private void Awake()
    {
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            current = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("Mission") && currentLoadingScreenType == LoadingScreenType.StartingMission)
        {
            StartMission(GameManager.current.currentMission);
        }
        else if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("Home") && currentLoadingScreenType == LoadingScreenType.EndingMission)
        {

        }
        else if (currentLoadingScreenType == LoadingScreenType.LocationChange)
        {
            GetComponent<Animator>().SetTrigger("locationChangeOut");
        }
        else if (currentLoadingScreenType == LoadingScreenType.BlackScreen)
        {
            GetComponent<Animator>().SetTrigger("blackFadeOut");
        }
        currentLoadingScreenType = LoadingScreenType.None;

        //CHOOSE CORRECT MUSIC
        if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("Mission"))
        {
            List<string> designMusic = new List<string>(){ "Design1"};
            int randInt = UnityEngine.Random.Range(0, designMusic.Count);
            AudioManager.current.PlayMusic(designMusic[randInt]);
        }
        else if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("Home"))
        {
            AudioManager.current.PlayMusic("Home");
        }
        else if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("Furniture Plaza"))
        {
            AudioManager.current.PlayMusic("CoolShop");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //CHOOSE CORRECT MUSIC
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Mission"))
        {
            List<string> designMusic = new List<string>() { "Design1" };
            int randInt = UnityEngine.Random.Range(0, designMusic.Count);
            AudioManager.current.PlayMusic(designMusic[randInt]);
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Home"))
        {
            AudioManager.current.PlayMusic("Home");
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Furniture Plaza"))
        {
            AudioManager.current.PlayMusic("CoolShop");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (locationChangeActive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                ScrollLocation("up");
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                ScrollLocation("down");
            }
        }
    }

    public void SetLocationChangeMenu(bool active)
    {
        if (active)
        {
            Debug.Log("ran in");
            GetComponent<Animator>().SetTrigger("locationChangeIn");
            locationChangeActive = true;
        }
        else
        {
            Debug.Log("ran out");
            GetComponent<Animator>().SetTrigger("locationChangeOut");
            locationChangeActive = false;
        }
    }

    public void ScrollLocation(string direction)
    {
        if (direction == "up")
        {
            GetComponent<Animator>().SetTrigger("locationScrollUp");
            selectedLocationIdx = ListOffset(selectedLocationIdx, locationIconSprites.Count, 1);
            SelectLocation(selectedLocationIdx, true);
        }
        else if (direction == "down")
        {
            GetComponent<Animator>().SetTrigger("locationScrollDown");
            selectedLocationIdx = ListOffset(selectedLocationIdx, locationIconSprites.Count, -1);
            SelectLocation(selectedLocationIdx, false);
        }
    }

    public void SelectLocation(int idx = 0, bool up = true)
    {
        int locationsLength = locationIconSprites.Count;

        if (up == true)
        {
            locationIcons[0].sprite = locationIconSprites[ListOffset(idx, locationsLength, locationsLength - 4)];
            locationIcons[1].sprite = locationIconSprites[ListOffset(idx, locationsLength, -1)];
            locationIcons[2].sprite = locationIconSprites[ListOffset(idx, locationsLength, -2)];
            locationIcons[3].sprite = locationIconSprites[ListOffset(idx, locationsLength, -3)];
            nextLocationName.text = locationNames[idx];
            nextLocationImage.sprite = locationImageSprites[idx];

            StartCoroutine(Timer(x => locationIcons[0].sprite = locationIconSprites[idx], 0.125f));
        }
        else
        {
            locationIcons[0].sprite = locationIconSprites[idx];
            locationIcons[1].sprite = locationIconSprites[ListOffset(idx, locationsLength, -1)];
            locationIcons[2].sprite = locationIconSprites[ListOffset(idx, locationsLength, -2)];
            locationIcons[3].sprite = locationIconSprites[ListOffset(idx, locationsLength, locationsLength - 3)];
            nextLocationName.text = locationNames[idx];
            nextLocationImage.sprite = locationImageSprites[idx];

            StartCoroutine(Timer(x => locationIcons[3].sprite = locationIconSprites[ListOffset(idx, locationsLength, -3)], 0.125f));
        }
    }

    public void ConfirmLocation()
    {
        if (selectedLocationIdx == 1 || selectedLocationIdx == 3)
        {
            Debug.Log(locationNames[selectedLocationIdx] + " scene not found.");
            return;
        }

        SetLocationChangeMenu(false);

        StartCoroutine(Timer(x => LoadScene(locationNames[selectedLocationIdx], LoadingScreenType.BlackScreen), 0.5f));
    }

    /// <summary>
    /// Loads the specified scene.
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName, LoadingScreenType loadingScreenType)
    {
        currentLoadingScreenType = loadingScreenType;

        if (loadingScreenType == LoadingScreenType.BlackScreen)
        {
            GetComponent<Animator>().SetTrigger("blackFadeIn");
            StartCoroutine(Timer(x => SceneManager.LoadScene(sceneName), 0.5f));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void UpdateMissionPreview(MissionInfo missionInfo)
    {
        currentMissionInfo = missionInfo;
        missionNameText.text = missionInfo.missionName;
        npcDisplay.CreateNPCDisplay(missionInfo.clientInfo);
        dimensionsText.text = "Dimensions:\n" + missionInfo.gridWidth + " x " + missionInfo.gridHeight;
        affixesDisplay.SetCurrentMissionAffixes(missionInfo.affixes);
        affixesDisplay.PopulateAffixDisplay();
        FindObjectOfType<RoomConstructor>().CreateRoom(missionInfo.borderSprite, missionInfo.wallSprite, missionInfo.floorSprite, missionInfo.gridWidth, missionInfo.gridHeight);
        moneyText.text = missionInfo.missionMoney.ToString();
        if (missionInfo.itemBlueprint != null)
        {
            itemBlueprintIcon.gameObject.SetActive(true);
            itemBlueprintIcon.sprite = missionInfo.itemBlueprint.GetItemSpriteToDisplay();
            itemBlueprintName.text = missionInfo.itemBlueprint.name;
        }
        else
        {
            itemBlueprintIcon.gameObject.SetActive(false);
            itemBlueprintIcon.sprite = null;
            itemBlueprintName.text = "";
        }
    }

    public void OpenMissionPreview(MissionInfo missionInfo)
    {
        missionPreviewStatus.text = "Opening...";
        GetComponent<Animator>().SetTrigger("missionPreviewOpen");
        UpdateMissionPreview(missionInfo);
        StartCoroutine(Timer(x => missionPreviewStatus.text = "Accept Job", 1.25f));
    }

    public void CloseMissionPreview()
    {
        missionPreviewStatus.text = "Closing...";
        GetComponent<Animator>().SetTrigger("missionPreviewClose");
    }

    public void AcceptMission()
    {
        GetComponent<Animator>().SetTrigger("missionPreviewAccept");

        StartCoroutine(Timer(x => GameManager.current.currentMission = currentMissionInfo, 1.25f));
        StartCoroutine(Timer(x => SceneLoader.current.LoadScene("Mission", LoadingScreenType.StartingMission), 1.25f));
    }

    public void StartMission(MissionInfo missionInfo)
    {
        //roomConstructor = RoomConstructor.FindObjectOfType<RoomConstructor>();
        missionPreviewStatus.text = "Starting...";
        UpdateMissionPreview(missionInfo);
        GetComponent<Animator>().SetTrigger("missionPreviewStart");
    }

    /*IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            currentCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            currentCapture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            currentCapture.Apply();
            //Sprite sprite = Sprite.Create(currentCapture, new Rect(0, 0, Screen.width, Screen.height - (Screen.height * 200/1080)), new Vector2(0, 0));
            //test.sprite = sprite;

            SceneLoader.current.LoadScene("Mission", LoadingScreenType.StartingMission);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Screen capture failed!");
            Debug.LogError(e.ToString());
        }
    }*/

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }

    public int ListOffset(int idx, int listLength, int offset)
    {
        if (offset > 0)
        {
            for (int i = 0; i < offset; i++)
            {
                idx += 1;
                if (idx >= listLength)
                {
                    idx = 0;
                }
            }
        }
        else
        {
            for (int i = 0; i > offset; i--)
            {
                idx -= 1;
                if (idx < 0)
                {
                    idx = listLength - 1;
                }
            }
        }
        return idx;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<SceneLoader>() != null)
            return;

        Instantiate(Resources.Load("SceneLoader"));
    }
}

public enum LoadingScreenType
{
    StartingMission, EndingMission, LocationChange, BlackScreen, None
}
