using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shopNpc : MonoBehaviour {

	// Use this for initialization
    void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //已经打开了面板，冻结npc
            if(UIManager.Instance.PanelSatck.Count>1)
            {
                return;
            }
            UIManager.Instance.PushPanel(UIPanelType.Shop);
        }
    }
}
