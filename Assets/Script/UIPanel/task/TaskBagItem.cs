using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//任务完成进度
public enum TaskGropress
{
    Finish,
    NoFinish
}
public class TaskBagItem : MonoBehaviour {
      Text des;
      Text gropress;
      Button cancel;
      Image finish;
      int id;
      taskinfo info;
    public int Id
    {
        get { return id; }
        set { id = value; }
    }
      TaskItem task;
      Playerstatus player;
      TaskGropress taskgropress = TaskGropress.NoFinish;
	// Use this for initialization
	void Awake () {
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        des = transform.Find("taskdes").GetComponent<Text>();
        gropress = transform.Find("taskprogress").GetComponent<Text>();
        cancel = transform.Find("cancel").GetComponent<Button>();
        finish = transform.Find("finish").GetComponent<Image>();

        cancel.onClick.AddListener(OnClickCancel);
	}

      void OnClickCancel()
    {
        //调用任务背包的放弃任务函数
        TaskBagPanel.Instance.GiveUp(id);
        GameObject.Destroy(this.gameObject);
    }
	//设置信息
    public void Setinfo(int id)
    {
        this.id = id;
        //得到id对应的接取字典中的任务
        task = TaskPanel.Instance.GetAcceptTaskById(id);
        //设置信息
        des.text ="任务: "+ task.Taskinfo.des;
        finish.gameObject.SetActive(false);
        ShowState(taskgropress);
    }

    //显示任务进度
    public void  ShowState(TaskGropress taskgropress)
    {
        switch (taskgropress)
        {
            case TaskGropress.Finish:
                gropress.gameObject.SetActive(false);
                finish.gameObject.SetActive(true);
                cancel.gameObject.SetActive(false);
                break;
            case TaskGropress.NoFinish:
                finish.gameObject.SetActive(false);
                info = TaskinfoList.Instance.GetTaskById(id);
                if(info.monstertype==MonsterType.Bear)
                {
                    gropress.text = "完成进度:  " + player.killbear + "/" + info.killcount;
                }
                else if (info.monstertype == MonsterType.Nian)
                {
                    gropress.text = "完成进度:  " + player.killnian + "/" + info.killcount;
                }
                else if (info.monstertype == MonsterType.Lowrie)
                {
                    gropress.text = "完成进度:  " + player.killLowrie + "/" + info.killcount;
                }
                break;
        }
    }

    //判断是否完成任务
    public bool JudegeIsFinish()
    {
        switch (info.monstertype)
        {
            case MonsterType.Nian:
                //击杀数量大于任务完成
                if (player.killnian >= info.killcount)
                {
                    ////击杀数量清零
                    //player.killWolf = 0;
                    //改变任务状态
                    taskgropress = TaskGropress.Finish;
                    //更新显示界面
                    ShowState(taskgropress);
                    return true;
                }
                break;
            case MonsterType.Lowrie:
                //击杀数量大于任务完成
                if (player.killLowrie >= info.killcount)
                {
                    ////击杀数量清零
                    //player.killLowrie = 0;
                    taskgropress = TaskGropress.Finish;
                    ShowState(taskgropress);
                    return true;
                }
                break;
            case MonsterType.Bear:
                //击杀数量大于任务完成
                if (player.killbear >= info.killcount)
                {
                    ////击杀数量清零
                    //player.killBat = 0;
                    taskgropress = TaskGropress.Finish;
                    ShowState(taskgropress);
                    return true;
                }
                break;
        }
        ShowState(taskgropress);
        return false;
    }
    ////更新任务进度
    //public void Updatestate()
    //{
    //    switch (task.State)
    //    {
    //        case taskState.Notreceived:
    //            break;
    //        case taskState.Access:
    //            ShowState(taskgropress);
    //            break;
    //        case taskState.Complete:
    //            taskgropress = TaskGropress.Finish;
    //            ShowState(taskgropress);
    //            break;
    //        case taskState.Final:
    //            break;
    //    }
    //}
	// Update is called once per frame
	void Update () {
		
	}
}
