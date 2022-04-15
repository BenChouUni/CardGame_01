using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour,IPointerDownHandler
{
    public GameObject card;
    public GameObject summonBlock;
    public GameObject AttackBlock;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (summonBlock.activeInHierarchy)
        {
            BattleManager.Instance.SummonConfirm(transform);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        summonBlock.SetActive(false);
        AttackBlock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
