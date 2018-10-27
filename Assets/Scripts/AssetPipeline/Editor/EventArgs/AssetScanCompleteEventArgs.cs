
namespace Galaxy.AssetPipeline
{
    public class AssetScanCompleteEventArgs : GameEventArgs
    {
        public AssetScanCompleteEventArgs(bool success)
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
