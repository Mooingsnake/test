using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Shows every Character's properties by assigning to roles
skills(技能)
对地/对空/普通   消耗MP 基础威力   动画   影响范围

DEF (防御)
speed（速度）
ATK（攻击）
HIT（命中等级）
HP（生命）
MP(魔力)
AVD(闪避等级)
MGK（魔法攻击值）
RGS（魔法抗性）
weaponRange(WRNG，武器范围)
MoveAbility（走多远，遇到地形高低减地势差减去）
JumpAbility（跳上/下多高）


实际命中率：max{(命中等级-闪避等级)*加成系数+基础命中率，基础命中率}
实际伤害：ATK-DEF

*/
public class CharacterProperties : MonoBehaviour
{
    public int ch_Speed;
    public int ATK;
    public int DEF;
    public int HIT;
    public int HP;
    public int MP;
    public int AVD;
    public int MGK;
    public float RGS;
    public int weaponRange;
    public int MoveAbility;
    public int JumpAbility;

    void Start()
    {

    }
    void Update()
    {

    }
}
