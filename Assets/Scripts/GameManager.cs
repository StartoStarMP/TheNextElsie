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

    private void Awake()
    {
        current = this;
        npcReps = new List<int>() { 10, 20, 30 };
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

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetSceneByBuildIndex(level) == SceneManager.GetSceneByName("MissionSceneNew"))
        {
            MissionPreview.current.StartMission(currentMission);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void RuntimeInit()
    {
        if (!Debug.isDebugBuild || FindObjectOfType<GameManager>() != null)
            return;

        var go = new GameObject { name = "GameManager" };
        Instantiate(Resources.Load("GameManager"));
    }
}

public enum NPCVendor { Vander, Silco, Caitlyn }