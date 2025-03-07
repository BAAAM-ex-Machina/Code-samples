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
    [SerializeField] private WoundCards woundCon;
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
        Wound
    }
    public Mode mode
    {
        get { return _mode; }
        set 
        {
            if (_mode == Mode.Wound && value != Mode.Wound)
            {
                woundCon.DisableUI();
                zoom.UnlockCamera();
            }
            if (value == Mode.Player)
            {
                if (_mode != Mode.Wound)
                {
                    charaCon.StartTurn();
                }
            }
            else if (value == Mode.Wound && _mode != Mode.Wound)
            {
                woundCon.EnableUI();
                zoom.LockCamera();
                previousMode = _mode;
            }
            _mode = value;
        }
    }


    private Mode _mode;
    private Mode previousMode;
    
    private void OnEnable()
    {
        MonsterController.HitLocationStartEvent += Wound;
    }

    private void OnDisable()
    {
        MonsterController.HitLocationStartEvent -= Wound;
    }

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
            case Mode.Wound:
                if (Input.GetMouseButtonDown(0))
                {
                    if (woundCon.State())
                    {
                        woundCon.Click();
                    }
                }
                
                if (Input.GetMouseButtonDown(1))
                {
                    woundCon.ToggleUI();
                    zoom.ToggleLock();
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
        switch (mode)
        {
            case Mode.Monster:
                break;
            case Mode.Player:
                bool temp = charaCon.Advance();
                if (!temp)
                {
                    mode = Mode.Monster;
                }
                break;
            case Mode.Wound:
                bool tempW = monsterCon.WoundAdvance();
                if (!tempW)
                {
                    mode = previousMode;
                }
                break;
            case Mode.Starting:
                mode = Mode.Player;
                starting.Reset();
                break;
            default:
                break;

        }
    }
        
    public void Wound(bool check)
    {
        if (check)
        {
            mode = Mode.Wound;
            charaCon.WoundMode(check);
        }
        else
        {
            mode = previousMode;
            charaCon.WoundMode(check);
        }
    }
        
        
        
        
  

}
