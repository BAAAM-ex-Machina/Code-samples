using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowdownGridObject
{
    private Grid<ShowdownGridObject> grid;
    private int x;
    private int y;
    public bool passable = true;
    public bool used = false;
    public bool monster
    {
        get { return _monster; }
        set
        {
            _monster = value;
            if (_monster)
            {
                passable = false;
            }
            if (!_monster)
            {
                passable = true;
            }
        }
    }
    private bool _monster = false;
    public bool character
    {
        get{ return _character; }
        set
        {
            _character = value;
            if (_character)
            {
                passable = false;
            }
            if (!_character)
            {
                passable = true;
            }
        }
    }
    private bool _character = false;

    public ShowdownGridObject(Grid<ShowdownGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

}
