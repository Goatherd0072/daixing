using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFunction : MonoBehaviour
{
    public static SceneFunction Instance;

    void Awake()
    {
        Instance = this;
    }
    public void Change_Scene(string scene)//场景切换
    {
        SceneManager.LoadScene(scene);
    }
    public void QuitGame()//退出游戏
    {
        Application.Quit();
    }
}
