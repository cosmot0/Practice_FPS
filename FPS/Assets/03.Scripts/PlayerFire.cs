using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �ǰ� ����Ʈ ������Ʈ
    public GameObject bulletEffect;

    // �ǰ� ����Ʈ ��ƼŬ �ý���
    ParticleSystem ps;

    // �߻� ��ġ
    public GameObject firePosition;

    // ��ô ���� ������Ʈ
    public GameObject bombFactory;

    // ��ô �Ŀ�
    public float throwPower = 15f;

    // �߻� ���� ���ݷ�
    public int weaponPower = 5;

    // �ִϸ����� ����
    Animator anim;


    void Start()
    {
        // �ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();

        // �ִϸ����� ������Ʈ ��������
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // ���� ���°� '������' �����϶��� ������ �� �ְ� �Ѵ�
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        // ���콺 ������ ��ư ������ �ü��� �ٶ󺸴� �������� ����ź ������ �ʹ�

        // 1. ���콺 ������ ��ư�� �Է¹޴´�
        if (Input.GetMouseButtonDown(1))
        {
            // ����ź ������Ʈ ���� �� ����ź�� ���� ��ġ�� �߻� ��ġ�� �Ѵ�
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            // ����ź ������Ʈ�� rigidbody ������Ʈ�� �����´�
            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            // ī�޶� ���� �������� ����ź�� �������� ���� ���Ѵ�
            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }


        // (�ѹ߻�) ���콺 ���� ��ư ������ �ü� �ٶ󺸴� �������� �� �߻��ϰ� �ʹ�
        
        // ���콺 ���� ��ư�� �Է¹޴´�
        if(Input.GetMouseButton(0))
        {
            //���� �̵� ���� Ʈ�� �Ķ������ ���� 0�̶��, ���� �ִϸ��̼��� �ǽ�
            if(anim.GetFloat("MoveMotion") == 0)
            {
                anim.SetTrigger("Attack");
            }   

            // ���̸� ������ �� �߻�� ��ġ�� ���� ���� �����Ѵ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // ���̰� �ε��� ����� ������ ������ ���� ����
            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��� �� ���� �ε��� ��ü�� ������ �ǰ� ����Ʈ�� ǥ���Ѵ�
            if(Physics.Raycast(ray, out hitInfo))
            {
                // ���� ���̿� �ε��� ����� ���̾ Enemy ��� ������ �Լ��� ����
                if(hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }

                // �׷��� �ʴٸ� ���̿� �ε��� ������ �ǰ� ����Ʈ�� �÷����Ѵ�
                else
                {
                    // �ǰ� ����Ʈ�� ��ġ�� ���̰� �ε��� �������� �̵���Ų��
                    bulletEffect.transform.position = hitInfo.point;

                    // �ǰ� ����Ʈ�� forward ������ ���̰� �ε��� ������ ���� ���Ϳ� ��ġ��Ų��
                    bulletEffect.transform.forward = hitInfo.normal;

                    // �ǰ� ����Ʈ�� �÷����Ѵ�
                    ps.Play();

                }

            }
        }

        
    }
}
