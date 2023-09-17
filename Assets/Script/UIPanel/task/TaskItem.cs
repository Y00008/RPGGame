using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum taskState
{
    Notreceived,//未接取
    Access,//接取未完成
    Complete,//完成未领取奖励
    Final//完成且领取奖励
}
public class TaskItem : MonoBehaviour {
      Text taskdesLabel;
      Text rewardNumLabel;
      Button asseptBtn;
      Button finishBtn;
      Image nofinish;
      Image rewadicon;
      taskinfo taskinfo;
      int id;
    public taskinfo Taskinfo
    {
        get { return taskinfo; }
        set { taskinfo = value; }
    }
      taskState state = taskState.Notreceived;

    public taskState State
    {
        get { return state; }
        set { state = value; }
    }
	// Use this for initialization
	void Awake () {
        taskdesLabel = transform.Find("taskdes").GetComponent<Text>();
        rewardNumLabel = transform.Find("RewardNum").GetComponent<Text>();
        asseptBtn = transform.Find("Accept").GetComponent<Button>();
        finishBtn = transform.Find("finish").GetComponent<Button>();
        nofinish = transform.Find("nofinish").GetComponent<Image>();
        rewadicon = transform.Find("icon").GetComponent<Image>();

        asseptBtn.onClick.AddListener(OnClickAsseptBtn);
        finishBtn.onClick.AddListener(OnClickFinishBtn);

	}
    //领取奖励
      void OnClickFinishBtn()
    {
        //发放奖励
        TaskPanel.Instance.Getreward(id);
        //删除任务
        GameObject.Destroy(this.gameObject); 
    }
    //点击接取任务
      void OnClickAsseptBtn()
    {
            //改变任务状态
        state = taskState.Access;
        DealState(state);
        //添加到接取任务字典中
        TaskPanel.Instance.PickTask(id, this);
    }

	
    //设置信息
    public void SetInfo(int id)
    {
        this.id = id;
        taskinfo = TaskinfoList.Instance.GetTaskById(id);
        taskdesLabel.text = taskinfo.des;
        rewardNumLabel.text = "x" + taskinfo.rewardcount;
        rewadicon.sprite = Resources.Load("Icon/" + taskinfo.rewardicon, typeof(Sprite)) as Sprite;
        DealState(state);
    }
    //处理不同状态的显示
    public void DealState(taskState state)
    {
        //根据不同的状态显示不同的界面和按钮
        switch (state)
        {
                //未接取状态，显示接取按钮
            case taskState.Notreceived:
                asseptBtn.gameObject.SetActive(true);
                finishBtn.gameObject.SetActive(false);
                nofinish.gameObject.SetActive(false);
                break;
            case taskState.Access:
                asseptBtn.gameObject.SetActive(false);
                finishBtn.gameObject.SetActive(false);
                nofinish.gameObject.SetActive(true);
                break;
            case taskState.Complete:
                asseptBtn.gameObject.SetActive(false);
                finishBtn.gameObject.SetActive(true);
                nofinish.gameObject.SetActive(false);
                break;
            case taskState.Final:

                break;
        }
    }

    
	// Update is called once per frame
	void Update () {
        //JudegeIsFinish();
        //DealState(state);
	}
}
