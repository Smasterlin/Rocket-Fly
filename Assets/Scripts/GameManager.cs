using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
public enum GAMESTATE
{
    PLAYING,
    SUCCESS,
    GAMEOVER,
    NULL
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //游戏状态
    public GAMESTATE gameState;
    public int score;

    //排行榜
    private string url = "http://dreamlo.com/lb/";
    private string privateCode = "XOXqtCPGqEKBoAeDbwuuAAi0EJ4FabWE6yQjbrC71R1A";
    private string publicCode = "66554ef28f40bb12c85d74d4";

    public List<UserData> userDataList = new();
    //实例化单例模式
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        gameState = GAMESTATE.NULL;
        //StartCoroutine(AddNewHighestScore("Nike", 150));
        StartCoroutine(GetAllPlayerData());
    }
    public void UpdateHighetsScore()
    {
        StartCoroutine(AddNewHighestScore(GlobalValue.playerName,GlobalValue.playerBestScore));
    }
    /// <summary>
    /// 把排行榜数据上传到网站
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    IEnumerator AddNewHighestScore(string userName, int score)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url + privateCode + "/add/"
            + UnityWebRequest.UnEscapeURL(userName) + "/" + score);

        yield return unityWebRequest.SendWebRequest();
        #region 输出
        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(unityWebRequest.error);
        }
        else
        {
            Debug.Log("添加完成");
        } 
        #endregion
    }

    /// <summary>
    /// 从网站上获得玩家排行榜的数据
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllPlayerData()
    {
        //获取数据链接
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url + publicCode + "/json");
        //发送请求，获取数据
        yield return unityWebRequest.SendWebRequest();
        //请求失败的回调
        if (unityWebRequest.result==UnityWebRequest.Result.ProtocolError||
            unityWebRequest.result==UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(unityWebRequest.error);
        }
        else//请求成功的回调
        {
            JsonData jsonData = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
            JsonData userData = jsonData["dreamlo"]["leaderboard"]["entry"];
            int count = 0;
            if (userData.IsArray)
            {
                Debug.Log("有多组数据");
                foreach (JsonData item in userData)
                {
                    if (count >= 5) break;
                    userDataList.Add(new UserData(item["name"].ToString(), System.Convert.ToInt32(item["score"].ToString())));
                }
                count++;
            }
            else
            {
                Debug.Log("只有一个数据");
                userDataList.Add(new UserData(userData["name"].ToString(), System.Convert.ToInt32(userData["score"].ToString())));
                Debug.Log("userName:" + userData["name"] + "score:" + userData["score"]);
            }
            //输出拉取到的数据
            foreach (UserData item in userDataList)
            {
                Debug.Log("name: " + item.userName + " score: " + item.score);
            }

            //Debug.Log(unityWebRequest.downloadHandler.text);
            //{"dreamlo":{"leaderboard":{"entry":
            //{"name":"Master","score":"9999","seconds":"0","text":"","date":"5/27/2024 9:20:11 AM"}}}}
        }
    }
    #region 游戏胜负状态
    /// <summary>
    /// 游戏失败
    /// </summary>
    public void GameOver()
    {
        gameState = GAMESTATE.GAMEOVER;
    }
    /// <summary>
    /// 游戏胜利
    /// </summary>
    public void Victory()
    {
        gameState = GAMESTATE.SUCCESS;
    } 
    #endregion
}
public class UserData
{
    public string userName;
    public int score;
    public UserData(string userName, int score)
    {
        this.userName = userName;
        this.score = score;
    }
}

