using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Test2 : MonoBehaviour
{
    private Button btn_show;
    [SerializeField] private Test1 script1;
    private void Awake()
    {
        btn_show = GetComponent<Button>();
    }
    void Start()
    {
        btn_show.onClick.AddListener(()=>
        {
            script1.action();
            Debug.Log("显示金币变化数值，和奖励");
        });
    }
}
