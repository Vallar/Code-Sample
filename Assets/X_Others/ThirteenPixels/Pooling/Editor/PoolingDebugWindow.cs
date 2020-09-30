// Copyright © Sascha Graeff/13Pixels.

namespace ThirteenPixels.Pooling.Editor
{
    using UnityEngine;
    using UnityEditor;
    using System.Diagnostics;
    using System.Collections.Generic;

    internal class PoolingDebugWindow : EditorWindow
    {
        [MenuItem("Window/13Pixels/Pooling Debugger")]
        private static void OpenWindow()
        {
            var window = GetWindow(typeof(PoolingDebugWindow), false, "Pooling");
            window.Show();
        }

        private struct PoolData
        {
            public string title;
            public int activeObjectCount;
            public int inactiveObjectCount;

            public PoolData(string title, int activeObjectCount, int inactiveObjectCount)
            {
                this.title = title;
                this.activeObjectCount = activeObjectCount;
                this.inactiveObjectCount = inactiveObjectCount;
            }
        }

        private static Texture2D logo;
        private const long updateInterval = 1000;
        private Stopwatch stopwatch = new Stopwatch();
        private List<PoolData> poolData = new List<PoolData>();

        private void OnEnable()
        {
            if (!logo)
            {
                LoadLogo();
            }

            UpdateMonitors();
            stopwatch.Start();
            EditorApplication.update += Update;
        }

        private static void LoadLogo()
        {
            logo = Resources.Load<Texture2D>("13Pixels-Pooling-Icon");
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
            stopwatch.Stop();
        }

        private void Update()
        {
            if (stopwatch.ElapsedMilliseconds >= updateInterval)
            {
                stopwatch.Reset();
                stopwatch.Start();
                UpdateMonitors();
            }
        }

        private void UpdateMonitors()
        {
            var pools = Pooling.GetPools();
            poolData.Clear();
            foreach (var pool in pools)
            {
                poolData.Add(new PoolData(pool.Key.name, pool.Value.activeObjectCount, pool.Value.inactiveObjectCount));
            }
            Repaint();
        }

        private void OnGUI()
        {
            DisplayHeader();

            if (Application.isPlaying)
            {
                DisplayRuntimeGUI();
            }
            else
            {
                GUILayout.Label("Start playmode to start debugging pools and pooled objects.");
            }
        }

        private static void DisplayHeader()
        {
            GUILayout.BeginHorizontal();
            if (logo)
            {
                GUILayout.Box(logo);
            }
            else
            {
                DrawTitle("13Pixels Pooling");
            }
            GUILayout.FlexibleSpace();
            DrawTitle("Debug Window");
            GUILayout.EndHorizontal();
        }

        private void DisplayRuntimeGUI()
        {
            DisplayVisibilityButton();

            DisplayPoolStatistics();
        }

        private static void DisplayVisibilityButton()
        {
            DrawTitle("Pooled object visibility");

            GUILayout.BeginHorizontal();
            var previousColor = GUI.color;

            GUILayout.Label("You can make pooled objects visible to debug their state.");

            string buttonText;
            if (Pooling.objectsAreVisibleInHierarchy)
            {
                buttonText = "Hide pooled instances";
                GUI.color *= new Color(1, 0.5f, 0.5f, 1);
            }
            else
            {
                buttonText = "Show pooled instances";
                GUI.color *= new Color(0.5f, 1, 0.5f, 1);
            }

            if (GUILayout.Button(buttonText, GUILayout.Width(150), GUILayout.Height(50)))
            {
                Pooling.SetEditorObjectVisibility(!Pooling.objectsAreVisibleInHierarchy);
                RepaintHierarchyWindow();
            }

            GUI.color = previousColor;
            GUILayout.EndHorizontal();
        }

        private static void RepaintHierarchyWindow()
        {
            try
            {
                EditorApplication.RepaintHierarchyWindow();
                EditorApplication.DirtyHierarchyWindowSorting();
            }
            catch { }
        }

        private void DisplayPoolStatistics()
        {
            DrawTitle("Current pooling activity");
            if (poolData.Count > 0)
            {
                DrawTableRow("", "Active", "Inactive");
                foreach (var pool in poolData)
                {
                    DrawTableRow(pool.title, pool.activeObjectCount + "", pool.inactiveObjectCount + "");
                }
            }
            else
            {
                GUILayout.Label("There are no active pools.");
            }
        }

        private static void DrawTitle(string s)
        {
            GUILayout.Label(s, EditorStyles.boldLabel);
        }

        private void DrawTableRow(string prefix, string column0, string column1)
        {
            const float prefixWidth = 250;
            float columnWidth = (this.position.width - prefixWidth) / 2;

            GUILayout.BeginHorizontal();
            GUILayout.Label(prefix, EditorStyles.boldLabel, GUILayout.Width(prefixWidth));
            GUILayout.Label(column0, GUILayout.Width(columnWidth));
            GUILayout.Label(column1, GUILayout.Width(columnWidth));
            GUILayout.EndHorizontal();
        }
    }
}
