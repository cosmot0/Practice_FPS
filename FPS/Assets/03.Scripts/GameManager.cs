using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // PlayerMove Ŭ���� ����
    PlayerMove player;

    // ���� ���� UI ������Ʈ ����
    public GameObject gameLabel;

    // ���� ���� UI �ؽ�Ʈ ������Ʈ ����
    Text gameText;

    // �̱��� ����
    public static GameManager gm;
    
    void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    // ���� ���� ���
    public enum GameState
    {
        Ready,
        Run,
        Pause,
        GameOver
    }

    // ������ ���� ���� ����
    public GameState gState;

    // �ɼ� ȭ�� UI ������Ʈ ����
    public GameObject gameOption;

    void Start()
    {
        // �ʱ� ���� ���´� �غ� ���·� �����Ѵ�
        gState = GameState.Ready;

        // ���� ���� UI ������Ʈ���� Text ������Ʈ�� �����´�
        gameText = gameLabel.GetComponent<Text>();

        // ���� �ؽ�Ʈ�� ������ Ready �� �Ѵ�
        gameText.text = "Ready...";

        // ���� �ؽ�Ʈ�� ������ ��Ȱ������ �Ѵ�
        gameText.color = new Color32(255, 185, 0, 255);

        // ���� �غ� -> ���� �� ���·� ��ȯ�ϱ�
        StartCoroutine(ReadyToStart());

        // �÷��̾� ������Ʈ ã�� �� �÷��̾��� PlayerMove ������Ʈ �޾ƿ���
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        // ���� �÷��̾��� hp�� 0 ���϶��
        if(player.hp <= 0)
        {
            // �÷��̾��� �ִϸ��̼��� �����
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            // ���� �ؽ�Ʈ�� Ȱ��ȭ�Ѵ�
            gameLabel.SetActive(true);

            // ���� �ؽ�Ʈ�� ������ Game Over�� �Ѵ�
            gameText.text = "Game Over";

            // ���� �ؽ�Ʈ�� ������ ���������� �Ѵ�
            gameText.color = new Color(255, 0, 0, 255);

            // ���� �ؽ�Ʈ�� �ڽ� ������Ʈ Ʈ������ ������Ʈ�� �����´�
            Transform buttons = gameText.transform.GetChild(0);

            // ��ư ������Ʈ�� Ȱ��ȭ
            buttons.gameObject.SetActive(true);

            // ���¸� ���� ���� ���·� �����Ѵ�
            gState = GameState.GameOver;
        }

    }

    IEnumerator ReadyToStart()
    {
        //2�ʰ� ����Ѵ�
        yield return new WaitForSeconds(2f);

        //���� �ؽ�Ʈ�� ������ Go �� �Ѵ�
        gameText.text = "Go!";

        //0.5�ʰ� ����Ѵ�
        yield return new WaitForSeconds(0.5f);

        //���� �ؽ�Ʈ�� ��Ȱ��ȭ �Ѵ�
        gameLabel.SetActive(false);

        // ���¸� ������ ���·� �����Ѵ�
        gState = GameState.Run;
    }

    // �ɼ� ȭ�� �ѱ�
    public void OpenOptionWindow()
    {
        //�ɼ� â�� Ȱ��ȭ�Ѵ�
        gameOption.SetActive(true);

        //  ���� �ӵ��� 0������� ��ȯ�Ѵ�
        Time.timeScale = 0f;

        // ���� ���¸� �Ͻ� ���� ���·� ����
        gState = GameState.Pause;
    }

    // ����ϱ� �ɼ�
    public void CloseOptionWindow()
    {
        //�ɼ� â�� ��Ȱ��ȭ
        gameOption.SetActive(false);

        // ���� �ӵ��� 1������� ��ȯ
        Time.timeScale = 1f;

        // ���� ���¸� ���� �� ���·� ����
        gState = GameState.Run;
    }

    // �ٽ��ϱ� �ɼ�
    public void RestartGame()
    {
        // ���� �ӵ��� 1������� ��ȯ
        Time.timeScale = 1f;

        // ���� �� ��ȣ�� �ٽ� �ε�
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // �ε� ȭ�� ���� �ε�
        SceneManager.LoadScene(1);
    }

    // ���� ���� �ɼ�
    public void QuitGame()
    {
        // ���ø����̼��� ����
        Application.Quit();
    }
}
