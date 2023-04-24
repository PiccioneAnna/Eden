using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI uI;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (uI == null)
        {
            uI = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
