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
    //��Ϸ״̬
    public GAMESTATE gameState;
    public int score;

    //���а�
    private string url = "http://dreamlo.com/lb/";
    private string privateCode = "XOXqtCPGqEKBoAeDbwuuAAi0EJ4FabWE6yQjbrC71R1A";
    private string publicCode = "66554ef28f40bb12c85d74d4";

    public List<UserData> userDataList = new();
    //ʵ��������ģʽ
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
    /// �����а������ϴ�����վ
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="score"></param>
    /// <returns></returns>
    IEnumerator AddNewHighestScore(string userName, int score)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url + privateCode + "/add/"
            + UnityWebRequest.UnEscapeURL(userName) + "/" + score);

        yield return unityWebRequest.SendWebRequest();
        #region ���
        if (unityWebRequest.result == UnityWebRequest.Result.ProtocolError ||
            unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(unityWebRequest.error);
        }
        else
        {
            Debug.Log("������");
        } 
        #endregion
    }

    /// <summary>
    /// ����վ�ϻ��������а������
    /// </summary>
    /// <returns></returns>
    IEnumerator GetAllPlayerData()
    {
        //��ȡ��������
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url + publicCode + "/json");
        //�������󣬻�ȡ����
        yield return unityWebRequest.SendWebRequest();
        //����ʧ�ܵĻص�
        if (unityWebRequest.result==UnityWebRequest.Result.ProtocolError||
            unityWebRequest.result==UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(unityWebRequest.error);
        }
        else//����ɹ��Ļص�
        {
            JsonData jsonData = JsonMapper.ToObject(unityWebRequest.downloadHandler.text);
            JsonData userData = jsonData["dreamlo"]["leaderboard"]["entry"];
            int count = 0;
            if (userData.IsArray)
            {
                Debug.Log("�ж�������");
                foreach (JsonData item in userData)
                {
                    if (count >= 5) break;
                    userDataList.Add(new UserData(item["name"].ToString(), System.Convert.ToInt32(item["score"].ToString())));
                }
                count++;
            }
            else
            {
                Debug.Log("ֻ��һ������");
                userDataList.Add(new UserData(userData["name"].ToString(), System.Convert.ToInt32(userData["score"].ToString())));
                Debug.Log("userName:" + userData["name"] + "score:" + userData["score"]);
            }
            //�����ȡ��������
            foreach (UserData item in userDataList)
            {
                Debug.Log("name: " + item.userName + " score: " + item.score);
            }

            //Debug.Log(unityWebRequest.downloadHandler.text);
            //{"dreamlo":{"leaderboard":{"entry":
            //{"name":"Master","score":"9999","seconds":"0","text":"","date":"5/27/2024 9:20:11 AM"}}}}
        }
    }
    #region ��Ϸʤ��״̬
    /// <summary>
    /// ��Ϸʧ��
    /// </summary>
    public void GameOver()
    {
        gameState = GAMESTATE.GAMEOVER;
    }
    /// <summary>
    /// ��Ϸʤ��
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

