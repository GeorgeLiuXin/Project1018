
namespace Galaxy.AssetPipeline
{
    public class BundleBuildCompleteEventArgs : GameEventArgs
    {
        public BundleBuildCompleteEventArgs(bool success)
        {
            Success = success;
        }

        public bool Success
        {
            get;
            private set;
        }
    }
}
