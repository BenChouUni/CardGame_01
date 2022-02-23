using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPackage : MonoBehaviour
{
    public int drawCardNumber = 5;

    public GameObject Canvas;
    public GameObject cardPrefab;


    CardStore cardStore;

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
        for (int i = 0; i < drawCardNumber; i++)
        {
            GameObject newCard = GameObject.Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newCard.transform.SetParent(Canvas.transform, false);

            newCard.GetComponent<CardDisplay>().card = cardStore.RandomCard();
           // string cardName = newCard.transform.GetChild(1).gameObject.GetComponent<Text>().text;
            //Debug.Log("抽到" + cardName);//嘗試檢查抽到卡牌
        }

       
    }
}
