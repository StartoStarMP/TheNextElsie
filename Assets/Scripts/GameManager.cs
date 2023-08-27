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
    public bool inEditMode = false;

    [Header("Items")]
    public ItemInfo[] wallObjects;
    public ItemInfo[] floorObjects;
    public ItemInfo[] rugObjects;
    public ItemInfo[] wallpapers;
    public ItemInfo[] floorTiles;

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

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<GameManager>() != null)
            return;

        Instantiate(Resources.Load("GameManager"));
    }
}

public enum NPCVendor { Walter, Flo, Ruth }