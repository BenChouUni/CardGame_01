using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CardState
{
    Library, Deck
}
public class ClickCard : MonoBehaviour, IPointerDownHandler
{
    private DeckManager DeckManager;
    private PlayerData PlayerData;


    public CardState State;
    // Start is called before the first frame update
    void Start()
    {
        DeckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
        PlayerData = GameObject.Find("DataManager").GetComponent<PlayerData>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventdata)
    {
        int id = this.GetComponent<CardDisplay>().card.id;
        State = CardState.Deck;
        Debug.Log(this.tag);
        if (this.tag == "DeckCard")
        {
            State = CardState.Deck;
            Debug.Log("Deck");
        }
        else if (this.tag == "LibraryCard")
        {
            State = CardState.Library;
            Debug.Log("Library");
        }
        DeckManager.UpdateCard(State, id);
      

    }
}
