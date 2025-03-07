using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public int movement;
    public int speed;
    public int strength;
    public int accuracy;
    public int luck;
    public int survival;
    public int icon;
    public (int, int) position;
    public bool act;
    public bool canMove;
    public bool canAction;
    public Inventory inventory;


    public Character()
    {
        movement = 5;
        speed = 0;
        strength = 0;
        accuracy = 0;
        luck = 0;
        survival = 0;
        icon = 0;
        position = (-1, -1);
        act = false;
        canMove = false;
        canAction = false;
        inventory = new Inventory();
    }

    public Character(int movement, int icon, ScriptableItem defaultAttack, List<ScriptableItem> items)
    {
        this.movement = movement;
        this.icon = icon;
        position = (-1, -1);
        act = false;
        canMove = false;
        canAction = false;
        inventory = new Inventory(defaultAttack, items);
    }

}
