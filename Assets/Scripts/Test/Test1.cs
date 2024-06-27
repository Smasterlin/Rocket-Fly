using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Test1 : MonoBehaviour
{
    public Action action;
    private void Start()
    {
       
        action += AddCoins;
        action += AddBonus;
    }
    private void AddCoins()
    {
        Debug.Log("金币数值发生了变化");
    }
    private void AddBonus()
    {
        Debug.Log("获得了额外的奖励");
    }
}
