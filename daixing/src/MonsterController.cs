using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public List<DeckCardController.HeroCard> mhCards;
    public static MonsterController instance;
    int CardCounter;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        CardCounter = 0;
        ShuffleCard();
    }


    //清洗怪物的英灵牌
    void ShuffleCard()
    {
        DeckCardController MH = GameObject.Find("DeckCardsController").GetComponent<DeckCardController>();
        mhCards = MH.MonsterH;
        List<DeckCardController.HeroCard> tmp = new List<DeckCardController.HeroCard>();
        for (int i = 0; i < mhCards.Count; i++)
        {
            int rad = Random.Range(0, mhCards.Count);
            tmp.Add(mhCards[i]);
            mhCards[i] = mhCards[rad];
            mhCards[rad] = tmp[0];
            tmp.RemoveAt(0);
        }
    }

    //生成怪物的英灵
    public void CreatCube()
    {
        if (CardCounter <= mhCards.Count && CardCounter >= 0)
        {
            GameObject n = Instantiate(Resources.Load("Prefabs/HeroCube")) as GameObject;//读取英灵方块预制体
            n.transform.SetParent(GameObject.Find("EnemyFieldCards").transform);//将英灵方块置于玩家区域

            if (n.GetComponent<HeroValue>() == null)
            { n.AddComponent<HeroValue>(); }
            n.GetComponent<HeroValue>().SetHeroValue(mhCards[0].Gist, mhCards[0].Health, mhCards[0].School);//初始化英灵方块的属性

            n.name = mhCards[0].Name + mhCards[0].Gist;
            n.transform.Find("Name").Find("Text").GetComponent<Text>().text = mhCards[0].Name;
            n.transform.Find("Effect").Find("Text").GetComponent<Text>().text = mhCards[0].Effect;
            n.transform.Find("Health").Find("Text").GetComponent<Text>().text = mhCards[0].Health.ToString();
            n.transform.Find("School").Find("Text").GetComponent<Text>().text = mhCards[0].School;
            n.transform.Find("Identifier").Find("Text").GetComponent<Text>().text = mhCards[0].Identifier.ToString();
            switch (mhCards[0].Gist)
            {
                case 0:
                    n.transform.Find("Gist").Find("Text").GetComponent<Text>().text = "0";
                    break;
                case 1:
                    n.transform.Find("Gist").Find("Text").GetComponent<Text>().text = "刃";
                    break;
                case 2:
                    n.transform.Find("Gist").Find("Text").GetComponent<Text>().text = "气";
                    break;
                case 3:
                    n.transform.Find("Gist").Find("Text").GetComponent<Text>().text = "神";
                    break;
                case 4:
                    n.transform.Find("Gist").Find("Text").GetComponent<Text>().text = "智";
                    break;
            }
            Debug.Log("怪物英灵方块打出成功！");
            mhCards.RemoveAt(0);

            CardCounter++;

        }
        else if (mhCards.Count == 0)
        {
            Debug.Log("没有怪物英灵");
            return;
        }
        else
        {
            CardCounter = 0;
            ShuffleCard();
            CreatCube();
        }
    }

    //怪进行普通攻击,造成数值 D（int）的伤害，即增加 D 点侵蚀度
    public void Attack_Normal(int D)
    {
        // Function.instance.SimpleAttack(D, TurnController.instance.PlayerValue, TurnController.instance.MosterValue);
    }
    //怪对自身进行治疗,恢复数值 H（int）的血量，即减少 H 点侵蚀度
    public void Heal_Normal(int H)
    {
        int tempH = TurnController.instance.MErosion;
        tempH -= H;
        if (tempH > 0)
        {
            TurnController.instance.MErosion = tempH;
        }
        else
        {
            TurnController.instance.MErosion = 0;
        }
    }
    //给目标增添Buff
    public void buffGenerator()
    {

    }
}
