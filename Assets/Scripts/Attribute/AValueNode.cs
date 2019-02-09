namespace XWorld
{
    public class AValueNode
    {
        private AValueNode m_pExNode; // Buff子节点，使用特殊的合并方式
        public bool NeedCalculate;

        public AValueMask m_NodeMask;

        public AValueStruct m_NodeVS; // 节点当前属性结果

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

            // 重置标脏的属性数据
            vs.Reset(m_NodeMask);

            // 合并子节点的数值
            for (int i = 0; i < m_nChildNodeCnt; i++)
            {
                AValueNode pNode = m_pChildNode[i];
                if (null != pNode)
                {
                    if (pNode.NeedCalculate)
                        pNode.CalculateAValues(); // 如果子节点需要重新计算...

                    // 合并子节点的当前属性结构
                    pNode.CombineAValues(vs, m_NodeMask, true);
                }
            }

            // 合并本节点的属性结构
            AddupAValues(vs, m_NodeMask);

            ExNodeCalculate(vs, m_NodeMask);

            ExportResult(vs, m_NodeMask);

            m_NodeMask.Reset(); // 复位
        }
        public virtual void AddupAValues(AValueStruct vs, AValueMask pMask) { }

        public void ExNodeCalculate(AValueStruct vs, AValueMask pMask)
        {
            if (null != m_pExNode )
            {
                if (m_pExNode.NeedCalculate)
                    m_pExNode.CalculateAValues(); // 如果子节点需要重新计算...

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