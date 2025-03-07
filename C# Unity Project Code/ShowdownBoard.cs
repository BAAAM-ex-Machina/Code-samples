using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowdownBoard
{
    private Grid<ShowdownGridObject> grid;

    public ShowdownBoard(int width, int height, float size, Vector3 originPosition)
    {
        grid = new Grid<ShowdownGridObject>(width, height, size, originPosition, (Grid<ShowdownGridObject> grid, int z, int q) => new ShowdownGridObject(grid, z, q));
    }

    public Grid<ShowdownGridObject> GetGrid() { return grid; }
}
