
namespace Galaxy.AssetPipeline
{
    public class AssetScanUpdateEventArgs : GameEventArgs
    {
        public AssetScanUpdateEventArgs(float progress, int current, int total, string status = "")
        {
            Progress = progress;
            Current = current;
            Total = total;
            Status = status;
        }
        
        public string Status
        {
            get;
            private set;
        }

        public float Progress {
            get;
            private set;
        }

        public int Current {
            get;
            private set;
        }

        public int Total {
            get;
            private set;
        }
    }
}
