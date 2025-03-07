using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemEnum;

public class Action
{
    private Active currentActive;
    private ActiveCost currentCost;
    private CharacterController cc;
    private ScriptableItem currentItem;

    public Action(CharacterController cc)
    {
        this.cc = cc;
        currentActive = Active.none;

    }

    public bool StartAction(ScriptableItem item)
    {
        currentActive = item.active;
        currentCost = item.activeCost;
        currentItem = item;

        switch (currentActive)
        {
            case Active.foundingStone:
                return FoundingStone();
            default:
                return false;
        }
    }

    public void ResolveAction()
    {
        if (currentActive != Active.none)
        {
            switch (currentActive)
            {
                case Active.foundingStone:
                    ResolveFoundingStone();
                    break;
                default:
                    break;
            }
        }
        currentActive = Active.none;
        currentItem = null;
    }

    public bool FoundingStone()
    {
        (int, int) temp = cc.CurrentPos();
        cc.attack.Atk(99, temp.Item1, temp.Item2);
        return true;
    }

    public void ResolveFoundingStone()
    {
        cc.attack.Reset();
        Cost();
        cc.AttackMonster(null, 1, -100, 0, 100);
        cc.currentChara.inventory.Archive(currentItem);
    }


    public void Cost()
    {
        switch (currentCost)
        {
            case ActiveCost.action:
                cc.currentChara.canAction = false;
                cc.hotbar.ActionUsed();
                break;
            case ActiveCost.movement:
                cc.currentChara.canMove = false;
                cc.hotbar.MovementUsed();
                break;
            default:
                break;
        }
    }

    public void Reset()
    {
        cc.attack.Reset();
    }



}
