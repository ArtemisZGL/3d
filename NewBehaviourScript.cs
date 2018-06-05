using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour {

    private Button yourButton;
    public Text text;
    private int frame = 30;
    private int count = 0;
    private bool click;
    private bool inORout;
    float rotatex = 0;
    float rotatex2 = -90;
    public float size;
    float sizey;
    float sizey2 = 0;

    // Use this for initialization  
    void Start()
    {
        Button btn = this.gameObject.GetComponent<Button>();
        btn.onClick.AddListener(button_click);
        click = false;
        sizey = size;
    }

    void Update()
    {
        if(click)
        {
            if (!inORout)
            {
                rotatex -= 90f / frame;         //绕x轴旋转90度
                sizey -= size / frame;
                text.transform.rotation = Quaternion.Euler(rotatex, 0, 0);
                text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, sizey);
                count++;
                if (count == frame - 1)
                {
                    text.gameObject.SetActive(false);
                    count = 0;
                    click = false;
                    rotatex = 0;
                    sizey = size;
                }

            }
            else
            {
                rotatex2 += 90f / frame;
                sizey2 += size / frame;
                text.transform.rotation = Quaternion.Euler(rotatex2, 0, 0);
                text.rectTransform.sizeDelta = new Vector2(text.rectTransform.sizeDelta.x, sizey2);
                if (count == 0)
                {
                    text.gameObject.SetActive(true);
                }
                count++;
                if (count == frame - 1)
                {
                    count = 0;
                    click = false;
                    rotatex2 = -90;
                    sizey2 = 0;
                }
            }
        }
    }

    void button_click()
    {
        click = true;
        if (text.gameObject.activeSelf)
        {
            inORout = false;
        }
        else
            inORout = true;
    }
}
