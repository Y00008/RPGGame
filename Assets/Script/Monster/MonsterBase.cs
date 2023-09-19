using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MonsterState
{
    Idle,
    Walk,
    Attack,
    Dead
}
public class MonsterBase : MonoBehaviour {
    protected Playerstatus playerstatus;
    protected CharacterController character;
    protected Transform body;//材质球所在位置
    protected Color normalColor;//怪物正常颜色
    protected Transform attackTarget;//攻击目标
    protected Image hpFill;
    protected Image missImage;
    protected Animator animator;
    protected float timeNowAttack = 0;//播放动画时间
    protected int aniNowAttack = 3;//当前攻击播放动画,默认赋值为普通攻击
    protected int aniCrazyAttack = 4;//暴击动画
    protected int aniNormalAttack = 3;//普通攻击动画
    protected int aniIdle = 2;//站立动画
    protected int aniWalk = 1;//行走动画
    protected int aniNow = 1;//默认动画
    protected float maxHp;
    protected MonsterState state = MonsterState.Idle;
    protected float missTime = 1.5f;
    protected float timenow = 0;//轮换计时
    protected float distancePatro=10;//巡逻范围
    protected bool isdamage;//控制一次攻击制造成一次伤害
    public CreateMonsterBase createmonsterbase;
 

    protected float time;//切换巡逻状态时间
    protected float attack;
    protected float def;
    protected float hp;
    protected int exp;
    //巡逻速度
    protected float moveSpeed;
    //追击速度
    protected float chase;
    protected float missRate;
    protected float minAttackDistance;
    protected float maxAttackDistance;
    protected float timeNormalAttack;    //普通攻击动画时间
    protected float timeCrazyAttack;    //暴击动画时间
    protected float crazyAttackRate;//暴击几率
    protected float attactRate;    //攻击速率
    protected    MonsterType monsterType;//怪物类型

    // Use this for initialization
     public virtual void Start()
    {
        playerstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
       // body = transform.Find("Monster_FoxElite/hl_jy/hl_jy_0");
        character = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
        hpFill = transform.Find("MonsterCanvas/HpBg/HpFill").GetComponent<Image>();
        missImage = transform.Find("MonsterCanvas/miss").GetComponent<Image>();

        missImage.gameObject.SetActive(false);
        //保存怪物颜色
        normalColor = body.GetComponent<Renderer>().material.color;
        maxHp = hp;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        switch (state)
        {
            case MonsterState.Idle:
            case MonsterState.Walk:
                Patrol();
                break;
            case MonsterState.Attack:
                AutoAttack();
                break;
            case MonsterState.Dead:
                break;
            default:
                break;
        }
        //开始计时
        if (missImage.gameObject.activeInHierarchy)
        {
            missTime -= Time.deltaTime;
            if (missTime <= 0)
            {
                //时间重置
                missTime = 1.5f;
                //隐藏图片
                missImage.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(30);
        }
    }
    //巡逻  
   public  virtual void Patrol()
    {
        animator.SetInteger("Monster", aniNow);
        if (aniNow == aniWalk)
        {
            character.SimpleMove(transform.forward * moveSpeed);
            if(Vector3.Distance(transform.localPosition,Vector3.zero)>distancePatro)
            {
                transform.LookAt(transform.parent);
            }
        }
        //大于切换状态时间，切换状态
        timenow += Time.deltaTime;
        if (timenow >= time)
        {
            timenow = 0;
            RandState();
        }
    }
    //随机巡逻状态
   public virtual void RandState()
    {
        int value = Random.Range(0, 2);
        if (value == 0)
        {
            aniNow = aniIdle;
        }
        else
        {
            if (aniNow != aniWalk)
            {
                transform.Rotate(transform.up * Random.Range(0, 360) * Time.deltaTime * 15);
            }
            aniNow = aniWalk;
        }
    }
    //收到伤害
    public virtual bool TakeDamage(int attack)
    {
        if (state == MonsterState.Dead)
        {
            return true;
        }
        float value = Random.Range(0f, 1f);
        if (value < missRate)//miss
        {
            missImage.gameObject.SetActive(true);
            return false;
        }
        else
        {
            hp -= attack;//扣血
            hpFill.fillAmount = hp / maxHp;//血条减少
            StartCoroutine(Showred());//开启协程
            //锁定攻击目标，主角
            attackTarget = GameObject.FindGameObjectWithTag(Tags.player).transform;
            //切换状态为攻击状态
            state = MonsterState.Attack;
            if (hp <= 0)//死亡
            {
                //切换状态
                state = MonsterState.Dead;
                //播动画
                animator.SetTrigger("Dead");
                //影藏血条
                hpFill.transform.parent.gameObject.SetActive(false);
                playerstatus.GetExp(exp);
                //增加任务击杀数量
                TaskAddNum();
                //销毁
                GameObject.Destroy(this.gameObject, 2);
                createmonsterbase.NowMonsterNum--;
                return true;
            }
            return false;
        }
    }

    //任务增加击杀数量
    public virtual void TaskAddNum()
    {
        switch (monsterType)
        {
            case MonsterType.Bear:
                //接取了击杀狼的任务
                if (playerstatus.killBearTaskNum > 0)
                {
                    playerstatus.killbear++;
                }
                break;
            case MonsterType.Lowrie:
                //接取了击杀小狐狸的任务
                if (playerstatus.killLowrieTaskNum > 0)
                {
                    playerstatus.killLowrie++;
                }
                break;
            case MonsterType.Nian:
                //接取了击杀蝙蝠的任务
                if (playerstatus.killNianTaskNum > 0)
                {
                    playerstatus.killnian++;
                }
                break;
            default:
                break;
        }
    }
    //受伤颜色变红
    public virtual IEnumerator Showred()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        body.GetComponent<Renderer>().material.color = normalColor;
    }

