using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{
    public GameObject[] eff_Flash;

    // ���� ��� �ؽ�Ʈ
    public Text wModeText;

    // ī�޶� Ȯ�� Ȯ�ο� ����
    bool ZoomMode = false;

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

    // ���� ��� ����
    enum WeaponMode
    {
        Normal,
        Sniper
    }

    WeaponMode wMode;


    void Start()
    {
        // �Ϲ� ��� �ؽ�Ʈ ���
        wModeText.text = "Normal Mode";

        // ���� �⺻ ��带 ��� ���� ����
        wMode = WeaponMode.Normal;

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

        // ��ָ�� : ���콺 ������ ��ư ������ �ü� �������� ����ź ������ �ʹ�
        // �������� ��� : ���콺 ������ ��ư ������ ȭ�� Ȯ���ϰ� �ʹ�

        // ���콺 ������ ��ư ������ �ü��� �ٶ󺸴� �������� ����ź ������ �ʹ�

        // 1. ���콺 ������ ��ư�� �Է¹޴´�
        if (Input.GetMouseButtonDown(1))
        {

            switch(wMode)
            {
                case WeaponMode.Normal:

                // ����ź ������Ʈ ���� �� ����ź�� ���� ��ġ�� �߻� ��ġ�� �Ѵ�
                GameObject bomb = Instantiate(bombFactory);
                bomb.transform.position = firePosition.transform.position;

                // ����ź ������Ʈ�� rigidbody ������Ʈ�� �����´�
                Rigidbody rb = bomb.GetComponent<Rigidbody>();

                // ī�޶� ���� �������� ����ź�� �������� ���� ���Ѵ�
                rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

                break;

                case WeaponMode.Sniper:

                    //����, �ܸ�� ���°� �ƴ϶�� ī�޶� Ȯ��, �ܸ�� ���·� ����
                    if(!ZoomMode)
                    {
                        Camera.main.fieldOfView = 15f;
                        ZoomMode = true;
                    }

                    // �׷��� ������ ī�޶� ���� ���·� �ǵ����� �� ��� ���� ����
                    else
                    {
                        Camera.main.fieldOfView = 60f;
                        ZoomMode = false;
                    }

                    break;

            }
        }

 
        // (�ѹ߻�) ���콺 ���� ��ư ������ �ü� �ٶ󺸴� �������� �� �߻��ϰ� �ʹ�
        
        // ���콺 ���� ��ư�� �Է¹޴´�
        if(Input.GetMouseButtonDown(0))
        {
            // �� ����Ʈ�� �ǽ��Ѵ�
            StartCoroutine(ShootEffectOn(0.05f));

            //���� �̵� ���� Ʈ�� �Ķ������ ���� 0�̶��, ���� �ִϸ��̼��� �ǽ�
            //if(anim.GetFloat("MoveMotion") == 0)
            //{
            // }  
            anim.SetBool("IsFire", true);
                anim.SetTrigger("Attack");
            anim.SetBool("IsFire", false);
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

        //���� Ű������ ���� 1�� �Է��� ������ ���� ��� -> �Ϲ� ��� ����
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            wMode = WeaponMode.Normal;

            //ī�޶��� ȭ���� �ٽ� ������� ������
            Camera.main.fieldOfView = 60f;

            // �Ϲ� ��� �ؽ�Ʈ ���
            wModeText.text = "Normal Mode";
        }

        //���� Ű������ ���� 2�� �Է��� ������ ������ -> �������� ��� ����
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            wMode = WeaponMode.Sniper;

            // �������� ��� �ؽ�Ʈ ���
            wModeText.text = "Sniper Mode";
        }
            
        // �ѱ� ����Ʈ �ڷ�ƾ �Լ�
         IEnumerator ShootEffectOn(float duration)
        {
            // �����ϰ� ���� �̴´�
            int num = Random.Range(0, eff_Flash.Length);

            // ����Ʈ ������Ʈ �迭���� ���� ���ڿ� �ش��ϴ� ����Ʈ ������Ʈ Ȱ��ȭ
            eff_Flash[num].SetActive(true);

            // ������ �ð���ŭ ��ٸ���
            yield return new WaitForSeconds(duration);

            // ����Ʈ ������Ʈ �ٽ� ��Ȱ��
            eff_Flash[num].SetActive(false);
        }


    }
}
