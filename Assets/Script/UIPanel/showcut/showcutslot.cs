using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShowCotType
{
    None,
    Drug,
    Skill
}
public enum DrugType
{
    None,
    HP,
    MP
}
public class showcutslot : MonoBehaviour {
    //快捷栏类型
    private  ShowCotType showCutType=ShowCotType.None;
    private Objectinfo Druginfo;
    //临时变量，当做参数使用，没有意义
    private showcutslot temp;
    private ShowCutPanel showcut;//快捷栏面板
    private Playerstatus player;
    //药品类型
    private DrugType drugType = DrugType.None;
    private int id;
    private bool isCold;//是否在cd中
    private SkillInfo Skillinfo;
    private Image cdmask;
    private float TimeCd;
    private PlayerAttack playerattack;
    public static bool isforbidden;//是否禁用
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
       KeyCode keycode;
    public KeyCode Keycode
    {
        get { return keycode; }
        set { keycode = value; }
    }

      Image icon;
	// Use this for initialization
	void Start () {
        playerattack = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerAttack>();
        cdmask = transform.Find("Cdmask").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        showcut = transform.parent.GetComponent<ShowCutPanel>();
        icon = transform.Find("Icon").GetComponent<Image>();

        cdmask.gameObject.SetActive(false);
        icon.gameObject.SetActive(false);
	}

    void Update()
    {
        if(isforbidden||playerattack.state==PlayerState.Death)
        {
            return;
        }
        if(Input.GetKeyDown(keycode))
        {
            if(showCutType==ShowCotType.Drug)
            {
                Debug.Log("使用药品");
                UseDrug(drugType);
            }
            else if(showCutType==ShowCotType.Skill)
            {
                Debug.Log("使用技能");
                if(!isCold)
                {
                    //释放单体，群体技能
                    if(Skillinfo.skilltype==skillType.MultiTarget || Skillinfo.skilltype==skillType.SingleTarget)
                    {
                        if(Skillinfo.mp<=player.remainMp)//判断蓝量是否足够
                        {
                            //TODO 切换鼠标样式

                            //蓝量足够切换状态
                            playerattack.state = PlayerState.SkillAttack;
                            //传递技能信息
                            playerattack.SelectTraget(Skillinfo, this);
                        }
                        return;
                    }
                    Useskill();
                }
            }
        }
        if(isCold)
        {
            TimeCd += Time.deltaTime;
            cdmask.fillAmount = 1-TimeCd / Skillinfo.cd;
            if(TimeCd>=Skillinfo.cd)
            {
                TimeCd = 0;
                isCold = false;
            }
        }
    }

    //使用增益或者增强技能
    public bool Useskill()
    {

        //判断蓝量是否足够，足够扣除蓝量
        bool issucces = player.JudgeSkillMp(Skillinfo.mp);
        if (issucces)//技能使用成功
        {
            isCold = true;
            cdmask.gameObject.SetActive(true);
            cdmask.fillAmount = 1;
            //如果是增益或者buff技能，调用使用技能函数，如果是单体群体进入cd
            if(Skillinfo.skilltype==skillType.Passive||Skillinfo.skilltype==skillType.Buff)
            {
                playerattack.UseSkill(Skillinfo);
            }
        }
        else
        {
            Debug.Log("蓝量不足");
        }
        return issucces;
    }
	//设置技能图片
    public void SetSkillIcon(int id)
    {
        if(showcut.JudgeShowCutSlot(id,out temp))
        {
            this.id = id;
            //显示图片
            icon.gameObject.SetActive(true);
            Skillinfo = SkillInfoList.Instance.GetskillByid(id);
            icon.sprite = Resources.Load("SkillIco/" + Skillinfo.iconame, typeof(Sprite)) as Sprite;
            showCutType = ShowCotType.Skill;
        }
        else
        {
            Debug.Log("已经有了快捷方式");
        }

    }
    //设置药品图片
    public void SetDrugIcon(int id)
    {
        if(showcut.JudgeShowCutSlot(id,out temp))
        {
            Druginfo = Objectinfolist.Instance.GetObjectifobyId(id);
            if (Druginfo.objectType == ObjectType.Drug)
            {
                this.id = id;
                icon.gameObject.SetActive(true);
                icon.sprite = Resources.Load("Icon/" + Druginfo.iconame, typeof(Sprite)) as Sprite;
                showCutType = ShowCotType.Drug;
                if (Druginfo.hp > 0)
                {
                    drugType = DrugType.HP;
                }
                else if (Druginfo.mp > 0)
                {
                    drugType = DrugType.MP;
                }
            }
        }
        else
        {
            Debug.Log("已经有了快捷方式");
        }
    }
    //使用药品
    void UseDrug(DrugType drug)
    {
        //是否可用使用药品
        if(player.JudgeIsCanUseDrug(drug))
        {
            int num = BagPanel.Instance.UseDrug(id);
            if(num>0)
            {
                player.AddProperty(Druginfo.hp, Druginfo.mp);
                //BagPanel.Instance.Bagitemdic[id] -= 1;
            }
                //临界值
            else if(num==0)
            {
                player.AddProperty(Druginfo.hp, Druginfo.mp);
                clear();
            }
        }
        else
        {
            Debug.Log("hp或mp是满的");
        }
    }
    //清空快捷栏数据
    public void clear()
    {
        icon.gameObject.SetActive(false);
        showCutType = ShowCotType.None;
        Druginfo = null;
        Skillinfo = null;
        drugType = DrugType.None;
        id = 0;
    }
}
