using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RcpObject : MonoBehaviour
{
    [SerializeField]
    private Image backImage = null;

    [SerializeField]
    private Image rcpImage = null;

    private Genome genom = null;
    private bool live = true;
    private int state = 0;

    public int State
    {
        get { return state; }
        set { state = value; }
    }

    public bool Live
    {
        get { return live; }
    }

    public void SetRct(Sprite sp, int st)
    {
        rcpImage.sprite = sp;
        state = st;
    }

    public void SetLive(bool value)
    {
        live = value;
        if (value)
        {
            rcpImage.color = Color.white;
        }
        else
        {
            rcpImage.color = new Color32(100, 100, 100, 255);
        }
    }
}
