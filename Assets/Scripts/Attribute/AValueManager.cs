using System.Collections.Generic;

namespace XWorld
{	
	class AValueManager
	{
        ~AValueManager()
        {
            Clear();
        }

        public virtual void Clear()
        {
            m_mapAValueStructs.Clear();
        }

        // class LoadTemplate override function
        //public virtual bool LoadDataFromDB(GDBInterface* pDBI);

        //public virtual bool LoadAValueGroup( GDBInterface* pDBI );
        //public virtual bool LoadAValueItem( GDBInterface* pDBI );

        //public AValueStruct GetAValueStruct( int nAvalueID );

        protected string m_strDefineTable;
        protected string m_strValueTable;

        Dictionary<int, AValueStruct> m_mapAValueStructs;
	};
}