using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * FromJson:把json信息转换成一个对象
 * ToJson:把一个对象转换成json信息
 * 
 * UIManager:
 * 1.解析并保存所有的面板存储路径的信息
 * 2.创建并保存所有面板实例化后的游戏物体
 * 3.管理并保存所有显示出来的面板
 */
public class UIManager
{
      static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }
      Transform canvasTransform;
    public Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Instantiate(Resources.Load<GameObject>("UIPanelPrefab/Canvas")).transform;
            }
            return canvasTransform;
        }
    }
      UIManager()
    {
        ParseUIPanelTypeJson();
    }
      private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板预制体的路径的字典

      public Dictionary<UIPanelType, string> PanelPathDict
      {
          get { return panelPathDict; }
          set { panelPathDict = value; }
      }
      private Dictionary<UIPanelType, BasePanel> panelDict;//存储所有实例化后的面板的basepanel的字典

      public Dictionary<UIPanelType, BasePanel> PanelDict
      {
          get { return panelDict; }
          set { panelDict = value; }
      }
      private Stack<BasePanel> panelStack;//管理显示面板的栈
    public  Stack<BasePanel> PanelSatck
    {
        get { return panelStack; }
        set { panelStack = value; }
    }

    void ParseUIPanelTypeJson()
    {
        panelPathDict = new Dictionary<UIPanelType, string>();
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/UIPanelType");
        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);
        foreach (UIPanelInfo item in jsonObject.PanelTypeInfoList)
        {
            panelPathDict.Add(item.panelType, item.path);
        }
    }
    //实例化游戏UI面板
    public BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }
        
        BasePanel panel;
        panelDict.TryGetValue(panelType, out panel);
        if (panel == null)//说明该类型的面板没有被实例化
        {
            string path;
            panelPathDict.TryGetValue(panelType, out path);
            if (path != null)//说明路径存在，实例化面板
            {
                GameObject instPanel = GameObject.Instantiate(Resources.Load<GameObject>(path));
                AddScriptsComponent(panelType, instPanel);
                instPanel.transform.SetParent(CanvasTransform, false);

                panelDict.Add(panelType, instPanel.GetComponent<BasePanel>());
                return instPanel.GetComponent<BasePanel>();
            }
        }
        return panel;
    }
    void AddScriptsComponent(UIPanelType panelType, GameObject instpanel)
    {
        //    instpanel.AddComponent<CanvasGroup>();
        //反射，.Net中获取运行时类型信息的方式，Assembly,Module,Class
        string panelTypeStr = Enum.GetName(panelType.GetType(), panelType);//获取到枚举所对应的字符串
        string scriptsName = panelTypeStr + "Panel";
        Type scriptsType = Type.GetType(scriptsName);
        if (!instpanel.GetComponent(scriptsType))
        {
            instpanel.AddComponent(scriptsType);
        }

       //switch (panelType)
       //{
            //case UIPanelType.MainMenu:
            //    //instpanel.AddComponent<MainMenuPanel>();
            //    GetAndAddComponent<MainMenuPanel>(instpanel);
            //    break;
            //case UIPanelType.Bag:
            //    Debug.Log("添加背包组件");
            //    instpanel.AddComponent<BagPanel>();
            //    break;
            //case UIPanelType.Task:
            //    instpanel.AddComponent<TaskPanel>();
            //    break;
            //case UIPanelType.Shop:
            //    instpanel.AddComponent<ShopPanel>();
            //    break;
            //case UIPanelType.Skill:
            //    instpanel.AddComponent<SkillPanel>();
            //    break;
            //case UIPanelType.System:
            //    instpanel.AddComponent<SystemPanel>();
            //    break;
            //case UIPanelType.ItemMessage:
            //    instpanel.AddComponent<ItemMessagePanel>();
            //    break;
        //}
    }
    public void PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        {
            panelStack = new Stack<BasePanel>();
        }
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();//获取栈顶的元素-》当前已经打开的界面
            topPanel.OnPause();
        }
        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
    }
    //界面关闭，出栈
    public void PopPanel()
    {
        if (panelStack == null)
        {
            return;
        }
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel topPanel = panelStack.Pop();//关闭界面，从栈中移除
        topPanel.OnExit();
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel topPanel2 = panelStack.Peek();//获取到新的栈顶元素-》上一个未关闭的面板
        topPanel2.OnResume();
    }



    //泛型 即通过参数化类型来实现在同一份代码上操作多种数据类型
      T GetAndAddComponent<T>(GameObject instPanel) where T : Component
    {
        if (!instPanel.GetComponent<T>())//获取游戏物体上对应的T组件
        {
            instPanel.AddComponent<T>();//给其挂载脚本
        }
        return instPanel.GetComponent<T>();//本身有脚本的直接返回，没有的挂载后返回
    }
}
