using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Shows every Character's properties by assigning to roles
skills(技能)
对地/对空/普通   消耗MP 基础威力   动画   影响范围


ch_Name
DEF (防御)
ch_Speed(角色速度，影响进度条变换，我的观点是操场跑圈用update每次刷新，定一个终点然后每次抓在终点的人)
if(某个人>400m)某个人-400m； push（某个人）；
解决的一个想法：在生成大小为300turn 的序列之前一直用update加算每个人速度*时间的值，到一个值了就清零压栈

ATK（攻击）
HIT（命中等级）
HP（生命）
MP(魔力)
AVD(闪避等级)
MGK（魔法攻击值）
RGS（魔法抗性）百分比
weaponRange(WRNG，武器范围)
MoveAbility（走多远，遇到地形高低减地势差减去）
JumpAbility（跳上/下多高）
career (职业，直接决定了能用什么武器和装备，重击的攻击范围形状，例如弓箭范围是前5格，重器范围是横着前三格，法术是周围6格)

实际命中率：max{(命中等级-闪避等级)*加成系数+基础命中率，基础命中率}
实际伤害：ATK-DEF

*/
public class CharacterProperties : MonoBehaviour
{
    public string ch_Name;
    public  int ch_Speed;
    public int ATK;
    public int DEF;
    public int HIT;
    public int HP;
    public int MP;
    public int AVD;
    public int MGK;
    public float RGS;
    public int WeaponRange;
    public int MoveAbility;
    public int JumpAbility;
    public int Career ;//职业，用switch决定他们的武器范围

    void Start()
    {

    }
    void Update()
    {

    }
}
