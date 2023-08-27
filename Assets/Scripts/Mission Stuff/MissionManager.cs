using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

/// <summary>
/// Handles Mission setup, adding new Missions, etc.
/// Interacts with SceneLoader.cs to load Scenes.
/// </summary>
public class MissionManager : MonoBehaviour
{
    public static MissionManager current;

    private string[] missionNames1 = new string[] { "Decorate", "Design", "Beautify", "Set up", "Furnish" };
    private string[] missionNames2 = new string[] { "the", "my", "this", "a", "our" };
    private string[] missionNames3 = new string[] { "Business", "Living Space", "Office", "Room", "Area" };
    public Sprite[] backgrounds;
    public Sprite[] faces;
    public Sprite[] hairstyles;
    public Sprite[] eyes;
    public Sprite[] mouths;
    public Color[] skinColors;
    public Color[] hairColors;

    public Sprite[] borderOptions;
    public Sprite[] wallOptions;
    public Sprite[] floorOptions;

    public Text laptopStatus;
    public Transform missionButtonPool;
    [HideInInspector]
    public List<MissionButton> missionButtons;

    int procGenMissionIndex = 0;

    private void Awake()
    {
        current = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 0; i < missionButtonPool.childCount; i++)
        {
            missionButtons.Add(missionButtonPool.GetChild(i).GetComponent<MissionButton>());
        }
        RefreshMissions();
    }

    public void Update()
    {
    }

    public void SetMissionsActive(bool active)
    {
        if (active && !gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
            laptopStatus.text = "Opening...";
            GetComponent<Animator>().SetTrigger("open");
        }
        else if (!active && gameObject.activeInHierarchy)
        {
            laptopStatus.text = "Closing...";
            GetComponent<Animator>().SetTrigger("close");
            StartCoroutine(Timer(x => gameObject.SetActive(false), 0.75f));
        }
    }

    public void GenerateMissions(int count)
    {
        
    }

    public MissionInfo GenerateRandomMission()
    {
        MissionInfo newMission = ScriptableObject.CreateInstance<MissionInfo>();
        //GENERATE MISSION DETAILS
        newMission.missionName = missionNames1[Random.Range(0, missionNames1.Length)] + " " + missionNames2[Random.Range(0, missionNames2.Length)] +  " " + missionNames3[Random.Range(0, missionNames3.Length)];

        //GENERATE NPC
        ClientInfo newClient = ScriptableObject.CreateInstance<ClientInfo>();

        newClient.backgroundSprite = backgrounds[Random.Range(0, backgrounds.Length)];
        newClient.faceSprite = faces[Random.Range(0, faces.Length)];
        newClient.hairSprite = hairstyles[Random.Range(0, hairstyles.Length)];
        newClient.eyeSprite = eyes[Random.Range(0, eyes.Length)];
        newClient.mouthSprite = mouths[Random.Range(0, mouths.Length)];

        newClient.bg1Color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        newClient.bg2Color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        newClient.faceColor = skinColors[Random.Range(0, skinColors.Length)];
        newClient.hairColor = hairColors[Random.Range(0, hairColors.Length)];

        newMission.clientInfo = newClient;

        //GENERATE GRID
        newMission.borderSprite = borderOptions[Random.Range(0, borderOptions.Length)];
        newMission.wallSprite = wallOptions[Random.Range(0, wallOptions.Length)];
        newMission.floorSprite = floorOptions[Random.Range(0, floorOptions.Length)];

        newMission.gridWidth = Random.Range(3, 10);
        newMission.gridHeight = Random.Range(4, 10);

        //GENERATE AFFIXES
        int numAffixes = Random.Range(0,4);

        for (int i = 0; i < numAffixes; i++)
        {
            Requirement newRequirement = new Requirement();
            newRequirement.reqType = (RequirementType)Random.Range(0, (int)System.Enum.GetValues(typeof(RequirementType)).Cast<RequirementType>().Max());

            if (newRequirement.reqType == RequirementType.Color)
            {
                newRequirement.color = (ColorType)Random.Range(0, (int)System.Enum.GetValues(typeof(ColorType)).Cast<ColorType>().Max());
            }
            else if (newRequirement.reqType == RequirementType.Item)
            {
                newRequirement.item = GameManager.current.wallObjects[Random.Range(0, GameManager.current.wallObjects.Length)];
            }
            else if (newRequirement.reqType == RequirementType.ItemType)
            {
                newRequirement.itemType = (ItemType)Random.Range(0, (int)System.Enum.GetValues(typeof(ItemType)).Cast<ItemType>().Max());
            }
            else if (newRequirement.reqType == RequirementType.RoomType)
            {
                newRequirement.roomType = (RoomType)Random.Range(0, (int)System.Enum.GetValues(typeof(RoomType)).Cast<RoomType>().Max());
            }
            else if (newRequirement.reqType == RequirementType.Theme)
            {
                newRequirement.theme = (ThemeType)Random.Range(0, (int)System.Enum.GetValues(typeof(ThemeType)).Cast<ThemeType>().Max());
            }
            else if (newRequirement.reqType == RequirementType.Unique)
            {
                newRequirement.uniqueCondition = (UniqueConditionType)Random.Range(0, (int)System.Enum.GetValues(typeof(UniqueConditionType)).Cast<UniqueConditionType>().Max());
            }

            newMission.affixes.Add(newRequirement);
        }

        //GENERATE REWARDS
        newMission.missionMoney = 50 * Random.Range(2, 10);

        return newMission;
    }

    public void RefreshMissions()
    {
        //missions.RemoveAll(x => x.isStoryMission == false);
        //GenerateMissions(3);
        foreach (MissionButton missionButton in missionButtons)
        {
            missionButton.SetMission(GenerateRandomMission());
        }
    }

    IEnumerator Timer(System.Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}
