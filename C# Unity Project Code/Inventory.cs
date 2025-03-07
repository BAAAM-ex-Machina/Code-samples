using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemEnum;



public class Inventory
{
    private ScriptableItem defaultAttack;
    private ScriptableItem[,] gearGrid;


    public Inventory()
    {
        gearGrid = new ScriptableItem[3, 3];
    }


    public Inventory(ScriptableItem defaultAttack, List<ScriptableItem> items)
    {
        this.defaultAttack = defaultAttack;
        gearGrid = new ScriptableItem[3,3];
        int i = 0;
        int j = 0;
        foreach (ScriptableItem item in items) {
            gearGrid[j,i] = item;
            i++;
            if (i == 3) {
                i = 0;
                j += 1;
            }
        }
    }

    public List<ScriptableItem> Weapons()
    {
        List<ScriptableItem> temp = new List<ScriptableItem>();
        temp.Add(defaultAttack);
        foreach (ScriptableItem item in gearGrid) {
            if (item != null && item.type == ItemType.weapon)
            {
                temp.Add(item);
            }
        }
        return temp;
    }

    public List<ScriptableItem> Actives()
    {
        List<ScriptableItem> temp = new List<ScriptableItem>();
        foreach (ScriptableItem item in gearGrid)
        {
            if (item != null && item.active != Active.none)
            {
                temp.Add(item);
            }
        }
        return temp;
    }

    public void Archive(ScriptableItem item)
    {
        bool check = true;
        for (int i = 0; i < gearGrid.Length; i++)
        {
            for (int j = 0; j < gearGrid.Length; j++)
            {
                if (gearGrid[i,j] != null && gearGrid[i, j] == item)
                {
                    gearGrid[i,j] = null;
                    check = false;
                    break;
                }
            }
            if (!check)
            {
                break;
            }
        }
        if (check)
        {
            Debug.Log("Archive failed");
        }


    }




}
