using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    // Start is called before the first frame update
    // �� Inspector �н� Panel ����������ֶ�
    public GameObject panel;

    // ��ť���ʱ���õķ���
    public void Show()
    {
        // ��ʾ���
        panel.SetActive(true);
        // ����Э�̣��� 5 ����������
        StartCoroutine(HideAfterSeconds(5f));
    }

    // Э�̵ȴ�ָ���������������
    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.SetActive(false);
    }
}
