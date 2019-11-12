using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    float moveValue;
    public float jumpValue = 5f;

    public Text textInfo;
    string str;

    public int vol;
    public byte[] sw = new byte[2];
    public byte[] swOld = new byte[2];

    void PlayerMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            this.transform.position += new Vector3(0f, 0f, moveValue);
        if (Input.GetKey(KeyCode.DownArrow))
            this.transform.position += new Vector3(0f, 0f, -moveValue);

        if (Input.GetKey(KeyCode.LeftArrow))
            this.transform.position += new Vector3(-moveValue, 0f, 0f);
        if (Input.GetKey(KeyCode.RightArrow))
            this.transform.position += new Vector3(moveValue, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpValue;
        }

        if (swOld[0] == 1 && sw[0] == 0)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * jumpValue;
        }
        if (0 == sw[1])
        {
            this.transform.position += new Vector3(0f, 0f, moveValue);
        }
    }

    void DebugText()
    {
        //str = string.Format("{0:F2}, {1:F2}, {2:F2}", this.transform.position.x, this.transform.position.y, this.transform.position.z);
        str = string.Format("{0} {1} {2}", vol, sw[0], sw[1]);
        textInfo.text = str;
    }

    void Start()
    {
        moveValue = 0.1f;
        jumpValue = 5f;

        DebugText();

        vol = 0;
        for (int i = 0; i < 2; i++)
        {
            sw[i] = 1;
            swOld[i] = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        DebugText();

        for (int i = 0; i < sw.Length; i++)
        {
            sw[i] = swOld[i];
        }
    }
}
