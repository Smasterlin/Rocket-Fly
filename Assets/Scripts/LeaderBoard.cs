using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderBoard : MonoBehaviour
{
    [Header("***ui��ʾ***")]
    [SerializeField] private GameObject[] emp_playerData;
    [SerializeField] private TextMeshProUGUI[] txt_name;
    [SerializeField] private TextMeshProUGUI[] txt_score;

    [SerializeField] GameObject emp_InfoLoaded;
    [SerializeField] GameObject emp_InfoUnLoaded;
    void OnEnable()
    {
        //��ȡ���
        //for (int i = 0; i < emp_playerData.Length; i++)
        //{
        //    txt_name[i] = emp_playerData[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //    txt_score[i]= emp_playerData[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        //}
        //�Ȱ����е��������أ�Ϊ�˺���ֻ��ʾ�����
        if (GameManager.instance.userDataList.Count>0)
        {
            emp_InfoLoaded.SetActive(true);
            emp_InfoUnLoaded.SetActive(false);
            for (int i = 0; i < emp_playerData.Length; i++)
            {
                emp_playerData[i].SetActive(false);
            }

            //��ֵ
            for (int i = 0; i < GameManager.instance.userDataList.Count; i++)
            {
                if (i >= 5)
                {
                    break;
                }
                emp_playerData[i].SetActive(true);
                txt_name[i].text = GameManager.instance.userDataList[i].userName;
                txt_score[i].text = GameManager.instance.userDataList[i].score.ToString();
            }
        }
        else
        {
            emp_InfoLoaded.SetActive(false);
            emp_InfoUnLoaded.SetActive(true);
        }
       
    }
}