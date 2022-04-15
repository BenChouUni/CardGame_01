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

    public Transform Canvas;

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

    public UnityEvent phaseChangeEvent = new UnityEvent();//切換狀態事件
    

    public int[] SummonCountMax = new int[2]; //0 player 1 enemy
    public int[] SummonCount = new int[2];

    //招喚輔助變量
    private GameObject waitingMonster;
    private int waitingPlayer;

    public GameObject ArrowPrefab;//箭頭
    private GameObject arrow;

    //攻擊輔助變涼
    private GameObject attackingMonster;
    private int attackingPlayer;
    public GameObject attckArrow;

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
        if (Input.GetMouseButton(1))//點擊右鍵
        {
            DestroyArrow();
            waitingMonster = null;
            TurnOffBlock();
        }
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

        NextPhase();

        for (int i = 0; i < 2; i++)//不能存指標
        {
            SummonCount[i] = SummonCountMax[i];
        }
        
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

    public void OnplayerDraw()//外面點擊按鈕呼叫
    {
        if (GamePhase == GamePhase.playerDraw)
        {
            DrawCard(0, 1);
            NextPhase();
        }
        
        
    }
    public void OnenemyDraw()//外面點擊按鈕呼叫
    {
        if (GamePhase == GamePhase.enemyDraw)
        {
            DrawCard(1, 1);
            NextPhase();
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
            card.GetComponent<BattleCard>().playerID = _player;
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
        if (GamePhase == GamePhase.playerAction || GamePhase == GamePhase.enemyAction)
        {
            for (int i = 0; i < 2; i++)//重置招喚次數
            {
                SummonCount[i] = SummonCountMax[i];
            }

            if (GamePhase==GamePhase.playerAction)
            {
                ResetAttack(0);
            }
            else
            {
                ResetAttack(1);
            }
            NextPhase();
            
        }
        
    }

    public void NextPhase()//切換到下一個階段
    {
        if ((int)GamePhase == System.Enum.GetNames(GamePhase.GetType()).Length-1)//最後一個
        {
            GamePhase = GamePhase.playerDraw;
        }
        else
        {
            GamePhase += 1;
        }
        
        phaseChangeEvent.Invoke();
    }

    public void SummonRequest(int _player, GameObject _monsterCard)
    {
        GameObject[] blocks;
        bool hasEmptyBlock = false;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = playerBlock;
        }
        else if(_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = enemyBlock;
        }
        else
        {
            return;
        }

        if (SummonCount[_player]>0)
        {
            foreach (var block in blocks)
            {
                if (block.GetComponent<Block>().card == null)//判斷格子是空的
                {
                    //可以做等待招喚的格子提示
                    block.GetComponent<Block>().summonBlock.SetActive(true);
                    hasEmptyBlock = true;
                    
                }
            }
        }
        else
        {
            Debug.Log("沒有招喚次數了");
        }

        if (hasEmptyBlock)
        {
            waitingMonster = _monsterCard;
            waitingPlayer = _player;
            CreatArrow(_monsterCard.GetComponent<RectTransform>(),ArrowPrefab);
        }
        
    }

    public void SummonConfirm(Transform _block)
    {
        Summon(waitingPlayer,waitingMonster,_block);
        TurnOffBlock();
        DestroyArrow();

    }

    public void Summon(int _player, GameObject _monsterCard, Transform _block)
    {
        _monsterCard.transform.SetParent(_block);
        _monsterCard.transform.localPosition = Vector3.zero;
        _monsterCard.GetComponent<BattleCard>().state = BattleCardState.inBlock;
        _block.GetComponent<Block>().card = _monsterCard;

        SummonCount[_player]-=1;
        
        MonsterCard mc = _monsterCard.GetComponent<CardDisplay>().card as MonsterCard;//要這樣寫才能成功獲取
        _monsterCard.GetComponent<BattleCard>().Attackcount = mc.attackTime;
        //招喚時要重置一次攻擊次數
        _monsterCard.GetComponent<BattleCard>().ResetAttack();

    }

    public void TurnOffBlock()//關閉格子發亮
    {
        int playerBlockLength = playerBlock.Length;
        int enemyBlockLength = enemyBlock.Length;
        GameObject[] blocks = new GameObject[playerBlockLength + enemyBlockLength];
        
        for (int i = 0; i < playerBlockLength; i++)
        {
            blocks[i] = playerBlock[i];
            
        }
        for (int i = 0; i < enemyBlockLength; i++)
        {
            blocks[playerBlockLength + i] = enemyBlock[i];
            
        }
    
        foreach (var block in blocks)
        {
            block.GetComponent<Block>().summonBlock.SetActive(false);
            block.GetComponent<Block>().AttackBlock.SetActive(false);
        }
    }

    public void ResetAttack(int _GamePhase)//0是玩家1是敵人
    {
        GameObject[] blocks = null;
        
        if (_GamePhase == 0)
        {
            blocks = playerBlock;
            
        }
        else if(_GamePhase==1)
        {
            blocks = enemyBlock;
            
        }
        foreach (var block in blocks)
        {
            if (block.GetComponent<Block>().card != null)
            {
                block.GetComponent<Block>().card.GetComponent<BattleCard>().ResetAttack();

            }
            
        }
       
    }

    

    public void AttackRequest(int _player, GameObject _monster)
    {
        GameObject[] blocks;
        bool hasMonsterBlock = false;
        if (_player == 0 && GamePhase == GamePhase.playerAction)
        {
            blocks = enemyBlock;//攻擊敵方所以查看敵方格子，我方場上確定有卡牌才能呼叫此函數，所以不用再判斷
        }
        else if (_player == 1 && GamePhase == GamePhase.enemyAction)
        {
            blocks = playerBlock;
        }
        else
        {

            return;
        }

        foreach (var block in blocks)
        {
            if (block.GetComponent<Block>().card != null)//判斷格子上有怪物
            {
                //等待攻擊顯示
                block.GetComponent<Block>().AttackBlock.SetActive(true);//能攻擊的格子發亮
                //可攻擊
                block.GetComponent<Block>().card.GetComponent<AttackTarget>().attackable = true;
                hasMonsterBlock = true;
                

            }
            else
            {
                Debug.Log("沒有攻擊目標");
            }
        }

        if (hasMonsterBlock)
        {

            attackingMonster = _monster;
            CreatArrow(_monster.GetComponent<RectTransform>(), attckArrow);
        }
    }
    /// <summary>
    /// 發動攻擊怪獸
    /// </summary>
    /// <param name="_target">攻擊目標</param>
    public void AttackConfirm(GameObject _target)
    {
        Attack(attackingMonster,_target);
        DestroyArrow();
        TurnOffBlock();
        //關閉所有格子可攻擊狀態
        GameObject[] blocks = null;
        if (GamePhase == GamePhase.playerAction)
        {
            blocks = enemyBlock;
        }
        else if(GamePhase==GamePhase.enemyAction)
        { 
            blocks = playerBlock;
        }

        foreach (var block in blocks)//每次攻擊完目標不再可以被攻擊，直到下一次被申請
        {
            if (block.GetComponent<Block>().card != null)
            {
                block.GetComponent<Block>().card.GetComponent<AttackTarget>().attackable = false;
            }
        }

    }

    public void Attack(GameObject _attaker, GameObject _target)
    {
        MonsterCard monster = _attaker.GetComponent<CardDisplay>().card as MonsterCard;

        _target.GetComponent<AttackTarget>().ApplyDamage(monster.attack);
        _attaker.GetComponent<BattleCard>().CostAttaclCount();//減少攻擊次數

    }
    public void CreatArrow(RectTransform _startPoint, GameObject _prefab)
    {
        arrow = GameObject.Instantiate(_prefab, _startPoint);
        arrow.transform.SetParent(Canvas);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(_startPoint.position.x, _startPoint.position.y));
    }

    public void DestroyArrow()
    {
        Destroy(arrow);
        
    }


}
