using System;
using System.Collections.Generic;

namespace ShangHaiPro
{
    public class Send_Report_Data
    {
        public string fileName;//�����ļ�����
        public int fileType;//�����ļ����� 0�Ƿֿ�ͳ�Ʒ��� 1�Ǻϲ�ͳ�Ʒ���
        public DateTime Time;//���ɱ���ʱ��

        public string reportName;//�����������
        public List<Report_Data> reportDataList;//�����б�

        public float totalVolume;//�����
        public float totalWeight;//������

        public List<string> peopleList;//������Ա
        public string supervisionAuditDepartment;//����
        public string financeDepartment;//����
        public string operationDepartment;//���в�
        public string operationDepartment2;//����2��
        public string maintenanceDepartment;//���޲�
        public string combustionPipesDepartment;//ȼ�ܲ�
        public List<string> reviewersList;//�����Ա

        public string remark;//��ע
    }
}
