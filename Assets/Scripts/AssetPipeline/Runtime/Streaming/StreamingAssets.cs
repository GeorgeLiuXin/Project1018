using UnityEngine;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Collections.Generic;

namespace XWorld
{
    public class StreamingAssets
    {
        public const string CACHE_DIR = "Bundles";
        public const int BUFFER_SIZE = 4096; // 4K is optimum
        public static System.Action<float, int, int> processHandle;
        public static System.Action<string> logHandle;
        public static object receiver;

        public static string dataPath = "";
        public static string temporaryCachePath = "";

        private static int realSign = 0;
        private static int fileCount = 0;
        private static float progress = 0;
        private static Dictionary<int, float> progressMap = new Dictionary<int, float>();

        public static void SingleProgressHandler(object sender, ProgressEventArgs e)
        {
            int cou = int.Parse(e.Name);
            progressMap[cou] = e.PercentComplete / 100.0f;

            float tempP = 0;
            int completeCount = 0;
            foreach (float value in progressMap.Values)
            {
                if (value >= 1f)
                {
                    completeCount++;
                }
                tempP += value;
            }
            progress = (tempP / (float)(fileCount));
            if (processHandle != null)
                processHandle.Invoke(progress, fileCount, completeCount);
        }

        public static void ExtractStreamingAssets()
        {
            ExtractZipFile(dataPath, "assets/" + CACHE_DIR + "/", temporaryCachePath);
        }

        public static void ExtractZipFile(string archiveFilenameIn, string baseFolder, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                fileCount = 0;

                if(logHandle != null)
                    logHandle("解压包的数量" + zf.Count);
                //add for fileCount
                for (int i = 0; i < zf.Count; i++)
                {
                    ZipEntry zipEntry = zf[i];
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }

                    string entryFileName = zipEntry.Name;
                    if (!entryFileName.StartsWith(baseFolder))
                    {

                        if (logHandle != null)
                            logHandle("跳过:" + entryFileName);
                        continue;
                    }
                    else
                    {
                        if (logHandle != null)
                            logHandle("记录的文件:" + entryFileName);
                    }
                    fileCount++;
                }
                //add for progressMap
                progressMap.Clear();
                for (int j = 0; j < fileCount; j++)
                {
                    progressMap.Add(j, 0);
                }

                realSign = -1;
                for (int i = 0; i < zf.Count; i++)
                {
                    ZipEntry zipEntry = zf[i];
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }

                    string entryFileName = zipEntry.Name;
                    if (!entryFileName.StartsWith(baseFolder))
                    {
                        continue;
                    }

                    realSign++;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    string fullZipToPath = Path.Combine(outFolder, entryFileName.Replace(baseFolder, string.Empty));
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (logHandle != null)
                        logHandle("准备写入的文件：" + fullZipToPath + "  文件夹名：" + directoryName);
                    if (directoryName.Length > 0 && !Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer, SingleProgressHandler, new System.TimeSpan(500), receiver
                            , realSign.ToString());
                    }
                }

                if (fileCount == 0)
                {
                    if (processHandle != null)
                        processHandle.Invoke(1, 1, 1);
                }
            }
            catch (System.Exception e)
            {
                if (logHandle != null)
                    logHandle(e.ToString());
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }
    }
}
