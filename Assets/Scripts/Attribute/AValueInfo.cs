namespace XWorld
{
    public class AValueInfo
    {
        public AValueInfo()
        {
            ValueName = "";
            MaxDepth = 4;
            ShowNameID = -1;
            FightFactor = 0.0f;
            fBaseValueBalance = 0.0f;
        }

        public string ValueName;
        public int MaxDepth;
        public int ShowNameID;
        public float FightFactor;
        public float fBaseValueBalance;
    };
}