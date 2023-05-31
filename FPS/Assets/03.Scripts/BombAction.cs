using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    // ���� ȿ�� �ݰ�
    public float explosionRaious = 5f;

    // ����ź ������
    public int attackPower = 10;

    //���� ����Ʈ ������ ����
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ȿ�� �ݰ� ������ ���̾ Enemy �� ��� ���ӿ�����Ʈ���� collider ������Ʈ�� �迭�� ����
        Collider[] cols = Physics.OverlapSphere(transform.position, explosionRaious, 1 << 10);

        // ����� Collider �迭�� �ִ� ��� ���ʹ̿��� ����ź ������ ����
        for (int i = 0; i < cols.Length; i++)
        {
            cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
        }


        // ����Ʈ �������� �����Ѵ�
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ �������� ��ġ�� ����ź ������Ʈ �ڽ��� ��ġ�� ����
        eff.transform.position = this.transform.position;

        //�ڱ� �ڽ��� �����Ѵ�
        Destroy(this.gameObject);
    }
}
