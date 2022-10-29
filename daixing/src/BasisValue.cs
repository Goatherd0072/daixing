using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasisValue : MonoBehaviour
{
    public int Erosion;//侵蚀度
    public int BlockCount;//格挡值
    public float DamageRate;//伤害倍率 正值为增益，负值为减益
    public int AttackCount;//攻击力指数 增加/减少伤害
    public float BlockGetRate;//格挡值获取倍率 正值为增益，负值为减益
    public List<BuffManager.Buffer> BufferList = new List<BuffManager.Buffer>();//buff列表
    private void Awake()//开局初始化
    {
        Erosion = 0;
        BlockCount = 0;
        DamageRate = 1f;
        AttackCount = 0;
        BlockGetRate = 1f;
    }
    public void SetBasisValue(int erosion, int blockCount, float damageRate, int attackCount, float BlockgetRate)
    {
        Erosion = erosion;
        BlockCount = blockCount;
        DamageRate = damageRate;
        AttackCount = attackCount;
        BlockGetRate = BlockgetRate;
    }
    public void Init_Rate(string name)
    {
        if (name == "DamageRate") DamageRate = 1f;
        if (name == "BlockGetRate") BlockGetRate = 1f;
    }
    public void SetDamageRate(float rate)
    {
        DamageRate += rate;
    }
    public void SetBlockGetRate(float rate)
    {
        BlockGetRate += rate;
    }
    public void SetAttackCount(int count)
    {
        AttackCount += count;
    }
    public void SetBlock(int count)
    {
        BlockCount += count;
    }

}