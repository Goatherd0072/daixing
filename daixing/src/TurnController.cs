using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour
{
    public GameObject St1;
    public Statubar ST;
    public static TurnController instance;
    public int PlayerTurnNumber;
    public int MonsterTurnNumber;
    public int AllTurnNumber;//回合数
    public bool current_Player = true;
    public string curren_Monster = "Monsternull";
    public int Erosion;
    public int Shield;
    public int MErosion;
    public int MShield;
    public GameObject mResult_UI;
    public Text mResult_Text;
    public GameObject Gr;
    public GameObject Ma;

    public BasisValue PlayerValue;
    public BasisValue MosterValue;

    void Awake()
    {
        instance = this;

        // Erosion = PlayerValue.Erosion;
        // MErosion = MosterValue.Erosion;
    }
    void Start()
    {
        PlayerTurnNumber = 0;
        MonsterTurnNumber = 0;
        AllTurnNumber = 0;
        ST=St1.GetComponent<Statubar>();

    }

    // Update is called once per frame
    void Update()
    {


        if (Erosion >= 100)
        {
            lose();
        }
        else if (MErosion >= 100)
        {
            win();
        }
        if (GameObject.Find("HandCardsController").GetComponent<HandCardController>().await == false && current_Player == false)
        {
            current_Player = true;
        }

    }
    void win()
    {
        MErosion = 0;
        //mResult_UI.SetActive(true);
        //mResult_Text.text = "胜利";
        //mResult_Text.color = Color.black;
        Time.timeScale = 0;
        Gr.SetActive(false);
        Ma.SetActive(true);
        SaveGameManager.instance.mSavefunction();
        ST.Credibility=ST.Credibility+1;
    }
    void lose()
    {
        mResult_UI.SetActive(true);
        mResult_Text.text = "失败";
        mResult_Text.color = Color.red;
        Time.timeScale = 0;
    }
    public void Turnover()
    {
        if (GameObject.Find("HandCardsController").GetComponent<HandCardController>().await == false && current_Player != false)//确保不会在效果结算中，或怪物回合使被按下产生效果
        {
            PlayerPrefs.SetInt("cubeif", 0);
            GameObject.Find("HandCardsController").GetComponent<HandCardController>().await = true;//令玩家进入效果结算等待
            Function.OverTurn();//调用回合结束子程序，删卡，抓卡，这个部分调用的程序有待修改
            PlayerTurnNumber++;
            this.current_Player = false;//将回合玩家变为怪物
            GameObject.Find("Functions").SendMessage("Monster");//调用怪物脚本
            GameObject.Find("Functions").GetComponent<BuffManager>().BuffUpadate(MosterValue);//调用buff管理器，更新buff
        }
    }
    public void InitTurnCount()
    {
        PlayerTurnNumber = 0;
        MonsterTurnNumber = 0;
        AllTurnNumber = 0;
    }
}
