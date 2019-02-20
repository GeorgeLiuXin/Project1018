using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Galaxy
{

    public class EffectConfigEditor : Editor
    {
        public static void ShowEditor()
        {
            EffectConfigEditorWindow window = EditorWindow.GetWindow<EffectConfigEditorWindow>();
            if (window)
            {
                window.ShowUtility();
                window.minSize = new Vector2(800, 300);
                window.maxSize = window.minSize;
                var position = window.position;
                position.center = new Rect(0f, 0f, Screen.currentResolution.width, Screen.currentResolution.height).center;
                window.position = position;
                window.Show();
            }
        }
    }

    public class EffectConfigEditorWindow : EditorWindow
    {
        private GameObject m_effectObj;
        private GameObject effectObj
        {
            set
            {
                if (m_effectObj == value)
                    return;
                m_effectObj = value;
                if (m_effectObj != null)
                {
                    InitObject();
                }
            }
            get
            {
                return m_effectObj;
            }
        }

        private enum eEffectConfigMode
        {
            None,
            Animation,
            Animator,
            OnlyParticleSystem,
        }

        private string m_HintString
        {
            get
            {
                if (m_bPlaying)
                {
                    return "暂停";
                }
                return "继续";
            }
        }

        private eEffectConfigMode m_CurMode;
        private GameObject m_ObjectWithAnim;
        private Animation m_Animation;
        private AnimationClip m_AnimClip;
        private Animator m_Animtor;
        private ParticleSystem[] m_ParticleSystemList;

        private void InitObject()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            switch (m_CurMode)
            {
                case eEffectConfigMode.Animation:
                    AnimationMode.EndSampling();
                    AnimationMode.StopAnimationMode();
                    m_Animation = null;
                    m_AnimClip = null;
                    m_ParticleSystemList = null;
                    break;
                case eEffectConfigMode.Animator:
                    m_Animtor = null;
                    m_ParticleSystemList = null;
                    break;
                case eEffectConfigMode.OnlyParticleSystem:
                    m_ParticleSystemList = null;
                    break;
            }
            m_Animation = m_effectObj.GetComponentInChildren<Animation>();
            m_Animtor = m_effectObj.GetComponentInChildren<Animator>();
            m_ParticleSystemList = m_effectObj.GetComponentsInChildren<ParticleSystem>(true);

            m_bHasBake = false;
            bake();
        }

        private void OnEnable()
        {
            m_fPreviousTime = EditorApplication.timeSinceStartup;
            EditorApplication.update += inspectorUpdate;
            m_CurMode = eEffectConfigMode.None;
        }

        private void OnDisable()
        {
            m_effectObj = null;
            m_Animation = null;
            m_Animtor = null;
            m_ParticleSystemList = null;
            EditorApplication.update -= inspectorUpdate;
            m_CurMode = eEffectConfigMode.None;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField("默认当前特效仅有一个Animation或Animator组件");
                effectObj = EditorGUILayout.ObjectField("EffectObject", effectObj, typeof(GameObject), true) as GameObject;

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("播放"))
                    {
                        play();
                    }
                    if (GUILayout.Button(m_HintString))
                    {
                        m_bPlaying = !m_bPlaying;
                    }
                    if (GUILayout.Button("停止"))
                    {
                        stop();
                    }
                }
                GUILayout.EndHorizontal();

                EditorGUILayout.LabelField("当前播放帧数:");
                GUILayout.BeginHorizontal();
                {
                    m_fCurTime = EditorGUILayout.Slider("Time:", m_fCurTime, 0f, m_fRecorderStopTime);
                    m_bLooping = EditorGUILayout.Toggle("是否循环", m_bLooping);
                }
                GUILayout.EndHorizontal();
                manualUpdate();
            }
            GUILayout.EndVertical();
        }

        #region 具体逻辑

        /// <summary>
        /// 滑动杆的当前时间
        /// </summary>
        private float m_fCurTime;

        /// <summary>
        /// 是否已经烘培过
        /// </summary>
        private bool m_bHasBake;

        /// <summary>
        /// 当前是否是预览播放状态
        /// </summary>
        private bool m_bPlaying;

        /// <summary>
        /// 当前是否是循环播放
        /// </summary>
        private bool m_bLooping;

        /// <summary>
        /// 当前运行时间
        /// </summary>
        private float m_fRunningTime;

        /// <summary>
        /// 上一次系统时间
        /// </summary>
        private double m_fPreviousTime;

        /// <summary>
        /// 总的记录时间
        /// </summary>
        private float m_fRecorderStopTime;

        /// <summary>
        /// 滑动杆总长度
        /// </summary>
        private const float m_fDuration = 30f;

        /// <summary>
        /// 进行预览播放
        /// </summary>
        private void play()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            m_fRunningTime = 0f;
            m_bPlaying = true;
        }

        /// <summary>
        /// 停止预览播放
        /// </summary>
        private void stop()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            m_bPlaying = false;
            m_fCurTime = 0f;
        }

        /// <summary>
        /// 预览播放状态下的更新
        /// </summary>
        private void update()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            if (m_fRunningTime >= m_fRecorderStopTime)
            {
                if (!m_bLooping)
                {
                    m_bPlaying = false;
                    return;
                }

                m_fRunningTime -= m_fRecorderStopTime;
            }

            UpdateAll(m_fRunningTime);
            Repaint();

            m_fCurTime = m_fRunningTime;
        }


        /// <summary>
        /// 烘培记录动画数据
        /// 当前仅有一个动画的时候表现正确
        /// </summary>
        private void bake()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            if (m_bHasBake)
                return;

            m_fRecorderStopTime = 30f;
            float frameRate = 30f;
            if (m_Animtor != null && m_Animtor.GetCurrentAnimatorClipInfoCount(0) != 0)
            {
                int frameCount = (int)((m_Animtor.GetCurrentAnimatorStateInfo(0).length * frameRate) + 2);
                m_Animtor.Rebind();
                m_Animtor.StopPlayback();
                m_Animtor.recorderStartTime = 0;

                // 开始记录指定的帧数
                m_Animtor.StartRecording(frameCount);

                for (var i = 0; i < frameCount - 1; i++)
                {
                    m_Animtor.Update(1.0f / frameRate);
                }
                // 完成记录
                m_Animtor.StopRecording();

                // 开启回放模式
                m_Animtor.StartPlayback();
                m_bHasBake = true;
                m_fRecorderStopTime = GetMaxLifeTime(m_Animtor.recorderStopTime);
                m_CurMode = eEffectConfigMode.Animator;
                m_ObjectWithAnim = m_Animtor.gameObject;
            }
            else if (m_Animation != null && m_Animation.clip != null)
            {
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                m_bHasBake = true;
                m_AnimClip = m_Animation.clip;
                m_fRecorderStopTime = GetMaxLifeTime(m_AnimClip.length);
                m_CurMode = eEffectConfigMode.Animation;
                m_ObjectWithAnim = m_Animation.gameObject;
            }
            else
            {
                m_bHasBake = true;
                m_fRecorderStopTime = GetMaxLifeTime(0f);
                m_CurMode = eEffectConfigMode.OnlyParticleSystem;
                m_ObjectWithAnim = m_effectObj.gameObject;
            }
        }
        private float GetMaxLifeTime(float fOtherLifeTime)
        {
            if (m_ParticleSystemList == null)
                return fOtherLifeTime;

            float fMaxTime = 0f;
            foreach (var item in m_ParticleSystemList)
            {
                float fTime = item.main.startDelayMultiplier + item.main.duration;
                if (fTime > fMaxTime)
                {
                    fMaxTime = fTime;
                }
            }
            fMaxTime = Mathf.Max(fMaxTime, fOtherLifeTime);
            return fMaxTime;
        }

        /// <summary>
        /// 非预览播放状态下，通过滑杆来播放当前动画帧
        /// </summary>
        private void manualUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }

            if (!m_bPlaying && m_bHasBake && m_fCurTime < m_fRecorderStopTime)
            {
                UpdateAll(m_fCurTime);
            }
        }

        private void inspectorUpdate()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            var delta = EditorApplication.timeSinceStartup - m_fPreviousTime;
            m_fPreviousTime = EditorApplication.timeSinceStartup;

            if (m_bPlaying)
            {
                m_fRunningTime = Mathf.Clamp(m_fRunningTime + (float)delta, 0f, m_fDuration);
                update();
            }
        }

        private void UpdateAll(float time)
        {
            switch (m_CurMode)
            {
                case eEffectConfigMode.Animation:
                    if (m_Animation != null)
                    {
                        if (m_AnimClip != null)
                        {
                            float process = time / m_AnimClip.length;
                            process = Mathf.Clamp(process, 0, 1);
                            AnimationMode.SampleAnimationClip(m_ObjectWithAnim, m_AnimClip, process * m_AnimClip.length);
                        }
                    }
                    break;
                case eEffectConfigMode.Animator:
                    if (m_Animtor != null)
                    {
                        m_Animtor.playbackTime = time;
                        m_Animtor.Update(0);
                    }
                    break;
                case eEffectConfigMode.OnlyParticleSystem:
                    break;
            }
            if (m_ParticleSystemList != null)
            {
                foreach (var item in m_ParticleSystemList)
                {
                    item.Simulate(time * item.main.simulationSpeed);
                }
            }
        }

        #endregion

    }

}