using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPackage : MonoBehaviour
{
    public int drawCardNumber = 5;
    public int OpenCoin = 3;//開一次卡包所需要金額

    public GameObject CardPool;
    public GameObject cardPrefab;


    CardStore cardStore;
    List<GameObject> cards = new List<GameObject>();

    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        cardStore = GetComponent<CardStore>();
        OpenCoin = 3;
        drawCardNumber = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickOpen()
    {
        if (playerData.playerCoin < OpenCoin)//金幣扣款
        {
            return;
        }
        else
        {
            playerData.playerCoin -= OpenCoin;
        }

        ClearPool();

        for (int i = 0; i < drawCardNumber; i++)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(CardPool.transform, false);

            newCard.GetComponent<CardDisplay>().card = cardStore.RandomCard();

            cards.Add(newCard);
           

        }
        SaveCardData();//找出已經添加進cards中的卡，給她紀錄加一
        playerData.SavePlayerData();
       
    }

    public void ClearPool()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }

    public void SaveCardData()
    {
        foreach(var card in cards)
        {
            int id = card.GetComponent<CardDisplay>().card.id;
            playerData.PlayerCards[id] += 1; //使特定id的卡紀錄多一張
        }
    }
}
