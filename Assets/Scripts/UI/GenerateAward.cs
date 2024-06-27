using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GenerateAward : MonoBehaviour
{
    [SerializeField] private GameObject txt_award;
    private int count=8;
    [SerializeField] Vector3 initPos;
    // Start is called before the first frame update
    void Start()
    {
        float angle = 360 / count;
        float addtionAngle = -45;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(txt_award, transform.position, Quaternion.identity);

            addtionAngle += angle;
            obj.transform.Rotate(Vector3.forward, addtionAngle);

            obj.GetComponent<TextMeshProUGUI>().text = UIManager.instance.awardNum[i].ToString();
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
            Debug.Log("正常生成文字");
        }
    }

}
