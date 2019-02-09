namespace XWorld
{
    public class AValueNode
    {
        private AValueNode m_pExNode; // Buff�ӽڵ㣬ʹ������ĺϲ���ʽ
        public bool NeedCalculate;

        public AValueMask m_NodeMask;

        public AValueStruct m_NodeVS; // �ڵ㵱ǰ���Խ��

        public AValueNode m_pParentNode;

        public const int MAXCHILDNODE = 6;
        public int m_nChildNodeCnt;
        public AValueNode[] m_pChildNode;

        public AValueNode()
        {
            NeedCalculate = true;

            m_pParentNode = new AValueNode();
            m_NodeVS = new AValueStruct();
            m_nChildNodeCnt = 0;

            m_pChildNode = new AValueNode[MAXCHILDNODE];

            m_pExNode = null;
        }

        public void AddChildNode(AValueNode pNode)
        {
            if (pNode != null && m_nChildNodeCnt < MAXCHILDNODE)
            {
                m_pChildNode[m_nChildNodeCnt] = pNode;
                m_nChildNodeCnt++;

                pNode.m_pParentNode = this;
            }
        }

        public void SetExNode(AValueNode pNode)
        {
            m_pExNode = pNode;
            if (null != pNode)
            {
                pNode.m_pParentNode = this;
            }
        }

        public void ReCalculate(bool bAtOnce, AValueMask pMask, bool fcChangeNotify)
        {
            NeedCalculate = true;

            if (null != pMask)
                m_NodeMask.Combine(pMask);
            else
                m_NodeMask.CalculateAll = true;

            if (null != m_pParentNode)
                m_pParentNode.ReCalculate(bAtOnce, pMask, fcChangeNotify);
        }

        public void CalculateAValues()
        {
            NeedCalculate = false;

            AValueStruct  vs = m_NodeVS;

            if (m_NodeMask.CalculateAll)
            {
                m_NodeMask.CalculateAll = false;
                m_NodeMask = null;
            }

            // ���ñ������������
            vs.Reset(m_NodeMask);

            // �ϲ��ӽڵ����ֵ
            for (int i = 0; i < m_nChildNodeCnt; i++)
            {
                AValueNode pNode = m_pChildNode[i];
                if (null != pNode)
                {
                    if (pNode.NeedCalculate)
                        pNode.CalculateAValues(); // ����ӽڵ���Ҫ���¼���...

                    // �ϲ��ӽڵ�ĵ�ǰ���Խṹ
                    pNode.CombineAValues(vs, m_NodeMask, true);
                }
            }

            // �ϲ����ڵ�����Խṹ
            AddupAValues(vs, m_NodeMask);

            ExNodeCalculate(vs, m_NodeMask);

            ExportResult(vs, m_NodeMask);

            m_NodeMask.Reset(); // ��λ
        }
        public virtual void AddupAValues(AValueStruct vs, AValueMask pMask) { }

        public void ExNodeCalculate(AValueStruct vs, AValueMask pMask)
        {
            if (null != m_pExNode )
            {
                if (m_pExNode.NeedCalculate)
                    m_pExNode.CalculateAValues(); // ����ӽڵ���Ҫ���¼���...

                m_pExNode.CombineAValues(vs, pMask, true);
            }
        }
        public virtual void ExportResult(AValueStruct vs, AValueMask pMask) { }
        public void CombineAValues(AValueStruct vs, AValueMask pMask, bool bLinear)
        {
            vs.Combine(m_NodeVS, pMask, bLinear);
        }
    };
}