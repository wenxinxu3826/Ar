using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPanel : MonoBehaviour
{
    // Start is called before the first frame update
    // 在 Inspector 中将 Panel 对象拖入此字段
    public GameObject panel;

    // 按钮点击时调用的方法
    public void Show()
    {
        // 显示面板
        panel.SetActive(true);
        // 启动协程，在 5 秒后隐藏面板
        StartCoroutine(HideAfterSeconds(5f));
    }

    // 协程等待指定秒数后隐藏面板
    IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.SetActive(false);
    }
}
