using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canshu : MonoBehaviour
{
    int a = 1;
    int b = 2;
    public int c=5;
    // Start is called before the first frame update
    void Start()
    {
        GameObject a = new GameObject();

        //Add(ref a, b);
        Add2();
        
        Debug.Log("a的值是" + a);
        Debug.Log("b的值是" + b);
    }

    private void Add(ref int a,int b)
    {
        a += 2;
        b += 3;
    }
    private void Add2()
    {
        a += 4;
        b += 5;
    }
    public static void Mad(string a)
    {

    }
}
