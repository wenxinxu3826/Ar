using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides Text-to-Speech functionality by using the Android TtsWrapper plugin, which wraps around the Android Text-To-Speech API.  
/// Android TTS Wrapper Link: https://github.com/martijnj42/AndroidTTSWrapper.
/// Android TTS Link: https://developer.android.com/reference/android/speech/tts/TextToSpeech.
/// </summary>
public class AndroidTTS : IDisposable
{
    private AndroidJavaObject _ttsObject;
    private AndroidJavaObject _ttsWrapperObject;
    public static readonly string pluginName = "com.martijn.androidttswrapper.TtsWrapper";
    private string engineName;

    /// <summary>
    /// The Java TTS object, can be used to call the Android TTS functions directly.
    /// </summary>
    public AndroidJavaObject TtsObject => _ttsObject;

    private bool _isInitialised = false;
    public bool IsInitialised => _isInitialised;

    private List<string> _availableEngines;
    public List<string> AvailableEngines => _availableEngines;
    private List<string> _availableVoices;
    public List<string> AvailableVoices => _availableVoices;
    private List<string> _unavailableVoices;
    public List<string> UnavailableVoices => _unavailableVoices;


    /// <summary>
    /// Interface to define callback for initialisation Succes and Failure.
    /// </summary>
    public interface IOnInitialisationCallback {
        void onSuccess(int languageSetResultCode);
        void onFailure();
    }


    /// <summary>
    /// Initialise with a default Text-to-Speech engine.
    /// </summary>
    public AndroidTTS(IOnInitialisationCallback callback){
        InitialiseTTS(callback);
    }

    /// <summary>
    /// Initialise with a specified Text-to-Speech engine.
    /// </summary>
    public AndroidTTS(IOnInitialisationCallback callback, string engineName){
        this.engineName = engineName;
        InitialiseTTS(callback);
    }

    private void InitialiseTTS(IOnInitialisationCallback callback) {
        using (var activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            // Get current activity
            AndroidJavaObject currentActivity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

            // Create new callback object
            AndroidJavaProxy initialisationCallback = new TtsInitialisationCallBack(this, callback);

            // Initialise wrapper class, use engineName if defined
            AndroidJavaObject ttsWrapperClass;
            if (engineName != null) { 
                ttsWrapperClass = new AndroidJavaObject(pluginName, currentActivity, initialisationCallback, engineName);
            }
            else { 
                ttsWrapperClass = new AndroidJavaObject(pluginName, currentActivity, initialisationCallback);
       	    }

            // Initialse TTS
            //ttsWrapperClass.Call("checkAndInitialiseTtsData");

            _ttsWrapperObject = ttsWrapperClass;
        }
    }

    class TtsInitialisationCallBack : AndroidJavaProxy {
        private AndroidTTS androidTTS;
        private IOnInitialisationCallback callback;

        public TtsInitialisationCallBack(AndroidTTS androidTTS, IOnInitialisationCallback callback) : base (AndroidTTS.pluginName + "$TtsInitialisationCallBack") {
            this.androidTTS = androidTTS;
            this.callback = callback;
	    }

        public void onSuccess(System.Int32 languageSetResultCode) {
            Debug.Log("TTS Initialisation Succes: " + languageSetResultCode.ToString());
            androidTTS._isInitialised = true;
            androidTTS._ttsObject = androidTTS._ttsWrapperObject.Call<AndroidJavaObject>("getTtsObject");

            androidTTS._availableVoices = AndroidTTS.JavaToCsharpList<string>(androidTTS._ttsWrapperObject.Call<AndroidJavaObject>("getAvailableVoices"));
            androidTTS._unavailableVoices = AndroidTTS.JavaToCsharpList<string>(androidTTS._ttsWrapperObject.Call<AndroidJavaObject>("getUnavailableVoices"));

            androidTTS._availableEngines = AndroidTTS.JavaToCsharpList<string>(androidTTS._ttsObject.Call<AndroidJavaObject>("getEngines"));
            callback.onSuccess(0);
        }

        public void onFailure() {
            Debug.Log("TTS Initialisation Failed");
	    }
    }

    /// <summary>
    /// Set TTS language with language and country code, Android TTS documentation for more information on the codes. 
    /// More SetLanguage methods are possible be calling the native API via TtsObject, see Android Documentation.
    /// </summary>
    /// <returns>See Android documentsations for setLanguage return codes. -13 if TTS object is not initialsed.</returns>
    public int SetLanguage(string languageCode, string countryCode) {
        if (!IsInitialised) {
            Debug.LogWarning("TTS object is not initialised");
            return -13;
	    }

        return _ttsWrapperObject.Call<int>("setLanguage", languageCode, countryCode);
    }

    /// <summary>
    /// Converts the input text to speech and play the speech when it is ready.
    /// </summary>
    public void Speak(string text) {
        if (!IsInitialised) {
            Debug.LogWarning("TTS object is not initialised");
            return;
	    }

        _ttsObject.Call<int>("speak", text, 0, null, null);
    }

    /// <summary>
    /// Returns if there is speech playing, returns false if TTS not initialised yet.
    /// </summary>
    public bool IsSpeaking()
    {
        if (!IsInitialised) {
            Debug.LogWarning("TTS object is not initialised");
            return false;
	    }
        
	    return _ttsObject.Call<bool>("isSpeaking");
    }

    /// <summary>
    /// If there not speech playing. Converts the input text to speech and play the speech when it is ready.
    /// </summary>
    public void SpeakSafely(string text)
    {
        if (!IsSpeaking())
        {
            Speak(text);
        }
    }

    public void Dispose() {
        _isInitialised = false;
        if (_ttsWrapperObject != null)
        {
            _ttsWrapperObject.Call("onDestroy");
        }
    }

    /// <summary>
    /// Helper function to convert a Java list to an C# list.
    /// </summary>
    public static List<T> JavaToCsharpList<T>(AndroidJavaObject javaList) {
        int size = javaList.Call<int>("size");
        List<T> cSharpList = new List<T>();
        for (int i = 0; i < size; i++)
        {
            cSharpList.Add(javaList.Call<T>("get", i));
        }
        return cSharpList;
    }
}