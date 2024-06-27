using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;

public class Rocket : MonoBehaviour
{
    private bool isStopPlay;
    [Header("***������ٶȺ���ת***")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [HideInInspector] public int score;//����
    [SerializeField] private float addScoreSpeed;
    private float currentScore;
    private float[] rocketLengthPosList;//�������λ��
    private float gameTime;
    [Header("***�������***")]
    [SerializeField] private Transform rocketTrans;
    [SerializeField] private Transform[] rocketBodyTrans;//���������
    [SerializeField] private Transform[] rocketBodyPartsTrans;//��������ģ��
    [SerializeField] private GameObject[] tileGos;//��������ʾ��ʾ
    [HideInInspector]public int currentIndex;//�ڼ��ڻ��
    private float addSpeed;
    private Tween rocketBodyPartsTween;
    [SerializeField] private Transform rocketHead;
    [Header("***�����������***")]
    [SerializeField] private CameraFollow cameraController;

    [Header("***����***")]
    [SerializeField] private GameObject lowTower;
    [SerializeField] private GameObject highTower;

    [Header("***��Ƥ��***")]
    [SerializeField] private GameObject[] attachGos;
    [SerializeField] private GameObject[] rocketBodyGos;
    [SerializeField] private GameObject[] rocketHeadGos;
    public Material[] attachMaterials;
    public Material[] rocketBodyMaterials;
    private MeshRenderer[] attachMeshRenders;
    private MeshRenderer[] rocketBodyMeshRenders;
    private int testIndex;

    [Header("***��Ч***")]
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private ParticleSystem lightEffect;//��ը����Ч
    [SerializeField] private ParticleSystem fireEffect;//����β��
    [SerializeField] private ParticleSystem[] clickEffect;//�������β��Ч��
    [SerializeField] private ParticleSystem addLengthEffect;
    [SerializeField] private ParticleSystem rocketStartEffect;
    [SerializeField] private ParticleSystem rocketBodyStartEffect;
    private Tween fireTween;
    private Tween clikeEffectTween;
    float index=1;
    int lastClickEffectIndex;
    [SerializeField] private Transform bottomRocketTrans;//��ʮһ��
    private float[] clickValue;

    [Header("***��Ч***")]
    [SerializeField] AudioClip[] rocketHeadClips;
    [SerializeField] AudioClip[] flyClips;
    [SerializeField] AudioClip gameWinClip;
    [SerializeField] AudioClip gameOverClip;
    [SerializeField] AudioClip[] judgeScoreClip;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] AudioClip addRocketLengthClip;

