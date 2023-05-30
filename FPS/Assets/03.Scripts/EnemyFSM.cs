using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    // ���ʹ� ���� ��� ������
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    // ���ʹ� ���� ����
    EnemyState m_State;

    // �÷��̾� �߰� ����
    public float findDistance = 8f;

    // �÷��̾� Ʈ������
    Transform player;


    // ���� ���� ����
    public float attackDistance = 2f;

    // �̵�  �ӵ�
    public float moveSpeed = 5f;

    // ĳ���� ��Ʈ�ѷ� ������Ʈ
    CharacterController cc;

    // ���� �ð�
    float currentTime = 0;

    // ���� ������ �ð�
    float attackDelay = 2f;

    // ���ʹ��� ���ݷ�
    public int attackPower = 3;

    // �ʱ� ��ġ ����� ����
    Vector3 originPos;
    Quaternion originRot;

    // �̵� ���� ����
    public float moveDistance = 20f;

    // ���ʹ��� ü��
    public int hp = 15;

    // ���ʹ��� �ִ� ü��
    int maxHP = 15;

    // ���ʹ� hp Slider ����
    public Slider hpSlider;

    // �ִϸ����� ����
    Animator anim;

    // ������̼� ������Ʈ ����
    NavMeshAgent smith;


    void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ڽ��� �ʱ� ��ġ, ȸ���� �����ϱ�
        originPos = transform.position;
        originRot = transform.rotation;

        // �ڽ� ������Ʈ�κ��� �ִϸ����� ���� �޾ƿ���
        anim = transform.GetComponentInChildren<Animator>();

        smith = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        // ���� ���¸� üũ�� �ش� ���º��� ������ ����� �����ϰ� �ϰ� �ʹ�
        switch(m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;
        }

        // ���� hp(%)�� hp �����̴��� value �� �ݿ��Ѵ�
        hpSlider.value = (float)hp / (float)maxHP;
        
    }

    void Idle()
    {

        // ���� �÷��̾���� �Ÿ��� �׼ǽ��� ���� �̳���� move ���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");

            // �̵� �ִϸ��̼����� ��ȯ�ϱ�
            anim.SetTrigger("IdleToMove");
        }
        
    }

    void Move()
    {
        // ���� ���� ��ġ�� �ʱ� ��ġ���� �̵� ���� ������ �Ѿ�ٸ�...
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("������ȯ : move -> return");
        }

        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵�
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            /*
            // �̵� ���� ����
            Vector3 dir = (player.position - transform.position).normalized;

            // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // �÷��̾ ���� ������ ��ȯ
            transform.forward = dir;
            */

            // ������̼� ������Ʈ�� �̵� ���߰� ��� �ʱ�ȭ
            smith.isStopped = true;

            smith.ResetPath();
           

            // ������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�
            smith.stoppingDistance = attackDistance;

            // ������̼��� �������� �÷��̾��� ��ġ�� �����Ѵ�
            smith.destination = player.position;

        }
        // �׷��� �ʴٸ�, ���� ���¸� �������� ��ȯ
        else
        {
            m_State = EnemyState.Attack;
            print("������ȯ : move -> attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������ѳ��´�
            currentTime = attackDelay;

            // ���� ��� �ִϸ��̼� �÷���
            anim.SetTrigger("MoveToAttackDelay");
        }

       
    }
    void Attack()
    {
        // ����, �÷��̾ ���� ���� �̳��� �ִٸ� �÷��̾ ����
        if(Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // ������ �ð����� �÷��̾ ����
            currentTime += Time.deltaTime;

            if(currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;

                // ���� �ִϸ��̼� �÷���
                anim.SetTrigger("StartAttack");
            }
        }
        else // �׷����ʴٸ� ���� ���¸� �̵����� ��ȯ�Ѵ� (���߰�)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ Attack -> Move");
            currentTime = 0;

            // �̵� �ִϸ��̼� �÷���
            anim.SetTrigger("AttackToMove");
            
        }

    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
    }

    void Return()
    {
        // ���� �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ������ �̵��Ѵ�
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            /*
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);

            // ������ ���� �������� ��ȯ�Ѵ�
            transform.forward = dir;
            */

            // ������̼� �������� �ʱ� ������ġ�� ����
            smith.destination = originPos;

            // ������̼����� �����ϴ� �ּ� �Ÿ��� 0���� ����
            smith.stoppingDistance = 0;

        }
        // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� ������ ���� ���¸� ���
        else
        {

            // ������̼� ������Ʈ�� �̵� ���߰� ��� �ʱ�ȭ
            smith.isStopped = true;
            smith.ResetPath();

            // ��ġ ���� ȸ�� �� �ʱ� ���·� ��ȯ
            transform.position = originPos;
            transform.rotation = originRot;

            //hp �� �ٽ� ȸ���Ѵ�
            hp = maxHP;
            
            m_State = EnemyState.Idle;
            print("������ȯ : return -> idle");

            // ��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ�������� ȣ���Ѵ�
            anim.SetTrigger("MoveToIdle");
        }
    }
    void Damaged()
    {
        StartCoroutine(DamageProcess());
    }
    void Die()
    {
        // ���� ���� �ǰ� �ڷ�ƾ�� ����
        StopAllCoroutines();

        // ���� ���¸� ó���ϱ� ���� �ڷ�ƾ ����
        StartCoroutine(DieProcess());
    }

    //������ ���� �Լ�
    public void HitEnemy(int hitPower)
    {
        // ���� �̹� �ǰ� �����̰ų� ���, �Ǵ� ���� ���¶�� �ƹ� ó������ �Լ� ����
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        // �÷��̾��� ���ݷ� ��ŭ ���ʹ��� ü���� ���ҽ�Ų��
        hp -= hitPower;

        // ������̼� ������Ʈ�� �̵� ���߰� ��� �ʱ�ȭ
        smith.isStopped = true;
        smith.ResetPath();

        // ���ʹ��� ü���� 0���� ũ�� �ǰ� ���·� ��ȯ
        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("������ȯ : anystate -> damaged");

            //�ǰ� �ִϸ��̼��� �÷���
            anim.SetTrigger("Damaged");

            Damaged();
        }

        // �׷��� �ʴٸ� ���� ���·� ��ȯ�Ѵ�
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : any state -> Die");

            // ���� �ִϸ��̼��� �÷���
            anim.SetTrigger("Die");

            Die();
        }

    }

    IEnumerator DamageProcess()
    {
        // �ǰ� ��� �ð���ŭ ��ٸ���
        yield return new WaitForSeconds(1.0f);

        // ���� ���¸� �̵� ���·� ��ȯ
        m_State = EnemyState.Move;
        print("���� ��ȯ : damaged -> move");
    }

    IEnumerator DieProcess()
    {
        // ĳ���� ��Ʈ�ѷ� ������Ʈ ��Ȱ��
        cc.enabled = false;

        // 2�� ���� ��ٸ� �� �ڱ� �ڽ� ����
        yield return new WaitForSeconds(2f);
        print("�Ҹ�");
        Destroy(this.gameObject);
    }
}
