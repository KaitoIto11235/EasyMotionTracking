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
    DateTime dt; //日付用の変数
    float time; //ファイルが開かれてからの時間
    [SerializeField] float animationTime;
    
    [SerializeField] Transform avaHand; //位置を記録したいオブジェクトのTransform
    Transform face; // 鼻根のTransform。CameraCache.Main.Transformを参照する

    //オブジェクトの位置
    Vector3 avaHandPos, facePos;

    Vector3 faceNormal; // ユーザーの鼻根の正規化法線ベクトル
    Vector3 handDir;// ユーザーの鼻根からアバターの手首へのベクトル

    float theta; // 法線ベクトルと手首へのベクトルによる角度
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
        if(!fileOpenFlag)
        {
            dt = DateTime.Now;// これがファイル名に追加される
            

            string file = Application.persistentDataPath + "/FaceHand_log" + Convert.ToString(dt.Hour) +
                " " + Convert.ToString(dt.Minute) + " " + Convert.ToString(dt.Second) + ".csv";

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

    public void SaveData()
    {
        // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
        string[] s1 =
        {
            Convert.ToString(time), Convert.ToString(visible), Convert.ToString(theta),

            Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

            Convert.ToString(handDir.x), Convert.ToString(handDir.y), Convert.ToString(handDir.z)
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
            // カメラのTransform
            face = CameraCache.Main.transform;

            // ユーザーの鼻根の正規化法線ベクトル
            faceNormal = face.forward;

            //オブジェクトの座標を取得
            avaHandPos = avaHand.position;
            facePos = face.position;

            handDir = avaHandPos - facePos;// ユーザーの鼻根からアバターの手首へのベクトル

            visible = IsVisible(handDir);

            SaveData();
            
            if(time > animationTime)
            {
                CloseData();
                startButton = false;
            }
        }

        
    }

    ///<summary>
    ///ターゲットとのズレの角度を計算し、ターゲットが見えているときTrueを返す
    ///</summary>
    public bool IsVisible(Vector3 target)
    {
        // HoloLens2の視野角＝φ°とするときのcos(φ/2)
        float cosHalf_holo = 0.969f;

        // 自身とターゲットへの向きの内積計算:cos(θ)
        float TargetCos = Vector3.Dot(faceNormal, target.normalized); // ターゲットへの向きベクトルを正規化する必要があることに注意
        // 自身とターゲットへの向きの角度:θ°
        theta = Mathf.Acos(TargetCos) * Mathf.Rad2Deg;

        // 視界判定
        return TargetCos > cosHalf_holo;
    }
}
