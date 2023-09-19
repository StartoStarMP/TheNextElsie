using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelSlider : MonoBehaviour
{
    public Text playerLevel;
    public Slider playerCurrentXP;
    public Text xpAddedText;
    public Animator xpAddedAnim;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerLevelXP();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddXP(70);
        }
    }

    public void SetPlayerLevelXP()
    {
        playerLevel.text = (GameManager.current.playerXP / 100).ToString();
        playerCurrentXP.value = GameManager.current.playerXP % 100;
    }

    public void AddXP(int xpToAdd)
    {
        xpAddedText.text = "+ " + xpToAdd.ToString();
        xpAddedAnim.SetTrigger("add");
        StartCoroutine(AdjustXPSlider(xpToAdd));
    }

    public IEnumerator AdjustXPSlider(int xpToAdd)
    {
        int finalXP = (int)playerCurrentXP.value + xpToAdd;

        while (playerCurrentXP.value < finalXP)
        {
            playerCurrentXP.value += 1;
            if (playerCurrentXP.value == 100)
            {
                playerLevel.text = (int.Parse(playerLevel.text) + 1).ToString();
                playerCurrentXP.value = 0;
                finalXP -= 100;
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
