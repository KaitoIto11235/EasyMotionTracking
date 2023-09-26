using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;   //ファイルに書き込むために必要
using System;      //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class AutoSpeedUpSystemSaver : MonoBehaviour
{
    StreamWriter sw;                                    //座標記録用
    float time;                                         //ファイルが開かれてからの時間
    float animationTime = 4f;                           // animationの時間。この秒数だけデータが記録される

    [SerializeField] Transform aWrist;                  //位置を記録したいオブジェクトのTransform
    [SerializeField] Transform aIndex;                  //位置を記録したいオブジェクトのTransform
    [SerializeField] Animator n_animator, _animator;
    bool aVisibleWrist = true;                          // Targetを追えているときTure

    bool fileOpenFlag = false;
    //bool speedUpFlag = true;
    [SerializeField] TextMesh textOnButton;
    float level = 1f;                                   // 試行レベル記録用変数
    public float speed = 0.1f;                          // アニメーションのスピード

    /*[SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sound1;
    bool audioFlag = true;*/


    public void ClickStartButton()
    {
        StartCoroutine(DelayCoroutine());
        Vector3 aPos = this.transform.position;
        aPos.y = CameraCache.Main.transform.position.y - 1.56f;
        aPos.z = CameraCache.Main.transform.position.y - 0.08f;
        this.transform.position = aPos;

        animationTime = animationTime / n_animator.GetFloat("S_keisuu");
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
        OpenData();
    }

    /// <summary>
    /// Start!ボタンが押されると3秒後に呼び出され、ファイルを作成する
    /// </summary>
    public void OpenData()
    {
        
        if (!fileOpenFlag)
        {
            DateTime dt = DateTime.Now;// これがファイル名に追加される
            string file;
            if (level < 10)
            {
                file = Application.persistentDataPath + "/Day" + Convert.ToString(dt.Day) + " " +
                    Convert.ToString(dt.Hour) + "_" + Convert.ToString(dt.Minute) + "_" + Convert.ToString(dt.Second) 
                    + "FaceHand_Level 0" + Convert.ToString(level) + ".csv";
            }
            else
            {
                file = Application.persistentDataPath + "/Day" + Convert.ToString(dt.Day) + " " +
                    Convert.ToString(dt.Hour) + "_" + Convert.ToString(dt.Minute) + "_" + Convert.ToString(dt.Second)
                    + "FaceHand_Level " + Convert.ToString(level) + ".csv";
            }

            if (!File.Exists(file))
            {
                sw = File.CreateText(file);
                sw.Flush();
                sw.Dispose();
            }

            //UTF-8で生成...2番目の引数はtrueで末尾に追記，falseでファイルごと上書き．
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

            sw.WriteLine("Level:" + Convert.ToString(level));

            string[] s1 =
            {
            "time", "θ", "aVisibleWrist",

            "FaceRay_x", "FaceRay_y", "FaceRay_z",

            "aWrist_x", "aWrist_y", "aWrist_z",
            "aIndex_x", "aIndex_y", "aIndex_z",
            "uWrist_x", "uWrist_y", "uWrist_z",
            "uIndex_x", "uIndex_y", "uIndex_z",
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
            Debug.Log("File has already opened.");
        }

        time = 0f;
    }

    

    void SaveData(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 aWristDir, Vector3 aIndexDir) // ユーザーの手首が外れた時
    {
        textOnButton.text = "None.";
        // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
        string[] s1 =
        {
            Convert.ToString(time), Convert.ToString(theta), Convert.ToString(aVisibleWrist),

            Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

            Convert.ToString(aWristDir.x), Convert.ToString(aWristDir.y), Convert.ToString(aWristDir.z),
            Convert.ToString(aIndexDir.x), Convert.ToString(aIndexDir.y), Convert.ToString(aIndexDir.z)
        };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        sw.Flush();
    }

    void SaveDataClear(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 aWristDir, Vector3 aIndexDir) // ユーザーの手首が入っている時
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
        {
            textOnButton.text = "Wrist";
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Right, out MixedRealityPose PI))
            {
                textOnButton.text = "Wrist, Index";
                Vector3 uWristDir = PW.Position - facePos;
                Vector3 uIndexDir = PI.Position - facePos;
                // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
                string[] s1 =
                {
                    Convert.ToString(time), Convert.ToString(theta), Convert.ToString(aVisibleWrist),

                    Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

                    Convert.ToString(aWristDir.x), Convert.ToString(aWristDir.y), Convert.ToString(aWristDir.z),
                    Convert.ToString(aIndexDir.x), Convert.ToString(aIndexDir.y), Convert.ToString(aIndexDir.z),
                    Convert.ToString(uWristDir.x), Convert.ToString(uWristDir.y), Convert.ToString(uWristDir.z),
                    Convert.ToString(uIndexDir.x), Convert.ToString(uIndexDir.y), Convert.ToString(uIndexDir.z)
                };

                string s2 = string.Join(",", s1);
                sw.WriteLine(s2);
                sw.Flush();
            }
            else
            {
                Debug.Log("User Index wasn't found.");
            }
        }
    }

    void CloseData()
    {
        sw.Dispose();
        Debug.Log("Close_csv");
        fileOpenFlag = false;
        /*speed = n_animator.GetFloat("S_keisuu");

        if (speedUpFlag && level <10)   // 手首を見失わなかった場合の処理
        {
            level += 1f;   //レベル記録用変数

            animationTime = animationTime * speed;

            speed += 0.1f; // 次回の速さを＋0.1する
            textOnButton.text = "   SpeedUp!\n\n   Level:" + level.ToString() + "\n\n Start!\n\n\n\n";

            animationTime = animationTime / speed;


            Debug.Log("Speed Up!");
        }
        else
        {
            textOnButton.text = "   リトライ\n\n   Level:" + level.ToString() + "\n\n Start!\n\n\n\n";

            aVisibleWrist = true;
            speedUpFlag = true;
        }*/
    }

    void Start()
    {
        //animationTime = animationTime / 0.1f; // speedが0.1倍で始まるため。
    }

    void FixedUpdate()
    {
        //時間を取得
        time += Time.deltaTime;

        if (fileOpenFlag)
        {
            Transform face = CameraCache.Main.transform; // カメラのTransform
            Vector3 aWristDir = aWrist.position - face.position;
            Vector3 aIndexDir = aIndex.position - face.position;

            float theta = ThetaCal(aWristDir, face.forward);
            aVisibleWrist = IsVisible(aWrist.position);

            // 追えていなければスピードアップしない
            /*if (aVisibleWrist == false)
            {
                speedUpFlag = false;

            }*/

            // ユーザーの手首の位置が取得できたとき"SaveDataClear()"を呼び出し、出来なかったとき"SaveData()"でaVisibleWristをfalseにする
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
            {
                SaveDataClear(theta, face.position, face.forward, aWristDir, aIndexDir);
            }
            else
            {
                SaveData(theta, face.position, face.forward, aWristDir, aIndexDir);
            }

            if (time > animationTime)
            {
                CloseData();
            }
        }
    }

    ///<summary>
    ///ターゲットとのズレの角度を計算する
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 faceDirction)
    {
        float TargetCos = Vector3.Dot(faceDirction, target.normalized); // 自身とターゲットへの向きの内積計算:cos(θ)
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;                   // 自身とターゲットへの向きの角度:θ°
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


