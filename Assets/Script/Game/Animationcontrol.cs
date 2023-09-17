using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animationcontrol : MonoBehaviour {

      Animator anim;
      PlayerMove Playanistate;
      PlayerAttack playerattack;
	// Use this for initialization
	void Start () {
        anim = this.GetComponent<Animator>();
        Playanistate = this.GetComponent<PlayerMove>();
        playerattack = this.GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerattack.state == PlayerState.ControlWalk)
        {
            switch (Playanistate.AniState)
            {
                case PlayeranimationState.Idle:
                    anim.SetInteger("Player", 1);
                    break;
                case PlayeranimationState.Moving:
                    anim.SetInteger("Player", 3);
                    break;
                //case PlayeranimationState.Jump:
                //    anim.SetBool("jump", true);
                //    break;
            }
        }
        else if (playerattack.state == PlayerState.NormalAttack)
        {
            if (playerattack.attackstate == Attackstate.Moving)
            {
                anim.SetInteger("Player", 3);
            }
        }
	}
}
