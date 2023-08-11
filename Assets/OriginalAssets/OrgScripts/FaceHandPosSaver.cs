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
    [SerializeField] Transform avaHand; //�ʒu���L�^�������I�u�W�F�N�g��Transform
    Transform _face; // �@����Transform�BCameraCache.Main.Transform���Q�Ƃ���
    DateTime t2; //�����p�̕ϐ�

    void Start()
    {
        //���Ԃ��擾
        t2 = DateTime.Now;

        //csv�̂Ƃ�
        string file = Application.persistentDataPath + "/FaceHandPos.csv";

        //txt�̂Ƃ�
        //string file1 = Application.persistentDataPath + "/BallPos_x.txt";
        //string file2 = Application.persistentDataPath + "/BallPos_y.txt";
        //string file3 = Application.persistentDataPath + "/BallPos_z.txt";

        if (!File.Exists(file))
        {
            Debug.Log("�t�@�C�����쐬���܂�");
            sw = File.CreateText(file);
            sw.Flush();
            sw.Dispose();
        }
        else
        {
            Debug.Log("The file already exists.�t�@�C���͊��ɑ��݂��Ă��܂�");
        }

        

        //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
        sw = new StreamWriter(file, true, Encoding.UTF8);

        //���s
        sw.WriteLine();

        //������������
        sw.WriteLine(Convert.ToString(t2));

        //Debug.Log(t2);
    }

    void Update()
    {
        _face = CameraCache.Main.transform; // �J������Transform

        // ���[�U�[�̕@���̐��K���@���x�N�g��
        float face_x = _face.forward.x;
        float face_y = _face.forward.y;
        float face_z = _face.forward.z;

        //�I�u�W�F�N�g�̃��[�J�����W���擾
        Vector3 avaHandPos = avaHand.position;
        Vector3 facePos = _face.position;

        // ���[�U�[�̕@���i�т���j����A�o�^�[�̎��ւ̃x�N�g��
        float hand_x = avaHandPos.x - facePos.x;
        float hand_y = avaHandPos.y - facePos.y;
        float hand_z = avaHandPos.z - facePos.z;

        // ��������̃X�N���v�g"A_Holo_SavaCSV_Production.cs"���Q�l�ɍ쐬
        string[] s1 =
        {
            Convert.ToString(face_x), Convert.ToString(face_y), Convert.ToString(face_z),

            Convert.ToString(hand_x), Convert.ToString(hand_y), Convert.ToString(hand_z)
        };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);
        sw.Flush();

        //txt�̂Ƃ�
        //sw_x.Write(Convert.ToString(x) + ",");
        //sw_y.Write(Convert.ToString(y) + ",");
        //sw_z.Write(Convert.ToString(z) + ",");

        /*if (CentralBall.transform.position.y <= -0.5f)//�{�[���������̏���
        {
            //���s
            sw_x.WriteLine();
            sw_y.WriteLine();
            sw_z.WriteLine();

            //����t2�Ƀ{�[���������������Ƃ������e�L�X�g����������
            sw_x.WriteLine(Convert.ToString(t2) + "�Ƀ{�[��������");
            sw_y.WriteLine(Convert.ToString(t2) + "�Ƀ{�[��������");
            sw_z.WriteLine(Convert.ToString(t2) + "�Ƀ{�[��������");
        }*/
        //Debug.Log(pingpongBall.transform.position.y);
    }
}
