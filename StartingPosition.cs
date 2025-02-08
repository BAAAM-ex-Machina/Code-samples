using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Positions;
using System;

namespace Positions
{
    public enum Position
    {
        nAway,
        nAwayCorners,
        BottomLine,
        SideLines
    }
}



public class StartingPosition 
{


    private Grid<ShowdownGridObject> grid;
    public List<(int, int)> nodes = new List<(int, int)>();
    private ShowdownBoardVisual vis;

    public StartingPosition(Grid<ShowdownGridObject> grid, ShowdownBoardVisual vis)
    {
        this.grid = grid;
        this.vis = vis;
    }


    public void StartingPos(Position type, int size, int n)
    {
        StartingPos(type, ((int)(grid.GetWidth() / 2 - 1), (int)(grid.GetHeight() / 2 - 1)), size, n);
    }

    public void StartingPos(Position type, (int, int) monster, int size, int n)
    {
        size--;
        switch (type)
        {
            case Position.nAway:
                NAway(monster, size, n);
                break;
            case Position.nAwayCorners:
                NAwayCorners(monster, size, n);
                break;
            case Position.BottomLine:
                BottomLine();
                break;
            case Position.SideLines:
                SideLines();
                break;
        }

        foreach (var node in nodes)
        {
            vis.ColourNodeStartPos(node.Item1, node.Item2);
        }


    }


    public void Reset()
    {
        foreach (var node in nodes)
        {
            vis.ColourNodeReset(node.Item1, node.Item2);
            grid.gridArray[node.Item1, node.Item2].used = false;

        }
        nodes.Clear();
    }





    private void BottomLine()
    {
        int i = 0;
        while (i < grid.GetWidth())
        {
            nodes.Add((i, 0));
            i++;
        }
    }

    private void SideLines()
    {
        int i = 0;
        while (i < grid.GetHeight())
        {
            nodes.Add((0, i));
            nodes.Add((grid.GetWidth() - 1, i));
            i++;
        }
    }


    private void NAwayCorners ((int, int) monster, int size, int n)
    {
        nodes.Add((monster.Item1 - n, monster.Item2 - n));
        nodes.Add((monster.Item1 - n, monster.Item2 + size + n));
        nodes.Add((monster.Item1 + size + n, monster.Item2 - n));
        nodes.Add((monster.Item1 + size + n, monster.Item2 + size + n));
    }

    private void NAway((int, int) monster, int size, int n)
    {
        List<(int, int)> usedNodes = new List<(int, int)>();

        int i = 0;
        (int, int) temp;
        Stack<(int, int)> frontier = new Stack<(int, int)>();
        (int, int) temp2;
        Stack<(int, int)> frontier2 = new Stack<(int, int)>();

        while (i <= size)
        {
            int j = 0;

            while (j <= size)
            {
                temp = (i + monster.Item1, j + monster.Item2);
                usedNodes.Add(temp);
                grid.gridArray[i + monster.Item1, j + monster.Item2].used = true;

                if (i == 0 || i == size || j == 0 || j == size)
                {
                    frontier.Push(temp);

                }
                j++;
            }
            i++;
        }



        while (n > 1)
        {

            while (frontier.Count > 0)
            {
                temp2 = frontier.Pop();
                if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; usedNodes.Add(temp); frontier2.Push(temp); } }
                if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; usedNodes.Add(temp); frontier2.Push(temp); } }
                if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; usedNodes.Add(temp); frontier2.Push(temp); } }
                if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; usedNodes.Add(temp); frontier2.Push(temp); } }
            }
            n--;
            if (n > 1)
            {
                while (frontier2.Count > 0)
                {
                    temp2 = frontier2.Pop();
                    if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; usedNodes.Add(temp); frontier.Push(temp); } }
                    if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; usedNodes.Add(temp); frontier.Push(temp); } }
                    if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; usedNodes.Add(temp); frontier.Push(temp); } }
                    if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; usedNodes.Add(temp); frontier.Push(temp); } }

                }
            }
            n--;
        }


        while (frontier.Count > 0)
        {

            temp2 = frontier.Pop();
            if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 + 1, temp2.Item2].passable == true) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; nodes.Add(temp); } }
            if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 - 1, temp2.Item2].passable == true) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; nodes.Add(temp); } }
            if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 + 1].passable == true) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; nodes.Add(temp); } }
            if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 - 1].passable == true) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; nodes.Add(temp); } }

        }
        while (frontier2.Count > 0)
        {

            temp2 = frontier2.Pop();
            if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 + 1, temp2.Item2].passable == true) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; nodes.Add(temp); } }
            if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 - 1, temp2.Item2].passable == true) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; nodes.Add(temp); } }
            if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 + 1].passable == true) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; nodes.Add(temp); } }
            if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 - 1].passable == true) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; nodes.Add(temp); } }

        }

        foreach (var node in usedNodes)
        {
            grid.gridArray[node.Item1, node.Item2].used = false;
        }
        usedNodes.Clear();
    }

}





