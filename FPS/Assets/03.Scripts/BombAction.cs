using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //���� ����Ʈ ������ ����
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // ����Ʈ �������� �����Ѵ�
        GameObject eff = Instantiate(bombEffect);

        // ����Ʈ �������� ��ġ�� ����ź ������Ʈ �ڽ��� ��ġ�� ����
        eff.transform.position = this.transform.position;

        //�ڱ� �ڽ��� �����Ѵ�
        Destroy(this.gameObject);
    }
}
