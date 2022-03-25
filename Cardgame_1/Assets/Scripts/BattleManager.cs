using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;//事件

public enum GamePhase
{
    gameStart, playerDraw, playerAction, enemyDraw, enemyAction
}
public class BattleManager : MonoSingleton<BattleManager>//單例母版
{
    public static BattleManager Instance;

    public PlayerData playerData;
    public PlayerData enemyData;//資料

    public List<Card> playerDeckList = new List<Card>();
    public List<Card> enemyDeckList = new List<Card>();//卡組

    public GameObject cardPrefab;//卡牌

    public Transform playerHand;
    public Transform enemyHand;//手牌

    public GameObject[] playerBlock;
    public GameObject[] enemyBlock;//格子

    public GameObject playerIcon;
    public GameObject enemyIcon;//頭像

    public GamePhase GamePhase = GamePhase.gameStart;

    public UnityEvent phaseChangeEvent = new UnityEvent();

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
        GameStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //遊戲流程
    //開始遊戲：載入數據 洗牌 初始手牌
    //遊戲階段
    //回合結束
    public void GameStart()
    {
        //讀取數據
        ReadDeck();
        //洗牌
        ShuffleDeck(0);
        ShuffleDeck(1);
        if (playerDeckList[0]!=null)
        {
            Debug.Log("玩家卡組最上面一張牌是" + playerDeckList[0].cardName);
        }
        else
        {
            Debug.Log("玩家卡組最上面沒牌");
        }
            
        //雙方抽牌5
        DrawCard(0, 5);
        DrawCard(1, 5);

        GamePhase = GamePhase.playerDraw;
        phaseChangeEvent.Invoke();
    }

    public void ReadDeck()//加載玩家卡組
    {
        for (int i = 0; i < playerData.PlayerDeck.Length; i++)
        {
            if (playerData.PlayerDeck[i]!=0)
            {
                int count = playerData.PlayerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    //playerDeckList.Add(playerData.CardStore.CardList[i]);這樣寫會影響到卡庫
                    playerDeckList.Add(playerData.CardStore.CopyCard(i));

                }
            }
        }
        //加載敵人卡組
        for (int i = 0; i < enemyData.PlayerDeck.Length; i++)
        {
            if (enemyData.PlayerDeck[i] != 0)
            {
                int count = enemyData.PlayerDeck[i];
                for (int j = 0; j < count; j++)
                {
                    
                    enemyDeckList.Add(enemyData.CardStore.CopyCard(i));

                }
            }
        }

        //test
        Debug.Log("玩家卡組有"+ playerDeckList.Count);
        Debug.Log("敵人卡組有" + enemyDeckList.Count);
    }


    public void ShuffleDeck(int _player)//0玩家 1敵人
    {
        List<Card> shuffleDeck = new List<Card>();
        if (_player == 0)
        {
            shuffleDeck = playerDeckList;
        }
        else if (_player == 1)
        {
            shuffleDeck = enemyDeckList;
        }
        //洗牌算法一種，隨機選出一張牌跟第i張互換，如此往復
        for (int i = 0; i < shuffleDeck.Count; i++)
        {
            int rad = Random.Range(0, shuffleDeck.Count);
            Card a = shuffleDeck[i];
            shuffleDeck[i] = shuffleDeck[rad];
            shuffleDeck[rad] = a; //三行用來互換
        }

        if (_player == 0)
        {
            playerDeckList = shuffleDeck;
        }
        else if (_player == 1)
        {
            enemyDeckList = shuffleDeck;
        }
        Debug.Log("洗牌完成");
    }

    public void OnplayerDraw()
    {
        if (GamePhase == GamePhase.playerDraw)
        {
            DrawCard(0, 1);
            GamePhase = GamePhase.playerAction;
            phaseChangeEvent.Invoke();
        }
        
        
    }
    public void OnenemyDraw()
    {
        if (GamePhase == GamePhase.enemyDraw)
        {
            DrawCard(1, 1);
            GamePhase = GamePhase.enemyAction;
            phaseChangeEvent.Invoke();
        }
        
    }

    public void DrawCard(int _player, int _count)
    {
        List<Card> drawDeck = new List<Card>();
        Transform hand = null;

        if (_player ==0)
        {
            drawDeck = playerDeckList;
            hand = playerHand;
        }
        else if (_player == 1)
        {
            drawDeck = enemyDeckList;
            hand = enemyHand;
        }

        for (int i = 0; i < _count; i++)
        {
            GameObject card = Instantiate(cardPrefab, hand);
            card.GetComponent<CardDisplay>().card = drawDeck[0];//抽卡組最上面一張牌
            drawDeck.RemoveAt(0);//並將此牌移除
        }

        //Debug.Log("抽完後接下一張："+ drawDeck[0].cardName);
    }

    public void OnClickTurnEnd()//規範的寫法，方便後續更新調整
    {
        TurnEnd();
    }

    public void TurnEnd()
    {
        if (GamePhase == GamePhase.playerAction)
        {
            GamePhase = GamePhase.enemyDraw;
            phaseChangeEvent.Invoke();
        }
        else if (GamePhase == GamePhase.enemyAction)
        {
            GamePhase = GamePhase.playerDraw;
            phaseChangeEvent.Invoke();
        }
    }
    
}
