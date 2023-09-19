using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 序列化：对象转换成二进制
 * 反序列化：二进制转换成对象
 */
[Serializable]
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType panelType;
    public string panelTypeName;
    public string path;

    //反序列化后调用这个方法  文本信息-》对象
    public void OnAfterDeserialize()
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeName);//将字符串转换对应的枚举类型
        panelType = type;//将强转后的枚举保存到反序列化中
    }
    //序列化前调用这个方法
    public void OnBeforeSerialize()
    {

    }
}
class UIPanelTypeJson
{
    public List<UIPanelInfo> PanelTypeInfoList;
}
