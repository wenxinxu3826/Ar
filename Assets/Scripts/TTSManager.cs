using UnityEngine;
using UnityEngine.Android; // ����AndroidȨ������

public class TTSManager : MonoBehaviour
{
    private static AndroidJavaObject tts;

    // ��ʼ��TTS����
    public static void InitTTS()
    {
        if (Application.platform != RuntimePlatform.Android) return;

        // ��鲢����Ȩ��
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }

        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        tts = new AndroidJavaObject("com.elringus.unityandroidtts.AndroidTTS");
        tts.Call("setContext", activity);
        tts.Call("setLanguage", "zh", "CN"); // ��������
    }

    // ��������
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
