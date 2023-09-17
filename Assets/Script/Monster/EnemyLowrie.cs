using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLowrie : MonsterBase {

    public override void Start()
    {
        attack = 20;
        def = 10;
        hp = 200;
        exp = 30;
        moveSpeed = 1;        //巡逻速度
        time = 1;           //切换巡逻状态时间
        chase = 2f;        //追击速度
        missRate = 0.2f;
        minAttackDistance = 1.5f;
        maxAttackDistance = 5f;
        timeNormalAttack = 1.467f;        //普通攻击动画时间
        timeCrazyAttack = 2.533f;        //暴击动画时间
        crazyAttackRate = 0.8f;         //暴击几率
        attactRate = 0.37f;        //攻击速率
        monsterType = MonsterType.Lowrie;//怪物类型
        body = transform.Find("Monster_FoxElite/hl_jy/hl_jy_0");
        base.Start();

    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    //巡逻
    public override void Patrol()
    {
        base.Patrol();
    }

    ////随机巡逻状态
    public override void RandState()
    {
        base.RandState();
    }

    ////收到伤害
    public override bool TakeDamage(int attack)
    {
        return base.TakeDamage(attack);
    }


    ////任务增加击杀数量
    public override void TaskAddNum()
    {
        base.TaskAddNum();
    }
    ////受伤颜色变红
    public override IEnumerator Showred()
    {
        return base.Showred();
    }

    ////处理怪物攻击
    public override void AutoAttack()
    {
        base.AutoAttack();
    }

    ////随机攻击状态
    public override void RandAttcak()
    {
        base.RandAttcak();
    }
}
