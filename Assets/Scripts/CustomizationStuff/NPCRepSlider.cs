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

    public void AddRep(int pointsToAdd)
    {
        addedPointsText.text = "+ " + pointsToAdd.ToString();
        addedPointsAnim.SetTrigger("add");
        StartCoroutine(AdjustSlider(pointsToAdd));

        //PLAY SOUND EFFECT ADD REP
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
            if (repLevel.text == (npcCurrentRep / 100).ToString())
            {

            }
            else
            {
                repLevel.text = (npcCurrentRep / 100).ToString();
                //PLAY SOUND EFFECT REP LEVEL UP
            }
            yield return new WaitForSeconds(0.02f);
        }

        GameManager.current.npcReps[(int)npcVendor] = finalRep;
    }
}
