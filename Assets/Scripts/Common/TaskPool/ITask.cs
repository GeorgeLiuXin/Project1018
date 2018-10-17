

namespace XWorld
{
    public interface ITask
    {
        int SerialId
        {
            get;
        }
        
        bool Done
        {
            get;
        }
    }
}
