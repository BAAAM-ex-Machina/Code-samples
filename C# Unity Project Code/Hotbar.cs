using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ItemEnum;
using TMPro;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private List<Button> actionButtons;
    [SerializeField] private List<Button> survivalButtons;
    [SerializeField] private List<Button> attackButtons;
    [SerializeField] private Image icon;
    [SerializeField] private Image iconBorder;
    [SerializeField] private Image hotbarIcon;
    [SerializeField] private List<GameObject> buff;
    [SerializeField] private Sprite act;
    [SerializeField] private Sprite act2;
    [SerializeField] private Image actIcon;
    [SerializeField] private Image actBorder;
    [SerializeField] private Sprite advance;
    [SerializeField] private GameObject hover;
    [SerializeField] private Image hoverIcon;
    [SerializeField] private TextMeshProUGUI hoverTitle;
    [SerializeField] private TextMeshProUGUI hoverBody;
    [SerializeField] private TextMeshProUGUI hoverBodyRight;
    [SerializeField] private MonsterController mc;
    private List<ScriptableItem> currentAction;
    private List<ScriptableItem> currentAttack;
    private int speed;
    private int accuracy;
    private int strength;
    private int luck;
    private List<Color> colours;




    // Start is called before the first frame update
    void Start()
    {
        UIDisable();
        colours = new List<Color>();
        colours.Add(new Color(0.69f, 0.886f, 0.918f, 1));
        colours.Add(new Color(0.784f, 0.902f, 0.678f, 1));
        colours.Add(new Color(0.439f, 0.573f, 0.745f, 1));
        colours.Add(new Color(1f, 0.886f, 0.616f, 1));

        actIcon.sprite = act2;
        actBorder.color = new Color(0f, 0f, 0f, 1f);
    }

    private void OnEnable()
    {
        Hover.HoverEvent += HoverDesc;
    }

    private void OnDisable()
    {
        Hover.HoverEvent -= HoverDesc;
    }


    public void Icon(Sprite image)
    {
        icon.sprite = image;
    }

    public void Icon(bool monster)
    {
        iconBorder.gameObject.SetActive(false);
        if (!monster)
        {
            icon.sprite = act2;
        }
        else
        {
            icon.sprite = advance;
        }
        icon.gameObject.GetComponent<Button>().enabled = true;
        icon.gameObject.SetActive(true);
    }

    public void Icon(int i)
    {
        icon.sprite = act;
        iconBorder.gameObject.SetActive(true);
        iconBorder.color = colours[i];
    }

    public void DisableButton()
    {
        icon.gameObject.GetComponent<Button>().enabled = false;
    }

    public void Disable()
    {
        icon.gameObject.SetActive(false);
    }

    public void Enable()
    {
        icon.gameObject.SetActive(true);
    }

    public void UIDisable()
    {
        foreach (Button b in actionButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in survivalButtons)
        {
            b.gameObject.SetActive(false);
        }
        foreach (Button b in attackButtons)
        {
            b.gameObject.SetActive(false);
        }
        buff[2].SetActive(false);
        buff[5].SetActive(false);

        this.gameObject.SetActive(false);
    }

    public void CharacterUI(Sprite image, List<ScriptableItem> weapons, List<ScriptableItem> actions, bool canAction, bool canMove, bool act, int speed, int accuracy, int strength, int luck)
    {
        this.speed = speed;
        this.accuracy = accuracy;
        this.strength = strength;
        this.luck = luck;
        this.gameObject.SetActive(true);
        survivalButtons[0].gameObject.SetActive(true);
        survivalButtons[1].gameObject.SetActive(true);
        for (int i = 0; i < attackButtons.Count; i++)
        {
            if (i < weapons.Count)
            {
                attackButtons[i].gameObject.SetActive(true);
                attackButtons[i].gameObject.GetComponent<Image>().sprite = weapons[i].icon;
            }
            else
            {
                attackButtons[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < actionButtons.Count; i++)
        {
            if (i < actions.Count)
            {
                actionButtons[i].gameObject.SetActive(true);
                actionButtons[i].gameObject.GetComponent<Image>().sprite = actions[i].icon;
            }
            else
            {
                actionButtons[i].gameObject.SetActive(false);
            }
        }

        

        hotbarIcon.sprite = image;
        currentAttack = weapons;
        currentAction = actions;
        int temp = 2;
        if (weapons.Count % 2 != 0)
        {
            buff[2].SetActive(true);
        }else
        {
            buff[2].SetActive(false);
        }
        if (actions.Count == 0)
        {
            buff[3].SetActive(false);
            buff[4].SetActive(false);
            buff[5].SetActive(false);
        }
        else if (actions.Count % 2 != 0)
        {
            buff[3].SetActive(true);
            buff[4].SetActive(true);
            buff[5].SetActive(true);
            temp++;
        }
        else
        {
            buff[3].SetActive(true);
            buff[4].SetActive(true);
            buff[5].SetActive(false);
            temp++;
        }
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2((float)(Math.Ceiling((double)weapons.Count / 2) + Math.Ceiling((double)actions.Count / 2) + temp) * 65, rt.sizeDelta.y);

        if (!canAction)
        {
            ActionUsed();
        }
        if (!canMove)
        {
            MovementUsed();
        }
        if (act)
        {
            actIcon.gameObject.SetActive(false);
        }
        else
        {
            actIcon.gameObject.SetActive(true);
        }


    }

    public void ButtonPress(int button)
    {

    }

    public void AttackPress(int button)
    {

    }

    public void ActionPress(int button)
    {

    }

    public void ChangeAct(int i)
    {
        if (i >= 0)
        {
            actIcon.sprite = act;
            actBorder.color = colours[i];
        }
        else
        {
            actIcon.sprite = act2;
            actBorder.color = new Color(0f,0f,0f,1f);
        }
    }

    public void ActionUsed()
    {
        for (int i=0; i < currentAction.Count; i++)
        {
            if (currentAction[i].activeCost == ActiveCost.action)
            {
                actionButtons[i].interactable = false;
            }
        }
        foreach (Button b in attackButtons) {
            b.interactable = false;
        }
    }

    public void MovementUsed()
    {
        int i = 0;
        foreach (ScriptableItem item in currentAction)
        {
            if (item.activeCost == ActiveCost.movement)
            {
                actionButtons[i].interactable = false;
            }
            i++;
        }
        //TODO check for cumbersome weapons here
        survivalButtons[0].interactable = false;
    }

    public void ResetButtons()
    {
        foreach (Button b in actionButtons)
        {
            b.interactable = true;
        }
        foreach (Button b in survivalButtons)
        {
            b.interactable = true;
        }
        foreach (Button b in attackButtons)
        {
            b.interactable = true;
        }
    }

    private void HoverDesc(Button b, bool check) {
        if (check)
        {
            hover.SetActive(true);
            bool found = false;
            int i = 0;
            foreach (Button c in attackButtons)
            {
                if (b == c)
                {
                    found = true; 
                    break; 
                }
                i++;
            }
            if (found)
            {
                hoverBodyRight.gameObject.SetActive(true);
                hoverTitle.text = currentAttack[i].name;
                hoverIcon.sprite = currentAttack[i].icon;
                int wound;
                int acc;
                if (10-(mc.monster.toughness-currentAttack[i].strength - strength - 1) > luck + currentAttack[i].luck)
                {
                    wound = (10 - (mc.monster.toughness - currentAttack[i].strength - strength - 1)) * 10;
                }
                else
                {
                    wound = (luck + currentAttack[i].luck) * 10;
                }
                if (((10 - currentAttack[i].accuracy + accuracy + 1) * 10) >= 10){
                    acc = (10 - currentAttack[i].accuracy + accuracy + 1) * 10;
                }
                else 
                {
                    acc = 10;
                }
                hoverBody.text = "SPEED<br>ACCURACY<br>WOUND<br>CRIT";
                hoverBodyRight.text = (currentAttack[i].speed + speed) + " hits<br>" + acc + "%<br>" + wound + "%<br>" + ((luck + currentAttack[i].luck) * 10) + "%";
            }
            else
            {
                hoverBodyRight.gameObject.SetActive(false);
                i = 0;
                foreach (Button c in actionButtons)
                {
                    if (b == c)
                    {
                        found = true;
                        break;
                    }
                    i++;
                }
                if (found)
                {
                    hoverTitle.text = currentAction[i].name;
                    hoverIcon.sprite = currentAction[i].icon;
                    hoverBody.text = currentAction[i].desc;
                }
                else
                {
                    i = 0;
                    foreach (Button c in survivalButtons)
                    {
                        if (b == c)
                        {
                            found = true;
                            break;
                        }
                        i++;
                    }
                    if (i == 0)
                    {
                        hoverTitle.text = "MOVE";
                        hoverBody.text = "Move up to x squares away<br><br>";
                    }
                    else
                    {
                        hoverTitle.text = "Not Implemented";
                        hoverBody.text = "Does nothing";
                    }
                }
            }

        }
        else
        {
            hover.SetActive(false);
        }
    }
}
