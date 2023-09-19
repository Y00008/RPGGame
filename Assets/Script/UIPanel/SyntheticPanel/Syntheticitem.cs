using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Syntheticitem : MonoBehaviour {

    private Text needmaterialLabel;
    private Button confirmBtn;
    private Image icon;
    private Objectinfo equipinfo;
	// Use this for initialization
    private void Awake()
    {
        needmaterialLabel = transform.Find("Text").GetComponent<Text>();
        confirmBtn = transform.Find("Button").GetComponent<Button>();
        icon = transform.Find("equuipicon").GetComponent<Image>();

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);

        //id1 = id2 = id3 = id4 = 0;
        //num1 = num2 = num3 = num4 = 0;
    }

    private void OnClickConfirmBtn()
    {
        SyntheticPanel.instance.update();
        SyntheticPanel.instance.equipid = equipinfo.id;
        SyntheticPanel.instance.iconname = equipinfo.iconame;
        if (equipinfo.syntheticmaterialOneid!=0)
        {
            SyntheticPanel.instance.idAndnumdic.Add(equipinfo.syntheticmaterialOneid, equipinfo.syntheticmaterialOneNum);
        }
        else
        {
            SyntheticPanel.instance.idAndnumdic.Add(0, 0);
        }
        if (equipinfo.syntheticmaterialTwoid != 0)
        {
            SyntheticPanel.instance.idAndnumdic.Add(equipinfo.syntheticmaterialTwoid, equipinfo.syntheticmaterialTwoNum);
        }
        else
        {
            SyntheticPanel.instance.idAndnumdic.Add(-1, 0);
        }
        if (equipinfo.syntheticmaterialThreeid != 0)
        {
            SyntheticPanel.instance.idAndnumdic.Add(equipinfo.syntheticmaterialThreeid, equipinfo.syntheticmaterialThreeNum);
        }
        else
        {
            SyntheticPanel.instance.idAndnumdic.Add(-2, 0);
        }
        if (equipinfo.syntheticmaterialFourid != 0)
        {
            SyntheticPanel.instance.idAndnumdic.Add(equipinfo.syntheticmaterialFourid, equipinfo.syntheticmaterialFourNum);
        }
        else
        {
            SyntheticPanel.instance.idAndnumdic.Add(-3, 0);
        }
        SyntheticPanel.instance.SetInfo(equipinfo.iconame, SyntheticPanel.instance.idAndnumdic);
        //隐藏选择面板
        SyntheticPanel.instance.equipselectpanel.gameObject.SetActive(false);
    }
	
    //设置信息
    public void setinfo(int id)
    {
        //获取到武器信息
        equipinfo = Objectinfolist.Instance.GetObjectifobyId(id);
        icon.sprite = Resources.Load("Icon/" + equipinfo.iconame, typeof(Sprite)) as Sprite;
        string str = "所需材料:";
        if (equipinfo.syntheticmaterialOneid != 0)
        {
            //保存所需材料id
            Objectinfo materialinfo = Objectinfolist.Instance.GetObjectifobyId(equipinfo.syntheticmaterialOneid);
            str += materialinfo.name;
            str += "x " + equipinfo.syntheticmaterialOneNum + " ";
        }
        if (equipinfo.syntheticmaterialTwoid != 0)
        {
            Objectinfo materialinfo = Objectinfolist.Instance.GetObjectifobyId(equipinfo.syntheticmaterialTwoid);
            str += materialinfo.name;
            str += "x " + equipinfo.syntheticmaterialTwoNum + " ";
        }
        if (equipinfo.syntheticmaterialThreeid != 0)
        {
            //保存所需材料id
            Objectinfo materialinfo = Objectinfolist.Instance.GetObjectifobyId(equipinfo.syntheticmaterialThreeid);
            str += materialinfo.name;
            str += "x " + equipinfo.syntheticmaterialThreeNum + " ";
        }
        if (equipinfo.syntheticmaterialFourid != 0)
        {
            Objectinfo materialinfo = Objectinfolist.Instance.GetObjectifobyId(equipinfo.syntheticmaterialFourid);
            str += materialinfo.name;
            str += "x " + equipinfo.syntheticmaterialFourNum + " ";
        }
        needmaterialLabel.text = str;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
