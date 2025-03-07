using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WoundEnum;

namespace WoundEnum
{
    public enum Reflex
    {
        none,
        Reflex,
        Failure,
        Wound
    }

    public enum CritReward
    {
        none,
        Minus1Movement,
        Minus1Accuracy,
        AllGainPlus3InsanityIfNotDeaf,
        BasicResource,
        MonsterResource,
        InstantDeathOn10,
        AllStandIfNotDeaf,
        Spend1SurvivalToGain1PermanentStrength,
        Gain3Insanity,
        MonsterResource1,
        MonsterResource2,
        MonsterResource3,
        MonsterResource4,
        InsaneGains1StrengthToken,
        KnockedDown,
        Discard1Mood,
        DelayedDeathOn10OrKnockedDown,
        Minus1Toughness

    }
    //MR1 is Lion Testes
    //MR2 is Lion Claw
    //MR3 is Shimmering Mane
    //MR4 is Lion Tail

    public enum ReflexAction
    {
        none,
        FullMoveForward,
        BasicAction,
        JumpBack1Space,
        AttackerSuffers1BrainDamage,
        BasicActionPlus2Damage,
        On4Plus,
        BrainDamageXMonsterLevel,
        KnockedDown,
        PriorityTarget,
        On3UnderstandingGainSurvival
    }

    public enum PersistentInjury
    {
        none,
        LostDingDong,
        LostHand,
        RupturedTendon,
        BrokenFoot,
        Dazed,
        OrganTrail,
        NoJaw

    }
}


[CreateAssetMenu]
public class ScriptableWound : ScriptableObject
{
    public bool trap;
    public bool impervious;
    public Reflex reflex;
    public List<ReflexAction> reflexAction;
    public string desc;
    public bool crit;
    public string critDesc;
    public List<CritReward> critReward;
    
    public PersistentInjury injury;
    public string injuryDescription;
}
