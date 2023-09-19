using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskinfoList : MonoBehaviour {
      Dictionary<int, taskinfo> taskinfodic = new Dictionary<int, taskinfo>();
      static TaskinfoList instance;

    public static TaskinfoList Instance
    {
        get { return TaskinfoList.instance; }
        set { TaskinfoList.instance = value; }
    }
	// Use this for initialization
	void Start () {
        instance = this;
        Readinfo();
        Debug.Log("解析完成");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //开放接口，让外部通过id访问信息
    public taskinfo GetTaskById(int id)
    {
        taskinfo info = null;
        taskinfodic.TryGetValue(id, out info);
        return info;
    }
    //解析任务信息
    void  Readinfo()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Task");
        string[] taskarray= ta.text.Split('\n');
        foreach (string item in taskarray)
        {
            string[] task = item.Split(',');
            taskinfo info = new taskinfo();
            info.id = int.Parse(task[0]);
            info.des = task[1];
            info.killcount = int.Parse(task[2]);
            info.monstertype = (MonsterType)System.Enum.Parse(typeof(MonsterType), task[3]);
            info.rewardicon = task[4];
            info.rewardtype = (RewardType)System.Enum.Parse(typeof(RewardType), task[5]);
            info.rewardcount = int.Parse(task[6]);
            info.rewarditemid = int.Parse(task[7]);
            taskinfodic.Add(info.id, info);
        }
    }
}
//怪物类型
public enum MonsterType
{
    Bear,
    Lowrie,
    Nian
}
//奖励道具类型
public enum RewardType
{
    Equip,
    Drug,
    Gold
}
public class taskinfo
{
    public int id;
    public string des;
    public int killcount;
    public MonsterType monstertype;
    public string rewardicon;
    public RewardType rewardtype;
    public int rewardcount;
    public int rewarditemid;
}