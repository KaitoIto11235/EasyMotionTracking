using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //ファイルに書き込むために必要
using System; //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要
using Microsoft.MixedReality.Toolkit.Utilities;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw; //座標記録用
    [SerializeField] Transform avaHand; //位置を記録したいオブジェクトのTransform
    Transform _face; // 鼻根のTransform。CameraCache.Main.Transformを参照する
    DateTime t2; //時刻用の変数

    void Start()
    {
        //時間を取得
        t2 = DateTime.Now;

        //csvのとき
        string file = Application.persistentDataPath + "/FaceHandPos.csv";

        //txtのとき
        //string file1 = Application.persistentDataPath + "/BallPos_x.txt";
        //string file2 = Application.persistentDataPath + "/BallPos_y.txt";
        //string file3 = Application.persistentDataPath + "/BallPos_z.txt";

        if (!File.Exists(file))
        {
            Debug.Log("ファイルを作成します");
            sw = File.CreateText(file);
            sw.Flush();
            sw.Dispose();
        }
        else
        {
            Debug.Log("The file already exists.ファイルは既に存在しています");
        }

        

        //UTF-8で生成...2番目の引数はtrueで末尾に追記，falseでファイルごと上書き．
        sw = new StreamWriter(file, true, Encoding.UTF8);

        //改行
        sw.WriteLine();

        //時刻書き込み
        sw.WriteLine(Convert.ToString(t2));

        //Debug.Log(t2);
    }

    void Update()
    {
        _face = CameraCache.Main.transform; // カメラのTransform

        // ユーザーの鼻根の正規化法線ベクトル
        float face_x = _face.forward.x;
        float face_y = _face.forward.y;
        float face_z = _face.forward.z;

        //オブジェクトのローカル座標を取得
        Vector3 avaHandPos = avaHand.position;
        Vector3 facePos = _face.position;

        // ユーザーの鼻根（びこん）からアバターの手首へのベクトル
        float hand_x = avaHandPos.x - facePos.x;
        float hand_y = avaHandPos.y - facePos.y;
        float hand_z = avaHandPos.z - facePos.z;

        // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
        string[] s1 =
        {
            Convert.ToString(face_x), Convert.ToString(face_y), Convert.ToString(face_z),

            Convert.ToString(hand_x), Convert.ToString(hand_y), Convert.ToString(hand_z)
        };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        sw.Flush();

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
