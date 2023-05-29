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


    void Start()
    {
        // �ǰ� ����Ʈ ������Ʈ���� ��ƼŬ �ý��� ������Ʈ ��������
        ps = bulletEffect.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // ���콺 ������ ��ư ������ �ü��� �ٶ󺸴� �������� ����ź ������ �ʹ�

        // 1. ���콺 ������ ��ư�� �Է¹޴´�
        if (Input.GetMouseButton(1))
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
            // ���̸� ������ �� �߻�� ��ġ�� ���� ���� �����Ѵ�
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

            // ���̰� �ε��� ����� ������ ������ ���� ����
            RaycastHit hitInfo = new RaycastHit();

            // ���̸� �߻��� �� ���� �ε��� ��ü�� ������ �ǰ� ����Ʈ�� ǥ���Ѵ�
            if(Physics.Raycast(ray, out hitInfo))
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