    [Header("***�����ը***")]
    [SerializeField] private GameObject rocketExplode;
    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraFollow>();
        //һ��ʼ���������ʾ
        for (int i = 0; i < GlobalValue.rocketLength; i++)
        {
            rocketBodyTrans[i].gameObject.SetActive(false);
        }
        //������ȱ��֮���λ�ø���
        rocketLengthPosList = new float[6] { -1.31f, -1.092f, -0.889f, -0.592f, -0.312f, 0f };
        //������
        attachMeshRenders = new MeshRenderer[attachMaterials.Length];
        for (int i = 0; i < attachGos.Length; i++)
        {
            attachMeshRenders[i] = attachGos[i].GetComponent<MeshRenderer>();
        }
        rocketBodyMeshRenders = new MeshRenderer[rocketBodyGos.Length];
        for (int i = 0; i < rocketBodyGos.Length; i++)
        {
            rocketBodyMeshRenders[i] = rocketBodyGos[i].GetComponent<MeshRenderer>();
        }
        ChangeRocketSkin(GlobalValue.rocketID);
        testIndex = GlobalValue.rocketID;
        //��ʼ��
        InitRocketLength();
        //�������λ������
        for (int i = 0; i < tileGos.Length; i++)
        {
            tileGos[i].SetActive(false);
        }
        //�������е���Ч
        HideAllEffect();
        clickValue = new float[10] {0,0,0.3f,0.3f,0.3f,0.1f,0.3f,0.3f,0.4f,-0.6f };
    }
    /// <summary>
    /// ����������Ч
    /// </summary>
    private void HideAllEffect()
    {
        explosionEffect.gameObject.SetActive(false);
        lightEffect.gameObject.SetActive(false);
        fireEffect.gameObject.SetActive(false);
        for (int i = 0; i < clickEffect.Length; i++)
        {
            clickEffect[i].gameObject.SetActive(false);
        }

        addLengthEffect.gameObject.SetActive(false);
        rocketStartEffect.gameObject.SetActive(false);
        rocketBodyStartEffect.gameObject.SetActive(false);

    }
    /// <summary>
    /// ��Ϸ����
    /// </summary>
    private void GameOver()
    {
        Interstitial.instance.LoadAd();
        Interstitial.instance.ShowAd();
        AudioSourceManager.instance.PlayMusic(gameOverClip);
        AudioSourceManager.instance.PlaySound(explosionClip);

        explosionEffect.gameObject.SetActive(true);
        explosionEffect.transform.position = rocketBodyPartsTrans[currentIndex].position;
        explosionEffect.Play();

        lightEffect.gameObject.SetActive(true);
        lightEffect.transform.position = rocketBodyPartsTrans[currentIndex].position;
        lightEffect.Play();

        cameraController.transform.DOShakePosition(0.4f, 1.8f, 10, 180, true);
        GameManager.instance.GameOver();
        UIManager.instance.ShowGameOver();

        GameObject go = Instantiate(rocketExplode, transform.position, Quaternion.identity);
        go.SetActive(true);
        cameraController.transform.SetParent(null);

        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        #region ���Դ���
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    testIndex++;
        //    ChangeRocketSkin(testIndex);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    AddRocketLength();
        //}
        #endregion

        if (GameManager.instance.gameState == GAMESTATE.NULL || GameManager.instance.gameState == GAMESTATE.GAMEOVER)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverGameObject() == true||isStopPlay==true)
            {
                return;
            }
            JugeGetScore();
        }
        if (currentIndex <= rocketBodyPartsTrans.Length - 1 && rocketBodyPartsTrans[currentIndex].localScale.z < 0.17f * 0.05f)
        {
            GameOver();
        }
        //����ƶ�
        transform.Translate(moveSpeed * (addSpeed + 1) * ((int)gameTime / 3 + 1) * Time.deltaTime * Vector3.up, Space.World);
        gameTime += Time.deltaTime;
        rocketHead.Rotate(transform.up, rotateSpeed * Time.deltaTime, Space.World);

        if (currentScore<score)
        {
            currentScore = Mathf.MoveTowards(currentScore,score,addScoreSpeed*Time.deltaTime);
            UIManager.instance.ChangeSoreUI((int)currentScore);
        }
    }
    private bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }
    /// <summary>
    /// �������
    /// </summary>
    private void LandPart(int scoreLevel)
    {
        if (currentIndex > rocketBodyTrans.Length - 1)
        {
            return;
        }
        //�������
        rocketBodyTrans[currentIndex].SetParent(null);
        AudioSourceManager.instance.PlayMusic(flyClips[currentIndex / 4]);
        //��������ʱ�������̶�����ֹͣ���̶���
        if (rocketBodyPartsTween != null)
        {
            rocketBodyPartsTween.Kill();
        }
        //������һ��
        if (currentIndex == rocketBodyTrans.Length - 1)
        {
            MoveCamera(true);
            //GetNewOffsetAndTarget(rocketHead,true);
            GameWin();
            isStopPlay = true;
            Debug.Log("��Ϸʤ����");
        }
        else//���������
        {
            MoveCamera();
            //GetNewOffsetAndTarget(rocketBodyPartsTrans[(rocketBodyPartsTrans.Length - 1 - currentIndex) / 2 + currentIndex + 1]);
            currentIndex++;
            addSpeed++;
            ChangeShort(scoreLevel);
        }
    }
    /// <summary>
    /// ������
    /// </summary>
    public void ChangeShort(int scoreLevel)
    {
        if (currentIndex > rocketBodyPartsTrans.Length - 1)
        {
            return;
        }
        if (currentIndex == rocketBodyPartsTrans.Length - 1)
        {
            rocketBodyPartsTween = rocketBodyPartsTrans[currentIndex].DOScaleZ(0, 2);
            tileGos[currentIndex - 1].SetActive(false);
            tileGos[currentIndex].SetActive(true);
        }
        else
        {
            rocketBodyPartsTween = rocketBodyPartsTrans[currentIndex].DOScaleZ(0, 3);
            if (currentIndex != 0)
            {
                tileGos[currentIndex - 1].SetActive(false);
            }
            tileGos[currentIndex].SetActive(true);
        }
        
        EffectMoveCallBack(scoreLevel);
    }
    private void EffectMoveCallBack(int scoreLevel)
    {
        if (currentIndex+1>=rocketBodyTrans.Length)
        {
            return;
        }
        if (currentIndex==0)//��һ��
        {
            EffectMove(bottomRocketTrans.localPosition,rocketBodyTrans[currentIndex+1].localPosition,0.5f,scoreLevel);
        }
        else
        {
            EffectMove(rocketBodyTrans[currentIndex].localPosition,rocketBodyTrans[currentIndex+1].localPosition,0.5f,scoreLevel);    
        }
    }
    /// <summary>
    /// ������ƶ�
    /// </summary>
    private void MoveCamera(bool stopFollow = false)
    {
        cameraController.MoveController(stopFollow);
    }
    /// <summary>
    /// �жϵ÷֣�ͨ��z����ٵ������ж�
    /// </summary>
    public void JugeGetScore()
    {
        int scoreLevel;
        if (rocketBodyPartsTrans[currentIndex].localScale.z < 0.17f * 0.05f)
        {
            //��Ϸ����
            GameOver();
            return;
        }
        else if (rocketBodyPartsTrans[currentIndex].localScale.z < 0.17f * 0.1f)
        {
            //Excellent
            AudioSourceManager.instance.PlaySound(judgeScoreClip[0]);
            scoreLevel = 3;
            UIManager.instance.ShowScoreLevelUI(3);
            score += (currentIndex + 1) * 20;
        }
        else if (rocketBodyPartsTrans[currentIndex].localScale.z < 0.17f * 0.25f)
        {
            //perfect
            AudioSourceManager.instance.PlaySound(judgeScoreClip[1]);
            scoreLevel = 2;
            UIManager.instance.ShowScoreLevelUI(2);
            score += (currentIndex + 1) * 10;
        }
        else if (rocketBodyPartsTrans[currentIndex].localScale.z < 0.17f*0.5f)
        {
            //good
            AudioSourceManager.instance.PlaySound(judgeScoreClip[2]);
            scoreLevel = 1;
            UIManager.instance.ShowScoreLevelUI(1);
            score += (currentIndex + 1) * 6;
        }
        else
        {
            //ok
            AudioSourceManager.instance.PlaySound(judgeScoreClip[2]);
            scoreLevel = 0;
            UIManager.instance.ShowScoreLevelUI(0);
            score += (currentIndex + 1) * 2;
        }
        LandPart(scoreLevel);
    }
    private void InitRocketLength()
    {
        currentIndex = 10 - GlobalValue.rocketLength;
        //������ȱ仯
        rocketTrans.localPosition = new Vector3(0,
             rocketLengthPosList[GlobalValue.rocketLength - 5], 0);
        for (int i = currentIndex; i < rocketBodyTrans.Length; i++)
        {
            rocketBodyTrans[i].gameObject.SetActive(true);
        }
        //�����λ��
        cameraController.ChangeCameraPos();
        //������ʾ
        if (GlobalValue.rocketLength > 7)
        {
            highTower.SetActive(true);
            lowTower.SetActive(false);
        }
        else
        {
            highTower.SetActive(false);
            lowTower.SetActive(true);
        }
    }
    /// <summary>
    /// ���ӻ������
    /// </summary>
    public void AddRocketLength()
    {
        AudioSourceManager.instance.PlaySound(addRocketLengthClip);
        GlobalValue.rocketLength += 1;
        InitRocketLength();
        index += 0.1f;
        addLengthEffect.gameObject.SetActive(true);
        addLengthEffect.transform.localPosition = rocketTrans.transform.localPosition+new Vector3(0,1,0);
        addLengthEffect.transform.localScale = Vector3.one*0.2f*(index);
        addLengthEffect.Play();
    }
    /// <summary>
    /// ��Ƥ��
    /// </summary>
    /// <param name="rocketIndex"></param>
    public void ChangeRocketSkin(int rocketIndex)
    {
        //�������ӵ�Ƥ��
        for (int i = 0; i < attachGos.Length; i++)
        {
            attachMeshRenders[i].material = attachMaterials[rocketIndex];
        }
        //������������Ƥ��
        for (int i = 0; i < rocketBodyGos.Length; i++)
        {
            rocketBodyMeshRenders[i].material = rocketBodyMaterials[rocketIndex];
        }
        //���ͷ
        for (int i = 0; i < rocketHeadGos.Length; i++)
        {
            rocketHeadGos[i].SetActive(false);
        }
        rocketHeadGos[rocketIndex].SetActive(true);
        GlobalValue.rocketID = rocketIndex;
    }
    /// <summary>
    /// ��������β����Ч
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="moveTime"></param>
    private void EffectMove(Vector3 startPos, Vector3 endPos, float moveTime,int scoreLevel)
    {
        fireTween.Pause();
        fireEffect.Stop();
        //��Ч����ʼλ�ã���ʼ����
        fireEffect.transform.localPosition = startPos;
        fireEffect.Play();
        //��Ч�ƶ�
        fireEffect.transform.DOLocalMove(endPos, moveTime);
        fireTween.Play();

        clickEffect[lastClickEffectIndex].gameObject.SetActive(false);
        clickEffect[scoreLevel].transform.localPosition = startPos-new Vector3(0,clickValue[currentIndex],0);
        clickEffect[scoreLevel].gameObject.SetActive(true);
        clickEffect[scoreLevel].Play();
        clikeEffectTween.Pause();
        clikeEffectTween = clickEffect[scoreLevel].transform.DOLocalMove(endPos-new Vector3(0,clickValue[currentIndex],0), moveTime);
        clikeEffectTween.Play();
        lastClickEffectIndex = scoreLevel;
    }
    public void StartGame()
    {
        fireEffect.gameObject.SetActive(true);
        fireEffect.Play();
        //GameManager.instance.gameState = GAMESTATE.PLAYING;

        rocketStartEffect.gameObject.SetActive(true);
        rocketStartEffect.Play();

        rocketBodyStartEffect.gameObject.SetActive(true);
        rocketBodyStartEffect.Play();

        AudioSourceManager.instance.PlaySound(rocketHeadClips[GlobalValue.rocketID]);
    }
    private void GameWin()
    {
        GameManager.instance.Victory();
        UIManager.instance.UpdateHighestScore();
        ShowGameWins();
        //Invoke("ShowGameWin",1);
    }
    private void ShowGameWins()
    {
        AudioSourceManager.instance.PlaySound(gameWinClip);
        UIManager.instance.ShowGameWin();
    }
    #region ���������
    ///// <summary>
    ///// ���������
    ///// </summary>
    ///// <param name="newFollowTarget"></param>
    ///// <param name="stopFollow"></param>
    //private void GetNewOffsetAndTarget(Transform newFollowTarget, bool stopFollow = false)
    //{
    //    cameraController.GetNewOffsetAndFollowTarget(newFollowTarget, stopFollow);
    //} 
    #endregion
}
