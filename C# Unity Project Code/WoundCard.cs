using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WoundCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title, reflex, body, critical, critTitle, critRef, injuryTitle, injuryBody, outcome;
    [SerializeField] private Image topPanel;


    public void Setup(string title, string reflex, string body, string critical, string injury, string injuryBody, bool reflexB, bool critB, bool injuryB, bool trapB, bool impervious)
    {
        this.title.text = title;
        this.reflex.text ="   " + reflex;
        this.body.text = body;
        this.critical.text = critical;
        this.injuryTitle.text = "<br>Persistant Injury - " + injury;
        this.injuryBody.text = injuryBody;

        if (trapB)
        {
            critTitle.gameObject.SetActive(false);
            critRef.gameObject.SetActive(false);
            this.critical.gameObject.SetActive(false);
            this.reflex.gameObject.SetActive(false);
            injuryTitle.gameObject.SetActive(false);
            this.injuryBody.gameObject.SetActive(false);
            topPanel.color = new Color(0f, 0f, 0f, 1);
            return;
        }
        if (!reflexB || !critB)
        {
            critRef.gameObject.SetActive(false);
        }
        if (!reflexB)
        {
            this.reflex.gameObject.SetActive(false);
        }
        if (!critB)
        {
            critTitle.gameObject.SetActive(false);
            this.critical.gameObject.SetActive(false);
            injuryTitle.gameObject.SetActive(false);
            this.injuryBody.gameObject.SetActive(false);
        }
        else if (!injuryB)
        {
            injuryTitle.gameObject.SetActive(false);
            this.injuryBody.gameObject.SetActive(false);
        }
    }

    public void Outcome(int i)
    {
        outcome.gameObject.SetActive(true);
        switch (i)
        {
            case 0:
                outcome.text = "<b>Failure";
                break;
            case 1:
                outcome.text = "<b>Wound";
                break;
            default:
                outcome.text = "<b>Critical Wound";
                break;
        }
    }
}
