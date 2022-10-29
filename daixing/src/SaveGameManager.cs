using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Save
{
    public int Erosion = 0;
    public int MErosion = 0;
    //TODO：
    //回合数、手牌、牌库、墓地

}

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager instance;
    private void Awake()
    {
        instance = this;
    }
    //创建Save储存当前游戏状态信息
    private Save CreateSaveObj()
    {
        //新建Save对象
        Save save = new Save();
        save.Erosion = GameObject.Find("TurnsController").GetComponent<TurnController>().Erosion;
        save.MErosion = GameObject.Find("TurnsController").GetComponent<TurnController>().MErosion;

        //遍历所有的当前牌组、牌库、墓地
        //TODO：

        return save;
    }

    //保存存档
    public bool mSavefunction()
    {
        //创建Save对象并保存当前游戏状态
        Save save = CreateSaveObj();
        //创建一个二进制格式化程序
        BinaryFormatter bf = new BinaryFormatter();
        //检测是否存在存档文件夹
        if (Directory.Exists(Application.dataPath + "/SaveDate") == false )
        {
            Directory.CreateDirectory(Application.dataPath + "/SaveDate");
        }
        //创建一个文件流
        FileStream fileStream = File.Create(Application.dataPath + "/SaveDate" + "/gamedata.txt");
        //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
        bf.Serialize(fileStream, save);
        //关闭流
        fileStream.Close();

        //如果文件存在，则显示保存成功
        if (File.Exists(Application.dataPath + "/SaveDate" + "/gamedata.txt"))
        {
            Debug.Log("保存成功");
            return true;
        }
        else
        {
            return false;   
        }
    }

    //读取存档
    public void mLoadFunction()
    { }














}

