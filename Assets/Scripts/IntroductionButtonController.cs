using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class IntroductionButtonController : MonoBehaviour
{
    // 绑定介绍文本和AR卡片对象
    public Text descriptionText;
    public GameObject arCardObject; 

    private void Start()
    {
        // 初始隐藏文本
        descriptionText.gameObject.SetActive(false);

        // 绑定按钮点击事件
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ShowDescription);
    }

    void ShowDescription()
    {
        // 点击时切换文本显示状态
        bool isActive = descriptionText.gameObject.activeSelf;
        Debug.Log($"切换前状态：{isActive}");

        descriptionText.gameObject.SetActive(!isActive);

        Debug.Log($"切换后状态：{descriptionText.gameObject.activeSelf}");

        if (arCardObject != null)
        {
            // arCardObject.GetComponent<Animator>().Play("CardAnimation");
            TTSManager.PlayAudio(descriptionText.text);
        }
    }
}
