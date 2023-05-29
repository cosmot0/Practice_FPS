using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�߷� ����
    float gravity = -20f;

    // ���� �ӷ� ����
    float yVelocity = 0;


    public float moveSpeed = 7f;

    //ĳ���� ��Ʈ�ѷ� ����
    CharacterController cc;

    // ������ ����
    public float jumpPower = 10f;

    // ���� ���� ����
    public bool isJumping = false;



    

    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        
    }


    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. �̵� ���� �����Ѵ�
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;


        //2-1 ����ī�޶� �������� ���� ��ȯ�Ѵ�
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2 ���� �������̾��� �ٴڿ� �ٽ� �����ߴٸ�
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            
            isJumping = false;
            yVelocity = 0;
        }

        // 2-3 ���� Ű���� spacebar Ű �Է��߰� ���� ���� �ʴ� ���¶��

        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 2-4 ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
            


        //3. �̵� �ӵ��� ���� �̵��Ѵ�
        //p = p0 + vt
        cc.Move(dir * moveSpeed * Time.deltaTime);


        
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ� ���ݷ¸�ŭ �÷��̾��� ü�� ���
        //hp -= damage;
    }
}
