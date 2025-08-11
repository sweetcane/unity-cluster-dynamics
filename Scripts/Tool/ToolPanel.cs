using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [System.Serializable]
    public class Styles
    {
        private GUIStyle _rich;
        public GUIStyle Rich
        {
            get
            {
                if (_rich == null)
                {
                    _rich = new GUIStyle(GUI.skin.label)
                    {
                        richText = true,
                        wordWrap = true
                    };
                }

                return _rich;
            }
        }

        [SerializeField] private Color toolBoxColor = new Color(0.2f, 0.2f, 0.2f, 0.5f);
        private GUIStyle _toolBox;
        public GUIStyle ToolBox
        {
            get
            {
                if (_toolBox == null)
                {
                    _toolBox = new GUIStyle(GUI.skin.box)
                    {
                        normal = { background = MakeTex(2, 2, toolBoxColor) },
                        richText = true,
                        wordWrap = true,
                        padding = new RectOffset(6, 6, 3, 3)
                    };
                }

                return _toolBox;
            }
        }

        [SerializeField] private Color statusBarColor = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        private GUIStyle _statusBar;
        public GUIStyle StatusBar
        {
            get
            {
                if (_statusBar == null)
                {
                    _statusBar = new GUIStyle(GUI.skin.box)
                    {
                        normal = { background = MakeTex(2, 2, statusBarColor) },
                        richText = true,
                        wordWrap = true,
                        padding = new RectOffset(6, 6, 3, 3)
                    };
                }

                return _statusBar;
            }
        }

        private Texture2D MakeTex(int w, int h, Color c)
        {
            var tex = new Texture2D(w, h) { wrapMode = TextureWrapMode.Clamp };
            var pixels = new Color[w * h];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
            tex.SetPixels(pixels);
            tex.Apply();
            return tex;
        }
    }

    public class ToolPanel : MonoBehaviour
    {
        [SerializeField] private Styles styles;
        [SerializeField] private List<ToolAsset> tools;
        [SerializeField] private Rect screenRect = new Rect(0, 0, 360, 720);
        [SerializeField] private string search = "";

        private bool isBusy = false;


        [SerializeField] Color toolColor;

        // ---------------------------------------------------------------------------------------

        private void OnGUI()
        {
            GUILayout.BeginArea(screenRect, GUI.skin.window);
            GUILayout.Label("<b>Tool Panel</b>", styles.Rich);
        
            // // 搜索栏
            // GUILayout.BeginHorizontal();
            // GUILayout.Label("Search:", GUILayout.Width(50));
            // search = GUILayout.TextField(search);
            // GUILayout.EndHorizontal();

            GUILayout.Space(12);
            foreach (var tool in tools) // TODO: 分组
            {
                GUILayout.BeginVertical(styles.ToolBox);

                tool.DrawGUI();
                if (GUILayout.Button(new GUIContent(tool.ToolName), GUILayout.Height(24)))
                {
                    ExecuteTool(tool);
                }

                GUILayout.EndVertical();
            }

            DrawStatusBar(); // 状态栏
            GUILayout.EndArea();
        }

        private void ExecuteTool(ToolAsset tool)
        {
            if (isBusy) return;
            StartCoroutine(ExecuteToolCoroutine(tool));
        }

        private IEnumerator ExecuteToolCoroutine(ToolAsset tool)
        {
            SetStatus($"Executing: {tool.ToolName}");
            isBusy = true;

            yield return StartCoroutine(tool.Execute());
            SetStatus($"Executed: {tool.ToolName}");
            isBusy = false;
        }

        // =====================================================================================================================
        #region 状态栏

        [Header("状态栏")]
        [SerializeField] private float statusDuration = 2f;
        
        private string _status;
        private float _statusEndTime;

        private void SetStatus(string value)
        {
            _status = value;
            _statusEndTime = Time.realtimeSinceStartup + statusDuration;
        }
    
        private void DrawStatusBar()
        {
            if (string .IsNullOrEmpty(_status)) return;

            if (Time.realtimeSinceStartup >= _statusEndTime)
            {
                _status = null;
                return;
            }

            GUILayout.Space(6);
            GUILayout.Box(_status, styles.StatusBar);
        }

        #endregion
    }
}