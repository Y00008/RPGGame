using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour {
    private float speed = 0.1f;
    //private Light light;
    //private float time = 3;
    //private float timer=0;
    //private bool isDay;
	// Use this for initialization
	void Start () {
        //获取光照组件
        //light = this.GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.Rotate(-speed * Time.deltaTime, 0, 0);
        //timer += Time.deltaTime;
        //if(timer>time)
        //{
        //    timer = 0;
        //    if(!isDay)
        //    {
        //        light.intensity = Mathf.Lerp(light.intensity, 0f, 0.1f);
        //    }
        //    else
        //    {
        //        light.intensity = Mathf.Lerp(light.intensity, 1.3f, 0.1f);
        //    }
        //}
        //if (Mathf.Abs(light.intensity - 0) < 0.13 || Mathf.Abs(light.intensity - 1.3f) < 0.13)
        //{
        //    isDay = !isDay;
        //}
	}
}
