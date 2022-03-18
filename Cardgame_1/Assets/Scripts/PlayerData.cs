using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //可用來存讀取

public class PlayerData : MonoBehaviour
{
    public CardStore CardStore;

    public int playerCoin;
    public int[] PlayerCards; //PlayerCards[i]代表索引值i有多少張牌
    public int[] PlayerDeck;

    public TextAsset playerData;

    // Start is called before the first frame update
    void Awake()
    {
        DataLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DataLoad()
    {
        CardStore.LoadCardData();//先將卡牌資料庫存到CardList才能調用LoadPLayerData
        CardStore.TestLoad();
        LoadPlayerDate();
        //TestPlayerLoad();
    }

    public void LoadPlayerDate()
    {

        PlayerCards = new int[CardStore.CardList.Count];
        PlayerDeck = new int[CardStore.CardList.Count];

        string[] dataRow = playerData.text.Split('\n');//用換行來確定第幾列

        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');//用逗點確定一行中第幾個
            if (rowArray[0] == "#")//跳過開頭是＃
            {
                continue;
            }
            else if (rowArray[0] == "coin ")//玩家金幣
            {
                playerCoin = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")//卡牌
            {
                int ID = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);

                PlayerCards[ID] = num;
            }
            else if (rowArray[0] == "deck")//載入卡組
            {
                int ID = int.Parse(rowArray[1]);
                int num = int.Parse(rowArray[2]);

                PlayerDeck[ID] = num;
            }

        }
    }

    public void SavePlayerData()
    {
        string path = Application.dataPath + "/Data/playerData.csv"; //路徑名稱，務必確認
        

        List<string> datas = new List<string>();
        datas.Add("coin" + playerCoin.ToString());
        datas.Add("#");

        //保存玩家卡牌
        for (int i = 0; i < PlayerCards.Length; i++)
        {
            if (PlayerCards[i]!=0)
            {
                datas.Add("card," + i.ToString() + "," + PlayerCards[i].ToString());
            }
            
        }

        //保存玩家卡組
        for (int i = 0; i < PlayerDeck.Length; i++)
        {
            if (PlayerDeck[i] != 0)
            {
                datas.Add("deck," + i.ToString() + "," + PlayerDeck[i].ToString());
            }

        }


        //保存到特定路徑
        File.WriteAllLines(path, datas);
    }

    public void TestPlayerLoad() //測試用
    {
        Debug.Log("CardList" + CardStore.CardList.Count.ToString());

        Debug.Log("玩家金幣：" + playerCoin.ToString());
        for (int i = 0; i < CardStore.CardList.Count; i++)
        {
            
            Debug.Log("卡牌ID"+ i.ToString() + "有" + PlayerCards[i].ToString() + "張");
        }

    }
}
