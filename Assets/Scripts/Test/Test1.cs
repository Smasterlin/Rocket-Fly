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
        Debug.Log("�����ֵ�����˱仯");
    }
    private void AddBonus()
    {
        Debug.Log("����˶���Ľ���");
    }
}
