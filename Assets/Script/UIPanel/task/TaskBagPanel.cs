using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class TaskBagPanel : BasePanel
{
      static TaskBagPanel instance;

    public static TaskBagPanel Instance
    {
      get { return TaskBagPanel.instance; }
      set { TaskBagPanel.instance = value; }
    }
      Button closeBtn;
      CanvasGroup canvasGroup;
      Transform content;
      List<TaskBagItem> taskbagList = new List<TaskBagItem>();

    public List<TaskBagItem> TaskbagList
    {
        get { return taskbagList; }
        set { taskbagList = value; }
    }
    // Use this for initialization
    void Awake()
    {
        instance=this;
        content = transform.Find("Scroll View/Viewport/Content");
        canvasGroup = this.GetComponent<CanvasGroup>();
        transform.DOScale(0, 0f);
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);
    }

    //取消任务
    public TaskBagItem GiveUp(int id)
    {
        TaskBagItem item=null;
        //遍历任务链表，根据id找到取消的任务
        for (int i = 0; i < taskbagList.Count; i++)
        {
            if(taskbagList[i].Id==id)
            {
                item = taskbagList[i];
            }
        }
        //删除掉任务链表中的任务
        if(item!=null)
        {
            taskbagList.Remove(item);
            TaskPanel.Instance.Giveup(item.Id);
        }
        else
        {
            Debug.Log("任务背包中没有找到该任务");
        }
        return item;
    }

    //将任务存入背包
    public void   GetTaskItem(int id)
    {
        //实例化物品到任务背包中
        GameObject task = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/bagtask"),content);
        task.GetComponent<TaskBagItem>().Setinfo(id);
        //将接取的任务添加到链表中
        taskbagList.Add(task.GetComponent<TaskBagItem>());
    }

    //更新信息
    void Updateinfo()
    {
        foreach (TaskBagItem item in taskbagList)
        {
            //判断任务是否完成
            item.JudegeIsFinish();
            //更新任务背包的任务进度显示
            //item.Updatestate();
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
        //Vector3 temp = transform.localPosition;
        //temp.x = 2000;
        //transform.localPosition = temp;
        //transform.DOLocalMoveX(0, 0.5f);
        transform.DOScale(1, 0.7f);
        Updateinfo();
    }
    public override void OnExit()
    {
        //;
        canvasGroup.blocksRaycasts = false;
        transform.DOScale(0, 0.7f);
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
