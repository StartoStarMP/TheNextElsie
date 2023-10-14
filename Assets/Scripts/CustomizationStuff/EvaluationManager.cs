using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using System.Linq;

public class EvaluationManager : MonoBehaviour
{
    public static EvaluationManager current;

    [Header("Starting")]
    public NPCDisplay clientDisplay;
    public GameObject evaluationUI;
    public bool finishButtonOn = true;
    public Image certifiedIcon;
    public Animator evaluationStartAnim;

    [Header("Results")]
    public AffixManager affixManager;
    public List<float> calculatedPathLengths;
    public List<Vector3> currentPath;
    public float roomNavigationStat;
    public Slider evaluationSlider;
    public Text breakdownText;
    public Animator evaluationAnim;
    public ParticleSystem confetti;
    public List<string> breakdownList = new List<string>();
    public List<int> pointList = new List<int>();
    public Button forwardRecapButton;

    [Header("Recap")]
    public RawImage evaluationRoomDisplay;
    public int rankLevel;
    public Text flairText;
    public Image starsRank;
    public Sprite[] starRankSprites;
    public int recapPhase = 0;
    public Text scoreBreakdown;
    public PlayerLevelSlider playerLevelSlider;
    public GameObject playerRewards;
    public PlayerMoneyCounter moneyCount;
    public Image unlockedItemIcon;
    public Text unlockedItemText;
    public int[] numItemsPerCategory = new int[3];
    public GameObject repSliders;
    public Button endMissionButton;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        //StartCoroutine(RoomNavigation());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFilling()
    {
        if (!finishButtonOn)
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(_Fill());
    }

    public void StopFilling()
    {
        if (!finishButtonOn)
        {
            return;
        }

        /*if (GetComponent<GraphicRaycaster>() == null)
        {
            return;
        }*/

        StopAllCoroutines();
        StartCoroutine(_Unfill());
    }

    public IEnumerator _Fill()
    {
        while (certifiedIcon.fillAmount < 1)
        {
            certifiedIcon.fillAmount += 0.015f;
            yield return new WaitForSeconds(0.01f);
        }
        finishButtonOn = false;
        StartCoroutine(GatherPointsBreakdown());
    }

