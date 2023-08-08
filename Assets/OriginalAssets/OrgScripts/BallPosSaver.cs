using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�t�@�C���ɏ������ނ��߂ɕK�v
using System; //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v

public class BallPosSaver : MonoBehaviour
{
    StreamWriter sw_x; //x���W�L�^�p
    StreamWriter sw_y; //y���W�L�^�p
    StreamWriter sw_z; //z���W�L�^�p
    public GameObject CentralBall;//�ʒu���L�^�������I�u�W�F�N�g�p�ϐ�
    DateTime t2; //�����p�̕ϐ�

    void Start()
    {
        //���Ԃ��擾
        t2 = DateTime.Now;

        //csv�̂Ƃ�
        string file1 = Application.persistentDataPath + "/BallPos_x.csv";
        string file2 = Application.persistentDataPath + "/BallPos_y.csv";
        string file3 = Application.persistentDataPath + "/BallPos_z.csv";

        //txt�̂Ƃ�
        //string file1 = Application.persistentDataPath + "/BallPos_x.txt";
        //string file2 = Application.persistentDataPath + "/BallPos_y.txt";
        //string file3 = Application.persistentDataPath + "/BallPos_z.txt";

        if (!File.Exists(file1))
        {
            Debug.Log("�t�@�C��1���쐬���܂�");
            sw_x = File.CreateText(file1);
            sw_x.Flush();
            sw_x.Dispose();
        }
        else
        {
            Debug.Log("The file already exists.�t�@�C��1�͊��ɑ��݂��Ă��܂�");
        }
        
        if (!File.Exists(file2))
        {
            Debug.Log("�t�@�C��2���쐬���܂�");
            sw_y = File.CreateText(file2);
            sw_y.Flush();
            sw_y.Dispose();
        }
        else
        {
            Debug.Log("The file2 already exists.�t�@�C��2�͊��ɑ��݂��Ă��܂�");
        }

        if (!File.Exists(file3))
        {
            Debug.Log("�t�@�C��3���쐬���܂�");
            sw_z = File.CreateText(file3);
            sw_z.Flush();
            sw_z.Dispose();
        }
        else
        {
            Debug.Log("The file3 already exists.�t�@�C��3�͊��ɑ��݂��Ă��܂�");
        }

        //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
        sw_x = new StreamWriter(file1, true, Encoding.UTF8);
        sw_y = new StreamWriter(file2, true, Encoding.UTF8);
        sw_z = new StreamWriter(file3, true, Encoding.UTF8);

        //���s
        sw_x.WriteLine();
        sw_y.WriteLine();
        sw_z.WriteLine();

        //������������
        sw_x.WriteLine(Convert.ToString(t2));
        sw_y.WriteLine(Convert.ToString(t2));
        sw_z.WriteLine(Convert.ToString(t2));

        //Debug.Log(t2);
    }

    void Update()
    {
        //�I�u�W�F�N�g��transform���擾
        Transform ballTransform = CentralBall.transform;

        //�I�u�W�F�N�g�̃��[�J�����W���擾
        Vector3 ballPos = ballTransform.localPosition;
        float x = ballPos.x;
        float y = ballPos.y;
        float z = ballPos.z;

        //Debug.Log($"{x},{z}");

        //�t�@�C���̖����ɒl��ǉ��iConvert��float�^�̍��W�l��String�^�ɕϊ����Ă���j
        //csv�̂Ƃ�
        sw_x.WriteLine(Convert.ToString(x));
        sw_y.WriteLine(Convert.ToString(y));
        sw_z.WriteLine(Convert.ToString(z));


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
