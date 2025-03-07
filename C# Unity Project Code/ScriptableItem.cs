using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemEnum;


namespace ItemEnum
{
    public enum ItemType
    {
        head,
        body,
        waist,
        hands,
        feet,
        weapon,
        item
    }
    public enum Affinity
    {
        none,
        red,
        green,
        blue
    }
    public enum ActiveCost
    {
        none,
        action,
        movement,
    }
    public enum Active
    {
        none,
        foundingStone
    }

}





[CreateAssetMenu]
public class ScriptableItem : ScriptableObject
{
    public ItemType type;
    public Sprite icon;
    public int armour =0;
    public int speed =0;
    public int accuracy =0;
    public int strength = 0;
    public int luck = 0;
    public int reach = 0;
    [Space]
    public ActiveCost activeCost = ActiveCost.none;
    public Active active = Active.none;
    public string desc;
    [Space]
    public Affinity up;
    public Affinity down;
    public Affinity left;
    public Affinity right;

}

