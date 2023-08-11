using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; //�t�@�C���ɏ������ނ��߂ɕK�v
using System; //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v
using Microsoft.MixedReality.Toolkit.Utilities;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw; //���W�L�^�p
    float time; //�t�@�C�����J����Ă���̎���
    [SerializeField] float animationTime = 5f; // animation�̎��ԁB���̕b�������f�[�^���L�^�����
    [SerializeField] Transform avaHand; //�ʒu���L�^�������I�u�W�F�N�g��Transform
    [SerializeField] Animator n_animator, _animator;
    public bool visible; // Target��ǂ��Ă���Ƃ�Ture
    bool fileOpenFlag = false, speedConstFlag = false;

    public void ClickStartButton()
    {
        time = 0f;
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

            string file = Application.persistentDataPath + "/FaceHand_log" + Convert.ToString(dt.Hour) + " "
                + Convert.ToString(dt.Minute) + " " + Convert.ToString(dt.Second) + ".csv";

            if (!File.Exists(file))
            {
                sw = File.CreateText(file);
                sw.Flush();
                sw.Dispose();
            }

            //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
            sw = new StreamWriter(new FileStream(file, FileMode.Open), Encoding.UTF8);

            //������������
            sw.WriteLine(Convert.ToString(dt));

            string[] s1 =
            {
            "time", "��", "IsVisible",

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
        sw.Dispose();
        Debug.Log("Close_csv");
        fileOpenFlag = false;

        // ����������Ȃ������ꍇ�̏���
        if(!speedConstFlag)
        {
            float speed = n_animator.GetFloat("S_keisuu");
            animationTime = (animationTime - 3f) * speed + 3f;

            speed += 0.1f; // ����̑������{0.1����
            animationTime = (animationTime - 3f) / speed + 3f;
            n_animator.SetFloat("S_keisuu", speed);
            _animator.SetFloat("S_keisuu", speed);
            Debug.Log("Speed Up!");
        }
    }

    public void SaveData
    (float theta, Vector3 faceNormal, Vector3 avaHand)
    {
        // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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
        animationTime = (animationTime - 3f) / 0.1f + 3f; // speed��0.1�{�Ŏn�܂邽�߁B
    }

    void FixedUpdate()
    {
        //���Ԃ��擾
        time += Time.deltaTime;
        if (fileOpenFlag)
        {
            Transform face = CameraCache.Main.transform; // �J������Transform
            Vector3 faceNormal = face.forward; // ���[�U�[�̕@���̐��K���@���x�N�g��
            Vector3 avaHandPos = avaHand.position;
            Vector3 facePos = face.position;
            Vector3 avaHandDir = avaHandPos - facePos;

            float theta = ThetaCal(avaHandDir, faceNormal);
            visible = IsVisible(avaHandPos);
            if(visible == false)
            {
                speedConstFlag = true;
            }

            SaveData(theta, faceNormal, avaHandDir);

            if (time > animationTime)
            {
                CloseData();
            }
        }


    }

    ///<summary>
    ///�^�[�Q�b�g�Ƃ̃Y���̊p�x���v�Z����
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 faceNormal)
    {
        float TargetCos = Vector3.Dot(faceNormal, target.normalized); // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z:cos(��)
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg; // ���g�ƃ^�[�Q�b�g�ւ̌����̊p�x:�Ɓ�
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


