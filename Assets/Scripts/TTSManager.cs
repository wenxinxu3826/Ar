using UnityEngine;
using UnityEngine.Android; // 用于Android权限请求

public class TTSManager : MonoBehaviour
{
    private static AndroidJavaObject tts;

    // 初始化TTS引擎
    public static void InitTTS()
    {
        if (Application.platform != RuntimePlatform.Android) return;

        // 检查并请求权限
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        tts = new AndroidJavaObject("com.elringus.unityandroidtts.AndroidTTS");
        tts.Call("setContext", activity);
        tts.Call("setLanguage", "zh", "CN"); // 设置中文
    }

    // 播放语音
    public static void PlayAudio(string text)
    {
        if (tts == null) InitTTS();
        tts.Call("speak", text);
    }

    void OnDestroy()
    {
        if (tts != null)
        {
            tts.Call("shutdown");
            tts = null;
        }
    }
}
