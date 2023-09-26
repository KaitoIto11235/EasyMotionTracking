using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;   //�t�@�C���ɏ������ނ��߂ɕK�v
using System;      //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class AutoSpeedUpSystemSaver : MonoBehaviour
{
    StreamWriter sw;                                    //���W�L�^�p
    float time;                                         //�t�@�C�����J����Ă���̎���
    float animationTime = 4f;                           // animation�̎��ԁB���̕b�������f�[�^���L�^�����

    [SerializeField] Transform aWrist;                  //�ʒu���L�^�������I�u�W�F�N�g��Transform
    [SerializeField] Transform aIndex;                  //�ʒu���L�^�������I�u�W�F�N�g��Transform
    [SerializeField] Animator n_animator, _animator;
    bool aVisibleWrist = true;                          // Target��ǂ��Ă���Ƃ�Ture

    bool fileOpenFlag = false;
    //bool speedUpFlag = true;
    [SerializeField] TextMesh textOnButton;
    float level = 1f;                                   // ���s���x���L�^�p�ϐ�
    public float speed = 0.1f;                          // �A�j���[�V�����̃X�s�[�h

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
    /// Start!�{�^������������3�b��ɌĂяo����A�t�@�C�����쐬����
    /// </summary>
    public void OpenData()
    {
        
        if (!fileOpenFlag)
        {
            DateTime dt = DateTime.Now;// ���ꂪ�t�@�C�����ɒǉ������
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

            //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

            sw.WriteLine("Level:" + Convert.ToString(level));

            string[] s1 =
            {
            "time", "��", "aVisibleWrist",

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

    

    void SaveData(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 aWristDir, Vector3 aIndexDir) // ���[�U�[�̎�񂪊O�ꂽ��
    {
        textOnButton.text = "None.";
        // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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

    void SaveDataClear(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 aWristDir, Vector3 aIndexDir) // ���[�U�[�̎�񂪓����Ă��鎞
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
        {
            textOnButton.text = "Wrist";
            if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexKnuckle, Handedness.Right, out MixedRealityPose PI))
            {
                textOnButton.text = "Wrist, Index";
                Vector3 uWristDir = PW.Position - facePos;
                Vector3 uIndexDir = PI.Position - facePos;
                // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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

        if (speedUpFlag && level <10)   // ����������Ȃ������ꍇ�̏���
        {
            level += 1f;   //���x���L�^�p�ϐ�

            animationTime = animationTime * speed;

            speed += 0.1f; // ����̑������{0.1����
            textOnButton.text = "   SpeedUp!\n\n   Level:" + level.ToString() + "\n\n Start!\n\n\n\n";

            animationTime = animationTime / speed;


            Debug.Log("Speed Up!");
        }
        else
        {
            textOnButton.text = "   ���g���C\n\n   Level:" + level.ToString() + "\n\n Start!\n\n\n\n";

            aVisibleWrist = true;
            speedUpFlag = true;
        }*/
    }

    void Start()
    {
        //animationTime = animationTime / 0.1f; // speed��0.1�{�Ŏn�܂邽�߁B
    }

    void FixedUpdate()
    {
        //���Ԃ��擾
        time += Time.deltaTime;

        if (fileOpenFlag)
        {
            Transform face = CameraCache.Main.transform; // �J������Transform
            Vector3 aWristDir = aWrist.position - face.position;
            Vector3 aIndexDir = aIndex.position - face.position;

            float theta = ThetaCal(aWristDir, face.forward);
            aVisibleWrist = IsVisible(aWrist.position);

            // �ǂ��Ă��Ȃ���΃X�s�[�h�A�b�v���Ȃ�
            /*if (aVisibleWrist == false)
            {
                speedUpFlag = false;

            }*/

            // ���[�U�[�̎��̈ʒu���擾�ł����Ƃ�"SaveDataClear()"���Ăяo���A�o���Ȃ������Ƃ�"SaveData()"��aVisibleWrist��false�ɂ���
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
    ///�^�[�Q�b�g�Ƃ̃Y���̊p�x���v�Z����
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 faceDirction)
    {
        float TargetCos = Vector3.Dot(faceDirction, target.normalized); // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z:cos(��)
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;                   // ���g�ƃ^�[�Q�b�g�ւ̌����̊p�x:�Ɓ�
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


