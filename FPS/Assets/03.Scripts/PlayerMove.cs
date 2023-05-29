using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //중력 변수
    float gravity = -20f;

    // 수직 속력 변수
    float yVelocity = 0;


    public float moveSpeed = 7f;

    //캐릭터 콘트롤러 변수
    CharacterController cc;

    // 점프력 변수
    public float jumpPower = 10f;

    // 점프 상태 변수
    public bool isJumping = false;

    // 플레이어 체력 변수
    public int hp = 20;

    // 최대 체력 변수
    int maxHP = 20;

    //hp 슬라이더 변수
    public Slider hpSlider;

    // Hit 효과 오브젝트
    public GameObject hitEffect;

    // 애니메이터 변수
    Animator anim;

    void Start()
    {
        // 캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        // 애니메이터 받아오기
        anim = GetComponentInChildren<Animator>();
        
    }


    // Update is called once per frame
    void Update()
    {
        // 게임 상태가 '게임중' 상태일때만 조작할 수 있게 한다
        if(GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        //2. 이동 방향 설정한다
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨준다
        anim.SetFloat("MoveMotion", dir.magnitude);


        //2-1 메인카메라 기준으로 방향 변환한다
        dir = Camera.main.transform.TransformDirection(dir);


        // 2-2 만일 점프중이었고 바닥에 다시 착지했다면
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            // 점프하기 전 상태로 만든다
            isJumping = false;
            // 캐릭터 수직 속도 0으로 만든다
            yVelocity = 0;
        }

        // 2-3 만일 키보드 spacebar 키 입력했고 점프 하지 않는 상태라면

        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            // 캐릭터 수직 점프에 속도를 더하고 점프 상태로 변경
            yVelocity = jumpPower;
            isJumping = true;
        }

        // 2-4 캐릭터 수직 속도에 중력 값을 적용한다
        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;
            


        //3. 이동 속도에 맞춰 이동한다
        //p = p0 + vt
        cc.Move(dir * moveSpeed * Time.deltaTime);

        // 4. 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영한다
        hpSlider.value = (float)hp / (float)maxHP;
        
    }

    // 플레이어의 피격 함수
    public void DamageAction(int damage)
    {
        // 에너미 공격력만큼 플레이어의 체력 깍다
        hp -= damage;

        // 만일 플레이어의 체력이 0보다 크면 피격 효과 출력
        if (hp > 0)
        {
            // 피격 이펙트 코루틴을 시작
            StartCoroutine(PlayHitEffect());
        }
    }

    IEnumerator PlayHitEffect()
    {
        //1. 피격 UI를 활성화한다
        hitEffect.SetActive(true);

        //2. 0.3초간 대기
        yield return new WaitForSeconds(0.3f);

        //3. 피격 UI를 비활성화한다
        hitEffect.SetActive(false);
    }
}
