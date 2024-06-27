using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("***»ð¼ý±¬Õ¨Ô¤ÖÆÌå***")]
    [SerializeField] Rocket rocket;
    [SerializeField] private GameObject[] rocketHeadGos;
    [SerializeField] private GameObject[] rocketBodyGos;
    [SerializeField] private MeshRenderer[] attachMr;
    [SerializeField] private MeshRenderer[] rockerBodyMr;
    // Start is called before the first frame update
    void Start()
    {
        //»ð¼ýÍ·Æ¤·ô
        for (int i = 0; i < rocketHeadGos.Length; i++)
        {
            rocketHeadGos[i].SetActive(false);
        }
        rocketHeadGos[GlobalValue.rocketID].SetActive(true);
        //»ð¼ýÉíÌåÆ¤·ô
        for (int i = rocket.currentIndex; i <rocketBodyGos.Length; i++)
        {
            rocketBodyGos[i].SetActive(true);
        }
        for (int i = 0; i < rocket.currentIndex; i++)
        {
            rocketBodyGos[i].SetActive(false);
        }
        //»ð¼ýÁ¬½Óµã
        for (int i = 0; i < attachMr.Length; i++)
        {
            attachMr[i].material = rocket.attachMaterials[GlobalValue.rocketID];
        }
        for (int i = 0; i < rockerBodyMr.Length; i++)
        {
            rockerBodyMr[i].material = rocket.rocketBodyMaterials[GlobalValue.rocketID];
        }    
        //±¬Õ¨
        Explode();
    }
    private void Explode()
    {
        Collider[] boomObj = Physics.OverlapSphere( transform.position,5);
        foreach (Collider item in boomObj)
        {
            Rigidbody rigid = item.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.AddExplosionForce(400, transform.position, 5, 25);
                rigid.useGravity = true;
            }
             
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
