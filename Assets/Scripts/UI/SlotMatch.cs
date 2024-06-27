using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class SlotMatch : MonoBehaviour
{
    [Header("***类老虎机抽奖***")]
    [SerializeField] private Image[] imageIcon;
    [SerializeField] private Sprite[] headSprites;
    [SerializeField] private Button btn_drawing;
    [SerializeField] private Button btn_closePanel;
    [SerializeField] private TextMeshProUGUI txt_coins;

    [Header("***音效资源***")]
    [SerializeField] AudioClip moveClip;
    [SerializeField] AudioClip getGoldClip;

    Vector3[] goalPosition;
    private int moveSpeed;
    private bool isStop;
    private bool isUpdateStop;
    private float[] process;
    private void Awake()
    {
        btn_drawing.onClick.AddListener(StartDrawing);//点击抽奖按钮
        btn_closePanel.onClick.AddListener(Close);
    }
    void OnEnable()
    {
        txt_coins.text = GlobalValue.coins.ToString();
        if (GlobalValue.getSpriteCount >= 14)
        {
            return;
        }
        Debug.Log("rocketHeadList的长度是 "+GlobalValue.rocketHeadList.Length);
        moveSpeed = 3;
        goalPosition = new Vector3[] {Vector3.up*506,Vector3.up*253,Vector3.zero,
        Vector3.down*253,Vector3.down*506};
        isStop = isUpdateStop = false;

        process = new float[] {0f,1f,2f,3f};
        imageIcon[3].gameObject.SetActive(true);
        //随机四种火箭头皮肤
        for (int i = 0; i < imageIcon.Length; i++)
        {
            imageIcon[i].sprite = headSprites[GetRandomNum()];
            imageIcon[i].SetNativeSize();
            imageIcon[i].transform.localScale = Vector3.one * 0.7f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isUpdateStop||GlobalValue.getSpriteCount>=14)
        {
            return;
        }
        float s = moveSpeed * Time.fixedDeltaTime;
        //Debug.Log("s的值是 "+s);
        for (int i = 0; i < imageIcon.Length; i++)
        {
            process[i] += s;
            //Debug.Log("process[i]+=s的值是 "+process[i]);
            imageIcon[i].transform.localPosition = MovePosition(i);

            imageIcon[i].SetNativeSize();
            imageIcon[i].transform.localScale = Vector3.one * 0.7f;
        }
    }
    /// <summary>
    /// 移动到目标位置
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector3 MovePosition(int i)
    {
        int index = Mathf.FloorToInt(process[i]);//还没挪到下一个位置的时候，当前的位置序号
        //Debug.Log("当前的index是 "+index);

        if (index > goalPosition.Length - 2)//
        {
            process[i] -= index;//为了循环播放的时候，等间距移动
            index = 0;

            if (i == 3)//到了底部了
            {
                if (isStop==true)//抽奖完成
                {
                    isUpdateStop = true;
                    //imageIcon[i].gameObject.SetActive(false);
                    GetRocketHead(Convert.ToInt32(imageIcon[1].sprite.name));
                    AudioSourceManager.instance.PlaySound(getGoldClip);
                    //转盘结束，按钮恢复
                    btn_drawing.interactable = true;
                    btn_closePanel.interactable = true;
                    //计算已解锁的皮肤数量
                    GlobalValue.getSpriteCount++;
                    Debug.Log("已经解锁的皮肤数量是 "+GlobalValue.getSpriteCount);
                }
            }
            //抽奖未完成            
            imageIcon[i].sprite = headSprites[GetRandomNum()];
            imageIcon[i].SetNativeSize();
            imageIcon[i].transform.localScale = Vector3.one * 0.7f;
            return goalPosition[index];//最底部跑到上边
        }
        else
        {
            //Debug.Log("图标移动");
            return Vector3.Lerp(goalPosition[index], goalPosition[index + 1], process[i]-index);
        }
    }
    /// <summary>
    /// 开始抽奖
    /// </summary>
    private void StartDrawing()
    {
        if (GlobalValue.getSpriteCount>=14)
        {
            return;
        }
        if (GlobalValue.coins<400)
        {
            UIManager.instance.ShowHintPanel();
            return;
        }
        Debug.Log("点击抽奖按钮");
        //转动期间，不可再次点击按钮
        btn_drawing.interactable = false;
        btn_closePanel.interactable = false;
        //扣除金币和更新金币显示
        GlobalValue.coins -= 400;
        txt_coins.text = GlobalValue.coins.ToString();
        UIManager.instance.ChangePlayerGoldNum();
        //转动老虎机
        isStop = false;
        isUpdateStop = false;
        imageIcon[3].gameObject.SetActive(true);
        StartCoroutine("SetMoveSpeed");
    }
    IEnumerator SetMoveSpeed()
    {
        moveSpeed = 6;
        InvokeRepeating("PlaySound", 0.15f, 0.15f);
        yield return new WaitForSeconds(2);
        CancelInvoke();
        moveSpeed = 3;
        InvokeRepeating("PlaySound", 0.3f, 0.3f);
        yield return new WaitForSeconds(2);
        CancelInvoke();
        moveSpeed = 1;
        InvokeRepeating("PlaySound", 0.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        CancelInvoke();
        isStop = true;
    }
    private void PlaySound()
    {
        AudioSourceManager.instance.PlaySound(moveClip);
    }
    /// <summary>
    /// 更换获得的火箭头皮肤
    /// </summary>
    /// <param name="themID"></param>
    private void GetRocketHead(int themID)
    {
        GlobalValue.rocketHeadList[themID] = true;
        GlobalValue.SetBoolArray("RocketHeadList", GlobalValue.rocketHeadList);

        UIManager.instance.InitRocketSkin();
    }
    private int GetRandomNum()
    {
        int randomNum = UnityEngine.Random.Range(0, 15);

        while (GlobalValue.rocketHeadList[randomNum] == true /*&& GlobalValue.rocketHeadList.Length < 15*/)
        {
            //Debug.Log("产生不重复的随机数了");
            randomNum = UnityEngine.Random.Range(0, 15);
        }
        //Debug.Log("产生的随机数是 " + randomNum);
        return randomNum;
    }
    private void Close()
    {
        UIManager.instance.CloseSlotMatchPanel();
    }
}
