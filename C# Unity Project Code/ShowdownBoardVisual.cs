using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowdownBoardVisual : MonoBehaviour
{
    [SerializeField] private Transform visualNode;
    private Grid<ShowdownGridObject> grid;
    private Transform[,] visualNodeArray;
    //private List<Transform> visualNodeList;



    void Awake()
    {
        //visualNodeList = new List<Transform>();
    }

    public void Setup(Grid<ShowdownGridObject> grid)
    {
        this.grid = grid;
        visualNodeArray = new Transform[grid.GetWidth(),grid.GetHeight()];

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3 gridPosition = grid.GetWorldPosition(x,y) + new Vector3(grid.GetSize()/2, grid.GetSize() / 2);
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x,y] = visualNode;
                //visualNodeList.Add(visualNode);
            }
        }

    }

    private Transform CreateVisualNode (Vector3 position)
    {
        position.z = 10;
        Transform visualNodeTransform = Instantiate(visualNode, position, Quaternion.identity, this.transform);
        //visualNodeTransform.transform.localScale = new Vector3(grid.GetSize(), grid.GetSize());
        float size = visualNodeTransform.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3 rescale = visualNodeTransform.transform.localScale;
        rescale = grid.GetSize() * rescale / size;
        visualNodeTransform.transform.localScale = rescale;

        return visualNodeTransform;
    }

    public void ColourNodeMove(int x,int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            visualNodeArray[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 0.9f, 0.6f, 1);
        }
    }

    public void ColourNodeStartPos(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            visualNodeArray[x, y].GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 1, 1);
        }
    }

    public void ColourNodeAtk(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            visualNodeArray[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f, 1);
        }
    }

    public void ColourNodeAtkOutline(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            visualNodeArray[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 0.7f, 0.7f, 1);
        }
    }

    public void ColourNodeReset(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.GetWidth() && y < grid.GetHeight())
        {
            visualNodeArray[x, y].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

}
