using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public class GlobalValue : MonoBehaviour
{
    public static int getSpriteCount
    {
        get { return PlayerPrefs.GetInt("getSpriteCount",0); }
        set { PlayerPrefs.SetInt("getSpriteCount",value); }
    }
    public static int playerBestScore
    {
        get { return PlayerPrefs.GetInt("PlayerBestScore", 0); }
        set { PlayerPrefs.SetInt("PlayerBestScore", value); }
    }
    public static int coins
    {
        get { return PlayerPrefs.GetInt("Coins", 500); }
        set { PlayerPrefs.SetInt("Coins", value); }
    }
    public static int rocketLength
    {
        get { return PlayerPrefs.GetInt("RocketLength", 5); }
        set { PlayerPrefs.SetInt("RocketLength", value); }
    }
    public static float volumn
    {
        get { return PlayerPrefs.GetFloat("Sound", 1); }
        set { PlayerPrefs.SetFloat("Sound", value); }
    }
    public static int rocketID
    {
        get { return PlayerPrefs.GetInt("RocketID", 0); }
        set { PlayerPrefs.SetInt("RocketID", value); }
    }
    public static int hasHeadCount
    {
        get { return PlayerPrefs.GetInt("HeadCount", 1); }
        set { PlayerPrefs.SetInt("HeadCount", value); }
    }
    public static string playerName
    {
        get { return PlayerPrefs.GetString("playerName", null); }
        set { PlayerPrefs.SetString("playerName",value); }
    }
    public static bool firstEnter;
    public static void IsFirstEnter()
    {
        if (PlayerPrefs.HasKey("FirstEnter"))
        {
            firstEnter = true;
        }
        else
        {
            firstEnter = false;
            PlayerPrefs.SetInt("FirstEnter",0);
        }
    }
    #region ×ªÅÌÊ±¼ä
    public static DateTime lastTime;
    public static void GetLastTime()
    {
        if (PlayerPrefs.HasKey("LastTime"))
        {
            string currentTime = PlayerPrefs.GetString("LastTime");
            lastTime = DateTime.Parse(currentTime);
        }
        else
        {
            lastTime = DateTime.Parse("2020/1/1 00:00:00");
        }
    }
    public static string LastTime
    {
        get { return PlayerPrefs.GetString("LastTimes", lastTime.ToString()); }
        set { PlayerPrefs.SetString("LastTimes", value); }
    }
    //public static string LastTime
    //{
    //    get { return PlayerPrefs.GetString("LastTime", DateTime.Parse("2020/01/01 00:00:00").ToString()); }
    //    set { PlayerPrefs.SetString("LastTime",value); }
    //} 
    #endregion
    #region RocketHeadList
    public static bool[] rocketHeadList;
    public static void HasRocketHeadList()
    {
        if (PlayerPrefs.HasKey("RocketHeadList"))
        {
            rocketHeadList=GetBoolArray("RocketHeadList");
        }
        else
        {
            rocketHeadList = new bool[15]
            {
                true,false,false,false,false,
                false,false,false,false,false,
                false,false,false,false,false,
            };
        }
    }
    public static void SetBoolArray(string key, params bool[] boolArray)
    {
        StringBuilder sb = new();
        for (int i = 0; i < boolArray.Length - 1; i++)
        {
            sb.Append(boolArray[i]).Append('|');
        }
        sb.Append(boolArray[boolArray.Length - 1]);
        PlayerPrefs.SetString(key, sb.ToString());
    }
    private static bool[] GetBoolArray(string key)
    {
        string[] stringArray = PlayerPrefs.GetString(key).Split('|');
        bool[] boolArray = new bool[stringArray.Length];
        for (int i = 0; i < stringArray.Length; i++)
        {
            boolArray[i] = Convert.ToBoolean(stringArray[i]);
        }
        return boolArray;
    } 
    #endregion
}
