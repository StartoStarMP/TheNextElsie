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
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddMoney(70);
        }
    }

    public void SetPlayerMoney()
    {
        playerMoney.text = GameManager.current.playerMoney.ToString();
    }

    public void AddMoney(int moneyToAdd)
    {
        moneyAddedText.text = "+ " + moneyToAdd.ToString();
        moneyAddedAnim.SetTrigger("add");
        StartCoroutine(AdjustXPSlider(moneyToAdd));
    }

    public IEnumerator AdjustXPSlider(int moneyToAdd)
    {
        int currentMoney = GameManager.current.playerMoney;
        int finalMoney = GameManager.current.playerMoney + moneyToAdd;

        while (currentMoney < finalMoney)
        {
            currentMoney += 1;
            playerMoney.text = currentMoney.ToString();
            yield return new WaitForSeconds(0.02f);
        }

        GameManager.current.playerMoney = finalMoney;
    }
}
