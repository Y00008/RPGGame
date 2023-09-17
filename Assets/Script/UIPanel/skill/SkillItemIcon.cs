using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SkillItemIcon : MonoBehaviour,IPointerDownHandler,IDragHandler,IEndDragHandler,ICanvasRaycastFilter,IPointerUpHandler {
      Transform canvas;
      bool isRaycast = true;
      GameObject icoClone;
      int skillId;
	// Use this for initialization
	void Start() {
        canvas = GameObject.FindGameObjectWithTag(Tags.canvas).transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        //保存id
        skillId = transform.parent.GetComponent<SkillItem>().Id;
        icoClone = GameObject.Instantiate(this.gameObject);
        icoClone.transform.SetParent(canvas);
        icoClone.transform.position = Input.mousePosition;
        icoClone.GetComponent<Image>().raycastTarget = false;
        isRaycast = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        icoClone.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        if(go!=null)
        {
            //拖拽结束的位置是快捷栏格子
            if (go.tag.Equals(Tags.ShowCutSlot))
            {
                showcutslot a = go.GetComponent<showcutslot>();
                go.GetComponent<showcutslot>().SetSkillIcon(skillId);
            }
            //快捷栏有物品
            else if (go.tag.Equals(Tags.ShowCutSloticon))
            {
                go.transform.parent.GetComponent<showcutslot>().SetSkillIcon(skillId);
            }
        }

        isRaycast = true;
        GameObject.Destroy(icoClone);
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycast;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRaycast = true;
        GameObject.Destroy(icoClone);
    }
}