    //处理怪物攻击
    public virtual void AutoAttack()
    {
        if (attackTarget != null)
        {
            if(attackTarget.GetComponent<PlayerAttack>().state==PlayerState.Death)
            {
                attackTarget = null;
                state = MonsterState.Idle;
                return;
            }
            //获取主角和怪物之间距离
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            //大于最大攻击距离
            if (distance >= maxAttackDistance)
            {
                hp = maxHp;
                hpFill.fillAmount = hp / maxHp;
                state = MonsterState.Idle;
            }
            //在攻击范围内
            else if (distance <= minAttackDistance)
            {
                transform.LookAt(attackTarget);
                //播放攻击动画
                animator.SetInteger("Monster", aniNowAttack);
                timeNowAttack += Time.deltaTime;
                if (aniNowAttack == aniCrazyAttack)//攻击是暴击
                {
                    //动画播放到一定时间计算伤害
                    if (timeNowAttack > 1f)
                    {
                        if (!isdamage)
                        {
                            //计算伤害
                            attackTarget.GetComponent<PlayerAttack>().TakeDamage(attack);
                            isdamage = true;
                        }
                    }
                    //动画播放完成
                    if (timeNowAttack > timeCrazyAttack)
                    {
                        //攻击完切回站立，直到下次攻击开始
                        aniNowAttack = aniIdle;
                    }
                }
                else if (aniNowAttack == aniNormalAttack)//普通攻击
                {
                    if (timeNowAttack>1f)
                    {
                        if (!isdamage)
                        {
                            //计算伤害
                            attackTarget.GetComponent<PlayerAttack>().TakeDamage(attack * 2);
                            isdamage = true;
                        }
                    }
                    if (timeNowAttack > timeNormalAttack)
                    {

                        aniNowAttack = aniIdle;
                    }
                }
                //时间大于一次攻击的时间，即一次攻击的时间 ，1/攻速
                if (timeNowAttack > (1 / attactRate))
                {
                    timeNowAttack = 0;
                    isdamage = false;
                    RandAttcak();
                }
            }
            //在追击范围内，即最小攻击范围和最大攻击范围之间，追击
            else
            {
                //看向主角
                transform.LookAt(attackTarget);
                //朝着人物移动
                character.SimpleMove(transform.forward * chase);
                //播放移动动画
                animator.SetInteger("Monster", aniWalk);
            }
        }
        else//主角被打死或者超出攻击范围,血量回满，继续巡逻
        {
            hp = maxHp;
            hpFill.fillAmount = hp / maxHp;
            state = MonsterState.Idle;
        }
    }
    //随机攻击状态
    public virtual void RandAttcak()
    {
        float value = Random.Range(0f, 1f);
        //暴击
        if (value < crazyAttackRate)
        {
            aniNowAttack = aniCrazyAttack;
        }
        else//普通攻击
        {
            aniNowAttack = aniNormalAttack;
        }
    }
}
