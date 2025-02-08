using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemEnum;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] icons;
    private List<Character> characters;
    public Transform visualCharacter;
    private List<Transform> visualCharacters;
    private Grid<ShowdownGridObject> grid;
    private Queue toSpawn;
    [SerializeField]
    private ScriptableItem defaultAttack;
    [SerializeField]
    private List<ScriptableItem> startingItems;
    [SerializeField]
    public Hotbar hotbar;
    private ScriptableItem currentAttack;

    public Attack attack;
    private Movement movement;
    private Action active;

    public Character currentChara;
    public static event Action<CharacterController, ScriptableItem, int, int,int,int> AttackEvent;

    private Character actingChara
    {
        get
        {
            return _actingCharacter;
        }
        set
        {
            if (_actingCharacter != null)
            {
                _actingCharacter.act = false;
                _actingCharacter.canAction = false;
                _actingCharacter.canMove = false;
            }
            _actingCharacter = value;
        }
    }
    private Character _actingCharacter;


    private enum Mode
    {
        none,
        selected,
        movement,
        attack,
        active
    }
    private Mode mode
    {
        get { return _mode; }
        set
        {
            switch (_mode)
            {
                case Mode.attack:
                    attack.Reset();
                    break;
                case Mode.movement:
                    movement.Reset();
                    break;
                case Mode.active:
                    active.Reset();
                    break;
                default:
                    break;
            }
            _mode = value;
        }
    }
    private Mode _mode;



    // Start is called before the first frame update
    void Start()
    {
        characters = new List<Character> ();
        toSpawn = new Queue ();
        visualCharacters = new List<Transform> ();
        currentChara = null;
        mode = Mode.none;


        characters.Add(new Character(5, 0, defaultAttack, startingItems));
        characters.Add(new Character(5, 1, defaultAttack, startingItems));
        characters.Add(new Character(5, 2, defaultAttack, startingItems));
        characters.Add(new Character(5, 3, defaultAttack, startingItems));
        int i = 0;
        foreach (Character character in characters)
        {
            toSpawn.Enqueue(i);
            i++;
        }
        hotbar.Icon(icons[characters[(int) toSpawn.Peek()].icon]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Grid<ShowdownGridObject> grid, ShowdownBoardVisual visual)
    {
        this.grid = grid;
        movement = new Movement(grid, visual);
        attack = new Attack(grid, visual);
        active = new Action(this);
    }

    public void CreateCharacter(int x, int y)
    {
        if (toSpawn.Count > 0)
        {
            Character temp2 = (Character) characters[(int) toSpawn.Dequeue()];
            temp2.position = (x, y);

            Vector3 position = grid.GetWorldPosition(x, y) + new Vector3(grid.GetSize() / 2, grid.GetSize() / 2);
            position.z = 7;

            var temp = Instantiate(visualCharacter, position, Quaternion.identity, this.transform);
            temp.GetComponent<SpriteRenderer>().sprite = icons[temp2.icon];
            float size = temp.GetComponent<SpriteRenderer>().bounds.size.y;
            Vector3 rescale = temp.transform.localScale;
            rescale = grid.GetSize() * rescale / size;
            temp.transform.localScale = rescale;

            visualCharacters.Add(temp);
            grid.gridArray[x, y].character = true;
            if (toSpawn.Count > 0)
            {
                hotbar.Icon(icons[characters[(int)toSpawn.Peek()].icon]);
            }
            else
            {
                hotbar.Icon(true);
            }
        }
    }

    public void DestroyCharacter(int x, int y)
    {
        Vector3 position = grid.GetWorldPosition(x, y) + new Vector3(grid.GetSize() / 2, grid.GetSize() / 2);
        position.z = 7;
        for (int i = 0; i < visualCharacters.Count; i++)
        {
            if (visualCharacters[i].position == position)
            {
                Destroy(visualCharacters[i].gameObject);
                visualCharacters.RemoveAt(i);
                grid.gridArray[x,y].character = false;
            }
        }
        int j = 0;
        foreach (Character character in characters)
        {
            if (character.position == (x, y))
            {
                character.position = (-1, -1);
                toSpawn.Enqueue(j);
            }
            j++;
        }
        if (toSpawn.Count == 1)
        {
            hotbar.Enable();
            hotbar.DisableButton();
            hotbar.Icon(icons[characters[(int)toSpawn.Peek()].icon]);
        }
    }

    public void CycleSpawn()
    {
        if (toSpawn.Count > 0)
        {
            toSpawn.Enqueue(toSpawn.Dequeue());
            hotbar.Icon(icons[characters[(int)toSpawn.Peek()].icon]);
        }
    }

    public bool SpawnedCheck() 
    {
        if (toSpawn.Count > 0) { return false; }
        else { return true; }
    }

    public bool Advance()
    {
        bool actsLeft = false;
        if (actingChara != null)
        {
            actsLeft = true;
            hotbar.Icon(false);
            ChangeAct();

        }
        else
        {
            foreach (Character chara in characters)
            {
                if (chara.act == true)
                {
                    actsLeft = true;
                    Character temp = chara;
                    CharaNode(temp.position.Item1, temp.position.Item2);
                    break;
                }
            }
        }
        return actsLeft;
    }

    public void StartTurn()
    {
        foreach (Character chara in characters)
        {
            chara.act = true;
            chara.canMove = true;
            chara.canAction = true;
        }
        hotbar.Icon(false);
    }

    public void CharaNode(int j, int k)
    {
        if (mode == Mode.none || mode == Mode.selected)
        {
            CharaUI(j, k);
            mode = Mode.selected;
            hotbar.Disable();
        }
        
    }

    public void UsedNode(int j, int k)
    {
        if (actingChara == null && currentChara != null)
        {
            ChangeAct();
        }
        if (actingChara != null && actingChara == currentChara)
        {
            if (mode == Mode.movement)
            {
                CurrentMove(j, k);
                mode = Mode.selected;
            }
            else if (mode == Mode.attack)
            {
                CurrentAttack(j, k);
                mode = Mode.selected;
            }
            else if (mode == Mode.active)
            {
                active.ResolveAction();
                mode = Mode.selected;
            }
        }
    }

    public void RightClick()
    {
        switch (mode)
        {
            case Mode.movement:
                movement.Reset();
                mode = Mode.selected;
                break;
            case Mode.attack:
                attack.Reset();
                mode = Mode.selected;
                break;
            case Mode.selected:
                hotbar.UIDisable();
                hotbar.Enable();
                if (actingChara != null)
                {
                    hotbar.Icon(actingChara.icon);
                }
                currentChara = null;
                mode = Mode.none;
                break;
            case Mode.active:
                active.Reset();
                mode = Mode.selected;
                break;
            default:
                break;

        }
    }

    public void Move()
    {
        if (currentChara.canMove)
        {
            mode = Mode.movement;
            (int, int) temp = CurrentPos();
            movement.Move(currentChara.movement, temp.Item1, temp.Item2);
        }
    }

    public void Attack(int i)
    {
        if (currentChara.canAction)
        {
            mode = Mode.attack;
            (int, int) temp = CurrentPos();
            List<ScriptableItem> temp2 = currentChara.inventory.Weapons();
            currentAttack = temp2[i];
            attack.Atk(currentAttack.reach, temp.Item1, temp.Item2);
        }
    }

    public void Active(int i)
    {
        if (actingChara == null && currentChara != null)
        {
            ChangeAct();
        }
        if (actingChara != null && currentChara == actingChara)
        {
            ScriptableItem temp = currentChara.inventory.Actives()[i];
            switch (temp.activeCost)
            {
                case ActiveCost.action:
                    if (currentChara.canAction)
                    {
                        mode = Mode.active;
                        if (active.StartAction(temp))
                        {
                           
                        }
                    }
                    break;
                case ActiveCost.movement:
                    if (currentChara.canMove)
                    {
                        mode = Mode.active;
                        if (active.StartAction(temp))
                        {
                        }
                    }
                    break;
                default:
                    mode = Mode.active;
                    if (active.StartAction(temp))
                    {
                    }
                    break;
            }
        }
    }




    public void CharaUI(int x, int y)
    {
        foreach (Character character in characters)
        {
            if (character.position == (x, y))
            {
                currentChara = character;
                UpdateHotbar(true);
            }
        }
    }


    public void UpdateHotbar(bool check)
    {
        if (currentChara != null)
        {
            if (check)
            {
                hotbar.ResetButtons();
            }
            bool temp = false;
            if (actingChara == null && currentChara.act == false)
            {
                temp = true;
            }
            hotbar.CharacterUI(icons[currentChara.icon], currentChara.inventory.Weapons(), currentChara.inventory.Actives(), currentChara.canAction, currentChara.canMove, temp, currentChara.speed, currentChara.accuracy, currentChara.strength, currentChara.luck);
        }
    }

    public void ChangeAct()
    {
        if (actingChara != null)
        {
            actingChara = null;
            hotbar.ChangeAct(-1);
            UpdateHotbar(false);
            bool actsLeft = false;
            foreach (Character chara in characters)
            {
                if (chara.act == true)
                {
                    actsLeft = true;
                    break;
                }
            }
            if (!actsLeft)
            {
                hotbar.Icon(true);
                hotbar.Disable();
            }
        }
        else
        {
            if (currentChara != null && currentChara.act)
            {
                actingChara = currentChara;
                hotbar.ChangeAct(currentChara.icon);
            }
        }
    }



    public (int,int) CurrentPos()
    {
        return currentChara.position;
    }

    public void CurrentMove(int x, int y)
    {
        Vector3 position = grid.GetWorldPosition(currentChara.position.Item1, currentChara.position.Item2) + new Vector3(grid.GetSize() / 2, grid.GetSize() / 2);
        position.z = 7;

        for (int i = 0; i < visualCharacters.Count; i++)
        {
            if (visualCharacters[i].position == position)
            {
                visualCharacters[i].position = grid.GetWorldPosition(x, y) + new Vector3(grid.GetSize() / 2, grid.GetSize() / 2);
            }
        }
        grid.gridArray[currentChara.position.Item1, currentChara.position.Item2].character = false;
        currentChara.position = (x,y);
        grid.gridArray[x, y].character = true;
        currentChara.canMove = false;
        movement.Reset();
        hotbar.MovementUsed();

    }

    public void CurrentAttack(int x, int y)
    {
        currentChara.canAction = false;
        attack.Reset();
        AttackMonster(currentAttack, currentChara.speed + currentAttack.speed, currentChara.accuracy + currentAttack.accuracy, currentChara.strength + currentAttack.strength, currentChara.luck + currentAttack.luck);
        hotbar.ActionUsed();
    }

    public void AttackMonster(ScriptableItem weapon, int speed, int accuracy, int strength, int luck )
    {
        AttackEvent(this, weapon, speed, accuracy, strength, luck);
    }
}
