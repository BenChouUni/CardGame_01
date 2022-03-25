using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseDisplay : MonoBehaviour
{
    
    public Text phaseText;
    // Start is called before the first frame update
    void Start()
    {
        BattleManager.Instance.phaseChangeEvent.AddListener(UpdateText);//收聽事件廣播，後面要使用一函數
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateText();
    }

    void UpdateText()
    {
        phaseText.text = BattleManager.Instance.GamePhase.ToString();
    }
}
