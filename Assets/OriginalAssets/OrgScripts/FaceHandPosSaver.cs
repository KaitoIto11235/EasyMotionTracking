using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�t�@�C���ɏ������ނ��߂ɕK�v
using System; //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v
using Microsoft.MixedReality.Toolkit.Utilities;

public class FaceHandPosSaver : MonoBehaviour
{
    StreamWriter sw; //���W�L�^�p
    DateTime dt; //���t�p�̕ϐ�
    float time; //�t�@�C�����J����Ă���̎���
    [SerializeField] float animationTime;
    
    [SerializeField] Transform avaHand; //�ʒu���L�^�������I�u�W�F�N�g��Transform
    Transform face; // �@����Transform�BCameraCache.Main.Transform���Q�Ƃ���

    //�I�u�W�F�N�g�̈ʒu
    Vector3 avaHandPos, facePos;

    Vector3 faceNormal; // ���[�U�[�̕@���̐��K���@���x�N�g��
    Vector3 handDir;// ���[�U�[�̕@������A�o�^�[�̎��ւ̃x�N�g��

    float theta; // �@���x�N�g���Ǝ��ւ̃x�N�g���ɂ��p�x
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
        if(!fileOpenFlag)
        {
            dt = DateTime.Now;// ���ꂪ�t�@�C�����ɒǉ������
            

            string file = Application.persistentDataPath + "/FaceHand_log" + Convert.ToString(dt.Hour) +
                " " + Convert.ToString(dt.Minute) + " " + Convert.ToString(dt.Second) + ".csv";

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

    public void SaveData()
    {
        // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
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
        //���Ԃ��擾
        time += Time.deltaTime;
        if (startButton)
        {
            // �J������Transform
            face = CameraCache.Main.transform;

            // ���[�U�[�̕@���̐��K���@���x�N�g��
            faceNormal = face.forward;

            //�I�u�W�F�N�g�̍��W���擾
            avaHandPos = avaHand.position;
            facePos = face.position;

            handDir = avaHandPos - facePos;// ���[�U�[�̕@������A�o�^�[�̎��ւ̃x�N�g��

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
    ///�^�[�Q�b�g�Ƃ̃Y���̊p�x���v�Z���A�^�[�Q�b�g�������Ă���Ƃ�True��Ԃ�
    ///</summary>
    public bool IsVisible(Vector3 target)
    {
        // HoloLens2�̎���p���Ӂ��Ƃ���Ƃ���cos(��/2)
        float cosHalf_holo = 0.969f;

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z:cos(��)
        float TargetCos = Vector3.Dot(faceNormal, target.normalized); // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        // ���g�ƃ^�[�Q�b�g�ւ̌����̊p�x:�Ɓ�
        theta = Mathf.Acos(TargetCos) * Mathf.Rad2Deg;

        // ���E����
        return TargetCos > cosHalf_holo;
    }
}
