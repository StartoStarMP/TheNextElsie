using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoneyCounter : MonoBehaviour
{
    public Text playerMoney;
    public Text moneyAddedText;
    public Animator moneyAddedAnim;

    // Start is called before the first frame update
    void Start()
    {
        SetPlayerMoney();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPlayerMoney()
    {
        playerMoney.text = GameManager.current.playerMoney.ToString();
    }

    public void ChangeMoney(int moneyToAdd)
    {
        moneyAddedText.text = moneyToAdd.ToString();
        moneyAddedAnim.SetTrigger("add");
        StartCoroutine(AdjustMoney(moneyToAdd));
    }

    public IEnumerator AdjustMoney(int moneyToAdd)
    {
        int currentMoney = GameManager.current.playerMoney - moneyToAdd;
        int finalMoney = GameManager.current.playerMoney;

        if (currentMoney > finalMoney)
        {
            while (currentMoney > finalMoney)
            {
                currentMoney -= 1;
                playerMoney.text = currentMoney.ToString();
                yield return new WaitForSeconds(0.02f);
            }
        }
        else
        {
            while (currentMoney < finalMoney)
            {
                currentMoney += 1;
                playerMoney.text = currentMoney.ToString();
                yield return new WaitForSeconds(0.02f);
            }
        }
    }
}
