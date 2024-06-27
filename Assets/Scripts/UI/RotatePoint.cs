using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class RotatePoint : MonoBehaviour
{
    [Header("***转盘ui显示***")]
    [SerializeField] GameObject panel_GetGold;
    [SerializeField] TextMeshProUGUI txt_time;
    [SerializeField] TextMeshProUGUI txt_instruction;
    [SerializeField] TextMeshProUGUI txt_currentGold;

    [Header("***转盘按钮***")]
    [SerializeField] Button btn_rotate;
    [SerializeField] Button btn_getGold;
    [SerializeField] Button btn_close;

    [Header("***音效资源***")]
    [SerializeField] AudioClip rotateClip;

    
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float continueTime;
    //获得的金币
    private bool canGet;
    private int currentGold;

    //转动角度和时间
    private bool isRotate;
    private float angle;
    private DateTime lastTime;
    private float rotateTime;

    private void Awake()
    {
        //按钮注册事件
        btn_rotate.onClick.AddListener(SetTime);
        btn_getGold.onClick.AddListener(GetGold);
        btn_getGold.gameObject.SetActive(false);
    }
    void OnEnable()
    {
        transform.eulerAngles = Vector3.zero;

        GlobalValue.GetLastTime();
        lastTime = GlobalValue.lastTime;
        Debug.Log("上一次抽奖的时间是 "+lastTime);

        DateTime nowTime = DateTime.Now;
        TimeSpan timeSpan = nowTime.Subtract(lastTime).Duration();

        canGet = timeSpan.TotalSeconds> 300;
        //canGet = true;

        //旋转按钮是否可用，及其他显示
        if (canGet==false)
        {
            txt_instruction.gameObject.SetActive(true);
            txt_time.gameObject.SetActive(true);
            btn_rotate.interactable = false;
            Debug.Log("指针不可用");
        }
        else
        {
            Debug.Log("指针可以用");
            txt_instruction.gameObject.SetActive(false);
            txt_time.gameObject.SetActive(false);
            btn_rotate.interactable = true;
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        //显示距离下一次抽奖的时间
        if (canGet==false)
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan span = nowTime.Subtract(lastTime).Duration();
            double leftTime = 300 - span.TotalSeconds;

            
            if(leftTime > 0)
            {
                txt_time.text = ((int)(leftTime / 60/60)).ToString() + " : "+((int)(leftTime/60)).ToString() 
                    + " : "+ ((int)(leftTime%60)).ToString();
            }
            else if (leftTime <= 0)
            {
                txt_instruction.gameObject.SetActive(false);
                txt_time.gameObject.SetActive(false);
            }
            //else if (leftTime < 1&&leftTime>0)
            //{
            //    txt_time.text = ((int)(leftTime*60)).ToString() + " Seconds";
            //}

        }


        if (isRotate==false)
        {
            return;
        }
        if (Time.time < rotateTime)//转盘转动
        {
            transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
        }
        else//转动时间到了,最终停止为止
        {
            //rotate 的angle+360 才可以转一周之后才到达目标位置而不是以最小的角度去转动得到
            transform.DORotate(new Vector3(0, 0, -angle + 360), 1).OnComplete(
                () =>
                {
                    btn_close.gameObject.SetActive(true);
                    btn_getGold.gameObject.SetActive(true);
                    CancelInvoke();

                    lastTime = DateTime.Now;
                    PlayerPrefs.SetString("LastTime", DateTime.Now.ToString());
                });
            isRotate = false;
        }

    }
    /// <summary>
    /// 设置指针转的时间
    /// </summary>
    private void SetTime()
    {
        UIManager.instance.PlayButtonSound();
        btn_rotate.interactable = false;
        btn_close.gameObject.SetActive(false);
        rotateTime = Time.time + continueTime;

        SetAngle();
        isRotate = true;
        InvokeRepeating("PlaySound",0.2f,0.2f);
    }
    private void PlaySound()
    {
        AudioSourceManager.instance.PlaySound(rotateClip);
    }
    /// <summary>
    /// 设置转动角度
    /// </summary>
    private void SetAngle()
    {
        //在 UnitySystem之下也存在一个Random,所以当引入System库存的时候，就要用UnityEngine.Random
        int randomNum = UnityEngine.Random.Range(0, 8);
        currentGold = UIManager.instance.awardNum[randomNum];
        txt_currentGold.text = currentGold.ToString();
        angle = randomNum * 45;
    }
    /// <summary>
    /// 获得转动金币按钮
    /// </summary>
    private void GetGold()
    {
        UIManager.instance.PlayGetGoldSound();
        GlobalValue.coins += currentGold;
        UIManager.instance.ChangePlayerGoldNum();//显示总金币
        //btn_rotate.interactable = true;

        btn_getGold.gameObject.SetActive(false);
        panel_GetGold.SetActive(false);
    }
    /// <summary>
    /// 更新抽奖之后的时间
    /// </summary>
    private void OnDisable()
    {
        
    }
}
