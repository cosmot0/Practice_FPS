using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    //폭발 이펙트 프리팹 변수
    public GameObject bombEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // 이펙트 프리팹을 생성한다
        GameObject eff = Instantiate(bombEffect);

        // 이펙트 프리팹의 위치는 수류탄 오브젝트 자신의 위치와 동일
        eff.transform.position = this.transform.position;

        //자기 자신을 제거한다
        Destroy(this.gameObject);
    }
}
