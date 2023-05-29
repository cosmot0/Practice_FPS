using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // �̵� ���� ����
    public float moveDistance = 20f;


    void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        // ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        // �ڽ��� �ʱ� ��ġ �����ϱ�
        originPos = transform.position;
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
                Damaged();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        
    }

    void Idle()
    {
        // ���� �÷��̾���� �Ÿ��� �׼ǽ��� ���� �̳���� move ���·� ��ȯ
        if(Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ : Idle -> Move");
        }
        
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("������ȯ : move -> return");
        }

        // ���� �÷��̾���� �Ÿ��� ���� ���� ���̶�� �÷��̾ ���� �̵�
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // �̵� ���� ����
            Vector3 dir = (player.position - transform.position).normalized;

            // ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // �׷��� �ʴٸ�, ���� ���¸� �������� ��ȯ
        else
        {
            m_State = EnemyState.Attack;
            print("������ȯ : move -> attack");

            // ���� �ð��� ���� ������ �ð���ŭ �̸� ������ѳ��´�
            currentTime = attackDelay;
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
                player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;
            }
        }
        else // �׷����ʴٸ� ���� ���¸� �̵����� ��ȯ�Ѵ� (���߰�)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ Attack -> Move");
            currentTime = 0;
            
        }
    }
    void Return()
    {
        // ���� �ʱ� ��ġ������ �Ÿ��� 0.1f �̻��̶�� �ʱ� ��ġ������ �̵��Ѵ�
        if(Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // �׷��� �ʴٸ�, �ڽ��� ��ġ�� �ʱ� ��ġ�� ������ ���� ���¸� ���
        else
        {
            transform.position = originPos;

            //hp �� �ٽ� ȸ���Ѵ�
            //hp = maxHp;
            m_State = EnemyState.Idle;
            print("������ȯ : return -> idle");
        }
    }
    void Damaged()
    {

    }
    void Die()
    {

    }
}
