using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace NKStudio
{
    public class STFAbout : EditorWindow
    {
        private VisualTreeAsset _STFAboutUXML;
        private StyleSheet _STFAboutUSS;
        private TextAsset _packageJson;

        private void Awake()
        {
            _STFAboutUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Plugins/SnapToFloor/Scripts/Editor/About/STFAbout.uxml");
            _STFAboutUSS = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Plugins/SnapToFloor/Scripts/Editor/About/STFAbout.uss");
            _packageJson = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Plugins/SnapToFloor/package.json");
        }

        [MenuItem("Window/SnapToFloor/About")]
        public static void Init()
        {
            STFAbout wnd = GetWindow<STFAbout>();
            wnd.titleContent = new GUIContent("About");
            wnd.minSize = new Vector2(350, 120);
            wnd.maxSize = new Vector2(350, 120);
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;
        
            // Import UXML
            VisualTreeAsset visualTree = _STFAboutUXML;
            root.styleSheets.Add(_STFAboutUSS);
            VisualElement container = visualTree.Instantiate();
            root.Add(container);
            
            string version = GetVersion();
            root.Q<Label>("version-label").text = $"Version : {version}";
        }
        
        private string GetVersion()
        {
            PackageInfo packageInfo = JsonUtility.FromJson<PackageInfo>(_packageJson.text);
            return packageInfo.version;
        }
    }
    
    [Serializable]
    public class PackageInfo
    {
        public string name;
        public string displayName;
        public string version;
        public string unity;
        public string description;
        public List<string> keywords;
    }
}