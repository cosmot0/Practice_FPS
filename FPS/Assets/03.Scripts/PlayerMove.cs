using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // �÷��̾� ü�� ����
    public int hp = 20;

    // �ִ� ü�� ����
    int maxHP = 20;

    //hp �����̴� ����
    public Slider hpSlider;

    // Hit ȿ�� ������Ʈ
    public GameObject hitEffect;

    // �ִϸ����� ����
    Animator anim;

    void Start()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ִϸ����� �޾ƿ���
        anim = GetComponentInChildren<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {
        // ���� ���°� '������' �����϶��� ������ �� �ְ� �Ѵ�
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. �̵� ���� �����Ѵ�
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // �̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ��ش�
        anim.SetFloat("MoveMotion", dir.magnitude);


        //2-1 ����ī�޶� �������� ���� ��ȯ�Ѵ�
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2 ���� �������̾��� �ٴڿ� �ٽ� �����ߴٸ�
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // �����ϱ� �� ���·� �����
            isJumping = false;
            // ĳ���� ���� �ӵ� 0���� �����
            yVelocity = 0;
        }

        // 2-3 ���� Ű���� spacebar Ű �Է��߰� ���� ���� �ʴ� ���¶��

        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            // ĳ���� ���� ������ �ӵ��� ���ϰ� ���� ���·� ����
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 2-4 ĳ���� ���� �ӵ��� �߷� ���� �����Ѵ�
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
            


        //3. �̵� �ӵ��� ���� �̵��Ѵ�
        //p = p0 + vt
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 4. ���� �÷��̾� hp(%)�� hp �����̴��� value�� �ݿ��Ѵ�
        hpSlider.value = (float)hp / (float)maxHP;
        
    }

    // �÷��̾��� �ǰ� �Լ�
    public void DamageAction(int damage)
    {
        // ���ʹ� ���ݷ¸�ŭ �÷��̾��� ü�� ���
        hp -= damage;

        // ���� �÷��̾��� ü���� 0���� ũ�� �ǰ� ȿ�� ���
        if (hp > 0)
        {
            // �ǰ� ����Ʈ �ڷ�ƾ�� ����
            StartCoroutine(PlayHitEffect());
        }
    }

    IEnumerator PlayHitEffect()
    {
        //1. �ǰ� UI�� Ȱ��ȭ�Ѵ�
        hitEffect.SetActive(true);

        //2. 0.3�ʰ� ���
        yield return new WaitForSeconds(0.3f);

        //3. �ǰ� UI�� ��Ȱ��ȭ�Ѵ�
        hitEffect.SetActive(false);
    }
}
