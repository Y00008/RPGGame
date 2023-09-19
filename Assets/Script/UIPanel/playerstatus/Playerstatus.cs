using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerstatus : MonoBehaviour {
    public string heroName = "默认昵称";
    public int level = 1;
    public float attack = 30;
    public float attackPlus = 0;
    public float attackEquip = 0;
    public float def = 30;
    public float defPlus = 0;
    public float defEquip = 0;
    public float speed = 30;
    public float speedPlus = 0;
    public float speedEquip = 0;
    public float remainHp;
    public float maxHp = 100;
    public float remainMp;
    public float maxMp = 100;
    public float exp = 0;
    public float remainpoint=10;
    public int gold = 1000;
    public heroType herotype = heroType.Magician;
    public int killbear = 0;
    public int killnian = 0;//击杀蝙蝠数量
    public int killLowrie = 0;//击杀狐狸数量
    public int killBearTaskNum = 0;//接取击杀狼的任务数量
    public int killNianTaskNum = 0;
    public int killLowrieTaskNum = 0;
    void Awake()
    {
        remainHp = maxHp;
        remainMp = maxMp;
        heroName = PlayerPrefs.GetString("name");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetExp(300);
        }
    }
    //判断能否购买
    public bool Getcoin(int gold)
    {
        if(this.gold>=gold)
        {
            this.gold -= gold;
            return true;
        }
        return false;
    }
    //判断能否使用药品
    public bool JudgeIsCanUseDrug(DrugType drug)
    {
        //能否使用血药
        if(drug==DrugType.HP)
        {
            if (remainHp < maxHp)
            {
                return true;
            }
        }
         //萌发使用蓝药
        else if (drug == DrugType.MP)
        {
            if (remainMp < maxMp)
            {
                return true;
            }
        }
        return false;
    }


    //金币是否足够
    public bool JudegeGold(int num)
    {
        if(num<=gold)
        {
            gold -= num;
            return true;
        }
        return false;
    }
    //蓝量是否足够
    public bool JudgeSkillMp(int mp)
    {
        if(remainMp>=mp)
        {
            remainMp -= mp;
            return true;
        }
        return false;
    }
    //使用药品增加属性
    public void AddProperty(int hp,int mp)
    {
        remainHp += hp;
        remainMp += mp;
        if(remainHp>maxHp)
        {
            remainHp = maxHp;
        }
        if (remainMp > maxMp)
        {
            remainMp = maxMp;
        }
        HeadPanel.Instance.Updateshowinfo();
    }

    //处理经验
    public void GetExp(float exp)
    {
        this.exp += exp;
        int levelexp = 100 + level * 30;
        while(this.exp>=levelexp)
        {
            //提升等级
            level++;
            //减少经验
            this.exp -= levelexp;
            //刷新升级所需经验
            levelexp = 100 + level * 30;
            GameObject effect= GameObject.Instantiate(Resources.Load<GameObject>("Effect/FX_Healing_Cirle_02"), transform.position, Quaternion.identity);
            effect.transform.SetParent(this.gameObject.transform);
            effect.transform.localPosition = new Vector3(0, 0.5f, 0);
            GameObject.Destroy(effect, 2);
            remainpoint += 10;

        }
        HeadPanel.Instance.Updateshowinfo();
        HeadPanel.Instance.SetExp(this.exp / levelexp);
    }

    //增加金币
    public void AddGold(int num)
    {
        gold += num;
    }
}
public enum heroType
{
    Swordman,
    Magician
}