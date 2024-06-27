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

    [Header("***第一次进入游戏，输入玩家名字***")]
    [SerializeField] private GameObject panel_getName;
    [SerializeField] private TMP_InputField btn_input;

    [Header("***开始面板***")]
    [SerializeField] Button btn_start;
    [SerializeField] TextMeshProUGUI txt_btnStart;
    [SerializeField] GameObject panel_start;

    [SerializeField] GameObject bestScoreGo;
    [SerializeField] GameObject targetTrans_score;
    Tween tween_score;
    Vector3 scorePos;

    [SerializeField] TextMeshProUGUI txt_getGold;

    [Header("***提示面板***")]
    [SerializeField] GameObject panel_hint;
    [SerializeField] Button btn_closeHintPanel;

    [Header("***数值显示Ui***")]
    [SerializeField] Image imageMask;
    [SerializeField] TextMeshProUGUI txt_playerScore;
    [SerializeField] TextMeshProUGUI txt_playerBestScore;
    [SerializeField] TextMeshProUGUI txt_money;
    [SerializeField] GameObject newGo;

    [Header("***倒计时***")]
    [SerializeField] TextMeshProUGUI txt_countNum;
    [SerializeField] GameObject countNumGo;
    int countNum;
    [SerializeField] AudioClip[] countDownClips;
    [SerializeField] AudioClip takeOffClip;

    [Header("***设置面板***")]
    [SerializeField] Sprite muteSprite;
    [SerializeField] Sprite volSprite;
    [SerializeField] Image volImage;

    [SerializeField] Button btn_resetData;
    [SerializeField] Button btn_volum;

    [SerializeField] Button btn_set;
    [SerializeField] Button btn_close;
    [SerializeField] GameObject panel_set;

    [Header("***帮助面板***")]
    [SerializeField] GameObject panel_Help;
    [SerializeField] Button btn_help;
    [SerializeField] Button btn_ok;
    private bool ifOpen;

    [Header("***转盘抽奖面板***")]
    [SerializeField] GameObject panel_GetGold;
    [SerializeField] Button btn_closeGetGoldPanel;
    [HideInInspector] public int[] awardNum = new int[] { 850, 750, 650, 450, 350, 250, 150, 50 };

    [SerializeField] AudioClip getGoldClip;

    [Header("***排行榜***")]
    //[SerializeField] private LeaderBoard leadBoard;
    [SerializeField] private GameObject panel_leaderBoard;
    [SerializeField] private Button btn_closeLeaderBoard;
    [SerializeField] private Button btn_openLeaderBoard;

    [Header("***老虎机***")]
    [SerializeField] GameObject panel_slotMatch;
    public Button btn_openOrCloseSlotMatchPanel;
    private bool open = true;

    [Header("***增加火箭长度***")]
    [SerializeField] GameObject longestGo;
    [SerializeField] GameObject addLengthGo;
    [SerializeField] Button btn_addLength;
    [SerializeField] TextMeshProUGUI txt_AddLengthMoney;
    [SerializeField] Rocket rocket;
    int consumeCoins;

    [Header("***结算页面***")]
    [SerializeField] GameObject panel_gameWin;
    [SerializeField] GameObject panel_gameOver;
    [SerializeField] Button btn_gameOverReplay;
    [SerializeField] Button btn_gameWinReplay;
    [SerializeField] Image imgGameOver;
    [SerializeField] TextMeshProUGUI txt_getCoins;

    [SerializeField] AudioClip newBestScoreClip;
    [SerializeField] Transform btn_replayTargetTrans;

    [Header("***主页面板的ui动画***")]
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

    [Header("***切换皮肤的按钮***")]
    [SerializeField] private Button[] btns_selectedSkin;
    UnityAction action;

    [Header("***评分***")]
    [SerializeField] private GameObject[] scoreUI;
    private int lastScoreLevelIndex;

    [Header("***音效资源***")]
    [SerializeField] AudioClip buttonClip;
    void Start()
    {
        GlobalValue.HasRocketHeadList();
        instance = this;

        scorePos = txt_playerScore.transform.position;
        //第一次进入游戏，输入名字
        GlobalValue.IsFirstEnter();
        if (GlobalValue.firstEnter==false)
        {
            panel_getName.SetActive(true);
        }
        
        //输入按钮注册
        btn_input.onEndEdit.AddListener(GetName);
        //主页
        btn_start.onClick.AddListener(StartGame);
        //设置页面按钮注册
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
        //帮助面板
        btn_help.onClick.AddListener(OpenHelpPanel);
        btn_ok.onClick.AddListener(OpenHelpPanel);
        //转盘抽奖
        btn_getGold.onClick.AddListener(() => {
            panel_GetGold.SetActive(true);
            PlayButtonSound();
        });
        btn_closeGetGoldPanel.onClick.AddListener(() => {
            panel_GetGold.SetActive(false);
            PlayButtonSound();
        });
        //增加火箭长度
        btn_addLength.onClick.AddListener(AddRocketLength);
        //排行榜
        btn_openLeaderBoard.onClick.AddListener(delegate { OpenOrClosePanelLeader(open); });
        btn_closeLeaderBoard.onClick.AddListener(delegate { OpenOrClosePanelLeader(!open); });
        //结算页面
        btn_gameOverReplay.onClick.AddListener(Replay);
        btn_gameWinReplay.onClick.AddListener(Replay);
        //主页按钮事件
        btn_getHead.onClick.AddListener(GetRocketHead);
        btn_returnPanelStart.onClick.AddListener(ReturnPanelStart);
        //老虎机按钮注册事件
        btn_openOrCloseSlotMatchPanel.onClick.AddListener(delegate { OpenOrCloseSlotMatchPanel(open); });
        //提示面板关闭按钮
        btn_closeHintPanel.onClick.AddListener(()=> {
            PlayButtonSound();
            panel_hint.SetActive(false);
        });
        //切换皮肤的按钮
        for (int i = 0; i < btns_selectedSkin.Length; i++)
        {
            int index = new();
            index = i;
            btns_selectedSkin[index].onClick.AddListener(delegate { ChangeRocketSkin(index); });
        }
        //排行榜数据提前获取
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
    #region 转场黑屏
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
        //UI退出屏幕
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
    /// 音量
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
            Debug.Log("增加火箭长度的金币不足");
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
        //更新下次升级所需的金币显示
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
    /// 选中框的显示
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
    /// 让皮肤锁正常显示
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