using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBear : MonsterBase {

    public override void Start()
    {
        attack = 50;
        def = 25;
        hp = 600;
        exp = 70;
        moveSpeed = 1;        //巡逻速度
        time = 1;           //切换巡逻状态时间
        chase = 2f;        //追击速度
        missRate = 0.3f;
        minAttackDistance = 1.7f;
        maxAttackDistance = 5f;
        timeNormalAttack = 1.7f;        //普通攻击动画时间
        timeCrazyAttack = 2.4f;        //暴击动画时间
        crazyAttackRate = 0.3f;         //暴击几率
        attactRate = 0.37f;        //攻击速率
        monsterType = MonsterType.Bear;//怪物类型
        body = transform.Find("Monster_Bear/Bip01/Bip01 Pelvis01/Monster_bear/Monster_bear_0");
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
