using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(this.tag.Equals(Tags.Start))
        {
            if (other.tag.Equals(Tags.player))
            {
                Vector3 endpos = GameObject.FindGameObjectWithTag(Tags.end).transform.position;
                GameObject.FindGameObjectWithTag(Tags.player).transform.position = endpos + new Vector3(1f, 0, 0);
            }
        }
        else if(this.tag.Equals(Tags.end))
        {
            if (other.tag.Equals(Tags.player))
            {
                Vector3 endpos = GameObject.FindGameObjectWithTag(Tags.Start).transform.position;
                GameObject.FindGameObjectWithTag(Tags.player).transform.position = endpos + new Vector3(-1.5f, 0, 0);
            }
        }

    }
}
