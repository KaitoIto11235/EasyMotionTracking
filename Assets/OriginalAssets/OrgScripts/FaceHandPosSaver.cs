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
    [SerializeField] float animationTime; // animation�̎��ԁB���̕b�������f�[�^���L�^�����
    [SerializeField] Transform avaHand; //�ʒu���L�^�������I�u�W�F�N�g��Transform

    
    public bool visible; // Target��ǂ��Ă���Ƃ�Ture

    bool fileOpenFlag = false, startButton = false;

    public void ClickStartButton()
    {
        startButton = true;
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
            "time", "IsVisible", "��",

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

    }

    void FixedUpdate()
    {
        //���Ԃ��擾
        time += Time.deltaTime;
        if (startButton)
        {
            
            Transform face = CameraCache.Main.transform; // �J������Transform

            Vector3 faceNormal = face.forward; // ���[�U�[�̕@���̐��K���@���x�N�g��

            //�I�u�W�F�N�g�̍��W��Vector3�^�Ŏ擾
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
    ///�^�[�Q�b�g�Ƃ̃Y���̊p�x���v�Z����
    ///</summary>
    float ThetaCal(Vector3 target, Vector3 basePos, Vector3 faceNormal)
    {
        // HoloLens2�̎���p���Ӂ��Ƃ���Ƃ���cos(��/2)
        //float cosHalf_holo = 0.969f;

        // �J�����̈ʒu����̃x�N�g�����v�Z
        target -= basePos;

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z:cos(��)
        float TargetCos = Vector3.Dot(faceNormal, target.normalized); // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        // ���g�ƃ^�[�Q�b�g�ւ̌����̊p�x:�Ɓ�
        return Mathf.Acos(TargetCos) * Mathf.Rad2Deg;

        // ���E����
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


