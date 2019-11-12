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

    void TryParseByte(string message, int startIndex, int length)   //試作中
    {
        string receivedData;
        receivedData = message.Substring(5, 1);
        byte sw;
        byte.TryParse(receivedData, out sw);
    }

    void Start()
    {
        // 制御対象のオブジェクトを取得
        targetObject = GameObject.Find("Player");
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
        if (message[0] == 'S' && message[message.Length - 1] == 'E')
        {
            #region 必要に応じて変更すべき箇所

            string receivedData;

            // 必要な文字部分のバイト数（範囲）は常に把握する
            receivedData = message.Substring(1, 4);
            Debug.Log(receivedData);

            int vol;
            int.TryParse(receivedData, out vol);

            receivedData = message.Substring(5, 1);
            byte sw0;
            byte.TryParse(receivedData, out sw0);

            receivedData = message.Substring(6, 1);
            byte sw1;
            byte.TryParse(receivedData, out sw1);

            targetScript.vol = vol;
            targetScript.sw[0] = sw0;
            targetScript.sw[1] = sw1;
            // 必要な文字部分を抽出したら、データ形式に合わせてデコード、例えば以下のように。
            //float.TryParse(receivedData, out data);

            #endregion
        }
    }
}
