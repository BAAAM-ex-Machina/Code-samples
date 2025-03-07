using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    [SerializeField]
    public ScriptableMonster monster;
    [SerializeField] private Transform visualMonster;
    private Grid<ShowdownGridObject> grid;
    private int facing;
    [SerializeField] private WoundCards woundCards;
    public static event Action<bool> HitLocationStartEvent;


    private void Awake()
    {
        monster = Instantiate(monster, this.transform);
        facing = 0;

    }

    private void OnEnable()
    {
        CharacterController.AttackEvent += ResolveHits;
        WoundCards.WoundMoveEvent += MoveWounds; 
    }
    
    private void OnDisable()
    {
        CharacterController.AttackEvent -= ResolveHits;
        WoundCards.WoundMoveEvent -= MoveWounds;
    }


    public void Setup(Grid<ShowdownGridObject> grid)
    {
        this.grid = grid;

        Vector3 position = grid.GetWorldPosition((int)(grid.GetWidth() / 2 -1), (int)(grid.GetHeight()/2 -1)) + new Vector3(grid.GetSize() *monster.size / 2, grid.GetSize()*monster.size / 2);
        position.z = 5;

        visualMonster = Instantiate(visualMonster, position, Quaternion.identity);
        float size = visualMonster.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 rescale = visualMonster.transform.localScale;
        rescale = grid.GetSize() * monster.size * rescale / size;
        visualMonster.transform.localScale = rescale;

        monster.pos = (grid.GetWidth() / 2 - 1, grid.GetHeight() / 2 - 1);
        for (int i = 0; i < monster.size; i++) {
            for (int j = 0; j < monster.size; j++)
            {
                grid.gridArray[monster.pos.Item1 + i, monster.pos.Item2 + j].monster = true;
            }
        }
        monster.Setup();

    }

    public void LookLeft()
    {
        visualMonster.rotation = Quaternion.Euler(0, 0, 270);
        facing = 270;
    }
    public void LookRight()
    {
        visualMonster.rotation = Quaternion.Euler(0, 0, 90);
        facing = 90;
    }
    public void LookUp()
    {
        visualMonster.rotation = Quaternion.Euler(0, 0, 180);
        facing = 180;
    }
    public void LookDown()
    {
        visualMonster.rotation = Quaternion.Euler(0, 0, 0);
        facing = 0;
    }

    public void MoveMonster(int x, int y)
    {
        for (int i = 0; i < monster.size; i++)
        {
            for (int j = 0; j < monster.size; j++)
            {
                grid.gridArray[monster.pos.Item1 + i, monster.pos.Item2 + j].monster = false;
            }
        }
        for (int i = 0; i < monster.size; i++)
        {
            for (int j = 0; j < monster.size; j++)
            {
                grid.gridArray[x + i, y + j].monster = false;
            }
        }
    }

    private void ResolveHits(CharacterController charaCon, ScriptableItem weapon, int speed, int accuracy, int strength, int luck)
    {
        foreach ((int,int) blind in Blindspot())
        {
            if (charaCon.currentChara.position.Item1 == blind.Item1 && charaCon.currentChara.position.Item2 == blind.Item2)
            {
                accuracy--;
                Debug.Log("Hit Blindspot");
                break;
            }
        }
        if (weapon != null)
        {
            accuracy -= charaCon.currentChara.accuracy;
            speed += charaCon.currentChara.speed;
            luck += charaCon.currentChara.luck;
            strength += charaCon.currentChara.strength;
        }

        var rand = new System.Random();
        int hits = 0;
        for (int i = 0; i < speed; i++)
        {
            int temp = rand.Next(1, 11);
            if (temp == 10)
            {
                Debug.Log("Rolled a: " + temp + ". Perfect Hit!");
                hits++;
            }
            else if (temp >= accuracy)
            {
                Debug.Log("Rolled a: " + temp + ". Hit!");
                hits++;
            }
            else
            {
                Debug.Log("Rolled a: " + temp + ". Miss!");
            }
        }
        Debug.Log(hits + " hits!");
        if (hits > 0)
        {
            List<ScriptableWound> tempList = monster.Hit(hits);
            foreach (var hit in tempList)
            {
                woundCards.createWound(hit);
            }

            HitLocationStartEvent(true);
        }
        /*
        int wounds = 0;
        for (int i = 0; i < hits; i++)
        {
            int temp = rand.Next(1, 11);
            if (temp + luck >= 10)
            {
                Debug.Log("Rolled a: " + temp + ". Critical Wound!");
                wounds++;
            }
            else if (temp + strength >= monster.toughness)
            {
                Debug.Log("Rolled a: " + temp + ". Wound!");
                wounds++;
            }
            else
            {
                Debug.Log("Rolled a: " + temp + ". Failed to Wound!");
            }
        }
        Debug.Log(wounds + " wounds!");
        */
    }

    private List<(int,int)> Blindspot()
    {
        List<(int, int)> blind = new List<(int, int)> ();
        switch (facing)
        {
            case 0:
                Debug.Log("Looking Down at Pos: " + monster.pos);
                for (int i = 0; i < monster.size; i++)
                {
                    blind.Add(((int)monster.pos.Item1 + i,(int)monster.pos.Item2 + monster.size));
                    Debug.Log("Blind spot at : " + blind[i]);
                }
                break;
            case 90:
                for (int i = 0; i < monster.size; i++)
                {
                    blind.Add(((int)monster.pos.Item1 -1, (int)monster.pos.Item2 + i));
                    Debug.Log("Blind spot at : " + blind[i]);
                }
                break;
            case 180:
                for (int i = 0; i < monster.size; i++)
                {
                    blind.Add(((int)monster.pos.Item1 + monster.size, (int)monster.pos.Item2 -1));
                    Debug.Log("Blind spot at : " + blind[i]);
                }
                break;
            case 270:
                for (int i = 0; i < monster.size; i++)
                {
                    blind.Add(((int)monster.pos.Item1 + i, (int)monster.pos.Item2 + i));
                    Debug.Log("Blind spot at : " + blind[i]);
                }
                break;
            default:
                Debug.Log("Blindspot Int Error");
                break;
        }
        return blind;
    }

    private void MoveWounds(int i, int j)
    {
        monster.ReorderHL(i, j);
    }

    public bool WoundAdvance()
    {

        return true;
    }

}
