using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPackage : MonoBehaviour
{
    public int drawCardNumber = 5;

    public GameObject CardPool;
    public GameObject cardPrefab;


    CardStore cardStore;
    List<GameObject> cards = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cardStore = GetComponent<CardStore>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnclickOpen()
    {
        ClearPool();
        for (int i = 0; i < drawCardNumber; i++)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(CardPool.transform, false);

            newCard.GetComponent<CardDisplay>().card = cardStore.RandomCard();

            cards.Add(newCard);
           // string cardName = newCard.transform.GetChild(1).gameObject.GetComponent<Text>().text;
            //Debug.Log("抽到" + cardName);//嘗試檢查抽到卡牌
        }

       
    }

    public void ClearPool()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }

    }
}
