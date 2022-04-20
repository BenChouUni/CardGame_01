using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStore : MonoBehaviour
{
    public TextAsset cardData;
    public List<Card> CardList = new List<Card>();

    // Start is called before the first frame update
    void Start()
    {
        //LoadCardData();
        //TestLoad();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCardData()
    {
        string[] dataRow = cardData.text.Split('\n');

        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');
            if (rowArray[0] == "#")//跳過開頭是＃
            {
                continue; 
            }
            else if (rowArray[0] == "monster")//新建怪物卡
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                int atk = int.Parse(rowArray[3]);
                int health = int.Parse(rowArray[4]);
                MonsterCard monsterCard = new MonsterCard(id, name, atk, health);

                CardList.Add(monsterCard);

                //Debug.Log("讀取怪獸卡："+ monsterCard.cardName);

            }
            else if (rowArray[0] == "spell")//新建法術卡
            {
                int id = int.Parse(rowArray[1]);
                string name = rowArray[2];
                string effect = rowArray[3];

                SpellCard spellCard = new SpellCard(id, name, effect);
                CardList.Add(spellCard);

                //Debug.Log("讀取法術卡：" + spellCard.cardName);
            }
        }

    }

    public void TestLoad() //測試用
    {
        foreach (var item in CardList)
        {
            Debug.Log("卡牌" + item.id.ToString()+item.cardName);
        }
        
    }

    public Card RandomCard()
    { 
        Card card = CardList[Random.Range(0, CardList.Count)];
        return card;
    }

    public Card CopyCard(int _id)//避免外面調用卡操作時影響回去卡庫
    {
        Card copyCard = CardList[_id];
        if (copyCard is MonsterCard)
        {
            var monstercard = CardList[_id] as MonsterCard;
            copyCard = new MonsterCard(_id, monstercard.cardName, monstercard.attack, monstercard.healthPointMax);
        }
        else if (copyCard is SpellCard )
        {
            var spellcard = CardList[_id] as SpellCard;
            copyCard = new SpellCard(_id, spellcard.cardName, spellcard.effect);
        }

        return copyCard;
    }
}
