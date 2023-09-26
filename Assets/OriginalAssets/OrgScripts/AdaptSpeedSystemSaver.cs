using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;   //ファイルに書き込むために必要
using System;      //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class AdaptSpeedSystemSaver : MonoBehaviour
{
    StreamWriter sw;                                    //座標記録用
    float time;                                         //ファイルが開かれてからの時間
    float animationTime = 30f;                           // animationの時間。この秒数経つと強制的にCloseData()が実行される

    [SerializeField] Transform aWrist;                  //位置を記録したいオブジェクトのTransform
    [SerializeField] Transform aIndex;                  //位置を記録したいオブジェクトのTransform
    [SerializeField] Animator n_animator, _animator;
    bool aVisibleWrist = true;                          // Targetを追えているときTure
    bool fileOpenFlag = false;
    [SerializeField] TextMesh textOnButton;
    //float speed = 1.0f;                                 // アニメーションのスピード


    public void ClickStartButton()
    {
        StartCoroutine(DelayCoroutine());

        //Start!ボタンを押したときのカメラの位置に、アバターの目を合わせる
        Vector3 aPos = this.transform.position;
        aPos.y = CameraCache.Main.transform.position.y - 1.56f;
        aPos.z = CameraCache.Main.transform.position.y - 0.08f;
        this.transform.position = aPos;
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
            string file = Application.persistentDataPath + "/Day" + Convert.ToString(dt.Day) + " " +
                Convert.ToString(dt.Hour) + "_" + Convert.ToString(dt.Minute) + "_" + Convert.ToString(dt.Second)
                + "AdaptSpeedSystem"  + ".csv";

            if (!File.Exists(file))
            {
                sw = File.CreateText(file);
                sw.Flush();
                sw.Dispose();
            }

            //UTF-8で生成...2番目の引数はtrueで末尾に追記，falseでファイルごと上書き．
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

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

            //speed = n_animator.GetFloat("S_keisuu");

            //animationTime = 4.0f / speed;

            //n_animator.SetFloat("S_keisuu", speed);
            //_animator.SetFloat("S_keisuu", speed);
        }
        else
        {
            Debug.Log("File has already opened.");
        }

        time = 0f;
    }



    void SaveData(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 aWristDir, Vector3 aIndexDir) // ユーザーの手首が外れた時
    {
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
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Right, out MixedRealityPose PI))
            {
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
        }
    }

    void CloseData()
    {
        fileOpenFlag = false;
        aVisibleWrist = true;
        sw.Dispose();
        //n_animator.SetFloat("S_keisuu", speed);
        //_animator.SetFloat("S_keisuu", speed);
        Debug.Log("Close_csv");
    }

    void Start()
    {
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
            /*float adaptSpeed = speed - (theta * theta * speed / 196f);
            if (adaptSpeed >= 0)
            {
                n_animator.SetFloat("S_keisuu", adaptSpeed);
                _animator.SetFloat("S_keisuu", adaptSpeed);
            }*/

            aVisibleWrist = IsVisible(aWrist.position);


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
                sw.WriteLine("Not Finished.");
                CloseData();
            }

            if (n_animator.GetCurrentAnimatorStateInfo(1).IsName("Waiting") && time > 3.8f)
            {
                CloseData();
            }
        }

    }

    ///<summary>
    ///ターゲットとのズレの角度を計算する
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 faceDirection)
    {
        float TargetCos = Vector3.Dot(faceDirection, target.normalized); // 自身とターゲットへの向きの内積計算:cos(θ)
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;                   // 自身とターゲットへの向きの角度:θ°
    }

    bool IsVisible(Vector3 targetPos)
    {
        var leftScreenPos = Camera.main.WorldToViewportPoint(targetPos);
        var rightScreenPos = Camera.main.WorldToViewportPoint(targetPos);
        var topScreenPos = Camera.main.WorldToViewportPoint(targetPos);
        var bottomScreenPos = Camera.main.WorldToViewportPoint(targetPos);

        return (0f <= leftScreenPos.x && rightScreenPos.x <= 1f
        && 0f <= bottomScreenPos.y && topScreenPos.y <= 1f);
    }
}



