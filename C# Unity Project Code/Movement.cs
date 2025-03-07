using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement 
{

    private Grid<ShowdownGridObject> grid;
    private ShowdownBoardVisual vis;
    private List<(int,int)> nodes = new List<(int, int)>();


    public Movement(Grid<ShowdownGridObject> grid, ShowdownBoardVisual vis)
    {
        this.grid = grid;
        this.vis = vis;
    }

    public void Move(int mov, int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight()) {
            (int, int) temp = (x, y);
            (int, int) temp2;


            Stack<(int,int)> frontier = new Stack<(int, int)>();
            Stack<(int, int)> frontier2 = new Stack<(int, int)>();
            frontier.Push(temp);
            while (mov > 0) {
                while (frontier.Count > 0)
                {
                    temp2 = frontier.Pop();
                    if (temp2.Item1 + 1 < grid.GetWidth()){if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 + 1, temp2.Item2].passable == true) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used = true; nodes.Add(temp); frontier2.Push(temp); } }
                    if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 - 1, temp2.Item2].passable == true) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; nodes.Add(temp); frontier2.Push(temp); } }
                    if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 + 1].passable == true) { temp = (temp2.Item1, temp2.Item2+1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; nodes.Add(temp); frontier2.Push(temp); } }
                    if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 - 1].passable == true) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; nodes.Add(temp); frontier2.Push(temp); } }
                }
                mov--;
                if (mov > 0)
                {
                    while(frontier2.Count > 0)
                    {
                        temp2 = frontier2.Pop();
                        if (temp2.Item1 + 1 < grid.GetWidth()) { if (grid.gridArray[temp2.Item1 + 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 + 1, temp2.Item2].passable == true) { temp = (temp2.Item1 + 1, temp2.Item2); grid.gridArray[temp2.Item1 + 1, temp2.Item2].used= true; nodes.Add(temp); frontier.Push(temp); } }
                        if (temp2.Item1 - 1 >= 0) { if (grid.gridArray[temp2.Item1 - 1, temp2.Item2].used == false && grid.gridArray[temp2.Item1 - 1, temp2.Item2].passable == true) { temp = (temp2.Item1 - 1, temp2.Item2); grid.gridArray[temp2.Item1 - 1, temp2.Item2].used = true; nodes.Add(temp); frontier.Push(temp); } }
                        if (temp2.Item2 + 1 < grid.GetHeight()) { if (grid.gridArray[temp2.Item1, temp2.Item2 + 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 + 1].passable == true) { temp = (temp2.Item1, temp2.Item2 + 1); grid.gridArray[temp2.Item1, temp2.Item2 + 1].used = true; nodes.Add(temp); frontier.Push(temp); } }
                        if (temp2.Item2 - 1 >= 0) { if (grid.gridArray[temp2.Item1, temp2.Item2 - 1].used == false && grid.gridArray[temp2.Item1, temp2.Item2 - 1].passable == true) { temp = (temp2.Item1, temp2.Item2 - 1); grid.gridArray[temp2.Item1, temp2.Item2 - 1].used = true; nodes.Add(temp); frontier.Push(temp); } }

                    }
                }
                mov--;
            }

            foreach (var node in nodes)
            {
                vis.ColourNodeMove(node.Item1, node.Item2);
            }
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
}
