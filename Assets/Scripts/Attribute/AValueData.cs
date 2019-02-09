namespace XWorld
{
    public class LevelData
    {
        public float BaseValue;
        public float PrivatePercent;
        public float InternalPercent;
        public float PublicPercent1;
        public float PublicPercent2;
    };


    public class AValueData
    {
        public enum AValueDateEnum
        {
            BaseValueOffset = 0,
            PrivatePercentOffset,
            InternalPercentOffset,
            PublicPercent1Offset,
            PublicPercent2Offset,
            LevelDataCnt, // 5

            sizeofLevel = sizeof(float) * LevelDataCnt, // 20

            PrivateValue = 0,   // ˽��
            InternalValue = 1,  // ����
            Public1Value = 2,   // �ۼ� ����
            Public2Value = 5    // �ۼ� ������
        };

        private float[,] m_ValueDatas;//[LevelCnt][LevelDataCnt]

        private bool[] m_ValidLevel;//[LevelCnt]

        private LevelData[] m_LevelDatas;

        private int m_nLevelCnt;

        public bool ValidData;

        public AValueData(int levelCnt)
        {
            m_nLevelCnt = levelCnt;
            ValidData = false;
            m_ValueDatas = new float[m_nLevelCnt,(int)AValueDateEnum.LevelDataCnt];
            m_LevelDatas = new LevelData[m_nLevelCnt];
            m_ValidLevel = new bool [(int)AValueDateEnum.LevelDataCnt];
        }

        public void AddupBaseValue( int nLevel,  float absoluteValue)
        {
            if (nLevel >= 0 && nLevel < m_nLevelCnt)
            {
                m_ValueDatas[nLevel,(int)AValueDateEnum.BaseValueOffset] += absoluteValue;
                m_ValidLevel[nLevel] = true;
                ValidData = true;
            }
        }

        public void AddupPercentValue( int nLevel,  int nMode,  float percentValue)
        {
            if (nLevel >= 0 && nLevel < m_nLevelCnt)
            {
                switch ((AValueDateEnum)nMode)
                {
                    case AValueDateEnum.PrivateValue:
                        m_ValueDatas[nLevel,(int)AValueDateEnum.PrivatePercentOffset] += percentValue;
                        break;

                    case AValueDateEnum.InternalValue:
                        m_ValueDatas[nLevel,(int)AValueDateEnum.InternalPercentOffset] += percentValue;
                        break;

                    case AValueDateEnum.Public1Value:
                        m_ValueDatas[nLevel,(int)AValueDateEnum.PublicPercent1Offset] += percentValue;
                        break;

                    case AValueDateEnum.Public2Value:
                        m_ValueDatas[nLevel,(int)AValueDateEnum.PublicPercent2Offset] += percentValue;
                        break;
                }

                m_ValidLevel[nLevel] = true;
                ValidData = true;
            }
        }

        public void Combine( AValueData pSrcValue, ref bool bLinear)
        {
            if (pSrcValue == null) return;

            if (m_nLevelCnt != pSrcValue.m_nLevelCnt)
                return; /*something wrong*/

            if (!pSrcValue.ValidData)
                return; /*ԴAValueData������Ч����*/

            if (bLinear)
            {
                /*���Ժϲ�����*/
                if (!ValidData)
                {
                    /*Ŀ��AValueData������Ч�����Ұ����Ժϲ������帴��*/
                    m_ValueDatas = pSrcValue.m_ValueDatas;
                    m_ValueDatas = pSrcValue.m_ValueDatas;
                    ValidData = true;
                }
                else
                {
                    for (int i = 0; i < m_nLevelCnt; i++)
                    {
                        if (pSrcValue.m_ValidLevel[i])
                        {
                            if (m_ValidLevel[i])
                            {
                                for (int j = 0; j < (int)AValueDateEnum.LevelDataCnt; j++)
                                {
                                    m_ValueDatas[i,j] += pSrcValue.m_ValueDatas[i,j];
                                }
                            }
                            else
                            {
                                for (int j = 0; j < (int)AValueDateEnum.LevelDataCnt; j++)
                                {
                                    m_ValueDatas[i,j] = pSrcValue.m_ValueDatas[i,j];
                                }
                                m_ValidLevel[i] = true;
                            }
                        }
                    }
                }
            }
            else
            {/*�����Ժϲ�����*/
                for (int i = 0; i < m_nLevelCnt; i++)
                {
                    if (pSrcValue.m_ValidLevel[i])
                    {
                        for (int j = 0; j < (int)AValueDateEnum.LevelDataCnt; j++)
                        {
                            if (j == (int)AValueDateEnum.BaseValueOffset && pSrcValue.m_ValueDatas[i,j] < 0)
                            {/*ֻ�л���ֵΪ����ʱ�����з����Ժϲ�*/
                                float dbase = m_ValueDatas[i,j];
                                m_ValueDatas[i,j] = dbase * dbase / (dbase - pSrcValue.m_ValueDatas[i,j]);
                            }
                            else
                            {
                                m_ValueDatas[i,j] += pSrcValue.m_ValueDatas[i,j];
                            }
                        }
                        m_ValidLevel[i] = true;
                    }
                }
            }
        }

        public float Calculate(bool bPrivate, float extraBase = 0.0f)
        {
            float result = extraBase;
            const float COMPUTEERROR = 0.01f;

            for (int i = 0; i < m_nLevelCnt; i++)
            {
                float baseValue = result;
                //������ٷֱȻ���Ϊ�������ֵ//
                float internalPercent = m_ValueDatas[i,(int)AValueDateEnum.InternalPercentOffset];
                if (internalPercent > -COMPUTEERROR && internalPercent < +COMPUTEERROR)
                    baseValue += m_ValueDatas[i,(int)AValueDateEnum.BaseValueOffset];
                else
                    baseValue += m_ValueDatas[i,(int)AValueDateEnum.BaseValueOffset] * (1.0f + internalPercent);

                if (bPrivate)
                {
                    float privateValue = m_ValueDatas[i,(int)AValueDateEnum.PrivatePercentOffset] * baseValue; // ˽��ֵ
                    m_ValueDatas[i,(int)AValueDateEnum.BaseValueOffset] += privateValue; // �ۼӵ�����ֵ��
                    m_ValueDatas[i,(int)AValueDateEnum.PrivatePercentOffset] = 0; // ����Ѿ���ɼ����˽��ֵ
                    result += m_ValueDatas[i,(int)AValueDateEnum.BaseValueOffset]; // ˽��ֵ���������
                }
                else
                {
                    float publicPercent1 = m_ValueDatas[i,(int)AValueDateEnum.PublicPercent1Offset];

                    if (publicPercent1 > -COMPUTEERROR && publicPercent1 < +COMPUTEERROR)
                        result = baseValue;
                    else
                        result = baseValue * (1.0f + publicPercent1);

                    float publicPercent2 = m_ValueDatas[i,(int)AValueDateEnum.PublicPercent2Offset];

                    if (publicPercent2 > +COMPUTEERROR)
                        result *= (1.0f + publicPercent2);
                    else if (publicPercent2 < -COMPUTEERROR)
                        result /= (1.0f - publicPercent2);
                }
            }
            return result;
        }
	};
}