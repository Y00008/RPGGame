using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TaskPanel : BasePanel
{
      static TaskPanel instance;

    public static TaskPanel Instance
    {
        get { return TaskPanel.instance; }
        set { TaskPanel.instance = value; }
    }
      Dictionary<int, TaskItem> AcceptTaskDic = new Dictionary<int, TaskItem>();

    public Dictionary<int, TaskItem> AcceptTaskDic1
    {
        get { return AcceptTaskDic; }
        set { AcceptTaskDic = value; }
    }


      private  Button closeBtn;
      private CanvasGroup canvasGroup;
      private List<int> taskidList = new List<int>();
      private Transform content;
      private Playerstatus player;
      private Dictionary<int, TaskItem> taskDic = new Dictionary<int, TaskItem>();
      private Transform toolTip;//提示框
      private Button closetipBtn;
      private Text RewardNum;//提示框奖励数量
      private Image RewardIcon;//提示框奖励图标
    // Use this for initialization
    void Awake()
    {
        instance = this;
        toolTip = transform.Find("ToolTip");
        closetipBtn = toolTip.Find("Button").GetComponent<Button>();
        RewardNum = toolTip.Find("Num").GetComponent<Text>();
        RewardIcon = toolTip.Find("RewardIcon").GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        content = transform.Find("Scroll View/Viewport/Content");
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        //隐藏提示框
        toolTip.gameObject.SetActive(false);

        closetipBtn.onClick.AddListener(OnClickTipCloseBtn);
        closeBtn.onClick.AddListener(OnClickCloseBtn);

        ReadId();
        Instantiate();
    }

      void OnClickTipCloseBtn()
    {
        toolTip.gameObject.SetActive(false);
    }
    //通过id返回任务字典中的任务信息
    public TaskItem  GetAcceptTaskById(int id)
    {
        TaskItem task = null;
        AcceptTaskDic.TryGetValue(id, out task);
        return task;
    }
    //接取任务，放到任务背包
    public void PickTask(int id,TaskItem item)
    {
        //foreach (int it in AcceptTaskDic.Keys)
        //{
        //    if(item.Taskinfo.monstertype==AcceptTaskDic[it].Taskinfo.monstertype)
        //    {
        //        toolTip.gameObject.SetActive(true);
        //        Debug.Log("已经接取过同类型的任务，请先完成上个任务再来接取这个任务吧!");
        //        return false;
        //    }
        //}
        //将任务添加到接取任务列表中
        AcceptTaskDic.Add(id, item);
        TaskBagPanel.Instance.GetTaskItem(id);
        switch (item.Taskinfo.monstertype)
        {
            case MonsterType.Bear:
                player.killBearTaskNum++;
                break;
            case MonsterType.Lowrie:
                player.killLowrieTaskNum++;
                break;
            case MonsterType.Nian:
                player.killNianTaskNum++;
                break;
        }
        //return true;
    }
    //取消任务
    public void Giveup(int id)
    {
        //对任务数量做处理
        DealKillNum(AcceptTaskDic[id].Taskinfo);
        //将任务状态设置为未接取
        AcceptTaskDic[id].State = taskState.Notreceived;
        //做出相对应的表现形式
        AcceptTaskDic[id].DealState(AcceptTaskDic[id].State);
        //删除接取链表中的这个任务
        AcceptTaskDic.Remove(id);
    }

    //领取奖励,删除任务背包重点任务
    public void Getreward(int id)
    {
        taskinfo info = TaskinfoList.Instance.GetTaskById(id);
        toolTip.gameObject.SetActive(true);
        RewardNum.text ="x"+ info.rewardcount;
        RewardIcon.sprite = Resources.Load("Icon/" + info.rewardicon,typeof(Sprite)) as Sprite;
        DealKillNum(info);
        //发放奖励
        switch (info.rewardtype)
        {
            case RewardType.Equip:
            case RewardType.Drug:
                BagPanel.Instance.GetId(info.rewarditemid, info.rewardcount);
                break;
            case RewardType.Gold:
                player.gold += info.rewardcount;
                break;
        }
        //删除接取链表的任务,在任务背包中语句删除了接取链表中的任务，无须重复删除
        //AcceptTaskDic.Remove(id);
        //删除任务背包链表中的任务和任务背包面板的游戏物体
        GameObject.Destroy(TaskBagPanel.Instance.GiveUp(id).gameObject);
        //删除任务字典中的任务
        taskDic.Remove(id);
        //重新添加任务
        Instantiate();
    }

    //处理取消任务或者完成任务的时候击杀数量的处理
    public void  DealKillNum(taskinfo info)
    {
        //将击杀数量归零
        switch (info.monstertype)
        {
            case MonsterType.Bear:
                player.killBearTaskNum--;
                if (player.killBearTaskNum > 0)
                {
                    break;
                }
                player.killbear = 0;
                break;
            case MonsterType.Lowrie:
                player.killLowrieTaskNum--;
                if (player.killLowrieTaskNum > 0)
                {
                    break;
                }
                player.killLowrie = 0;
                break;
            case MonsterType.Nian:
                player.killNianTaskNum--;
                if (player.killNianTaskNum > 0)
                {
                    break;
                }
                player.killnian = 0;
                break;
        }
    }
    //读取任务的id并实例化任务添加到任务面板中
    void ReadId()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/TaskID");
        string[] ids=ta.text.Split(',');
        foreach (string id in ids)
        {
            taskidList.Add(int.Parse(id));
        }

    }
    //实例化任务
    void Instantiate()
    {
        while(taskDic.Count<5)
        {
            TaskItem item=null;
            //随机id，生成对应的任务
            int id = Random.Range(taskidList[0], taskidList[taskidList.Count-1]+1);
            //如果任务面板中已经有这个任务就重新生成id
            if(taskDic.TryGetValue(id,out item))
            {
                continue;
            }
            //实例化并且设置父节点
            GameObject task = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/task"),content);
            //根据id设置信息
            task.GetComponent<TaskItem>().SetInfo(id);
            taskDic.Add(id, task.GetComponent<TaskItem>());
        }
    }
    //更新信息
    void updateshow()
    {
        //通过键遍历其值
        //foreach (int item in AcceptTaskDic.Keys)
        //{
        //    //调用函数更新任务状态
        //    AcceptTaskDic[item].JudegeIsFinish();
        //    TaskBagPanel.Instance.TaskbagList
        //}
        //遍历任务背包
        foreach (TaskBagItem item in TaskBagPanel.Instance.TaskbagList)
        {
            //任务完成
            if(item.JudegeIsFinish())
            {
                //将任务面板中的任务状态改成完成
                AcceptTaskDic[item.Id].State = taskState.Complete;
                AcceptTaskDic[item.Id].DealState(AcceptTaskDic[item.Id].State);
            }
        }
    }

      void OnClickCloseBtn()
    {
        UIManager.Instance.PopPanel();
    }
    public override void OnEnter()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        Vector3 temp = transform.localPosition;
        temp.x = 2000;
        transform.localPosition = temp;
        transform.DOLocalMoveX(0, 0.5f);
        //更新面板显示
        updateshow();
    }
    public override void OnExit()
    {
        //;
        canvasGroup.blocksRaycasts = false;
        transform.DOLocalMoveX(2000, 0.5f).OnComplete(() => { canvasGroup.alpha = 0; });
    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
}
