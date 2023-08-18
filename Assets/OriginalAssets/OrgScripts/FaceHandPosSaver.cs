using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;   //�t�@�C���ɏ������ނ��߂ɕK�v
using System;      //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw;                                    //���W�L�^�p
    float time;                                         //�t�@�C�����J����Ă���̎���
    [SerializeField] float animationTime = 2f;          // animation�̎��ԁB���̕b�������f�[�^���L�^�����
    [SerializeField] Transform avaWrist;                 //�ʒu���L�^�������I�u�W�F�N�g��Transform
    [SerializeField] Animator n_animator, _animator;
    bool visible;                                       // Target��ǂ��Ă���Ƃ�Ture
    bool visibleWrist = true;
    bool fileOpenFlag = false, speedUpFlag = true;
    [SerializeField] TextMesh textOnButton;
    float level = 1f;                                   // ���s���x���L�^�p�ϐ�
    public float speed = 0.1f;                          // �A�j���[�V�����̃X�s�[�h


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
    /// Start!�{�^�����������ƌĂяo����A�t�@�C�����쐬����
    /// </summary>
    public void OpenData()
    {
        
        if (!fileOpenFlag)
        {
            DateTime dt = DateTime.Now;// ���ꂪ�t�@�C�����ɒǉ������
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

            //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

            sw.WriteLine("Level:" + Convert.ToString(level));

            string[] s1 =
            {
            "time", "��", "IsVisible",

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

    

    void SaveData(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 avaWristDir) // ���[�U�[�̎�񂪊O�ꂽ��
    {
        // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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

    void SaveDataUser(float theta, Vector3 facePos, Vector3 faceNormal, Vector3 avaWristDir) // ���[�U�[�̎�񂪓����Ă��鎞
    {
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, Handedness.Right, out MixedRealityPose PW))
        {
            Vector3 uWristDir = PW.Position - facePos;
            // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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

        if (speedUpFlag)   // ����������Ȃ������ꍇ�̏���
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
            visible = true;
            visibleWrist = true;
            speedUpFlag = true;
        }
    }

    void Start()
    {
        animationTime = animationTime / 0.1f; // speed��0.1�{�Ŏn�܂邽�߁B
    }

    void FixedUpdate()
    {
        //���Ԃ��擾
        time += Time.deltaTime;

        if (fileOpenFlag)
        {
            Transform face = CameraCache.Main.transform; // �J������Transform
            Vector3 avaWristDir = avaWrist.position - face.position;

            float theta = ThetaCal(avaWristDir, face.forward);
            visible = IsVisible(avaWrist.position);

            // �ǂ��Ă��Ȃ���΃X�s�[�h�A�b�v���Ȃ�
            if (visible == false || visibleWrist == false) // "vsible"��"IsVisible()"�̖߂�l�A"visibleWrist"��"SaveDataUser()"���Ăяo���ꂽ��
            {
                speedUpFlag = false;
            }

            // ���[�U�[�̎��̈ʒu���擾�ł����Ƃ�"SaveDataUser()"���Ăяo���A�o���Ȃ������Ƃ�"SaveData()"��visibleWrist��false�ɂ���
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
    ///�^�[�Q�b�g�Ƃ̃Y���̊p�x���v�Z����
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 faceDirction)
    {
        float TargetCos = Vector3.Dot(faceDirction, target.normalized); // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z:cos(��)
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;                 // ���g�ƃ^�[�Q�b�g�ւ̌����̊p�x:�Ɓ�
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


