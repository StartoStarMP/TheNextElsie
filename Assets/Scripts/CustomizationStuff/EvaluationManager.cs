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
    public GameObject evaluationUI;

    [Header("Results")]
    public List<float> calculatedPathLengths;
    public List<Vector3> currentPath;
    public float roomNavigationStat;
    public Slider evaluationSlider;
    public Text breakdownText;
    public Text rankText;
    public Animator bgAnim;
    public Animator sliderAnim;
    public ParticleSystem confetti;
    public List<string> breakdownList = new List<string>();
    public List<int> pointList = new List<int>();
    public Button toRecapButton;

    [Header("Recap")]
    public GameObject recapUI;
    public Text missionName;
    public Text scoreBreakdown;
    public Text rankLetter;
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

    public IEnumerator StartPointsBreakdown()
    {
        for (int i = 0; i < breakdownList.Count; i++)
        {
            breakdownText.text = breakdownList[i] + " + " + pointList[i];
            evaluationSlider.value += pointList[i];
            if (evaluationSlider.value < 250)
            {
                rankText.text = "F";
            }
            else if (evaluationSlider.value < 500)
            {
                rankText.text = "C";
            }
            else if (evaluationSlider.value < 750)
            {
                rankText.text = "B";
            }
            else if (evaluationSlider.value < 1000)
            {
                rankText.text = "A";
            }
            else if (evaluationSlider.value == 1000)
            {
                rankText.text = "S";
            }

            scoreBreakdown.text += breakdownList[i] + " + " + pointList[i] + "\n";
            sliderAnim.SetTrigger("bounce");
            yield return new WaitForSeconds(1f);
        }

        missionName.text = GameManager.current.currentMission.missionName;
        bgAnim.SetTrigger("focus");
        yield return new WaitForSeconds(0.5f);
        confetti.Play();
        sliderAnim.SetTrigger("finalBounce");
        if (evaluationSlider.value < 250)
        {
            breakdownText.text = "Awful...";
            rankLetter.text = "F";
        }
        else if (evaluationSlider.value < 500)
        {
            breakdownText.text = "Ok...";
            rankLetter.text = "C";
        }
        else if (evaluationSlider.value < 750)
        {
            breakdownText.text = "Great!";
            rankLetter.text = "B";
        }
        else if (evaluationSlider.value < 1000)
        {
            breakdownText.text = "Amazing!";
            rankLetter.text = "A";
        }
        else if (evaluationSlider.value == 1000)
        {
            breakdownText.text = "PERFECT!!!";
            rankLetter.text = "S";
        }

        toRecapButton.gameObject.SetActive(true);
    }

    public void StartRecapBreakdown()
    {
        TransitionCanvas.current.Animate("blackFadeIn");
        StartCoroutine(Timer(x => recapUI.SetActive(true), 0.5f));
        StartCoroutine(Timer(x => TransitionCanvas.current.Animate("blackFadeOut"), 0.5f));
    }

    public IEnumerator GatherPointsBreakdown()
    {
        evaluationUI.SetActive(true);

        //GET TOTAL POINT VALUE
        AddToBreakdown("Total Point Value", (int)(250 * TotalPointValue() / DesignManager.current.maxBudget * 1.2f));
        //ESSENTIAL REQUIREMENTS
        AddToBreakdown("Essentials", (int)(250 * Essentials()));
        //ROOM NAVIGATION
        CoroutineWithData cd = new CoroutineWithData(this, RoomNavigation());
        yield return cd.coroutine;
        AddToBreakdown("Room Navigation", (int)(250 * (float)cd.result));
        //REMAINING BUDGET
        AddToBreakdown("Remaining Budget", (int)(250 * DesignManager.current.currentBudget/DesignManager.current.maxBudget));
        //DECORATIONS
        AddToBreakdown("Decorations", (int)Decorations());

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
        SceneLoader.current.LoadScene("PlayerRoom");
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
        Item[] items = Item.FindObjectsOfType<Item>();
        int currentPoints = 0;
        int maxPoints = 0;

        /*foreach (Requirement requirement in GameManager.current.currentMission.affixes)
        {
            if (requirement.reqType == RequirementType.Item)
            {
                int count = 0;
                foreach (Item item in items)
                {
                    if (item.itemInfo == requirement.item)
                    {
                        count += 1;
                    }
                }
                currentPoints += count;
                maxPoints += requirement.count;
            }

            if (requirement.reqType == RequirementType.ItemType)
            {
                int count = 0;
                foreach (Item item in items)
                {
                    if (item.itemInfo.itemTypes.Contains(requirement.itemType))
                    {
                        count += 1;
                    }
                }
                currentPoints += count;
                maxPoints += requirement.count;
            }

            if (requirement.reqType == RequirementType.Color)
            {
                int count = 0;
                foreach (Item item in items)
                {
                    if (item.itemInfo.colors.Contains(requirement.color))
                    {
                        count += 1;
                    }
                }
                currentPoints += count;
                maxPoints += requirement.count;
            }
            

            //ADD EXISTING ITEMS

            //ADD ROOM TYPE ESSENTIAL ITEMS

            //ADD NEW REQUIREMENT TYPE THEME

            Debug.Log("Essentials: " + currentPoints);

            return (currentPoints/maxPoints);
        }
        */

        return 0;
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
            if (item.itemInfo.itemTypes.Contains(ItemType.Bed) || item.itemInfo.itemTypes.Contains(ItemType.Chair) || item.itemInfo.itemTypes.Contains(ItemType.Dresser)
                || item.itemInfo.itemTypes.Contains(ItemType.Table) || item.itemInfo.itemTypes.Contains(ItemType.Sofa) || item.itemInfo.itemTypes.Contains(ItemType.CoffeeTable)
                || item.itemInfo.itemTypes.Contains(ItemType.Stand) || item.itemInfo.itemTypes.Contains(ItemType.Bench))
            {
                Vector3 newPosition = new Vector3(item.transform.position.x, item.transform.position.y, 1);
                navigablePos.Add(newPosition);
            }
        }

        Debug.Log(navigablePos.Count);

        for (int i = 0; i < navigablePos.Count; i++)
        {
            if (i != navigablePos.Count - 1)
            {
                for (int j = i + 1; j < navigablePos.Count; j++)
                {
                    CoroutineWithData cd = new CoroutineWithData(this, StartPathing(navigablePos[i], navigablePos[j]));
                    yield return cd.coroutine;

                    Debug.Log("Path length: " + cd.result);
                    Debug.Log("Distance between beds: " + Vector3.Distance(navigablePos[i], navigablePos[j]));

                    Debug.Log(navigablePos[i]);
                    Debug.Log(navigablePos[j]);

                    if ((float)cd.result < Vector3.Distance(navigablePos[i], navigablePos[j]) + 2)
                    {
                        possiblePaths += 1;
                    }
                    allPaths += 1;
                }
            }
        }

        Debug.Log("possible paths: " + possiblePaths);
        Debug.Log("all paths: " + allPaths);
        roomNavigationStat = possiblePaths * 1.0f / allPaths;
        Debug.Log("room nav stat: " + roomNavigationStat);
        if (allPaths == 0)
        {
            yield return 0.01f;
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
