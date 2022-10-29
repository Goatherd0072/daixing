using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffManager : MonoBehaviour
{
    ////////////////////////////////
    Buffer buff1;
    Buffer buff2;
    ////////////////////////////////
    BasisValue _PlayerValue;
    BasisValue _MosterValue;
    public enum BufferType //buff的类型
    {
        Cycle,//循环， 每时间参数回合发动一次效果
        CountDown,//倒计时， 等待 时间参数 个回合后触发，时间参数为0为立即触发
        PassiveSkills//被动Buff
    }
    public class Buffer
    {
        private int _Id;//buff ID
        private string _Name;//名称
        private BufferType _BufferType; //类型
        private int _TimeIndex; //时间参数
        public object[] _Parameter = new object[3];//传入的三个参数，[0]为作用对象，[1],[2]根据效果不同而不同
        private string _FunctionName; //调用的效果函数名称
        public string _Target; // buff作用的对象
        public int _StartTurnCount; //开始回合数
        public int _CurrentTurnCount; //当前回合数
        public int _EndTurnCount; //结束回合数
        public Buffer(int id, string name, BufferType bufferType, int timeIndex, string FunctionName, object[] o)
        {
            _Id = id;
            _Name = name;
            _BufferType = bufferType;
            _TimeIndex = timeIndex;
            _FunctionName = FunctionName;
            _StartTurnCount = 0;
            _CurrentTurnCount = 0;
            _EndTurnCount = 0;
            _Parameter[0] = o[0];
            _Parameter[1] = o[1];
            _Parameter[2] = o[2];
        }
        public int Id { get { return _Id; } }
        public string Name { get { return _Name; } }
        public BufferType BufferType { get { return _BufferType; } }
        public int TimeIndex { get { return _TimeIndex; } }
        public string FunctionName { get { return _FunctionName; } }
    }
    public bool IsEntryHeroChoice = false;//是否进入英灵卡选择界面  
    public GameObject HeroChosen;
    // Start is called before the first frame update
    void Start()
    {
        _PlayerValue = TurnController.instance.PlayerValue;
        _MosterValue = TurnController.instance.MosterValue;

        //定义一个buff

        buff1 = new Buffer(1, "buff1", BufferType.Cycle, 3, "SetDamageRateChange", new object[3] { 0.25f, null, true });
        // buff2 = new Buffer(5, "buff5", BufferType.PassiveSkills, 0, "HeroCardGetBlock", new object[3] { 2, "Player", 1 });
        buff2 = new Buffer(4, "buff4", BufferType.PassiveSkills, 0, "DrapAndGet", new object[3] { 1, "Player", 1 });

    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(1);
        if (Input.GetKeyDown(KeyCode.W))
        {
            AddBuffer(buff2, "player", _PlayerValue);
        }

        //射线检测 
        //测试用
        // if (IsEntryHeroChoice == true)
        // {}
        //GameObject.Find("HandCardsController").GetComponent<HandCardController>().await = true;
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.Raycast(ray.origin, Vector2.zero, Mathf.Infinity);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.gameObject.name);
                HeroChosen = hit.collider.gameObject;
                // if (PlayerFieldCubes.Contains(hit.collider.gameObject))
                // {
                //     hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                // }
            }

        }



    }

    //添加Buff的函数，三个参数分别为：【Buffer类的对象】buff，【string】 buff作用的对象T的名称（player or moster），即为作用者的名称
    //【BasisValue类的对象】buff添加与哪一个对象的BasisValue中，即为施法者的BasisValue  
    //后期改为传入Buff id
    public void AddBuffer(Buffer buffer, string T, BasisValue B)
    {
        //更新回合数
        buffer._StartTurnCount = TurnController.instance.AllTurnNumber;
        buffer._EndTurnCount = buffer._StartTurnCount + buffer.TimeIndex;
        //更新作用目标
        buffer._Parameter[1] = T;
        //将Buff加入施法者的BuffList中
        _PlayerValue.BufferList.Add(buffer);
        //buff的触发的函数

        SendMessage(buffer.FunctionName, buffer._Parameter);
    }
    public void BuffUpadate(BasisValue Obj) //更新带时间参数的buff
    {
        if (Obj.BufferList.Count > 0)
        {
            Debug.Log(Obj.BufferList.Count);
            for (int i = 0; i < Obj.BufferList.Count; i++)
            {
                switch (Obj.BufferList[i].BufferType)
                {
                    case BufferType.Cycle:
                        Obj.BufferList[i]._CurrentTurnCount = TurnController.instance.AllTurnNumber;
                        if ((Obj.BufferList[i]._CurrentTurnCount == Obj.BufferList[i]._EndTurnCount)) //移除循环类buff效果
                        {
                            //buff的结束的函数
                            Obj.BufferList[i]._Parameter[2] = false;
                            // Debug.Log(Obj.BufferList[i]._Parameter[2]);
                            SendMessage(Obj.BufferList[i].FunctionName, Obj.BufferList[i]._Parameter);

                            Obj.BufferList.RemoveAt(i);
                        }
                        break;
                    case BufferType.CountDown:
                        Obj.BufferList[i]._CurrentTurnCount = TurnController.instance.AllTurnNumber;
                        if (Obj.BufferList[i]._CurrentTurnCount == Obj.BufferList[i]._EndTurnCount)
                        {
                            //buff的触发的函数
                            SendMessage(Obj.BufferList[i].FunctionName, Obj.BufferList[i]._Parameter);

                            Obj.BufferList.RemoveAt(i);
                        }
                        else
                        {
                            //剩余回合显示更新

                        }
                        break;
                    default:
                        break;
                }
                if (Obj.BufferList[i].Id == 5)
                {
                    SendMessage(Obj.BufferList[i].FunctionName, Obj.BufferList[i]._Parameter);
                }
            }
        }
    }


    //变化伤害倍率的函数，需传入 【伤害倍率 d（正为增加、负为减少）】，【目标的BasisValue类的值】，【bool值 ture】
    private void SetDamageRateChange(object[] o)
    {
        float d = (float)o[0];
        string traget = (string)o[1];
        bool IsRestore = (bool)o[2];

        if (IsRestore == false)//复原
        {
            d *= -1;
        }
        if (traget == "player")//伤害倍率更改
        {
            _PlayerValue.SetDamageRate(d);
        }
        else
        {
            _MosterValue.SetDamageRate(d);
        }
    }

    //变更格挡值获取率
    private void SetBlockRateChange(object[] o)
    {
        float b = (float)o[0];
        string traget = (string)o[1];
        bool IsRestore = (bool)o[2];

        if (IsRestore == false)//复原
        {
            b *= -1;
        }
        if (traget == "player")//伤害倍率更改
        {
            _PlayerValue.SetBlockGetRate(b);
        }
        else
        {
            _MosterValue.SetBlockGetRate(b);
        }
    }
    //T回合后，造成伤害值为（）的伤害
    private void LateDamage(object[] o)
    {
        int d = (int)o[0];
        string T = (string)o[1];

        if (T == "player")
        {
            _PlayerValue.Erosion += d;
        }
        else
        {
            _MosterValue.Erosion += d;
        }
    }

    //增加攻击力
    private void AddAttact(object[] o)
    {
        int d = (int)o[0];
        string T = (string)o[1];

        if (T == "player")
        {
            _PlayerValue.SetAttackCount(d);
        }
        else
        {
            _MosterValue.SetAttackCount(d);
        }
    }

    //弃牌和拉牌
    private void DrapAndGet(object[] o)
    {
        int DrapCount = (int)o[0];
        string T = (string)o[1];
        int GetCount = (int)o[2];

        //弃牌 DrapCount张
        GameObject.Find("DiscardUI").GetComponent<DiscardUIController>().Set_isRcover(true);

        //拉牌 GetCount张


    }

    //还需要加入英灵数量，英灵位置等是否合法的判断
    private bool HeroCardChoice(int Num)
    {
        IsEntryHeroChoice = true;
        if (HeroChosen != null)
        {
            IsEntryHeroChoice = false;
            GameObject.Find("HandCardsController").GetComponent<HandCardController>().await = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void HeroCardGetBlock(object[] o)
    {
        int BlockNum = (int)o[0];
        string Target = (string)o[1];
        int HeroCardNum = (int)o[2];

        HeroChosen.GetComponent<HeroValue>().SetBlock(BlockNum);

    }
    ////////////////////////////////////////////////////////////////////////////////////////
}




