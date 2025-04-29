using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class IntroductionButtonController : MonoBehaviour
{
    // 绑定介绍文本和AR卡片对象
    public Text descriptionText;
    public GameObject arCardObject;
    private AndroidTTS androidTTS;
    //public string engineName = "com.google.android.tts";
    public string engineName = "";
    //public InputField inputField;

    class InitCallback : AndroidTTS.IOnInitialisationCallback
    {
        public IntroductionButtonController main;

        public InitCallback(IntroductionButtonController main)
        {
            this.main = main;
        }

        public void onSuccess(int languageSetResultCode)
        {
            if (languageSetResultCode < 0)
            {
                Debug.Log("Failed to set the language, result code: " + languageSetResultCode.ToString());
            }
            bool isXiaomiEngine = main.androidTTS.AvailableEngines.Contains("com.miui.voiceassist");

            if (isXiaomiEngine)
            {
                // 小米引擎特殊处理
                HandleXiaomiLanguage();
            }
            else
            {
                if (main.androidTTS.SetLanguage("zh", "CN") < 0)
                {
                    Debug.LogError("简体中文语言不可用");
                }
            }
            

            // 设置英文
            //if (main.androidTTS.SetLanguage("en", "US") < 0)
            //{
            //    Debug.LogError("英语语言不可用");
            //}
        }

        private void HandleXiaomiLanguage()
        {
            // 尝试设置中文（优先简体）
            int result = main.androidTTS.SetLanguage("zho", "CN");

            // 小米引擎常见回退方案
            if (result < 0)
            {
                // 部分机型需要强制设置为空地区码
                result = main.androidTTS.SetLanguage("zho", "");

                // 终极回退到引擎默认
                if (result < 0)
                {
                    main.androidTTS.SetLanguage("zh", "CN");
                    Debug.LogWarning("Fallback to Xiaomi default");
                }
            }
        }

        public void onFailure() { }
    }

    private void Start()
    {
        // 初始隐藏文本
        descriptionText.gameObject.SetActive(false);

        // 绑定按钮点击事件
        Button button = GetComponent<Button>();
        button.onClick.AddListener(ShowDescription);

        StartCoroutine(StartInitialisation());
        //inputField.onSubmit.AddListener(text => TextToSpeech(text));
    }

    private IEnumerator StartInitialisation()
    {
        yield return new WaitForEndOfFrame();
        if (engineName.Length > 0)
        {
            androidTTS = new AndroidTTS(new InitCallback(this), engineName);
        }
        else
        {
            androidTTS = new AndroidTTS(new InitCallback(this));
        }
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
            //TextToSpeech(descriptionText.text);
            TextToSpeech("你好");
        }
    }

    public void TextToSpeech(string text)
    {
        Debug.Log("Speak: " + text);
        androidTTS.Speak(text);
    }

    private void OnDestroy()
    {
        if (androidTTS != null)
        {
            androidTTS.Dispose();
        }
    }
}
