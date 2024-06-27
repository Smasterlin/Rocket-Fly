using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class SlotMatch : MonoBehaviour
{
    [Header("***���ϻ����齱***")]
    [SerializeField] private Image[] imageIcon;
    [SerializeField] private Sprite[] headSprites;
    [SerializeField] private Button btn_drawing;
    [SerializeField] private Button btn_closePanel;
    [SerializeField] private TextMeshProUGUI txt_coins;

    [Header("***��Ч��Դ***")]
    [SerializeField] AudioClip moveClip;
    [SerializeField] AudioClip getGoldClip;

    Vector3[] goalPosition;
    private int moveSpeed;
    private bool isStop;
    private bool isUpdateStop;
    private float[] process;
    private void Awake()
    {
        btn_drawing.onClick.AddListener(StartDrawing);//����齱��ť
        btn_closePanel.onClick.AddListener(Close);
    }
    void OnEnable()
    {
        txt_coins.text = GlobalValue.coins.ToString();
        if (GlobalValue.getSpriteCount >= 14)
        {
            return;
        }
        Debug.Log("rocketHeadList�ĳ����� "+GlobalValue.rocketHeadList.Length);
        moveSpeed = 3;
        goalPosition = new Vector3[] {Vector3.up*506,Vector3.up*253,Vector3.zero,
        Vector3.down*253,Vector3.down*506};
        isStop = isUpdateStop = false;

        process = new float[] {0f,1f,2f,3f};
        imageIcon[3].gameObject.SetActive(true);
        //������ֻ��ͷƤ��
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
        //Debug.Log("s��ֵ�� "+s);
        for (int i = 0; i < imageIcon.Length; i++)
        {
            process[i] += s;
            //Debug.Log("process[i]+=s��ֵ�� "+process[i]);
            imageIcon[i].transform.localPosition = MovePosition(i);

            imageIcon[i].SetNativeSize();
            imageIcon[i].transform.localScale = Vector3.one * 0.7f;
        }
    }
    /// <summary>
    /// �ƶ���Ŀ��λ��
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    private Vector3 MovePosition(int i)
    {
        int index = Mathf.FloorToInt(process[i]);//��ûŲ����һ��λ�õ�ʱ�򣬵�ǰ��λ�����
        //Debug.Log("��ǰ��index�� "+index);

        if (index > goalPosition.Length - 2)//
        {
            process[i] -= index;//Ϊ��ѭ�����ŵ�ʱ�򣬵ȼ���ƶ�
            index = 0;

            if (i == 3)//���˵ײ���
            {
                if (isStop==true)//�齱���
                {
                    isUpdateStop = true;
                    //imageIcon[i].gameObject.SetActive(false);
                    GetRocketHead(Convert.ToInt32(imageIcon[1].sprite.name));
                    AudioSourceManager.instance.PlaySound(getGoldClip);
                    //ת�̽�������ť�ָ�
                    btn_drawing.interactable = true;
                    btn_closePanel.interactable = true;
                    //�����ѽ�����Ƥ������
                    GlobalValue.getSpriteCount++;
                    Debug.Log("�Ѿ�������Ƥ�������� "+GlobalValue.getSpriteCount);
                }
            }
            //�齱δ���            
            imageIcon[i].sprite = headSprites[GetRandomNum()];
            imageIcon[i].SetNativeSize();
            imageIcon[i].transform.localScale = Vector3.one * 0.7f;
            return goalPosition[index];//��ײ��ܵ��ϱ�
        }
        else
        {
            //Debug.Log("ͼ���ƶ�");
            return Vector3.Lerp(goalPosition[index], goalPosition[index + 1], process[i]-index);
        }
    }
    /// <summary>
    /// ��ʼ�齱
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
        Debug.Log("����齱��ť");
        //ת���ڼ䣬�����ٴε����ť
        btn_drawing.interactable = false;
        btn_closePanel.interactable = false;
        //�۳���Һ͸��½����ʾ
        GlobalValue.coins -= 400;
        txt_coins.text = GlobalValue.coins.ToString();
        UIManager.instance.ChangePlayerGoldNum();
        //ת���ϻ���
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
    /// ������õĻ��ͷƤ��
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
            //Debug.Log("�������ظ����������");
            randomNum = UnityEngine.Random.Range(0, 15);
        }
        //Debug.Log("������������� " + randomNum);
        return randomNum;
    }
    private void Close()
    {
        UIManager.instance.CloseSlotMatchPanel();
    }
}
