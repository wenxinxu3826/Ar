using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    // 跳转到指定场景，sceneName 为场景名称
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene("SampleScene");
    }
}
