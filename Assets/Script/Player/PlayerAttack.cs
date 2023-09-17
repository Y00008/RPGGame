using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//人物状态
public enum PlayerState
{
    ControlWalk,
    NormalAttack,
    SkillAttack,
    Death
}
public enum Attackstate
{
    Idle,
    Moving,
    Attack
}
public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public PlayerState state = PlayerState.ControlWalk;
    public Attackstate attackstate = Attackstate.Idle;
    public int aniNow = 2;
    public int aniIdle = 1;//站立动画
    public int aniNormalAttack = 2;//普通攻击
    public float timeNormalAttack = 1.267f;//普通攻击所需的时间
    public float timeNow = 0;//计时器
    private float attackRate = 0.7f;//攻击速度
    public Transform attackNormalTarget;
    public float mindistance = 1.5f;
    private PlayerMove playermove;
    private bool isDamage;
    private Playerstatus playerstatus;
    private SkillInfo skillInfo;//单体群体技能
    private showcutslot showcutslot;//快捷栏

    private Image missImage;
    private float misstime = 1f;//闪避图片显示时间
    private Transform body;
    private Color normalColor;
    private float missrate = 0.3f;
    private int PassiveSkill = 4;
    private Dictionary<string, string> effectInfoDict = new Dictionary<string, string>();
    private Transform deadcanvas;


    // Use this for initialization
    void Start()
    {
        deadcanvas = GameObject.FindGameObjectWithTag(Tags.GameOverCanvas).transform;
        missImage = transform.Find("PlayerCanvas/miss").GetComponent<Image>();
        body = transform.Find("Body");
        playerstatus = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        animator = this.GetComponent<Animator>();
        playermove = this.GetComponent<PlayerMove>();

        missImage.gameObject.SetActive(false);
        deadcanvas.gameObject.SetActive(false);
        normalColor = body.GetComponent<Renderer>().material.color;
        Readinfo();
    }

    // Update is called once per frame
    void Update()
    {
        if(state==PlayerState.Death)
        {
            deadcanvas.gameObject.SetActive(true);
        }
        if (Input.GetMouseButtonDown(0) && state != PlayerState.Death && state != PlayerState.SkillAttack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            //点击怪物
            if (Physics.Raycast(ray, out hitinfo) && hitinfo.collider.tag.Equals(Tags.monster))
            {
                attackNormalTarget = hitinfo.transform;
                state = PlayerState.NormalAttack;
                timeNow = 0;
            }
            else
            {
                attackNormalTarget = null;
                state = PlayerState.ControlWalk;
            }
        }
        switch (state)
        {
            case PlayerState.ControlWalk:
                break;
            case PlayerState.NormalAttack:
                NormalAttack();
                break;
            case PlayerState.SkillAttack:
                LockTarrget();
                break;
            case PlayerState.Death:
                break;
        }
        if (missImage.gameObject.activeInHierarchy)
        {
            misstime -= Time.deltaTime;
            if (misstime <= 0)
            {
                missImage.gameObject.SetActive(false);
            }
        }
    }

    private void NormalAttack()
    {
        if (attackNormalTarget != null)
        {
            //攻击目标和主角的距离
            float distance = Vector3.Distance(attackNormalTarget.position, transform.position);
            //在攻击范围内
            if (distance <= mindistance)
            {
                transform.LookAt(attackNormalTarget);//看向目标
                attackstate = Attackstate.Attack;
                animator.SetInteger("Player", aniNow);
                timeNow += Time.deltaTime;
                if (timeNow > timeNormalAttack)
                {
                    aniNow = aniIdle;
                    //播放特效
                    if (timeNow > 0.6f)
                    {
                        //是否造成了伤害
                        if (!isDamage)
                        {
                            //计算伤害
                            bool isdead = attackNormalTarget.GetComponent<MonsterBase>().TakeDamage(GetAttack());
                            isDamage = true;
                            if (isdead)
                            {
                                attackNormalTarget = null;
                                attackstate = Attackstate.Idle;
                            }
                        }
                    }
                }
                if (timeNow > (1 / attackRate))
                {
                    timeNow = 0;
                    aniNow = aniNormalAttack;
                    isDamage = false;
                }
            }
            else
            {
                playermove.SimpleMove(attackNormalTarget.position);
                attackstate = Attackstate.Moving;
            }
        }
        else//目标丢失
        {
            state = PlayerState.ControlWalk;
        }
    }

    //计算攻击
    int GetAttack()
    {
        return (int)((playerstatus.attack + playerstatus.attackEquip + playerstatus.attackPlus));
    }

    //伤害计算、
    public void TakeDamage(float attack)
    {
        if (state == PlayerState.Death)
        {
            return;
        }
        //计算伤害
        float def = (int)(playerstatus.def + playerstatus.defEquip + playerstatus.defPlus);
        float damageValue = attack - def;
        if (damageValue <= 0)
        {
            damageValue = 10;
        }
        float value = Random.Range(0f, 1f);
        if (value < missrate)//miss
        {
            missImage.gameObject.SetActive(true);
        }
        else//没闪避，计算伤害
        {
            //扣血
            playerstatus.remainHp -= damageValue;
            //变红
            StartCoroutine(showred());
            if (playerstatus.remainHp <= 0)//死亡
            {
                playerstatus.remainHp = 0;

                state = PlayerState.Death;
                //死亡
                animator.SetTrigger("Dead");
                //StartCoroutine(Dead());

            }
        }
        //更新血条
        HeadPanel.Instance.Updateshowinfo();
    }
    //死亡
    IEnumerator Dead()
    {

        yield return new WaitForSeconds(1.2f);
        //暂停时间
        Time.timeScale = 0;
    }
    IEnumerator showred()
    {
        body.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        body.GetComponent<Renderer>().material.color = normalColor;
    }

    //使用技能
    public void UseSkill(SkillInfo skillinfo)
    {
        switch (skillinfo.skilltype)
        {
            //增强
            case skillType.Buff:
                HeadPanel.Instance.Updateshowinfo();
                StartCoroutine(UseBuffSkill(skillinfo));
                break;
            //增益
            case skillType.Passive:
                StartCoroutine(UsePassiveSkill(skillinfo));
                break;
        }
    }

    //增益技能
    IEnumerator UsePassiveSkill(SkillInfo skillinfo)
    {
        state = PlayerState.SkillAttack;
        yield return new WaitForSeconds(0.5f);
        //yield return new WaitForSeconds(skillinfo.anitime);
        animator.SetInteger("Player", aniIdle);
        state = PlayerState.ControlWalk;
        int hp = 0, mp = 0;
        switch (skillinfo.addproperty)
        {
            case addproperty.HP:
                hp = skillinfo.applyvalue;
                break;
            case addproperty.MP:
                mp = skillinfo.applyvalue;
                break;
        }
        //恢复，更新血条和经验条
        playerstatus.AddProperty(hp, mp);
        // 播放特效
        effect(skillinfo.name);
    }

    //播放特效
    void effect(string name)
    {
        string path;
        effectInfoDict.TryGetValue(name, out path);
        GameObject effect = GameObject.Instantiate(Resources.Load<GameObject>(path), transform.position, Quaternion.identity);
        effect.transform.SetParent(transform);
        effect.transform.localPosition = new Vector3(0, 2, -0.5f);
        effect.transform.Rotate(90, 0, 0);
        effect.transform.localScale = new Vector3(3, 3, 3);
        GameObject.Destroy(effect, 4);
    }
    //buff技能
    IEnumerator UseBuffSkill(SkillInfo skillinfo)
    {
        state = PlayerState.SkillAttack;
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(skillinfo.anitime);
        animator.SetInteger("Player", aniIdle);
        state = PlayerState.ControlWalk;

        // 播放特效
        //effect(skillinfo.name);
        //增加buff效果
        switch (skillinfo.addproperty)
        {
            case addproperty.Attack:
                playerstatus.attack *= (skillinfo.applyvalue / 100);
                break;
            case addproperty.Def:
                playerstatus.def *= (skillinfo.applyvalue / 100);
                break;
            case addproperty.Speed:
                playerstatus.speed *= (skillinfo.applyvalue / 100);
                break;
            case addproperty.AttackSpeed:
                attackRate *= (skillinfo.applyvalue / 100);
                break;
        }
        yield return new WaitForSeconds(skillinfo.applytime);
        //  移除buff效果
        switch (skillinfo.addproperty)
        {
            case addproperty.Attack:
                playerstatus.attack /= (skillinfo.applyvalue / 100);
                break;
            case addproperty.Def:
                playerstatus.def /= (skillinfo.applyvalue / 100);
                break;
            case addproperty.Speed:
                playerstatus.speed /= (skillinfo.applyvalue / 100);
                break;
            case addproperty.AttackSpeed:
                attackRate /= (skillinfo.applyvalue / 100);
                break;
        }
    }

    //群体单体技能选取目标做准备
    public void SelectTraget(SkillInfo skillinfo, showcutslot showcutslot)
    {
        this.skillInfo = skillinfo;
        this.showcutslot = showcutslot;
    }

    //锁定目标
    void LockTarrget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;
            bool isColider = Physics.Raycast(ray, out hitinfo);
            //点击的是怪物,单体技能
            if (isColider && hitinfo.collider.tag.Equals(Tags.monster))
            {
                //距离
                JudgeDiatance(hitinfo);
            }
            //群体技能
            else if (isColider && skillInfo.skilltype == skillType.MultiTarget)
            {
                JudgeDiatance(hitinfo);
            }
            else//点击地面，取消实发
            {

                //切换鼠标

                state = PlayerState.ControlWalk;
                attackstate = Attackstate.Idle;
            }
        }
    }


    //判断距离
    void JudgeDiatance(RaycastHit hitinfo)
    {
        transform.LookAt(hitinfo.transform.position);
        float distance = Vector3.Distance(hitinfo.point, transform.position);
        //超出了技能范围
        if (distance > skillInfo.distansce)
        {
            Debug.Log("超出距离");
            //切换鼠标

            state = PlayerState.ControlWalk;
            attackstate = Attackstate.Idle;
        }
        else//在攻击范围，释放技能
        {
            //扣蓝
            if (showcutslot.Useskill())
            {
                HeadPanel.Instance.Updateshowinfo();
                //处理单体，群体技能的释放
                StartCoroutine(TargetSkill(hitinfo));
            }
        }
    }
    //单体技能或者群体技能
    IEnumerator TargetSkill(RaycastHit hitinfo)
    {
        //播放动画
        animator.SetInteger("Player", skillInfo.aniname);
        //等待动画播放完
        yield return new WaitForSeconds(0.5f);
        animator.SetInteger("Player", aniIdle);
        state = PlayerState.ControlWalk;
        // 播放特效
        string path;
        effectInfoDict.TryGetValue(skillInfo.name, out path);
        GameObject effect=null;
        if (skillInfo.skilltype == skillType.SingleTarget)
        {
            effect = GameObject.Instantiate(Resources.Load<GameObject>(path), hitinfo.transform.position, Quaternion.identity);
            Debug.Log(skillInfo.name);
        }
        else if (skillInfo.skilltype == skillType.MultiTarget)
        {
            effect = GameObject.Instantiate(Resources.Load<GameObject>(path), hitinfo.point, Quaternion.identity);
        }
        Debug.Log(hitinfo.transform.position);
        yield return new WaitForSeconds(0.5f);
        //计算伤害
        //单体技能
        if (skillInfo.skilltype == skillType.SingleTarget)
        {
            hitinfo.transform.GetComponent<MonsterBase>().TakeDamage(skillInfo.applyvalue);
            yield return new WaitForSeconds(1f);
            GameObject.Destroy(effect);
        }
        //群体技能
        else if (skillInfo.skilltype == skillType.MultiTarget)
        {
            Collider[] monster = Physics.OverlapSphere(effect.transform.position, 10);
            foreach (Collider item in monster)
            {
                if (item.tag.Equals(Tags.monster))
                {
                    item.gameObject.GetComponent<MonsterBase>().TakeDamage(skillInfo.applyvalue);
                }
            }  
            yield return new WaitForSeconds(2f);
            GameObject.Destroy(effect);
        }
        //TODO 鼠标样式切回
    }

    //技能表解析
    void Readinfo()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/EffectInfo");
        string[] effectList = ta.text.Split('\n');
        foreach (string item in effectList)
        {
            effectList = item.Split(',');
            effectInfoDict.Add(effectList[0], effectList[1]);
        }
    }

}
