// Unityでシリアル通信、Arduinoと連携する雛形
// 受信データでGameObjectを制御するソース
// 例えば空のGameObjectでも作って、それに関連付けする

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSomething : MonoBehaviour
{
    // 制御対象のオブジェクト用に宣言しておいて、Start関数内で名前で検索
    // 制御対象のオブジェクト名がPlayerで、Playerに関連付けられたスクリプトにPlayerの処理が書かれている前提
    GameObject targetObject;
    playerController targetScript;

    // シリアル通信のクラス、クラス名は正しく書くこと
    public SerialHandler serialHandler;

    void Start()
    {
        // 制御対象のオブジェクトを取得
        targetObject = GameObject.Find("スティック左");
        // 制御対象に関連付けられたスクリプトを取得、一般的にはこのスクリプトのメンバにアクセスして処理をすることが多いと思うので。
        targetScript = targetObject.GetComponent<playerController>();

        // 信号受信時に呼ばれる関数としてOnDataReceived関数を登録
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void Update()
    {
        //文字列を送信するなら例えばココ
        //serialHandler.Write("hogehoge");
    }

    //受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        // ここでデコード処理等を記述
        if (message == null)
            return;

        // 受け取ったデータを数値に変換
        if (message[0] == 'S' && message[1] == 'A' && message[message.Length - 1] == 'E')
        {
            #region 必要に応じて変更すべき箇所

            // 必要な文字部分のバイト数（範囲）は常に把握する
            string receivedData = message.Substring(1, 16);
            Debug.Log("allAccelData : " + receivedData);

            targetScript.accel[0] = DecodeFloat(2, 5);
            targetScript.accel[1] = DecodeFloat(7, 5);
            targetScript.accel[2] = DecodeFloat(12, 5);
            // 必要な文字部分を抽出したら、データ形式に合わせてデコード、例えば以下のように。
            //float.TryParse(receivedData, out data);

            #endregion
        }
        if (message[0] == 'S' && message[1] == 'G' && message[message.Length - 1] == 'E')
        {
            string receivedData = message.Substring(1, 13);
            Debug.Log("allGyroData : " + receivedData);

            targetScript.gyro[0] = DecodeFloat(2, 4);
            targetScript.gyro[1] = DecodeFloat(6, 4);
            targetScript.gyro[2] = DecodeFloat(10, 4);
        }

        float DecodeFloat(int start, int range)
        {
            string receivedData;

            receivedData = message.Substring(start, range);
            Debug.Log(receivedData);

            float vol;
            float.TryParse(receivedData, out vol);

            return vol;
        }
    }
}
