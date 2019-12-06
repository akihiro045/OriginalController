using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

struct accel
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}
struct gyro
{
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

public class playerController : MonoBehaviour
{
    public Text textInfoAccel;
    public Text textInfoGyro;
    public GameObject[] Stick;
    string str;

    private float[] firstPosition = new float[3];
    private float[] firstRotation = new float[3];
    public float[] accel = new float[3];
    public float[] gyro = new float[3];

    private float[] oldAccel = new float[3];
    private float[] oldGyro = new float[3];

    public bool[] jklPress = new bool[3];
    public bool[] jklToggle = new bool[3];
    float startTime;

    float[] velocityGyro = new float[3];
    float[] velocityAccel = new float[3];

    public bool weekHit;
    public bool strongHit;

    private void OnCollisionEnter(Collision other)
    {
        //2つの音
        if (gyro[0] > 80 || gyro[0] < -80)
        {
            strongHit = true;
        }
        else
        {
            weekHit = true;
        }
        //other.rigidbody.isKinematic = true;
        Debug.Log(other.gameObject);
    }
    private void OnCollisionExit(Collision other)
    {
        //other.rigidbody.isKinematic = false;
    }
    void PlayerMove()
    {
        this.transform.position = new Vector3(firstPosition[0], //- VelocityAccel(oldAccel, accel, velocityAccel, 0),
                                              firstPosition[1], //+ VelocityAccel(oldAccel, accel, velocityAccel, 2),
                                            firstPosition[2] /*+ VelocityAccel(oldAccel, accel, velocityAccel, 1) * 100*/);
        // this.transform.position = new Vector3(0, VelocityAccel(oldAccel, accel, velocityAccel, 2), 0);
        // this.transform.position = new Vector3(0, 0, VelocityAccel(oldAccel, accel, velocityAccel, 1));
        MoveGyro();
        // firstRotation[1] - VelocityGyro(oldGyro, gyro, velocityGyro, 2),
        // 0);

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
    }

    void MoveGyro()
    {
        //センサーのxが反転しているとき
        if (accel[2] > 0)
        {
            this.transform.rotation = Quaternion.Euler(firstRotation[0] + VelocityGyro(oldGyro, gyro, velocityGyro, 0), firstRotation[1], firstRotation[2]);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(firstRotation[0] - VelocityGyro(oldGyro, gyro, velocityGyro, 0), firstRotation[1], firstRotation[2]);
        }

        // if (accel[0] >0)
        // {
        //     this.transform.rotation = Quaternion.Euler(firstRotation[0] + VelocityGyro(oldGyro, gyro, velocityGyro, 2), firstRotation[1], firstRotation[2]);
        // }
        // else if(accel[0]<0)
        // {

        // }
    }

    float VelocityGyro(float[] oldGyro, float[] gyro, float[] velocity, int num)
    {
        velocity[num] += ((gyro[num] + oldGyro[num]) / 2) * Time.deltaTime;
        oldGyro[num] = gyro[num];
        return velocity[num];
    }
    float VelocityAccel(float[] oldAccel, float[] accel, float[] velocity, int num)
    {
        velocity[num] = ((accel[0] + oldAccel[0]) / 2) * Time.deltaTime;
        oldAccel[num] = accel[num];
        return velocity[num];
    }
    void DebugText()
    {
        //str = string.Format("{0:F2}, {1:F2}, {2:F2}", this.transform.position.x, this.transform.position.y, this.transform.position.z);
        str = string.Format("{0} {1} {2}", accel[0], accel[1], accel[2]);
        textInfoAccel.text = str;
        str = string.Format("{0} {1} {2}", gyro[0], gyro[1], gyro[2]);
        textInfoGyro.text = str;
    }

    private void Awake()
    {
        weekHit = false;
        strongHit = false;
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
        firstPosition[0] = this.transform.position.x;
        firstPosition[1] = this.transform.position.y;
        firstPosition[2] = this.transform.position.z;
        Debug.Log("firstPosition x:" + firstPosition[0]);
        Debug.Log("firstPosition y:" + firstPosition[1]);
        Debug.Log("firstPosition z:" + firstPosition[2]);


        firstRotation[0] = this.transform.eulerAngles.x;
        firstRotation[1] = this.transform.eulerAngles.y;
        firstRotation[2] = this.transform.eulerAngles.z;
        Debug.Log("firstRotation x:" + firstRotation[0]);
        Debug.Log("firstRotation y:" + firstRotation[1]);
        Debug.Log("firstRotation z:" + firstRotation[2]);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();

        DebugText();

        oldGyro[0] = gyro[0];
    }
}
