using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; //ファイルに書き込むために必要
using System; //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要
using Microsoft.MixedReality.Toolkit.Utilities;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw; //座標記録用
    float time; //ファイルが開かれてからの時間
    [SerializeField] float animationTime; // animationの時間。この秒数だけデータが記録される
    [SerializeField] Transform avaHand; //位置を記録したいオブジェクトのTransform

    
    public bool visible; // Targetを追えているときTure

    bool fileOpenFlag = false, startButton = false;

    public void ClickStartButton()
    {
        startButton = true;
        time = 0f;
        OpenData();
    }

    /// <summary>
    /// Start!ボタンが押されると呼び出され、ファイルを作成する
    /// </summary>
    public void OpenData()
    {
        if (!fileOpenFlag)
        {
            DateTime dt = DateTime.Now;// これがファイル名に追加される

            string file = Application.persistentDataPath + "/FaceHand_log" + Convert.ToString(dt.Hour) + " "
                + Convert.ToString(dt.Minute) + " " + Convert.ToString(dt.Second) + ".csv";

            if (!File.Exists(file))
            {
                sw = File.CreateText(file);
                sw.Flush();
                sw.Dispose();
            }

            //UTF-8で生成...2番目の引数はtrueで末尾に追記，falseでファイルごと上書き．
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

            //時刻書き込み
            sw.WriteLine(Convert.ToString(dt));

            string[] s1 =
            {
            "time", "IsVisible", "θ",

            "face_x", "face_y", "face_z",

            "hand_x", "hand_y", "hand_z"
            };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Flush();

            fileOpenFlag = true;
            Debug.Log("Create_csv");
            Debug.Log(file);
        }
        else
        {
            Debug.Log("File opened");
        }


    }

    public void CloseData()
    {
        if (fileOpenFlag)
        {
            sw.Dispose();
            Debug.Log("Close_csv");
            fileOpenFlag = false;
        }
        else
        {
            Debug.Log("No file opened (CloseData) ");
        }
    }

    public void SaveData
    (float theta, Vector3 faceNormal, Vector3 avaHand)
    {
        // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
        string[] s1 =
        {
            Convert.ToString(time), Convert.ToString(theta), Convert.ToString(visible),

            Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

            Convert.ToString(avaHand.x), Convert.ToString(avaHand.y), Convert.ToString(avaHand.z)
        };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        sw.Flush();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        //時間を取得
        time += Time.deltaTime;
        if (startButton)
        {
            
            Transform face = CameraCache.Main.transform; // カメラのTransform

            Vector3 faceNormal = face.forward; // ユーザーの鼻根の正規化法線ベクトル

            //オブジェクトの座標をVector3型で取得
            Vector3 avaHandPos = avaHand.position;
            Vector3 facePos = face.position;

            float theta = ThetaCal(avaHandPos, facePos, faceNormal);
            visible = IsVisible(avaHandPos);


            SaveData(theta, faceNormal, avaHandPos - facePos);

            if (time > animationTime)
            {
                CloseData();
                startButton = false;
            }
        }


    }

    ///<summary>
    ///ターゲットとのズレの角度を計算する
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 basePos, Vector3 faceNormal)
    {
        // HoloLens2の視野角＝φ°とするときのcos(φ/2)
        //float cosHalf_holo = 0.969f;

        // カメラの位置からのベクトルを計算
        target -= basePos;

        // 自身とターゲットへの向きの内積計算:cos(θ)
        float TargetCos = Vector3.Dot(faceNormal, target.normalized); // ターゲットへの向きベクトルを正規化する必要があることに注意
        // 自身とターゲットへの向きの角度:θ°
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;

        // 視界判定
        //return TargetCos > cosHalf_holo;
    }

    bool IsVisible(Vector3 target)
    {
        var leftScreenPos = Camera.main.WorldToViewportPoint(target);
        var rightScreenPos = Camera.main.WorldToViewportPoint(target);
        var topScreenPos = Camera.main.WorldToViewportPoint(target);
        var bottomScreenPos = Camera.main.WorldToViewportPoint(target);

        return (0f <= leftScreenPos.x && rightScreenPos.x <= 1f
        && 0f <= bottomScreenPos.y && topScreenPos.y <= 1f);
    }
}


