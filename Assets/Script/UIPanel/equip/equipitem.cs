using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class equipitem : MonoBehaviour,IPointerClickHandler {

    public int id;
      Image icno;
	// Use this for initialization
	void Awake () {
        icno = this.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //设置信息
    public void SetInfo(int id)
    {
        this.id = id;
        Objectinfo info= Objectinfolist.Instance.GetObjectifobyId(id);
        icno.sprite = Resources.Load<Sprite>("Icon/" + info.iconame);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EquipPanel.Instance.TakeOff(id, this.gameObject);
    }
}
