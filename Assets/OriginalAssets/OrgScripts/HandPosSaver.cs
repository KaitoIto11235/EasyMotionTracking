using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; //�t�@�C���ɏ������ނ��߂ɕK�v
using System; //Convert��DeteTime���g�����߂ɕK�v
using System.Text; //�����R�[�h���w�肷�邽�߂ɕK�v

public class HandPosSaver : MonoBehaviour
{
    StreamWriter sw_x; //x���W�L�^�p
    StreamWriter sw_y; //y���W�L�^�p
    StreamWriter sw_z; //z���W�L�^�p
    public GameObject handAva, realHead;//�ʒu���L�^�������I�u�W�F�N�g�p�ϐ�
    DateTime t2; //�����p�̕ϐ�

    void Start()
    {
        //���Ԃ��擾
        t2 = DateTime.Now;

        //csv�̂Ƃ�
        string file4 = Application.persistentDataPath + "/HandAvaPos_x.csv";
        string file5 = Application.persistentDataPath + "/HandAvaPos_y.csv";
        string file6 = Application.persistentDataPath + "/HandAvaPos_z.csv";

        //txt�̂Ƃ�
        //string file1 = Application.persistentDataPath + "/BallPos_x.txt";
        //string file2 = Application.persistentDataPath + "/BallPos_y.txt";
        //string file3 = Application.persistentDataPath + "/BallPos_z.txt";

        if (!File.Exists(file4))
        {
            Debug.Log("�t�@�C��4���쐬���܂�");
            sw_x = File.CreateText(file4);
            sw_x.Flush();
            sw_x.Dispose();
        }
        else
        {
            Debug.Log("The file4 already exists.�t�@�C��4�͊��ɑ��݂��Ă��܂�");
        }
        
        if (!File.Exists(file5))
        {
            Debug.Log("�t�@�C��5���쐬���܂�");
            sw_y = File.CreateText(file5);
            sw_y.Flush();
            sw_y.Dispose();
        }
        else
        {
            Debug.Log("The file5 already exists.�t�@�C��5�͊��ɑ��݂��Ă��܂�");
        }

        if (!File.Exists(file6))
        {
            Debug.Log("�t�@�C��6���쐬���܂�");
            sw_z = File.CreateText(file6);
            sw_z.Flush();
            sw_z.Dispose();
        }
        else
        {
            Debug.Log("The file6 already exists.�t�@�C��6�͊��ɑ��݂��Ă��܂�");
        }

        //UTF-8�Ő���...2�Ԗڂ̈�����true�Ŗ����ɒǋL�Cfalse�Ńt�@�C�����Ə㏑���D
        sw_x = new StreamWriter(file4, true, Encoding.UTF8);
        sw_y = new StreamWriter(file5, true, Encoding.UTF8);
        sw_z = new StreamWriter(file6, true, Encoding.UTF8);

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
        Transform handAvaTransform = handAva.transform;
        Transform headTransform = realHead.transform;

        //�I�u�W�F�N�g�̃��[�J�����W���擾
        Vector3 handAvaPos = handAvaTransform.position;
        Vector3 headPos = headTransform.position;

        float x = handAvaPos.x - headPos.x;
        float y = handAvaPos.y - headPos.y;
        float z = handAvaPos.z - headPos.z;

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
