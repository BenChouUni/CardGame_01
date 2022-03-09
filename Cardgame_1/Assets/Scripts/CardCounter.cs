using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardCounter : MonoBehaviour
{
    public Text Counter;
    private int counter = 0;

    public bool SetCounter(int _value)
    {
        counter += _value;
        OnCounterChange();
        if (counter == 0)
        {
            Destroy(gameObject);
            return false;
        }
        return true;
    }

    private void OnCounterChange()
    {
        Counter.text = counter.ToString();
        
    }
}
