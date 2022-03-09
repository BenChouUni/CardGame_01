using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public Transform deckPanel;
    public Transform libraryPanel;

    public GameObject deckPrefab;
    public GameObject LibraryPrefab;

    public GameObject DataManager;

    private PlayerData PlayerData;
    private CardStore CardStore;

    private Dictionary<int, GameObject> libraryDic = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> deckDic = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        

        PlayerData = DataManager.GetComponent<PlayerData>();
        CardStore = DataManager.GetComponent<CardStore>();
        PlayerData.DataLoad();
        UpdateLibrary();
        UpdateDeck();
    }

   //創建卡牌函數
    public void CreateCard(int _id, CardState _cardState)
    {
        Transform targetPanel = null;
        GameObject targetPrefab = null; //要初始化才能執行，不知道為什麼

        var refData = PlayerData.PlayerCards;
        Dictionary<int, GameObject> targetDic = libraryDic;

        if (_cardState == CardState.Library)
        {
            targetPanel = libraryPanel;
            targetPrefab = LibraryPrefab;
        }
        else if (_cardState == CardState.Deck)
        {
            targetPanel = deckPanel;
            targetPrefab = deckPrefab;

            refData = PlayerData.PlayerDeck;
            targetDic = deckDic;
        }

        GameObject newCard = GameObject.Instantiate(targetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        newCard.transform.SetParent(targetPanel, false);

        newCard.GetComponent<CardCounter>().SetCounter(refData[_id]);
        newCard.GetComponent<CardDisplay>().card = CardStore.CardList[_id];

        targetDic.Add(_id, newCard);
    }

    public void UpdateLibrary()
    {
        
        for (int i = 0; i < PlayerData.PlayerCards.Length; i++)
        {
            if (PlayerData.PlayerCards[i]>0)
            {
                CreateCard(i, CardState.Library);
            }


        }
        
    }

    public void UpdateDeck()
    {
        for (int i = 0; i < PlayerData.PlayerDeck.Length; i++)
        {
            if (PlayerData.PlayerDeck[i] > 0)
            {
                CreateCard(i, CardState.Deck);
            }


        }
    }

    public void UpdateCard(CardState _state, int _id)//點擊卡片時更新，點擊牌組之牌則將其從牌組移出到牌庫
    {
        if (_state == CardState.Deck)
        {
            PlayerData.PlayerDeck[_id] -= 1;
            PlayerData.PlayerCards[_id] += 1;
            //卡組中此牌-1
            //deckDic[_id].GetComponent<CardCounter>().SetCounter(-1);
            if (!deckDic[_id].GetComponent<CardCounter>().SetCounter(-1)) //卡組中沒牌時移除
            {
                deckDic.Remove(_id);
            }

            

            if (libraryDic.ContainsKey(_id))//查找此id中是否有對應的物件
            {
                libraryDic[_id].GetComponent<CardCounter>().SetCounter(1);//卡庫中此牌+1
            }
            else//如果沒有要創建一個
            {
                CreateCard(_id, CardState.Library);
            }
            
        }
        else if (_state == CardState.Library)
        {
            PlayerData.PlayerDeck[_id] += 1;
            PlayerData.PlayerCards[_id] -= 1;

            //卡庫中此牌-1
            if (!libraryDic[_id].GetComponent<CardCounter>().SetCounter(-1))
            {
                libraryDic.Remove(_id); 
            }

            if (deckDic.ContainsKey(_id))//查找此id中是否有對應的物件
            {
                deckDic[_id].GetComponent<CardCounter>().SetCounter(1);//卡組中此牌+1
            }
            else//如果沒有要創建一個
            {
                CreateCard(_id, CardState.Deck);
            }
            
        }

    }

    
}
