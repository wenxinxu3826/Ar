using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    // ��ת��ָ��������sceneName Ϊ��������
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }
}
