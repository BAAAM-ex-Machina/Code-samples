using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLevel;
using Positions;


namespace MonsterLevel
{
    public enum Level
    {
        Prologue,
        lvl1,
        lvl2,
        lvl3
    }
}


[CreateAssetMenu]
public class ScriptableMonster : ScriptableObject
{
    public Sprite icon;
    public Level monsterLevel;
    public Position startPos = Position.nAway;
    public int startPosN = 6;
    public int size = 2;
    public int movement;
    public int toughness;
    public (int, int) pos = (-1, -1);
    public List<ScriptableWound> hitLocations;
    public ScriptableWound startingHitLocation;

    private Queue<ScriptableWound> HLDeck;
    private List<ScriptableWound> CurrentHL;


    public void Setup()
    {
        CurrentHL = new List<ScriptableWound> ();
        HLDeck = new Queue<ScriptableWound>();
        if (startingHitLocation != null)
        {
            HLDeck.Enqueue(startingHitLocation);
        }
        Shuffle<ScriptableWound>(hitLocations, HLDeck, false);
    }

    public void ShuffleHLD()
    {
        Shuffle<ScriptableWound>(hitLocations, HLDeck, true);
    }

    private void Shuffle<T>(List<T> list, Queue<T> queue, bool check)
    {
        if (check)
        {
            while (queue.Count > 0)
            {
                list.Add(queue.Dequeue());
            }
        }

        System.Random r = new System.Random();
        while (list.Count > 0)
        {
            int i = r.Next(0, list.Count - 1);
            queue.Enqueue(list[i]);
            list.RemoveAt(i);
        }
    }

    public List<ScriptableWound> Hit(int i)
    {
        while (i > 0)
        {
            CurrentHL.Insert(0,HLDeck.Dequeue());
            if (CurrentHL[0].trap == true)
            {
                while (CurrentHL.Count > 1)
                {
                    hitLocations.Add(CurrentHL[1]);
                    CurrentHL.RemoveAt(1);
                }
                break;
            }
            i--;
        }
        return CurrentHL;
    }

    public void FinishHit()
    {
        bool trap = false;
        if (CurrentHL[0].trap == true)
        {
            trap = true;
        }

        while (CurrentHL.Count > 0)
        {
            hitLocations.Add(CurrentHL[0]);
            CurrentHL.RemoveAt(0);
        }
        if (trap)
        {
            Shuffle<ScriptableWound>(hitLocations, HLDeck, true);
        }
    }

    public void ReorderHL(int i, int j)
    {
        var heldCard = CurrentHL[i];
        CurrentHL.RemoveAt(i);
        CurrentHL.Insert(j, heldCard);
    }

    public bool Wound(int i)
    {
        return false;
    }


}
