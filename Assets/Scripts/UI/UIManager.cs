using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("***��һ�ν�����Ϸ�������������***")]
    [SerializeField] private GameObject panel_getName;
    [SerializeField] private TMP_InputField btn_input;

    [Header("***��ʼ���***")]
    [SerializeField] Button btn_start;
    [SerializeField] TextMeshProUGUI txt_btnStart;
    [SerializeField] GameObject panel_start;

    [SerializeField] GameObject bestScoreGo;
    [SerializeField] GameObject targetTrans_score;
    Tween tween_score;
    Vector3 scorePos;

    [SerializeField] TextMeshProUGUI txt_getGold;

    [Header("***��ʾ���***")]
    [SerializeField] GameObject panel_hint;
    [SerializeField] Button btn_closeHintPanel;

    [Header("***��ֵ��ʾUi***")]
    [SerializeField] Image imageMask;
    [SerializeField] TextMeshProUGUI txt_playerScore;
    [SerializeField] TextMeshProUGUI txt_playerBestScore;
    [SerializeField] TextMeshProUGUI txt_money;
    [SerializeField] GameObject newGo;

    [Header("***����ʱ***")]
    [SerializeField] TextMeshProUGUI txt_countNum;
    [SerializeField] GameObject countNumGo;
    int countNum;
    [SerializeField] AudioClip[] countDownClips;
    [SerializeField] AudioClip takeOffClip;

    [Header("***�������***")]
    [SerializeField] Sprite muteSprite;
    [SerializeField] Sprite volSprite;
    [SerializeField] Image volImage;

    [SerializeField] Button btn_resetData;
    [SerializeField] Button btn_volum;

    [SerializeField] Button btn_set;
    [SerializeField] Button btn_close;
    [SerializeField] GameObject panel_set;

    [Header("***�������***")]
    [SerializeField] GameObject panel_Help;
    [SerializeField] Button btn_help;
    [SerializeField] Button btn_ok;
    private bool ifOpen;

    [Header("***ת�̳齱���***")]
    [SerializeField] GameObject panel_GetGold;
    [SerializeField] Button btn_closeGetGoldPanel;
    [HideInInspector] public int[] awardNum = new int[] { 850, 750, 650, 450, 350, 250, 150, 50 };

    [SerializeField] AudioClip getGoldClip;

    [Header("***���а�***")]
    //[SerializeField] private LeaderBoard leadBoard;
    [SerializeField] private GameObject panel_leaderBoard;
    [SerializeField] private Button btn_closeLeaderBoard;
    [SerializeField] private Button btn_openLeaderBoard;

    [Header("***�ϻ���***")]
    [SerializeField] GameObject panel_slotMatch;
    public Button btn_openOrCloseSlotMatchPanel;
    private bool open = true;

    [Header("***���ӻ������***")]
    [SerializeField] GameObject longestGo;
    [SerializeField] GameObject addLengthGo;
    [SerializeField] Button btn_addLength;
    [SerializeField] TextMeshProUGUI txt_AddLengthMoney;
    [SerializeField] Rocket rocket;
    int consumeCoins;

    [Header("***����ҳ��***")]
    [SerializeField] GameObject panel_gameWin;
    [SerializeField] GameObject panel_gameOver;
    [SerializeField] Button btn_gameOverReplay;
    [SerializeField] Button btn_gameWinReplay;
    [SerializeField] Image imgGameOver;
    [SerializeField] TextMeshProUGUI txt_getCoins;

    [SerializeField] AudioClip newBestScoreClip;
    [SerializeField] Transform btn_replayTargetTrans;

    [Header("***��ҳ����ui����***")]
    [SerializeField] private Transform target_topUI;
    [SerializeField] private Transform target_getGold;
    [SerializeField] private Transform target_getRocketHead;
    [SerializeField] private Transform target_addRocketLength;
    [SerializeField] private Transform target_getScore;

    [SerializeField] private Transform topUITrans;
    [SerializeField] private Button btn_getGold;
    [SerializeField] private Button btn_getHead;
    [SerializeField] private Button btn_addRocketLength;

    [SerializeField] private Transform svTrans;
    [SerializeField] private Transform target_svTrans;

    private Tween tween_topUI;
    private Tween tween_getGold;
    private Tween tween_getRocketHead;
    private Tween tween_addRocketLength;
    private Tween tween_getScore;
    private Tween tween_svTrans;
    [SerializeField] Button btn_returnPanelStart;

    [Header("***�л�Ƥ���İ�ť***")]
    [SerializeField] private Button[] btns_selectedSkin;
    UnityAction action;

    [Header("***����***")]
    [SerializeField] private GameObject[] scoreUI;
    private int lastScoreLevelIndex;

    [Header("***��Ч��Դ***")]
    [SerializeField] AudioClip buttonClip;
    void Start()
    {
        GlobalValue.HasRocketHeadList();
        instance = this;

        scorePos = txt_playerScore.transform.position;
        //��һ�ν�����Ϸ����������
        GlobalValue.IsFirstEnter();
        if (GlobalValue.firstEnter==false)
        {
            panel_getName.SetActive(true);
        }
        
        //���밴ťע��
        btn_input.onEndEdit.AddListener(GetName);
        //��ҳ
        btn_start.onClick.AddListener(StartGame);
        //����ҳ�水ťע��
        btn_set.onClick.AddListener(() =>
        {
            panel_set.SetActive(true);
            panel_set.transform.DOScaleY(1, 0.5f);
            PlayButtonSound();
        });
        btn_close.onClick.AddListener(() => {
            panel_set.transform.DOScaleY(0, 0.5f).OnComplete(
                () => panel_set.SetActive(false));
            PlayButtonSound();
        });
        btn_resetData.onClick.AddListener(ResetData);
        btn_volum.onClick.AddListener(SetVolum);
        //�������
        btn_help.onClick.AddListener(OpenHelpPanel);
        btn_ok.onClick.AddListener(OpenHelpPanel);
        //ת�̳齱
        btn_getGold.onClick.AddListener(() => {
            panel_GetGold.SetActive(true);
            PlayButtonSound();
        });
        btn_closeGetGoldPanel.onClick.AddListener(() => {
            panel_GetGold.SetActive(false);
            PlayButtonSound();
        });
        //���ӻ������
        btn_addLength.onClick.AddListener(AddRocketLength);
        //���а�
        btn_openLeaderBoard.onClick.AddListener(delegate { OpenOrClosePanelLeader(open); });
        btn_closeLeaderBoard.onClick.AddListener(delegate { OpenOrClosePanelLeader(!open); });
        //����ҳ��
        btn_gameOverReplay.onClick.AddListener(Replay);
        btn_gameWinReplay.onClick.AddListener(Replay);
        //��ҳ��ť�¼�
        btn_getHead.onClick.AddListener(GetRocketHead);
        btn_returnPanelStart.onClick.AddListener(ReturnPanelStart);
        //�ϻ�����ťע���¼�
        btn_openOrCloseSlotMatchPanel.onClick.AddListener(delegate { OpenOrCloseSlotMatchPanel(open); });
        //��ʾ���رհ�ť
        btn_closeHintPanel.onClick.AddListener(()=> {
            PlayButtonSound();
            panel_hint.SetActive(false);
        });
        //�л�Ƥ���İ�ť
        for (int i = 0; i < btns_selectedSkin.Length; i++)
        {
            int index = new();
            index = i;
            btns_selectedSkin[index].onClick.AddListener(delegate { ChangeRocketSkin(index); });
        }
        //���а�������ǰ��ȡ
        //leadBoard.GetLeaderBoard();
        HideImageMask();
        countNum = 3;
        InitUI();
        TweenUIInit();

        //foreach (var item in GlobalValue.rocketHeadList)
        //{
        //    Debug.Log(item);
        //}
    }
    private void TweenUIInit()
    {
        tween_topUI = topUITrans.DOLocalMoveY(target_topUI.localPosition.y, 0.5f);
        tween_topUI.SetAutoKill(false);
        tween_topUI.Pause();

        tween_getGold = btn_getGold.transform.DOLocalMoveX(target_getGold.localPosition.x, 0.5f);
        tween_getGold.SetAutoKill(false);
        tween_getGold.Pause();

        tween_getRocketHead = btn_getHead.transform.DOLocalMoveX(target_getRocketHead.localPosition.x, 0.5f);
        tween_getRocketHead.SetAutoKill(false);
        tween_getRocketHead.Pause();

        tween_addRocketLength = btn_addLength.transform.DOLocalMoveX(target_addRocketLength.localPosition.x, 0.5f);
        tween_addRocketLength.SetAutoKill(false);
        tween_addRocketLength.Pause();

        tween_getScore = txt_playerScore.transform.DOLocalMove(target_getScore.localPosition, 0.5f);
        tween_getScore.SetAutoKill(false);
        tween_getScore.Pause();

        tween_svTrans = svTrans.DOLocalMoveX(target_svTrans.localPosition.x, 0.5f);
        tween_svTrans.SetAutoKill(false);
        tween_svTrans.Pause();

        tween_score = txt_playerScore.transform.DOLocalMove(targetTrans_score.transform.localPosition,1);
        tween_score.SetAutoKill(false);
        tween_score.Pause();

        for (int i = 0; i < btns_selectedSkin.Length; i++)
        {
            btns_selectedSkin[i].transform.Find("Img_Click").gameObject.SetActive(false);
        }

        btns_selectedSkin[GlobalValue.rocketID].transform.Find("Img_Click").gameObject.SetActive(true);
        //ChangeRocketSkin(2);
        InitRocketSkin();
    }
    private void InitUI()
    {
        txt_playerScore.text = 0.ToString(); ;
        txt_playerBestScore.text = GlobalValue.playerBestScore.ToString();
        txt_money.text = GlobalValue.coins.ToString();
        txt_countNum.text = countNum.ToString();
        volImage.color = new Color(1, 1, 1, GlobalValue.volumn);
        JugeEnoughMoney();
        //DOTween.To(()=>txt_btnStart.color,ToColor=>txt_btnStart.color=ToColor,new Color(255,255,255),0.7f).
        //    SetLoops(-1, LoopType.Yoyo);
        txt_btnStart.DOFade(1, 0.7f).SetLoops(-1, LoopType.Yoyo);
        UpdateVolumUI();
    }
    #region ת������
    public void ShowImageMask()
    {
        DOTween.To(() => imageMask.color, Tocolor => imageMask.color = Tocolor, new Color(0, 0, 0, 1), 2).
            OnComplete(() => {
                //newGo.SetActive(false);
                ExitReloadScenes();

            });
    }
    private void HideImageMask()
    {
        DOTween.To(() => imageMask.color, Tocolor => imageMask.color = Tocolor, new Color(0, 0, 0, 0), 2);
    }
    private void ExitReloadScenes()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    private void EndCountNum()
    {
        countNum--;
        txt_countNum.text = countNum.ToString();
        if (countNum > 0)
        {
            AudioSourceManager.instance.PlaySound(countDownClips[countNum - 1]);
        }
        else
        {
            GameManager.instance.gameState = GAMESTATE.PLAYING;
            rocket.ChangeShort(0);

            CancelInvoke();
            countNumGo.SetActive(false);
            //btn_start.gameObject.SetActive(false);
            //panel_start.SetActive(false);
        }
    }
    private void StartGame()
    {
        rocket.StartGame();
        btn_start.gameObject.SetActive(false);

        bestScoreGo.SetActive(false);
        //UI�˳���Ļ
        tween_topUI.PlayForward();
        tween_getGold.PlayForward();
        tween_getRocketHead.PlayForward();
        tween_addRocketLength.PlayForward();

        tween_score.PlayForward();
        
        countNumGo.SetActive(true);
        InvokeRepeating("EndCountNum", 1, 1);
        AudioSourceManager.instance.PlaySound(takeOffClip);
        AudioSourceManager.instance.PlaySound(countDownClips[countNum - 1]);
    }
    /// <summary>
    /// ����
    /// </summary>
    private void SetVolum()
    {
        PlayButtonSound();
        GlobalValue.volumn += 0.2f;
        UpdateVolumUI();
    }
    private void UpdateVolumUI()
    {
        if (GlobalValue.volumn > 1)
        {
            GlobalValue.volumn = 0;
            volImage.sprite = muteSprite;
            volImage.color = new Color(1, 0, 0, 0.7f);
            volImage.transform.localScale = Vector3.one;

        }
        else
        {
            volImage.sprite = volSprite;
            volImage.color = new Color(1, 1, 1, GlobalValue.volumn);
            volImage.transform.localScale = Vector3.one * 1.7f;
        }
    }
    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        PlayButtonSound();
    }
    private void OpenHelpPanel()
    {
        PlayButtonSound();
        Debug.Log(ifOpen);
        if (ifOpen == false)
        {
            panel_Help.SetActive(true);
            panel_Help.transform.DOScaleX(1, 0.5f);
            ifOpen = true;
        }
        else
        {
            panel_Help.transform.DOScaleX(0, 0.5f).OnComplete(() => panel_Help.SetActive(false));
            ifOpen = false;
        }
    }
    private void OpenOrClosePanelLeader(bool open)
    {
        PlayButtonSound();
        panel_leaderBoard.SetActive(open);
    }
    private void AddRocketLength()
    {
        PlayButtonSound();
        ConsumeCoinsAddLength();
        if (GlobalValue.coins < consumeCoins)
        {
            Debug.Log("���ӻ�����ȵĽ�Ҳ���");
            panel_hint.SetActive(true);
            return;
        }
        
        GlobalValue.coins -= consumeCoins;
        txt_money.text = GlobalValue.coins.ToString();
        rocket.AddRocketLength();
        JugeEnoughMoney();
    }
    private void ConsumeCoinsAddLength()
    {
        consumeCoins = 50 + (GlobalValue.rocketLength - 5) * 50;
    }
    private void JugeEnoughMoney()
    {
        if (GlobalValue.rocketLength >= 10)
        {
            longestGo.SetActive(true);
            addLengthGo.SetActive(false);
            btn_addLength.interactable = false;
            return;
        }
        //�����´���������Ľ����ʾ
        txt_AddLengthMoney.text = (50 + (GlobalValue.rocketLength - 5) * 50).ToString();
        if (GlobalValue.coins >= (50 + (GlobalValue.rocketLength - 5) * 50))
        {
            btn_addLength.interactable = true;
        }
        else
        {
            //btn_addLength.interactable = false;
        }
    }
    public void ShowGameWin()
    {
        panel_gameWin.SetActive(true);
        
        AudioSourceManager.instance.StopPlayMusic();
        bestScoreGo.SetActive(true);

        //tween_score.PlayBackwards();
        txt_playerScore.transform.DOMove(scorePos,1);
    }
    public void ShowGameOver()
    {
        panel_gameOver.SetActive(true);
        UpdateHighestScore();

        bestScoreGo.SetActive(true);
        tween_score.PlayBackwards();
        imgGameOver.DOFade(1, 4);
    }
    private void Replay()
    {
        PlayButtonSound();
        ShowImageMask();
    }
    public void ChangeSoreUI(int score)
    {
        txt_playerScore.text = score.ToString();
    }
    public void UpdateHighestScore()
    {
        GlobalValue.coins += rocket.score / 10;
        txt_getCoins.text ="+" +(rocket.score / 10).ToString();
        if (rocket.score > GlobalValue.playerBestScore)
        {
            AudioSourceManager.instance.PlaySound(newBestScoreClip);
            GlobalValue.playerBestScore = rocket.score;
            GameManager.instance.UpdateHighetsScore();

            newGo.SetActive(true);
            txt_playerBestScore.text = GlobalValue.playerBestScore.ToString();
        }
        txt_playerBestScore.text = GlobalValue.playerBestScore.ToString();
    }
    public void GetReward()
    {
        GlobalValue.coins += rocket.score / 10;
        txt_getCoins.text = "+" +((rocket.score / 10)*2).ToString();
        btn_gameWinReplay.transform.position = btn_replayTargetTrans.position;
    }
    private void GetRocketHead()
    {
        PlayButtonSound();
        //if (GlobalValue.rocketHeadList.Length >= 15 && GlobalValue.coins < 400)
        //{
        //    btn_openOrCloseSlotMatchPanel.interactable = false;
        //}
        //else
        //{
        //    btn_openOrCloseSlotMatchPanel.interactable = true;
        //}
        EventSystem.current.IsPointerOverGameObject();
        tween_getGold.PlayForward();
        tween_addRocketLength.PlayForward();
        tween_getRocketHead.PlayForward();
        tween_svTrans.PlayForward();
        btn_returnPanelStart.gameObject.SetActive(true);
        btn_start.gameObject.SetActive(false);
    }
    private void ReturnPanelStart()
    {
        PlayButtonSound();
        tween_getGold.PlayBackwards();
        tween_getRocketHead.PlayBackwards();
        tween_addRocketLength.PlayBackwards();
        tween_svTrans.PlayBackwards();
        btn_start.gameObject.SetActive(true);
        btn_returnPanelStart.gameObject.SetActive(false);
    }
    /// <summary>
    /// ѡ�п����ʾ
    /// </summary>
    /// <param name="rocketIndex"></param>
    private void ChangeRocketSkin(int rocketIndex)
    {
        PlayButtonSound();
        for (int i = 0; i < btns_selectedSkin.Length; i++)
        {
            btns_selectedSkin[i].transform.Find("Img_Click").gameObject.SetActive(false);
        }

        btns_selectedSkin[rocketIndex].transform.Find("Img_Click").gameObject.SetActive(true);
        rocket.ChangeRocketSkin(rocketIndex);
    }
    /// <summary>
    /// ��Ƥ����������ʾ
    /// </summary>
    public void InitRocketSkin()
    {
        for (int i = 0; i < btns_selectedSkin.Length; i++)
        {
            btns_selectedSkin[i].transform.Find("Img_Lock").gameObject.SetActive(!GlobalValue.rocketHeadList[i]);
            btns_selectedSkin[i].GetComponent<Button>().interactable = GlobalValue.rocketHeadList[i];
        }
    }

    public void ShowScoreLevelUI(int scoreLevel)
    {
        HideScoreLevelUI();
        scoreUI[scoreLevel].SetActive(true);
        lastScoreLevelIndex = scoreLevel;
        Invoke("HideScoreLevelUI", 1);
    }
    private void HideScoreLevelUI()
    {
        scoreUI[lastScoreLevelIndex].SetActive(false);
    }
    public void ChangePlayerGoldNum()
    {
        txt_money.text = GlobalValue.coins.ToString();
    }
    public void OpenOrCloseSlotMatchPanel(bool open)
    {
        if (GlobalValue.rocketHeadList.Length >= 15 && GlobalValue.coins < 400)
        {
            panel_hint.SetActive(true);
            return;
        }
        PlayButtonSound();
        panel_slotMatch.SetActive(open);
    }
    public void CloseSlotMatchPanel()
    {
        panel_slotMatch.SetActive(false);
    }
    public void PlayButtonSound()
    {
        AudioSourceManager.instance.PlaySound(buttonClip);
    }
    public void PlayGetGoldSound()
    {
        AudioSourceManager.instance.PlaySound(getGoldClip);
    }

    private void GetName(string nameStr)
    {
        GlobalValue.playerName = nameStr;
        panel_getName.SetActive(false);
        Debug.Log(GlobalValue.playerName);
    }
    public void ShowHintPanel()
    {
        panel_hint.SetActive(true);
    }
}