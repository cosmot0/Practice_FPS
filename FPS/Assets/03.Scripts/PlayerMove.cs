using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //중력 변수
    float gravity = -20f;

    // 수직 속력 변수
    float yVelocity = 0;


    public float moveSpeed = 10f;

    //캐릭터 콘트롤러 변수
    CharacterController cc;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    void Start()
    {
        // 캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. 이동 방향 설정한다
        Vector3 dir = new Vector3(h, 0, v);
        
        dir = dir.normalized;

        //2-1 메인카메라 기준으로 방향 변환한다
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2 캐릭터 수직 속도에 중력 값을 적용한다
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        if (Input.GetButtonDown("Jump"))
        {
            yVelocity = jumpPower;
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;

            // 2-2 만일 키보드 spacebar 키를 눌렀다면
            //if (Input.GetButtonDown("Jump"))
            //{
            //    yVelocity = jumpPower;
            //    yVelocity += gravity * Time.deltaTime;
            //    dir.y = yVelocity;
            //}

            // 2-2 만일 다시 바닥에 착지했다면
            if (cc.collisionFlags == CollisionFlags.Below)
        {
            // 만일 점프중이었다면
            if(isJumping)
            {
                // 점프 전 상태로 초기화한다
                isJumping = false;

                // 캐릭터 수직 속도를 0으로 만든다
                yVelocity = 0;
            }
        }

        // 2-3 만일 키보드 spacebar 키 입력했고 점프 하지 않는 상태라면
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            //캐릭터 수직 속도에 점프력을 작용하고 점프 상태로 변경한다
           
            isJumping = true;
            yVelocity = jumpPower;
            yVelocity += gravity * Time.deltaTime;
            dir.y = yVelocity;
        }


        //3. 이동 속도에 맞춰 이동한다
        //p = p0 + vt
        cc.Move(dir * moveSpeed * Time.deltaTime);


    }
}
