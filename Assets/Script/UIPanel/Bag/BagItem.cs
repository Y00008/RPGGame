using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BagItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ICanvasRaycastFilter, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private int num;//物品数量

    public int Num
    {
        get { return num; }
        set { num = value; }
    }
      private Text numLabel;
      private Image ico;
      private int id;


      public int Id
      {
          get { return id; }
          set { id = value; }
      }
      private Transform canvas;
      private Transform nowParent;
      private bool isRaycastLocationVaild = true;//默认是不能穿透
      private static bool isDrag;//是否处于拖拽状态
      private Objectinfo bagiteminfo;

      public Objectinfo Bagiteminfo
      {
          get { return bagiteminfo; }
          set { bagiteminfo = value; }
      }
    // Use this for initialization
    void Awake()
    {
        numLabel = transform.Find("count").GetComponent<Text>();
        ico = this.GetComponent<Image>();
        canvas = GameObject.FindWithTag(Tags.canvas).transform;
    }
    //相同物品存入时改变数量
    public void SetCount(int count)
    {
        num += count;
        numLabel.text = num.ToString();
    }
    //设置图片
    public void SetIco(string name)
    {
        ico.sprite = Resources.Load<Sprite>("Icon/" + name);
    }
    //创建物品是设置属性
    public void SetCount(int id, int count)
    {
        this.id = id;
        bagiteminfo = Objectinfolist.Instance.GetObjectifobyId(id);
        num = count;
        numLabel.text = num.ToString();
    }

    //开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("开始拖拽");
        isDrag = true;
        nowParent = transform.parent;
        //将物品放在画布下的最后一个节点，确保最后渲染不会被格子挡住
        transform.SetParent(canvas);
        transform.SetAsLastSibling();
        isRaycastLocationVaild = false;//射线能穿透物体
    }

    //拖拽过程中
    public void OnDrag(PointerEventData eventData)
    {
        //设置物品坐标，跟着鼠标移动
        transform.position = Input.mousePosition;
    }
    //拖拽结束
    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject go = eventData.pointerCurrentRaycast.gameObject;
        //落点不为空
        if (go != null)
        {
            //格子索引和原来格子一样，即落点为原格子
            if (go.tag.Equals(Tags.BagSlot) && go.GetComponent<BagSlot>().Index.Equals(nowParent.GetComponent<BagSlot>().Index))
            {
                SetparentAndPosition(nowParent, transform);
            }
            //空格子，且id为0，id限制:防止有物品的时候，拖到格子的空白位置会判断为空格子
            else if (go.tag.Equals(Tags.BagSlot))
            {
                SetparentAndPosition(go.transform, transform);
                //原格子
                BagSlot slot1 = nowParent.GetComponent<BagSlot>();
                //落点格子
                BagSlot slot2 = go.GetComponent<BagSlot>();
                //设置id
                slot2.Setid(slot1.Id);
                slot1.Clearid();
            }
            //有物品的格子
            else if (go.tag.Equals(Tags.Bagitem))
            {
                //原格子
                BagSlot slot1 = nowParent.GetComponent<BagSlot>();
                //落点格子,此时go为物品
                BagSlot slot2 = go.transform.parent.GetComponent<BagSlot>();
                //把物品存放到新格子
                SetparentAndPosition(slot2.transform, transform);
                //把新格子的物品放到原来格子
                SetparentAndPosition(nowParent, go.transform);
                //设置id
                int tenmp = slot1.Id;
                slot1.Setid(slot2.Id);
                slot2.Setid(tenmp);
            }
            //结束位置是空快捷栏
            else if (go.tag.Equals(Tags.ShowCutSlot))
            {
                go.GetComponent<showcutslot>().SetDrugIcon(id);
                SetparentAndPosition(nowParent, transform);
            }
            //结束位置是有物品的快捷栏
            else if (go.tag.Equals(Tags.ShowCutSloticon))
            {
                go.transform.parent.GetComponent<showcutslot>().SetDrugIcon(id);
                SetparentAndPosition(nowParent, transform);
            }
            else//无效位置
            {
                SetparentAndPosition(nowParent, transform);
            }
        }
        else//丢弃
        {
            showcutslot showslot;
            //遍历快捷栏，查看此物品是否有快捷方式，有就将此快捷栏格子返回出来
            ShowCutPanel.Insatnce.JudgeShowCutSlot(id, out showslot);
            if (showslot != null)
            {
                showslot.clear();
            }
            //BagPanel.Instance.Bagitemdic.Remove(id);
            //销毁物品
            GameObject.Destroy(this.gameObject);
            BagPanel.Instance.BagitemList.Remove(this);
            nowParent.GetComponent<BagSlot>().Clearid();
        }
        isDrag = false;
        isRaycastLocationVaild = true;


    }
    //ui是否穿透接口实现，true为不穿透
    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        return isRaycastLocationVaild;
    }

    //设置父节点和位置
    public void SetparentAndPosition(Transform parent, Transform child)
    {
        child.SetParent(parent);
        //child.position = parent.position;
        child.transform.localPosition = Vector3.zero;
    }
    //鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDrag)
        {
            UIManager.Instance.PushPanel(UIPanelType.BagDec);
            BagDecPanel.Instance.Showinfo(id);
        }
    }
    //鼠标退出
    public void OnPointerExit(PointerEventData eventData)
    {
        //详细面板是显示状态
        if (BagDecPanel.Instance != null && BagDecPanel.Instance.gameObject.activeSelf)
        {
            UIManager.Instance.PopPanel();
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //按下左键装备武器
        if(eventData.button==PointerEventData.InputButton.Left)
        {
            //是否穿戴成功
            bool issucess = EquipPanel.Instance.Dress(id);
            //成功就更新数量
            if (issucess)
            {
                UpdateItemNun();
            }
        }
        //按下右键分解或者售卖装备
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //显示分解，售卖面板
            BagPanel.Instance.function.gameObject.SetActive(true);
            //设置面板位置
            BagPanel.Instance.function.transform.position = Input.mousePosition+new Vector3(100,-100,0);
            //传递物品信息给背包面板
            BagPanel.Instance.operateItem = this;

        }

    }
    //快捷栏使用或者穿戴完装备更新数量
    public int UpdateItemNun(int count = 1)
    {
        if (num >= count)
        {
            num -= count;
            //临界值
            if (num == 0)
            {
                //更新物品链表
                BagPanel.Instance.BagitemList.Remove(this);
                //清空格子id
                transform.parent.GetComponent<BagSlot>().Clearid();
                GameObject.Destroy(this.gameObject);
                //手动隐藏详细面板，删除对象后无法调用关闭函数
                //BagDecPanel.Instance.gameObject.SetActive(false);
                if (BagDecPanel.Instance.gameObject.activeSelf)
                {
                    Debug.Log("关闭面板");
                    UIManager.Instance.PopPanel();
                }
            }
            numLabel.text = num.ToString();
        }
        return num;
    }
}
