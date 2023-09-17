using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerPanel : BasePanel {

      Button closeBtn;
      CanvasGroup canvasGroup;
      Button attackPlusBtn;
      Button defPlusBtn;
      Button speedPlusBtn;
      Text speedPropertylabel;
      Text attackPropertylabel;
      Text defPropertylabel;
      Text pointNumlabel;
      Text summarylabel;
      Playerstatus player;
    // Use this for initialization
    void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        closeBtn = transform.Find("CloseBtn").GetComponent<Button>();
        Debug.Log("点击关闭" + closeBtn);
        closeBtn.onClick.AddListener(OnClickCloseBtn);

        attackPlusBtn = transform.Find("AttackLabel/AttackPlusBtn").GetComponent<Button>();
        defPlusBtn = transform.Find("DefLabel/DefPlusBtn").GetComponent<Button>();
        speedPlusBtn = transform.Find("SpeedLabel/SpeedPlusBtn").GetComponent<Button>();

        attackPlusBtn.onClick.AddListener(OnClickAttackPlusBtn);
        defPlusBtn.onClick.AddListener(OnClickDefPlusBtn);
        speedPlusBtn.onClick.AddListener(OnClickSpeedPlusBtn);

        speedPropertylabel = transform.Find("SpeedLabel/SpeedProperty").GetComponent<Text>();
        attackPropertylabel = transform.Find("AttackLabel/AttackProperty").GetComponent<Text>();
        defPropertylabel = transform.Find("DefLabel/DefProperty").GetComponent<Text>();
        pointNumlabel = transform.Find("RemaindPoint/PointNumLabel").GetComponent<Text>();
        summarylabel = transform.Find("SumLabel/SumLabel").GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Playerstatus>();
    }

      void OnClickSpeedPlusBtn()
    {
        player.remainpoint--;
        player.speedPlus+=3;
        ShowStatusinfo();
    }

      void OnClickDefPlusBtn()
    {
        player.remainpoint--;
        player.defPlus+=3;
        ShowStatusinfo();
    }

      void OnClickAttackPlusBtn()
    {
        player.remainpoint--;
        player.attackPlus+=3;
        ShowStatusinfo();
    }

    //显示信息
    void ShowStatusinfo()
    {
        speedPropertylabel.text = player.speed + "+" + player.speedEquip+"+" + player.speedPlus;
        attackPropertylabel.text = player.attack + "+" +player.attackEquip+ "+" + player.attackPlus;
        defPropertylabel.text = player.def + "+" + player.defEquip+"+" + player.defPlus;
        pointNumlabel.text = player.remainpoint.ToString();
        summarylabel.text = "攻击力:" + (player.attack + player.attackPlus) + " 防御力:" + (player.def + player.defPlus) + " 速度:" + (player.speed + player.speedPlus);
        if(player.remainpoint>0)
        {
            attackPlusBtn.gameObject.SetActive(true);
            defPlusBtn.gameObject.SetActive(true);
            speedPlusBtn.gameObject.SetActive(true);
        }
        else
        {
            attackPlusBtn.gameObject.SetActive(false);
            defPlusBtn.gameObject.SetActive(false);
            speedPlusBtn.gameObject.SetActive(false);
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
        ShowStatusinfo();
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
