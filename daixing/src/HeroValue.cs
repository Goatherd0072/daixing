using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroValue : BasisValue //继承自BasisValue类，BasisValue函数有的它都能用
{
    public int _gist; //要旨
    public int _health; //心力
    public string _effect;//效果
    public string _school;//阵营

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetHeroValue(int g, int h, string s)
    {
        _gist = g;
        _health = h;
        _school = s;
    }
    public void Choosen()
    {

    }
    public void UnChoosen()
    {
        
    }
}