    public IEnumerator _Unfill()
    {
        while (certifiedIcon.fillAmount > 0)
        {
            certifiedIcon.fillAmount -= 0.05f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator StartPointsBreakdown()
    {
        for (int i = 0; i < breakdownList.Count; i++)
        {
            breakdownText.text = breakdownList[i] + " + " + pointList[i];
            evaluationSlider.value += pointList[i];
            if (evaluationSlider.value < 250)
            {
                //rankText.text = "F";
            }
            else if (evaluationSlider.value < 500)
            {
                //rankText.text = "C";
            }
            else if (evaluationSlider.value < 750)
            {
                //rankText.text = "B";
            }
            else if (evaluationSlider.value < 1000)
            {
                //rankText.text = "A";
            }
            else if (evaluationSlider.value == 1000)
            {
                //rankText.text = "S";
            }

            scoreBreakdown.text += breakdownList[i] + " + " + pointList[i];
            if (i != breakdownList.Count - 1)
            {
                scoreBreakdown.text += "\n";
            }

            evaluationAnim.SetTrigger("bounce");
            yield return new WaitForSeconds(1f);
        }

        evaluationAnim.SetTrigger("rankReveal");
        yield return new WaitForSeconds(0.5f);
        confetti.Play();
        if (evaluationSlider.value < 200)
        {
            rankLevel = 0;
            flairText.text = "Awful...";
            starsRank.sprite = starRankSprites[0];
        }
        else if (evaluationSlider.value < 400)
        {
            rankLevel = 1;
            flairText.text = "Ok...";
            starsRank.sprite = starRankSprites[1];
        }
        else if (evaluationSlider.value < 600)
        {
            rankLevel = 2;
            flairText.text = "Great!";
            starsRank.sprite = starRankSprites[2];
        }
        else if (evaluationSlider.value < 800)
        {
            rankLevel = 3;
            flairText.text = "Amazing!";
            starsRank.sprite = starRankSprites[3];
        }
        else if (evaluationSlider.value == 800)
        {
            rankLevel = 4;
            flairText.text = "PERFECT!!!";
            starsRank.sprite = starRankSprites[4];
        }
    }

    public void ForwardRecap()
    {
        StartCoroutine(ForwardRecapCoroutine());
    }

    public IEnumerator ForwardRecapCoroutine()
    {
        forwardRecapButton.interactable = false;
        recapPhase += 1;
        //SHOW SCORE BREAKDOWN
        if (recapPhase == 1)
        {
            StartCoroutine(EvaluationLevelDisplayDarken());
            yield return UIElementFlyInOut(scoreBreakdown.gameObject, true);

            forwardRecapButton.interactable = true;
        }
        //SHOW PLAYER LEVEL
        else if (recapPhase == 2)
        {
            StartCoroutine(UIElementFlyInOut(scoreBreakdown.gameObject, false));
            yield return UIElementFlyInOut(playerLevelSlider.gameObject, true);

            GameManager.current.AddPlayerXP(50 + 100 * rankLevel);
            forwardRecapButton.interactable = true;
        }
        //SHOW REWARDS
        else if (recapPhase == 3)
        {
            if (GameManager.current.currentMission.itemBlueprint != null)
            {
                unlockedItemIcon.gameObject.SetActive(true);
                unlockedItemIcon.sprite = GameManager.current.currentMission.itemBlueprint.GetItemSpriteToDisplay();
                unlockedItemText.text = GameManager.current.currentMission.itemBlueprint.name;
                GameManager.current.unlockedItems.Add(GameManager.current.currentMission.itemBlueprint);
                GameManager.current.lockedItems.Remove(GameManager.current.currentMission.itemBlueprint);
            }
            else
            {
                unlockedItemIcon.gameObject.SetActive(false);
                unlockedItemText.text = "";
            }

            StartCoroutine(UIElementFlyInOut(playerLevelSlider.gameObject, false));
            yield return UIElementFlyInOut(playerRewards, true);

            //moneyCount.AddMoney((int)(GameManager.current.currentMission.missionMoney * rankLevel / 3));
            GameManager.current.ChangePlayerMoney((int)(GameManager.current.currentMission.missionMoney * rankLevel / 3f));
            forwardRecapButton.interactable = true;
        }
        //SHOW REPUTATION
        else if (recapPhase == 4)
        {
            StartCoroutine(UIElementFlyInOut(playerRewards, false));
            yield return UIElementFlyInOut(repSliders, true);

            repSliders.transform.GetChild(0).GetComponent<NPCRepSlider>().AddRep(numItemsPerCategory[0] * 10);
            repSliders.transform.GetChild(1).GetComponent<NPCRepSlider>().AddRep(numItemsPerCategory[1] * 10);
            repSliders.transform.GetChild(2).GetComponent<NPCRepSlider>().AddRep(numItemsPerCategory[2] * 10);

            forwardRecapButton.interactable = true;
        }
        else if (recapPhase >= 5)
        {
            EndMission();
        }
    }

    public IEnumerator UIElementFlyInOut(GameObject uiElement, bool flyIn)
    {
        if (flyIn)
        {
            while (uiElement.GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                uiElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(uiElement.GetComponent<RectTransform>().anchoredPosition.x, uiElement.GetComponent<RectTransform>().anchoredPosition.y + 25, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (uiElement.GetComponent<RectTransform>().anchoredPosition.y < 300)
            {
                uiElement.GetComponent<RectTransform>().anchoredPosition = new Vector3(uiElement.GetComponent<RectTransform>().anchoredPosition.x, uiElement.GetComponent<RectTransform>().anchoredPosition.y + 25, 0);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    public IEnumerator EvaluationLevelDisplayDarken()
    {
        while (evaluationRoomDisplay.color.a > 0.5f)
        {
            evaluationRoomDisplay.color = new Color(1, 1, 1, evaluationRoomDisplay.color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator GatherPointsBreakdown()
    {
        clientDisplay.CreateNPCDisplay(GameManager.current.currentMission.clientInfo);
        evaluationStartAnim.SetTrigger("start");

        yield return new WaitForSeconds(1f);

        evaluationUI.SetActive(true);

        //GET TOTAL POINT VALUE
        AddToBreakdown("Total Point Value", (int)(250 * TotalPointValue() * 1.2f / DesignManager.current.maxBudget));
        //ESSENTIAL REQUIREMENTS
        AddToBreakdown("Essentials", (int)(250 * Essentials()));
        //ROOM NAVIGATION
        CoroutineWithData cd = new CoroutineWithData(this, RoomNavigation());
        yield return cd.coroutine;
        AddToBreakdown("Room Navigation", (int)(50 * (float)cd.result));
        //REMAINING BUDGET
        AddToBreakdown("Remaining Budget", (int)(250 * 0.5f * DesignManager.current.currentBudget/DesignManager.current.maxBudget));
        //DECORATIONS
        AddToBreakdown("Decorations", (int)Decorations());

        Item[] placedItems = Item.FindObjectsOfType<Item>();
        foreach (Item item in placedItems)
        {
            if (item.itemInfo.targetLayer == "WallObjects")
            {
                numItemsPerCategory[0] += 1;
            }
            else if (item.itemInfo.targetLayer == "FloorObjects")
            {
                numItemsPerCategory[1] += 1;
            }
            else if (item.itemInfo.targetLayer == "WallObjects")
            {
                numItemsPerCategory[2] += 1;
            }
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(StartPointsBreakdown());
    }

    public void AddToBreakdown(string category, int points)
    {
        breakdownList.Add(category);
        pointList.Add(points);
    }

    public void EndMission()
    {
        SceneLoader.current.LoadScene("Home", LoadingScreenType.BlackScreen);
    }

    public void OnPathComplete(Path p)
    {
        // We got our path back
        if (p.error)
        {
            // Nooo, a valid path couldn't be found
        }
        else
        {
            // Yay, now we can get a Vector3 representation of the path
            // from p.vectorPath
            currentPath = p.vectorPath;
            //Debug.Log(p.GetTotalLength());
        }
    }

    public float GetRoomDensity()
    {
        AstarPath.active.Scan();
        var gg = AstarPath.active.data.gridGraph;
        int walkableNodes = 0;
        gg.GetNodes(node =>
        {
            if (node.Walkable)
            {
                walkableNodes += 1;
            }
        });

        return (1 - walkableNodes / (gg.width * gg.depth * 1.0f));
    }

    public IEnumerator StartPathing(Vector3 position1, Vector3 position2)
    {
        int numPaths = 0;
        calculatedPathLengths.Clear();

        List<Vector3> position1Variants = new List<Vector3>();
        position1Variants.Add(position1);
        position1Variants.Add(position1 + (transform.up * 0.5f));
        position1Variants.Add(position1 - (transform.up * 0.5f));
        position1Variants.Add(position1 + (transform.right * 0.5f));
        position1Variants.Add(position1 - (transform.right * 0.5f));
        List<Vector3> position2Variants = new List<Vector3>();
        position2Variants.Add(position2);
        position2Variants.Add(position2 + (transform.up * 0.5f));
        position2Variants.Add(position2 - (transform.up * 0.5f));
        position2Variants.Add(position2 + (transform.right * 0.5f));
        position2Variants.Add(position2 - (transform.right * 0.5f));

        for (int i = 0; i < position1Variants.Count; i++)
        {
            for (int j = 0; j < position2Variants.Count; j++)
            {
                if (PathUtilities.IsPathPossible(AstarPath.active.GetNearest(position1Variants[i]).node, AstarPath.active.GetNearest(position2Variants[j]).node))
                {
                    numPaths += 1;
                    Path p = ABPath.Construct(position1Variants[i], position2Variants[j], GetPathLength);
                    AstarPath.StartPath(p);
                }
            }
        }

        yield return new WaitUntil(() => calculatedPathLengths.Count == numPaths);

        if (calculatedPathLengths.Count == 0)
        {
            yield return 10000;
        }
        else
        {
            yield return calculatedPathLengths.Min();
        }
    }

    public void GetPathLength(Path p)
    {
        calculatedPathLengths.Add(p.GetTotalLength());
    }

    public float EndPathing()
    {
        if (calculatedPathLengths.Count == 0)
        {
            return 10000;
        }
        else
        {
            return calculatedPathLengths.Min();
        }
    }

    public float Essentials()
    {
        //Item[] items = Item.FindObjectsOfType<Item>();
        int currentPoints = 0;
        int maxPoints = 0;
        affixManager.GetCurrentAffixesProgress();

        return affixManager.GetTotalAffixCompletionStat();
    }

    public int Decorations()
    {
        List<ThemeType> themesPresent = new List<ThemeType>();
        List<int> values = new List<int>();
        int decorationStat = 0;

        foreach (ThemeType themeType in DesignManager.current.themeTypes)
        {
            if (!themesPresent.Contains(themeType))
            {
                themesPresent.Add(themeType);
                values.Add(1);
            }
            else
            {
                values[themesPresent.IndexOf(themeType)] += 1;
            }
        }

        foreach (ThemeType themeType in themesPresent)
        {
            if (values[themesPresent.IndexOf(themeType)] >= 3)
            {
                decorationStat += 50;
            }
        }

        return decorationStat;
    }

    public IEnumerator RoomNavigation()
    {
        List<Vector3> navigablePos = new List<Vector3>();
        Item[] items = Item.FindObjectsOfType<Item>();
        int possiblePaths = 0;
        int allPaths = 0;

        foreach (Item item in items)
        {
            if (item.itemInfo.categoryTypes.Contains(CategoryType.Bed) || item.itemInfo.categoryTypes.Contains(CategoryType.Chair) || item.itemInfo.categoryTypes.Contains(CategoryType.Dresser)
                || item.itemInfo.categoryTypes.Contains(CategoryType.Table) || item.itemInfo.categoryTypes.Contains(CategoryType.Sofa) || item.itemInfo.categoryTypes.Contains(CategoryType.CoffeeTable)
                || item.itemInfo.categoryTypes.Contains(CategoryType.Stand) || item.itemInfo.categoryTypes.Contains(CategoryType.Bench))
            {
                Vector3 newPosition = new Vector3(item.transform.position.x, item.transform.position.y, 1);
                navigablePos.Add(newPosition);
            }
        }

        //Debug.Log(navigablePos.Count);

        for (int i = 0; i < navigablePos.Count; i++)
        {
            if (i != navigablePos.Count - 1)
            {
                for (int j = i + 1; j < navigablePos.Count; j++)
                {
                    CoroutineWithData cd = new CoroutineWithData(this, StartPathing(navigablePos[i], navigablePos[j]));
                    yield return cd.coroutine;

                    //Debug.Log("Path length: " + cd.result);
                    //Debug.Log("Distance between beds: " + Vector3.Distance(navigablePos[i], navigablePos[j]));

                    //Debug.Log(navigablePos[i]);
                    //Debug.Log(navigablePos[j]);

                    if ((float)cd.result < Vector3.Distance(navigablePos[i], navigablePos[j]) + 2)
                    {
                        possiblePaths += 1;
                    }
                    allPaths += 1;
                }
            }
        }

        //Debug.Log("possible paths: " + possiblePaths);
        //Debug.Log("all paths: " + allPaths);
        roomNavigationStat = possiblePaths * 1.0f / allPaths;
        //Debug.Log("room nav stat: " + roomNavigationStat);
        if (allPaths == 0)
        {
            yield return 1f;
        }
        else
        {
            yield return roomNavigationStat;
        }
    }

    public float TotalPointValue()
    {
        Item[] items = Item.FindObjectsOfType<Item>();

        List<ItemInfo> uniqueItems = new List<ItemInfo>();
        List<int> values = new List<int>();

        foreach (Item item in items)
        {
            if (!uniqueItems.Contains(item.itemInfo))
            {
                uniqueItems.Add(item.itemInfo);
                values.Add(1);
            }
            else
            {
                values[uniqueItems.IndexOf(item.itemInfo)] += 1;
            }
        }

        float totalPointValue = 0;

        for (int i = 0; i < uniqueItems.Count; i++)
        {
            if (values[i] > 4)
            {
                totalPointValue += uniqueItems[i].cost * 4;
                totalPointValue += uniqueItems[i].cost * 0.5f * (values[i] - 4);
            }
            else
            {
                totalPointValue += uniqueItems[i].cost * values[i];
            }
        }

        return totalPointValue;
    }

    IEnumerator Timer(Action<bool> assigner, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        assigner(true);
    }
}

public class CoroutineWithData
{
    public Coroutine coroutine { get; private set; }
    public object result;
    private IEnumerator target;
    public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
    {
        this.target = target;
        this.coroutine = owner.StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        while (target.MoveNext())
        {
            result = target.Current;
            yield return result;
        }
    }

    /*public void OnDrawGizmos()
    {
        if (currentPath == null)
        {
            return;
        }

        Gizmos.color = new Color(0.7F, 0.5F, 0.1F, 0.5F);

        if (currentPath != null)
        {
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine((Vector3)currentPath[i], (Vector3)currentPath[i + 1]);
            }
        }

        Gizmos.color = new Color(0, 1F, 0, 1F);

        if (currentPath != null)
        {
            for (int i = 0; i < currentPath.Count - 1; i++)
            {
                Gizmos.DrawLine(currentPath[i], currentPath[i + 1]);
            }
        }
    }*/
}
