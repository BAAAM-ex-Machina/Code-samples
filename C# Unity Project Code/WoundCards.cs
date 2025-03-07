using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoundEnum;

public class WoundCards : MonoBehaviour
{
    [SerializeField] private GameObject woundCard;
    [SerializeField] private GameObject woundContainer;
    [SerializeField] private GameObject panel;
    private List<GameObject> woundCards;
    private (float, float) offset;
    private GameObject heldCard;
    private bool toggle = false;
    private bool first = true;

    public static event Action<int,int> WoundMoveEvent;

    private void Start()
    {
        woundCards = new List<GameObject> ();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Click())
            {
                toggle = true;
            }
        }
        if (toggle)
        {
            if (Input.GetMouseButtonUp(0))
            {
                toggle = false;
            }
            else
            {
                Hold();
            }
        }
    }

    
    public void createWound(ScriptableWound hit)
    {
        bool refl = true;
        if (hit.reflex == Reflex.none)
        {
            refl = false;
        }
        bool injury = true;
        if (hit.injury == PersistentInjury.none)
        {
            injury = false;
        }

        GameObject card = Instantiate(woundCard, woundContainer.transform);
        card.GetComponent<WoundCard>().Setup(hit.name, hit.reflex.ToString(), hit.desc, hit.critDesc, hit.injury.ToString(), hit.injuryDescription, refl, hit.crit, injury, hit.trap, hit.impervious);
        woundCards.Add(card);
    }

    public void DisableUI()
    {
        woundContainer.SetActive(false);
        panel.SetActive(false);
    }

    public void EnableUI()
    {
        woundContainer.SetActive(true); 
        panel.SetActive(true);
    }

    public void ToggleUI()
    {
        if (woundContainer.activeSelf)
        {
            woundContainer.SetActive(false); 
            panel.SetActive(false);
        }
        else
        {
            woundContainer.SetActive(true);
            panel.SetActive(true);
        }
    }

    public bool State()
    {
        return woundContainer.activeSelf;
    }

    public bool Click()
    {
        //If mouse is ontop of a wound card, set wound card to move and the offset of mouse on the card
        Vector3 mousePos = Input.mousePosition;
        heldCard = null;
        foreach (var card in woundCards)
        {
            var cardRect = card.GetComponent<RectTransform>();
            if (cardRect.position.x + cardRect.sizeDelta.x/2 >= mousePos.x && mousePos.x >= cardRect.position.x - cardRect.sizeDelta.x / 2 && cardRect.position.y + cardRect.sizeDelta.y/2 >= mousePos.y && mousePos.y >= cardRect.position.y - cardRect.sizeDelta.x *3/4)
            {
                heldCard = card;
                offset = (mousePos.x - cardRect.position.x, mousePos.y - cardRect.position.y);
                return true;
            }
        }
        return false;
    }

    public void Hold()
    {

        //If wound card is over halfway into another slot, switch children order, list here, and list in monster
        Vector3 mousePos = Input.mousePosition;
        var cardRect = heldCard.GetComponent<RectTransform>();
        if (!(cardRect.position.x  + cardRect.sizeDelta.x / 2 >= mousePos.x - offset.Item2 && mousePos.x - offset.Item1 >= cardRect.position.x - cardRect.sizeDelta.x / 2 && cardRect.position.y + cardRect.sizeDelta.y / 2 >= mousePos.y - offset.Item2 && mousePos.y - offset.Item2 >= cardRect.position.y - cardRect.sizeDelta.x * 3 / 4))
        {
            int index = 0;
            int newIndex = -1;
            int i = 0;
            foreach (var card in woundCards)
            {
                if (heldCard == card)
                {
                    index = i;
                }
                cardRect = card.GetComponent<RectTransform>();
                if (cardRect.position.x + cardRect.sizeDelta.x / 2 >= mousePos.x - offset.Item2 && mousePos.x - offset.Item1 >= cardRect.position.x - cardRect.sizeDelta.x / 2 && cardRect.position.y + cardRect.sizeDelta.y / 2 >= mousePos.y - offset.Item2 && mousePos.y - offset.Item2 >= cardRect.position.y - cardRect.sizeDelta.x * 3 / 4)
                {
                    newIndex = i;
                }
                i++;
            }
            if (newIndex != -1)
            {
                MoveList(index, newIndex);
            }
        }

    }

    public void MoveList(int i, int j)
    {
        if (i != j)
        {
            WoundMoveEvent( i, j);
            woundCards.RemoveAt(i);
            woundCards.Insert(j, heldCard);
            heldCard.GetComponent<Transform>().SetSiblingIndex(j);
        }
        //1,2,3,4,5,6
        //3 is at index 2
        //5 needs to move to 3
        //Take value of 5
        //Remove 5
        //.Insert(2, 5value)
    }

    public int WoundAttempt()
    {
        return 0;
    }
}
