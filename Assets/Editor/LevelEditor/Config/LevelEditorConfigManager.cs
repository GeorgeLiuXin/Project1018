using System.IO;

namespace Galaxy
{
    public class LevelEditorConfigManager
    {
        public LevelEditor_SceneDefine m_sceneDefine;
        public LevelEditor_SceneContentSpawner m_sceneContentSpawner;
        public LevelEditor_NpcGrow m_npcGrow;
        public LevelEditor_ModelRes m_modelRes;
        public LevelEditor_CommonEffect m_effect;
        public LevelEditor_Collision m_collision;
        public LevelEditor_SceneGrow m_sceneGrow;
        public LevelEditor_TriggerDefine m_triggerDefine;

        public LevelEditorConfigManager()
        {
            m_modelRes = new LevelEditor_ModelRes();

            m_sceneDefine = new LevelEditor_SceneDefine();
            m_sceneContentSpawner = new LevelEditor_SceneContentSpawner();
            m_npcGrow = new LevelEditor_NpcGrow();
            m_collision = new LevelEditor_Collision();
            m_effect = new LevelEditor_CommonEffect();
            m_sceneGrow = new LevelEditor_SceneGrow();
            m_triggerDefine = new LevelEditor_TriggerDefine();
        }

        public bool LoadEditorConfig()
        {
            if (!PreLoadConfigs())
                return false;

            DirectoryInfo t_tempInfo = new DirectoryInfo(UnityEngine.Application.dataPath);
            t_tempInfo = t_tempInfo.Parent;
            t_tempInfo = t_tempInfo.Parent;
            string strPath = t_tempInfo.FullName + "/Server/bin32/Data/DataTable/";
            strPath = strPath.Replace('/', '\\');

            //scenedefine
            string strAllPath = strPath + m_sceneDefine.TableName;
            string strContent = GetFileContent(strAllPath);
            m_sceneDefine.SetModelRes(m_modelRes);
            m_sceneDefine.LoadData(strContent);

            // param_monster
            strAllPath = strPath + m_sceneContentSpawner.TableName;
            strContent = GetFileContent(strAllPath);
            m_sceneContentSpawner.SetModelRes(m_modelRes);
            m_sceneContentSpawner.LoadData(strContent);

            //Collision
            strAllPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/Dynamic/ClientData/" + m_collision.TableName;
            strContent = GetFileContent(strAllPath);
            m_collision.LoadData(strContent);


            //Grow
            strAllPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/Static/" + m_npcGrow.TableName;
            strContent = GetFileContent(strAllPath);
            m_npcGrow.SetModelRes(m_modelRes);
            m_npcGrow.SetCommonEffect(m_effect);
            m_npcGrow.LoadData(strContent);

            strAllPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/Static/" + m_sceneGrow.TableName;
            strContent = GetFileContent(strAllPath);
            m_sceneGrow.LoadData(strContent);
            m_sceneGrow.LoadGrowConfig(ref m_npcGrow);

            UnityEditor.AssetDatabase.Refresh();
            return true;
        }

        public void LoadClientConfig(int mapId)
        {
            //Trigger
            string strAllPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/LevelTrigger/" + "scene_content_trigger" + mapId.ToString() + ".txt";
            if (!File.Exists(strAllPath))
                return;

            string strContent = GetFileContent(strAllPath);
            m_triggerDefine.LoadData(strContent);
        }

        private bool PreLoadConfigs()
        {
            string modelResPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/Dynamic/ClientData/modelresdefine.txt";
            string strModelResContent = GetFileContent(modelResPath);
            if (strModelResContent != null)
            {
                m_modelRes = new LevelEditor_ModelRes();
                m_modelRes.LoadData(strModelResContent);
            }

            string strEffPath = UnityEngine.Application.dataPath + "/AssetDatas/Config/Dynamic/ClientData/commoneffects.txt";
            string strEffContent = GetFileContent(strEffPath);
            if(strEffContent != null)
            {
                m_effect = new LevelEditor_CommonEffect();
                m_effect.LoadData(strEffContent);
            }
            return true;
        }

        private string GetFileContent(string strPath)
        {
            FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read, FileShare.None);
            if (fs == null)
            {
                return null;
            }

            string strContent = null;
            StreamReader sr = new StreamReader(fs);
            if (sr != null)
            {
                strContent = sr.ReadToEnd();
                fs.Close();
                return strContent;
            }

            return null;
        }
    }
}
