using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCutPanel : MonoBehaviour {
      static ShowCutPanel insatnce;

    public static ShowCutPanel Insatnce
    {
        get { return ShowCutPanel.insatnce; }
        set { ShowCutPanel.insatnce = value; }
    }
    private List<showcutslot> showCutSlotList = new List<showcutslot>();
    private Button forbiddenBtn;
    private Text forbiddenLabel;
	// Use this for initialization
	void Start() {
        insatnce = this;
        for (int i = 0; i < 6; i++)
        {
            transform.Find("ShowCutSlot" + (i + 1)).GetComponent<showcutslot>().Keycode = KeyCode.Alpha0 + i+1;
            showCutSlotList.Add(transform.Find("ShowCutSlot" + (i + 1)).GetComponent<showcutslot>());
        }

        forbiddenLabel = transform.Find("forbiddenBtn/Text").GetComponent<Text>();
        forbiddenBtn = transform.Find("forbiddenBtn").GetComponent<Button>();

        forbiddenBtn.onClick.AddListener(OnClickForBiddenBtn);
    
	}

    private void OnClickForBiddenBtn()
    {
        if(!showcutslot.isforbidden)
        {
            showcutslot.isforbidden = true;
            forbiddenLabel.text = "启用";
        }
        else
        {
            showcutslot.isforbidden = false;
            forbiddenLabel.text = "禁用";
        }
    }
	
    //判断快捷栏里面是否能装备该物品
    public bool JudgeShowCutSlot(int id,out showcutslot slot)
    {
        //遍历快捷栏
        foreach (showcutslot item in showCutSlotList)
        {
            //快捷栏中有相同id的物品
            if(item.Id==id)
            {
                slot = item;
                return false;
            }
        }
        slot = null;
        return true;
    }
}
