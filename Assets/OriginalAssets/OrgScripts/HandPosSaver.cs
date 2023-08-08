using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //ファイルに書き込むために必要
using System; //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要

public class HandPosSaver : MonoBehaviour
{
    StreamWriter sw_x; //x座標記録用
    StreamWriter sw_y; //y座標記録用
    StreamWriter sw_z; //z座標記録用
    public GameObject handAva, realHead;//位置を記録したいオブジェクト用変数
    DateTime t2; //時刻用の変数

    void Start()
    {
        //時間を取得
        t2 = DateTime.Now;

        //csvのとき
        string file4 = Application.persistentDataPath + "/HandAvaPos_x.csv";
        string file5 = Application.persistentDataPath + "/HandAvaPos_y.csv";
        string file6 = Application.persistentDataPath + "/HandAvaPos_z.csv";

        //txtのとき
        //string file1 = Application.persistentDataPath + "/BallPos_x.txt";
        //string file2 = Application.persistentDataPath + "/BallPos_y.txt";
        //string file3 = Application.persistentDataPath + "/BallPos_z.txt";

        if (!File.Exists(file4))
        {
            Debug.Log("ファイル4を作成します");
            sw_x = File.CreateText(file4);
            sw_x.Flush();
            sw_x.Dispose();
        }
        else
        {
            Debug.Log("The file4 already exists.ファイル4は既に存在しています");
        }
        
        if (!File.Exists(file5))
        {
            Debug.Log("ファイル5を作成します");
            sw_y = File.CreateText(file5);
            sw_y.Flush();
            sw_y.Dispose();
        }
        else
        {
            Debug.Log("The file5 already exists.ファイル5は既に存在しています");
        }

        if (!File.Exists(file6))
        {
            Debug.Log("ファイル6を作成します");
            sw_z = File.CreateText(file6);
            sw_z.Flush();
            sw_z.Dispose();
        }
        else
        {
            Debug.Log("The file6 already exists.ファイル6は既に存在しています");
        }

        //UTF-8で生成...2番目の引数はtrueで末尾に追記，falseでファイルごと上書き．
        sw_x = new StreamWriter(file4, true, Encoding.UTF8);
        sw_y = new StreamWriter(file5, true, Encoding.UTF8);
        sw_z = new StreamWriter(file6, true, Encoding.UTF8);

        //改行
        sw_x.WriteLine();
        sw_y.WriteLine();
        sw_z.WriteLine();

        //時刻書き込み
        sw_x.WriteLine(Convert.ToString(t2));
        sw_y.WriteLine(Convert.ToString(t2));
        sw_z.WriteLine(Convert.ToString(t2));

        //Debug.Log(t2);
    }

    void Update()
    {
        //オブジェクトのtransformを取得
        Transform handAvaTransform = handAva.transform;
        Transform headTransform = realHead.transform;

        //オブジェクトのローカル座標を取得
        Vector3 handAvaPos = handAvaTransform.position;
        Vector3 headPos = headTransform.position;

        float x = handAvaPos.x - headPos.x;
        float y = handAvaPos.y - headPos.y;
        float z = handAvaPos.z - headPos.z;

        //Debug.Log($"{x},{z}");

        //ファイルの末尾に値を追加（Convertでfloat型の座標値をString型に変換している）
        //csvのとき
        sw_x.WriteLine(Convert.ToString(x));
        sw_y.WriteLine(Convert.ToString(y));
        sw_z.WriteLine(Convert.ToString(z));


        //txtのとき
        //sw_x.Write(Convert.ToString(x) + ",");
        //sw_y.Write(Convert.ToString(y) + ",");
        //sw_z.Write(Convert.ToString(z) + ",");

        /*if (CentralBall.transform.position.y <= -0.5f)//ボール落下時の処理
        {
            //改行
            sw_x.WriteLine();
            sw_y.WriteLine();
            sw_z.WriteLine();

            //時刻t2にボールが落下したことを示すテキストを書き込む
            sw_x.WriteLine(Convert.ToString(t2) + "にボールが落下");
            sw_y.WriteLine(Convert.ToString(t2) + "にボールが落下");
            sw_z.WriteLine(Convert.ToString(t2) + "にボールが落下");
        }*/
        //Debug.Log(pingpongBall.transform.position.y);
    }
}
