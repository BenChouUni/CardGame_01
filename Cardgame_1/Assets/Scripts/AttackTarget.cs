using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackTarget : MonoBehaviour,IPointerClickHandler
{
    public bool attackable;
    //攻擊目標可能有多種類，玩家/場上怪獸，而有沒有carddisplay作為判斷
    CardDisplay display = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (attackable)
        { 
            if (display == null)
            {
                //代表此目標是玩家
            }
            else
            {
                BattleManager.Instance.AttackConfirm(gameObject);
            }
            
        }
    }

    public void ApplyDamage(int _damage)
    {
        MonsterCard monster = GetComponent<CardDisplay>().card as MonsterCard;
        monster.healthPoint -= _damage;
        GetComponent<CardDisplay>().ShowCard();//刷新狀態
        if (monster.healthPoint<=0)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        display = GetComponent<CardDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
