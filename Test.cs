using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Positions;

public class Test : MonoBehaviour
{
    private ShowdownBoard showdownboard;
    private StartingPosition starting;
    [SerializeField] private MonsterController monsterCon;
    [SerializeField] private CharacterController charaCon;
    [SerializeField] private Zoom zoom;
    private int j, k;

    [SerializeField] private ShowdownBoardVisual showdownBoardVisual;
    
    [SerializeField]
    int width, height, x, y;
    [SerializeField]
    float size;
    


    public enum Mode
    {
        Starting,
        Monster,
        Player,
        Victory,
        Defeat,
        Test
    }
    public Mode mode
    {
        get { return _mode; }
        set 
        { 
            _mode = value; 
            if (_mode == Mode.Player)
            {
                charaCon.StartTurn();
            }
        }
    }


    private Mode _mode;
    

    private void Start()
    {
        showdownboard = new ShowdownBoard(width, height, size, new Vector3 (x,y));
        showdownBoardVisual.Setup(showdownboard.GetGrid());
        starting = new StartingPosition(showdownboard.GetGrid(), showdownBoardVisual);

        monsterCon.Setup(showdownboard.GetGrid());
        charaCon.Setup(showdownboard.GetGrid(), showdownBoardVisual);
        zoom.Setup(showdownboard.GetGrid());


        starting.StartingPos(monsterCon.monster.startPos,monsterCon.monster.size,monsterCon.monster.startPosN);

        mode = Mode.Starting;
    }

    private void Update()
    {
        switch (mode)
        {
            case Mode.Starting:
                if (Input.GetMouseButtonDown(0))
                {

                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    showdownboard.GetGrid().GetXY(mouseWorldPosition, out j, out k);
                    if (j >= 0 && k >= 0 && j < width && k < height && showdownboard.GetGrid().gridArray[j, k].used == true && showdownboard.GetGrid().gridArray[j, k].character == false)
                    {
                        charaCon.CreateCharacter(j,k);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    showdownboard.GetGrid().GetXY(mouseWorldPosition, out j, out k);
                    if (j >= 0 && k >= 0 && j < width && k < height && showdownboard.GetGrid().gridArray[j, k].used == true && showdownboard.GetGrid().gridArray[j, k].character == true)
                    {
                        charaCon.DestroyCharacter(j, k);
                    }
                    else
                    {
                        charaCon.CycleSpawn();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (charaCon.SpawnedCheck())
                    {
                        mode = Mode.Player;
                        starting.Reset();
                    }
                }
                break;
            case Mode.Monster:
                break;
            case Mode.Player:
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    showdownboard.GetGrid().GetXY(mouseWorldPosition, out j, out k);
                    if (j >= 0 && k >= 0 && j < width && k < height && showdownboard.GetGrid().gridArray[j, k].used == true)
                    {
                        charaCon.UsedNode(j, k);
                    }
                    else if (j >= 0 && k >= 0 && j < width && k < height && showdownboard.GetGrid().gridArray[j, k].character == true)
                    {
                        charaCon.CharaNode(j, k);
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    charaCon.RightClick();
                }

                    break;
            case Mode.Victory:
                break;
            case Mode.Defeat:
                break;
            default:
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f;
                    showdownboard.GetGrid().GetXY(mouseWorldPosition, out j, out k);

                }
                if (Input.GetMouseButtonDown(1))
                {
                    starting.Reset();
                }
                break;

        }

    }

    public void Advance()
    {
        if (mode == Mode.Monster)
        {
        }
        else if (mode == Mode.Player)
        {
            bool temp = charaCon.Advance();
            if (!temp)
            {
                mode = Mode.Monster;
            }
        }
        else if (mode == Mode.Starting)
        {
            mode = Mode.Player;
            starting.Reset();
        }
    }
        
        
        
        
        
        
  

}
