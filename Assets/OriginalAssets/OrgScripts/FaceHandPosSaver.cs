using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;   //ファイルに書き込むために必要
using System;      //ConvertとDeteTimeを使うために必要
using System.Text; //文字コードを指定するために必要
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw;                                    //座標記録用
    float time;                                         //ファイルが開かれてからの時間
    [SerializeField] float animationTime = 2f;          // animationの時間。この秒数だけデータが記録される
    [SerializeField] Transform avaWrist;                 //位置を記録したいオブジェクトのTransform
    [SerializeField] Animator n_animator, _animator;
    bool visible;                                       // Targetを追えているときTure
    bool visibleWrist = true;
    bool fileOpenFlag = false, speedUpFlag = true;
    [SerializeField] TextMesh textOnButton;
    float level = 1f;                                   // 試行レベル記録用変数
    public float speed = 0.1f;                          // アニメーションのスピード


    public void ClickStartButton()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
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
            string file;
            if (level < 10)
            {
                file = Application.persistentDataPath + "/FaceHand_Level 0" + Convert.ToString(level) + "  Day" + Convert.ToString(dt.Day) + " " +
                    Convert.ToString(dt.Hour) + "_" + Convert.ToString(dt.Minute) + "_" + Convert.ToString(dt.Second) + ".csv";
            }
            else
            {
                file = Application.persistentDataPath + "/FaceHand_Level " + Convert.ToString(level) + "  Day" + Convert.ToString(dt.Day) + " " +
                    Convert.ToString(dt.Hour) + "_" + Convert.ToString(dt.Minute) + "_" + Convert.ToString(dt.Second) + ".csv";
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
            "time", "θ", "IsVisible",

            "FaceRay_x", "FaceRay_y", "FaceRay_z",

            "HandA_x", "HandA_y", "HandA_z",

            "HandU_x", "HandU_y", "HandU_z"
            };
            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Flush();

            fileOpenFlag = true;
            Debug.Log("Create_csv");
            Debug.Log(file);

            n_animator.SetFloat("S_keisuu", speed);
            _animator.SetFloat("S_keisuu", speed);
        }
        else
        {
            Debug.Log("File opened");
        }

        time = 0f;
    }

    

    void SaveData(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 avaWristDir) // ユーザーの手首が外れた時
    {
        // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
        string[] s1 =
        {
            Convert.ToString(time), Convert.ToString(theta), Convert.ToString(visible),

            Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

            Convert.ToString(avaWristDir.x), Convert.ToString(avaWristDir.y), Convert.ToString(avaWristDir.z)
        };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        sw.Flush();
        visibleWrist = false;
    }

    void SaveDataUser(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 avaWristDir) // ユーザーの手首が入っている時
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
        {
            Vector3 uWristDir = PW.Position - facePos;
            // 松下さんのスクリプト"A_Holo_SavaCSV_Production.cs"を参考に作成
            string[] s1 =
            {
                Convert.ToString(time), Convert.ToString(theta), Convert.ToString(visible),

                Convert.ToString(faceNormal.x), Convert.ToString(faceNormal.y), Convert.ToString(faceNormal.z),

                Convert.ToString(avaWristDir.x), Convert.ToString(avaWristDir.y), Convert.ToString(avaWristDir.z),

                Convert.ToString(uWristDir.x), Convert.ToString(uWristDir.y), Convert.ToString(uWristDir.z)
            };

            string s2 = string.Join(",", s1);
            sw.WriteLine(s2);
            sw.Flush();
        }
    }

    void CloseData()
    {
        sw.Dispose();
        Debug.Log("Close_csv");
        fileOpenFlag = false;
        speed = n_animator.GetFloat("S_keisuu");

        if (speedUpFlag)   // 手首を見失わなかった場合の処理
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
            visible = true;
            visibleWrist = true;
            speedUpFlag = true;
        }
    }

    void Start()
    {
        animationTime = animationTime / 0.1f; // speedが0.1倍で始まるため。
    }

    void FixedUpdate()
    {
        //時間を取得
        time += Time.deltaTime;

        if (fileOpenFlag)
        {
            Transform face = CameraCache.Main.transform; // カメラのTransform
            Vector3 avaWristDir = avaWrist.position - face.position;

            float theta = ThetaCal(avaWristDir, face.forward);
            visible = IsVisible(avaWrist.position);

            // 追えていなければスピードアップしない
            if (visible == false || visibleWrist == false) // "vsible"は"IsVisible()"の戻り値、"visibleWrist"は"SaveDataUser()"が呼び出されたか
            {
                speedUpFlag = false;
            }

            // ユーザーの手首の位置が取得できたとき"SaveDataUser()"を呼び出し、出来なかったとき"SaveData()"でvisibleWristをfalseにする
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
            {
                SaveDataUser(theta, face.position, face.forward, avaWristDir);
            }
            else
            {
                SaveData(theta, face.position, face.forward, avaWristDir);
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
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;                 // 自身とターゲットへの向きの角度:θ°
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


