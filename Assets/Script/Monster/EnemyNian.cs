using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNian : MonsterBase {

    public override void Start()
    {
        attack = 30;
        def = 15;
        hp = 400;
        exp = 50;
        moveSpeed = 1;        //巡逻速度
        time = 1;           //切换巡逻状态时间
        chase = 2f;        //追击速度
        missRate = 0.3f;
        minAttackDistance = 1.8f;
        maxAttackDistance = 5f;
        timeNormalAttack = 2.03f;        //普通攻击动画时间
        timeCrazyAttack = 2.433f;        //暴击动画时间2.433
        crazyAttackRate = 0.3f;         //暴击几率
        attactRate = 0.35f;        //攻击速率
        monsterType = MonsterType.Nian;//怪物类型
        body = transform.Find("Monster_NianElite/Object009/Object009_0");
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
