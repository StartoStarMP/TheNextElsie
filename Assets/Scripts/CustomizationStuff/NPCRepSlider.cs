using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCRepSlider : MonoBehaviour
{
    public NPCVendor npcVendor;
    public Image npcImage;
    public Image npcRepRing;
    public Text addedPointsText;
    public Animator addedPointsAnim;
    public Text repLevel;

    // Start is called before the first frame update
    void Start()
    {
        SetSlider();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddRep(50);
        }
    }

    public void AddRep(int pointsToAdd)
    {
        addedPointsText.text = "+ " + pointsToAdd.ToString();
        addedPointsAnim.SetTrigger("add");
        StartCoroutine(AdjustSlider(pointsToAdd));
    }

    public void SetSlider()
    {
        npcRepRing.fillAmount = GameManager.current.npcReps[(int)npcVendor] % 100 / 100f;
        repLevel.text = (GameManager.current.npcReps[(int)npcVendor] / 100).ToString();
    }

    public IEnumerator AdjustSlider(int pointsToAdd)
    {
        int npcCurrentRep = GameManager.current.npcReps[(int)npcVendor];
        int finalRep = npcCurrentRep + pointsToAdd;

        while (npcCurrentRep < finalRep)
        {
            npcCurrentRep += 1;
            npcRepRing.fillAmount = npcCurrentRep % 100 / 100f;
            repLevel.text = (npcCurrentRep / 100).ToString();
            yield return new WaitForSeconds(0.02f);
        }

        GameManager.current.npcReps[(int)npcVendor] = finalRep;
    }
}
