using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayeranimationState
{
    Idle,
    Moving,
    Jump
}
public class PlayerMove : MonoBehaviour
{
    
      PlayeranimationState aniState = PlayeranimationState.Idle;
    public PlayeranimationState AniState
    {
        get { return aniState; }
    }
      float moveSpeed = 50;
      float jumpForce = 100;
      float gravity = 20f;
      CharacterController charactercontroller;
      Vector3 playerDir;
      Transform camera;
      float margin;
      PlayerAttack playerattack;
    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Camera").transform;
        charactercontroller = this.GetComponent<CharacterController>();
        playerattack = this.GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        //打开了面板冻结移动
        if(UIManager.Instance.PanelSatck.Count >1||playerattack.state==PlayerState.Death)
        {
            return;
        }
        //if(playerattack.state==PlayerState.ControlWalk)
        //{
            playerDir = Vector3.zero;
            if (charactercontroller.isGrounded)
            {
                float x = Input.GetAxis("Horizontal") * Time.deltaTime;
                float y = Input.GetAxis("Vertical") * Time.deltaTime;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    aniState = PlayeranimationState.Jump;

                    //playerDir.y = jumpForce;
                    //transform.position +=new Vector3(0,jumpForce,0);
                    transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, jumpForce, transform.position.z), 0.01f);
                }   
                else if (x != 0 || y != 0)
                {
                    aniState = PlayeranimationState.Moving;
                    //如果移动之前处于攻击状态
                    if(playerattack.state!=PlayerState.ControlWalk)
                    {
                        //有移动，将状态切换状态为移动
                        playerattack.state = PlayerState.ControlWalk;
                        //清空攻击目标
                        playerattack.attackNormalTarget = null;
                        playerattack.animator.SetInteger("Player", playerattack.aniIdle);
                    }

                    if (y < 0)
                    {
                        playerDir = new Vector3(-camera.forward.x, 0, -camera.forward.z);
                        Quaternion mRotation = Quaternion.LookRotation(playerDir);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, mRotation, 100);
                    }
                    if (y > 0)
                    {
                        playerDir = new Vector3(camera.forward.x, 0, camera.forward.z);
                        Quaternion mRotation = Quaternion.LookRotation(playerDir);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, mRotation, 100);
                    }
                    if (x < 0)
                    {
                        playerDir = new Vector3(-camera.right.x, 0, -camera.right.z) / 2;
                        Quaternion mRotation = Quaternion.LookRotation(playerDir);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, mRotation, 100);
                    }
                    if (x > 0)
                    {
                        playerDir = new Vector3(camera.right.x, 0, camera.right.z) / 2;
                        Quaternion mRotation = Quaternion.LookRotation(playerDir);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, mRotation, 100);
                    }
                }
                else if (x == 0 && y == 0)
                {
                    aniState = PlayeranimationState.Idle;

                }
            }
            else
            {
                playerDir.y -= gravity * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftShift))
            {
                charactercontroller.SimpleMove(playerDir * moveSpeed * Time.deltaTime * 2);
            }
            else
            {
                charactercontroller.SimpleMove(playerDir * moveSpeed * Time.deltaTime);
            }
        //}
    }

    public void SimpleMove(Vector3 position)
    {
        transform.LookAt(position);
        charactercontroller.SimpleMove(transform.forward * moveSpeed * Time.deltaTime);
    }
}
