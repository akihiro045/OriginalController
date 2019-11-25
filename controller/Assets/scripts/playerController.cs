﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerController : MonoBehaviour
{
    public Text textInfoAccel;
    public Text textInfoGyro;
    public GameObject[] Stick;
    string str;

    private float[] firstPosition = new float[3];
    private float[] firstRotation = new float[3];
    private Vector3 firstrotation;
    public float[] accel = new float[3];
    public float[] gyro = new float[3];

    private float[] oldAccel = new float[3];
    private float[] oldGyro = new float[3];

    public bool[] jklPress = new bool[3];
    public bool[] jklToggle = new bool[3];
    float startTime;

    void PlayerMove()
    {
        float[] tmpAccel = new float[3];
        float[] tmpGyro = new float[3];
        for (int i = 0; i < 3; i++)
        {
            tmpAccel[i] = oldAccel[i] - accel[i];
            tmpGyro[i] = oldGyro[i] - gyro[i];
        }

        //this.transform.position += new Vector3(tmpAccel[0], tmpAccel[2], tmpAccel[1]);
        this.transform.rotation = Quaternion.Euler(firstRotation[0] + gyro[0], /*firstRotation[1] + gyro[1], firstRotation[2] - gyro[2]*/0, 0);
        // if (tmpAccel[0] > 2 || tmpAccel[0] < -2)
        // {
        //     this.transform.position += new Vector3(tmpAccel[0], 0.0f, 0.0f);
        // }
        // if (tmpAccel[1] > 2 || tmpAccel[1] < -2)
        // {
        //     this.transform.position += new Vector3(0.0f, 0.0f, tmpAccel[1]);
        // }
        // if (tmpAccel[2] > 10 || tmpAccel[2] < 9)
        // {
        //     this.transform.position += new Vector3(0.0f, tmpAccel[2], 0.0f);
        // }
        // if (Input.GetKey(KeyCode.UpArrow))
        //     this.transform.position += new Vector3(0f, 0f, moveValue);
        // if (Input.GetKey(KeyCode.DownArrow))
        //     this.transform.position += new Vector3(0f, 0f, -moveValue);

        // if (Input.GetKey(KeyCode.LeftArrow))
        //     this.transform.position += new Vector3(-moveValue, 0f, 0f);
        // if (Input.GetKey(KeyCode.RightArrow))
        //     this.transform.position += new Vector3(moveValue, 0f, 0f);

        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     GetComponent<Rigidbody>().velocity = Vector3.up * jumpValue;
        // }

        if (Input.GetKeyDown(KeyCode.J))
        {
            jklPress[0] = true;
            jklToggle[0] = !jklToggle[0];
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            jklPress[1] = true;
            jklToggle[1] = true;
        }
        if (Input.GetKeyUp(KeyCode.K))//Jkeyの２回目の操作と同じ原理
        {
            jklPress[1] = true;
            jklToggle[1] = false;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            jklPress[2] = true;
            jklToggle[2] = true;
            startTime = 0;//計測開始
        }
        if (startTime >= 0)
        {
            startTime += Time.deltaTime;
            if (startTime >= 3)
            {
                jklPress[2] = true;
                jklToggle[2] = false;
                startTime = -1;
            }
        }
        Debug.Log(startTime);
    }

    void DebugText()
    {
        //str = string.Format("{0:F2}, {1:F2}, {2:F2}", this.transform.position.x, this.transform.position.y, this.transform.position.z);
        str = string.Format("{0} {1} {2}", accel[0], accel[1], accel[2]);
        textInfoAccel.text = str;
        str = string.Format("{0} {1} {2}", gyro[0], gyro[1], gyro[2]);
        textInfoGyro.text = str;
    }

    void Start()
    {
        DebugText();

        for (int i = 0; i < 3; i++)
        {
            accel[i] = 0;
            gyro[i] = 0;

            oldAccel[i] = 0;
            oldGyro[i] = 0;
        }

        for (int n = 0; n < 3; n++)
        {
            jklPress[n] = false;
            jklToggle[n] = false;
        }
        startTime = -1;

        firstRotation[0] = 270 + gyro[0];
        firstRotation[1] = gyro[2];
        firstRotation[2] = gyro[1];
        Debug.Log("firstRotation x:" + firstRotation[0]);
        Debug.Log("firstRotation y:" + firstRotation[1]);
        Debug.Log("firstRotation z:" + firstRotation[2]);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        DebugText();

    }
}
