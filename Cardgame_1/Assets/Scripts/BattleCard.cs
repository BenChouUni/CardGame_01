using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum BattleCardState
{
    inHand,inBlock
}

public class BattleCard : MonoBehaviour, IPointerDownHandler
{
    public int playerID;
    public BattleCardState state = BattleCardState.inHand;

    public int Attackcount;
    private int attackcount;

    public void OnPointerDown(PointerEventData eventData)
    {
        
        //當手牌點擊時，發動招喚請求
        if (GetComponent<CardDisplay>().card is MonsterCard)//必須是怪獸卡
        {
            if (state == BattleCardState.inHand)//必須在手牌中
            {
                BattleManager.Instance.SummonRequest(playerID, gameObject);
                Debug.Log("點擊" + GetComponent<CardDisplay>().card.cardName.ToString());
            }
            else if (state == BattleCardState.inBlock && attackcount > 0)//在場上時被點擊發動攻擊請求且要有攻擊次數
            {
                BattleManager.Instance.AttackRequest(playerID,gameObject);
                Debug.Log("點擊" + GetComponent<CardDisplay>().card.cardName.ToString() + "於場上");
            }
            
        }
        
        //當在場上時點擊，發起攻擊請求
    }

    public void ResetAttack()
    {
        attackcount = Attackcount;
    }

    public void CostAttaclCount()
    {
        attackcount--;
    }
    // Start is called before the first frame update
    void Start()
    {

    }
// Update is called once per frame
    void Update()
    {
        
    }
}
