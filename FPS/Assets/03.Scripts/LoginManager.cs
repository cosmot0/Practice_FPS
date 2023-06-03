using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // ����� �����͸� ���� �����ϰų� ����� �����͸� �о� ������� �Է°� ��ġ�ϴ��� �˻��ϰ�ʹ�

    // ����� ���̵� ����
    public InputField id;

    // ����� �н����� ����
    public InputField password;

    // �˻� �ؽ�Ʈ ����
    public Text notify;

  
    void Start()
    {
        // �˻� �ؽ�Ʈ â�� ����
        notify.text = "";
    }

    // ���̵�� �н����� ���� �Լ�
    public void SaveUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if(!CheckInput(id.text, password.text))
        {
            return;
        }

        // ���� �ý��ۿ� ����� �ִ� ���̵� �������� �ʴ´ٸ�
        if (!PlayerPrefs.HasKey(id.text))
        {
            // ������� ���̵�� Ű(key)// �н����� ��(value)���� �����Ѵ�
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "���̵� ������ �Ϸ�Ǿ����ϴ�";
        }
        // �׷��� �ʴٸ�, �̹� �����Ѵٴ� �޼��� ���
        else
        {
            notify.text = "�̹� �����ϴ� ���̵��Դϴ�";
        }

    }
    // �α��� �Լ�
    public void CheckUserData()
    {
        // ���� �Է� �˻翡 ������ ������ �Լ��� ����
        if (!CheckInput(id.text, password.text))
        {
            return;
        }

        // ����ڰ� �Է��� ���̵� Ű�� ����� �ý��ۿ� ����� ���� �ҷ��´�
        string pass = PlayerPrefs.GetString(id.text);

        // ���� ����ڰ� �Է��� �н������ �ý��ۿ��� �ҷ��� ���� ���ؼ� �����ϴٸ�
        if(password.text == pass)
        {
            // ������ (1����) �� �ε��Ѵ�
            SceneManager.LoadScene(1);
        }

        // �׷��� �ʰ� �� ������ ���� �ٸ���, ����� ���� ����ġ �޼��� �����
        else
        {
            notify.text = "�Է��Ͻ� ���̵�� �н����尡 ��ġ���� �ʽ��ϴ�";
        }

    }

    // �Է� �Ϸ� Ȯ�� �Լ�
    bool CheckInput(string id, string pwd)
    {
        // ���� ���̵�� �н����� �Է¶��� �ϳ��� ��������� ����� ���� �Է��� �䱸
        if(id == "" || pwd == "")
        {
            notify.text = "���̵� �Ǵ� �н����带 �Է����ּ���";
            return false;
        }
        // �Է��� ������� ������ true�� ��ȯ�Ѵ�
        else
        {
            return true;
        }
    }


}
