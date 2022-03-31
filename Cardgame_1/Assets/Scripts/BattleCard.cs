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
            
        }
        
        //當在場上時點擊，發起攻擊請求
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
