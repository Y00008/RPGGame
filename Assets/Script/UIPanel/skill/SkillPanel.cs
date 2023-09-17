using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SkillPanel : BasePanel {

      Playerstatus player;
      Button closeBtn;
      CanvasGroup canvasGroup;
      Transform content;
      List<int> skillid = new List<int>();
      SkillItem[] skillarray = new SkillItem[] { };
    //  Button itemBtn;
    // Use this for initialization
    void Awake()
    {
        content = transform.Find("Scroll View/Viewport/Content");
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();

        closeBtn.onClick.AddListener(OnClickCloseBtn);
        ReadIdAndInstance();
        skillarray = content.GetComponentsInChildren<SkillItem>();
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
        updateshow();
    }
    public override void OnExit()
    {
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

    //把所有的id从配置表中读取出来
    void ReadIdAndInstance()
    {
        TextAsset ta = Resources.Load<TextAsset>("TextInfo/Skillid");
        string[] ids=ta.text.Split(',');
        for (int i = 0; i < ids.Length; i++)
        {
            //得到id中的对象
            SkillInfo info = SkillInfoList.Instance.GetskillByid(int.Parse(ids[i]));
            if(info.applyrole.ToString()==player.herotype.ToString())
            {
                skillid.Add(int.Parse(ids[i]));
            }
        }
        for (int i = 0; i < skillid.Count; i++)
        {
            GameObject skill = GameObject.Instantiate(Resources.Load<GameObject>("IconPrefab/Skill"), content);
            skill.GetComponent<SkillItem>().SetSkillInfo(skillid[i]);
        }
    }

    void updateshow()
    {
        for (int i = 0; i < skillarray.Length; i++)
        {
            skillarray[i].Updateshow(player.level);
        }
    }
}
