using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfoList : MonoBehaviour {

      static SkillInfoList instance;

    public static SkillInfoList Instance
    {
        get { return SkillInfoList.instance; }
        set { SkillInfoList.instance = value; }
    }
      Dictionary<int, SkillInfo> skilldic = new Dictionary<int, SkillInfo>();
	// Use this for initialization
	void Awake () {
        instance = this;
        Readinfo();
	}
    //开放接口，通过id获取数据
    public SkillInfo GetskillByid(int id)
    {
        SkillInfo info = null;
        skilldic.TryGetValue(id,out info);
        return info;
    }
    void Readinfo()
{
 	TextAsset ta=Resources.Load<TextAsset>("TextInfo/SkillInfoList");
    string[] skillarray= ta.text.Split('\n');
    for (int i = 0; i < skillarray.Length; i++)
    {
        string[] skill = skillarray[i].Split(',');
        SkillInfo info = new SkillInfo();
        info.id = int.Parse(skill[0]);
        info.name = skill[1];
        info.iconame = skill[2];
        info.des = skill[3];
        info.skilltype = (skillType)System.Enum.Parse(typeof(skillType), skill[4]);
        info.addproperty = (addproperty)System.Enum.Parse(typeof(addproperty), skill[5]);
        info.applyvalue = int.Parse(skill[6]);
        info.applytime = int.Parse(skill[7]);
        info.mp = int.Parse(skill[8]);
        info.cd = int.Parse(skill[9]);
        info.applyrole = (applyrole)System.Enum.Parse(typeof(applyrole), skill[10]);
        info.level = int.Parse(skill[11]);
        info.releasetype = (releaseType)System.Enum.Parse(typeof(releaseType), skill[12]);
        info.distansce = float.Parse(skill[13]);
        info.effectname = skill[14];
        info.aniname =int.Parse(skill[15]);
        info.anitime = float.Parse(skill[16]);
        skilldic.Add(info.id,info);
    }
}
}
//技能类别（群体，增益，增强，单个目标)
public enum skillType
{
    SingleTarget,
    Buff,
    MultiTarget,
    Passive
}
//适用英雄
public enum addproperty
{
    Attack,
    Def,
    Speed,
    AttackSpeed,
    HP,
    MP
}
//释放类型(自己，敌人，位置)
public enum releaseType
{
    Enemy,
    Self,
    Position,
}
public enum applyrole
{
    Swordman,
    Magician
}
public class SkillInfo
{
    public int id;
    public string name;
    public string iconame;
    public string des;
    public skillType skilltype;
    public addproperty addproperty;
    public int applyvalue;
    public int applytime;
    public int mp;
    public int cd;
    public applyrole applyrole;
    public int level;
    public releaseType releasetype;
    public float distansce;
    public string effectname;
    public int aniname;
    public float anitime;
}
