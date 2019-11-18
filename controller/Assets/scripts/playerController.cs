using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    float moveValue;
    public float jumpValue = 5f;

    public Text textInfoAccel;
    public Text textInfoGyro;
    string str;

    public float[] accel = new float[3];
    public float[] gyro = new float[3];

    private float[] oldAccel = new float[3];
    private float[] oldGyro = new float[3];

    void PlayerMove()
    {
        float[] tmpAccel = new float[3];
        for (int i = 0; i < 3; i++)
        {
            tmpAccel[i] = oldAccel[i] - accel[i];
        }

        this.transform.position += new Vector3(tmpAccel[0], tmpAccel[1], tmpAccel[2]);

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
        moveValue = 0.1f;
        jumpValue = 5f;

        DebugText();

        for (int i = 0; i < 3; i++)
        {
            accel[i] = 0;
            gyro[i] = 0;

            oldAccel[i] = 0;
            oldGyro[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        DebugText();

        for (int i = 0; i < 3; i++)
        {
            oldAccel[i] = accel[i];
            oldGyro[i] = oldGyro[i];
        }
    }
}
