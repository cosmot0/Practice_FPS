using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // PlayerMove 클래스 변수
    PlayerMove player;

    // 게임 상태 UI 오브젝트 변수
    public GameObject gameLabel;

    // 게임 상태 UI 텍스트 컴포넌트 변수
    Text gameText;

    // 싱글턴 변수
    public static GameManager gm;
    
    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // 게임 상태 상수
    public enum GameState
    {
        Ready,
        Run,
        GameOver
    }

    // 현재의 게임 상태 변수
    public GameState gState;

    void Start()
    {
        // 초기 게임 상태는 준비 상태로 설정한다
        gState = GameState.Ready;

        // 게임 상태 UI 오브젝트에서 Text 컴포넌트를 가져온다
        gameText = gameLabel.GetComponent<Text>();

        // 상태 텍스트의 내용을 Ready 로 한다
        gameText.text = "Ready...";

        // 상태 텍스트의 색상을 주활생으로 한다
        gameText.color = new Color32(255, 185, 0, 255);

        // 게임 준비 -> 게임 중 상태로 전환하기
        StartCoroutine(ReadyToStart());

        // 플레이어 오브젝트 찾은 후 플레이어의 PlayerMove 컴포넌트 받아오기
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        // 만일 플레이어의 hp가 0 이하라면
        if(player.hp <= 0)
        {
            // 플레이어의 애니메이션을 멈춘다
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            // 상태 텍스트를 활성화한다
            gameLabel.SetActive(true);

            // 상태 텍스트의 내용을 Game Over로 한다
            gameText.text = "Game Over";

            // 상태 텍스트의 색상을 붉은색으로 한다
            gameText.color = new Color(255, 0, 0, 255);

            // 상태를 게임 오버 상태로 변경한다
            gState = GameState.GameOver;
        }

    }

    IEnumerator ReadyToStart()
    {
        //2초간 대기한다
        yield return new WaitForSeconds(2f);

        //상태 텍스트의 내용을 Go 로 한다
        gameText.text = "Go!";

        //0.5초간 대기한다
        yield return new WaitForSeconds(0.5f);

        //상태 텍스트를 비활성화 한다
        gameLabel.SetActive(false);

        // 상태를 게임중 상태로 변경한다
        gState = GameState.Run;
    }
}
