using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 负责启动UI框架
 */
public class GameRoot : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("ui框架启动"); 
        UIManager.Instance.GetPanel(UIPanelType.TaskBag);
        UIManager.Instance.GetPanel(UIPanelType.Head);
        UIManager.Instance.GetPanel(UIPanelType.MiniMap);
        UIManager.Instance.GetPanel(UIPanelType.ShowCut);
        UIManager.Instance.PushPanel(UIPanelType.Function);
        UIManager.Instance.PushPanel(UIPanelType.Equip);
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.Bag);
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.BagDec);
        UIManager.Instance.PopPanel();
        UIManager.Instance.PushPanel(UIPanelType.Signin);
        UIManager.Instance.PopPanel();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
