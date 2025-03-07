using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    private Grid<ShowdownGridObject> grid;
    public List<(int, int)> nodes = new List<(int, int)>();
    public List<(int, int)> outlineNodes = new List<(int, int)>();
    private ShowdownBoardVisual vis;

    public Attack(Grid<ShowdownGridObject> grid, ShowdownBoardVisual vis)
    {
        this.grid = grid;
        this.vis = vis;
    }


    public void Reset()
    {
        foreach (var node in nodes)
        {
            vis.ColourNodeReset(node.Item1, node.Item2);
            grid.gridArray[node.Item1, node.Item2].used = false;

        }
        foreach (var node in outlineNodes)
        {
            vis.ColourNodeReset(node.Item1, node.Item2);
        }
        nodes.Clear();
        outlineNodes.Clear();

    }

    public void Atk(int range, int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            (int, int) temp = (x, y);
            (int, int) temp2;


            Stack<(int, int)> frontier = new Stack<(int, int)>();
            Stack<(int, int)> frontier2 = new Stack<(int, int)>();
            frontier.Push(temp);
            while (range > 0)
            {
                while (frontier.Count > 0)
                {
                    temp2 = frontier.Pop();
                    if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier2.Push(temp); } }
                    if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier2.Push(temp); } }
                    if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; if (grid.gridArray[temp2.Item1, temp2.Item2+1].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier2.Push(temp); } }
                    if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier2.Push(temp); } }
                }
                range--;
                if (range > 0)
                {
                    while (frontier2.Count > 0)
                    {
                        temp2 = frontier2.Pop();
                        if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false ) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier.Push(temp); } }
                        if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); }; frontier.Push(temp); } }
                        if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier.Push(temp); } }
                        if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].monster == true) { nodes.Add(temp); } else { outlineNodes.Add(temp); } frontier.Push(temp); } }

                    }
                }
                range--;
                if (frontier.Count == 0 && frontier2.Count == 0)
                {
                    break;
                }
            }

            foreach (var node in nodes)
            {
                vis.ColourNodeAtk(node.Item1, node.Item2);
            }
            foreach (var node in outlineNodes)
            {
                vis.ColourNodeAtkOutline(node.Item1, node.Item2);
                grid.gridArray[node.Item1, node.Item2].used = false;
            }
        }
    }


}
