using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NiftyFramework.Tools
{
    public class AssetSearchWindow : EditorWindow
    {
        protected string _searchType;
        protected string _searchToken;
        protected int _maxResults = 50;
        protected Type _type;

        private List<string> _folderList;

        public event Action<Object[]> onSelectResults;
        public event Action<Object> onSelectResult;

        protected SerializedProperty _targetProperty;
        
        internal SearchResult[] _searchResultsCache;

        private SearchField _searchField;
        
        public static AssetSearchWindow Init(string[] folderList, Type type, SerializedProperty serializedProperty)
        {
            AssetSearchWindow window = (AssetSearchWindow)EditorWindow.GetWindow(typeof(AssetSearchWindow));
            window.Show(folderList, type, serializedProperty);
            return window;
        }
        
        public static AssetSearchWindow Init(string folder, Type type, SerializedProperty serializedProperty)
        {
            string[] folderList = new[] {folder};
            return Init(folderList, type, serializedProperty);
        }
        
        public static AssetSearchWindow Init(Type type, SerializedProperty serializedProperty)
        {
            string[] folderList = new[] {Application.dataPath};
            return Init(folderList, type, serializedProperty);
        }
        
        [MenuItem("Window/My Window")]
        public static AssetSearchWindow Init()
        {
            AssetSearchWindow window = (AssetSearchWindow)EditorWindow.GetWindow(typeof(AssetSearchWindow));
            Type type = typeof(ScriptableObject);
            window.Show(null, type, null);
            return window;
        }
        
        public void Show(string[] folderList, Type type, SerializedProperty serializedProperty)
        {
            _folderList = folderList?.ToList();
            _type = type;
            _searchType = _type.ToString();
            _targetProperty = serializedProperty;
            _searchField = new SearchField();
           
            if (_targetProperty != null && _targetProperty.objectReferenceValue != null)
            {
                _searchToken = _targetProperty.objectReferenceValue.name;
                DoSearch();
            }
        }
        
        protected void OnGUI()
        {
            GUILayout.Label("Asset Search", EditorStyles.boldLabel);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                RenderFolderSelect();

                _searchToken = _searchField.OnGUI(_searchToken);

                if (check.changed)
                {
                    DoSearch();
                }
                
                if (Event.current.type == EventType.KeyDown && Event.current.character == '\n')
                {
                    DoConfirm();
                }
                
                if (Event.current.type == EventType.MouseUp)
                {
                    DoConfirm();
                }
            }

            RenderSearchResults();
            RenderChangeTargetProperty();
        }

        private void RenderFolderSelect()
        {
            GUILayout.Label("Folders", EditorStyles.boldLabel);
            var icon = EditorGUIUtility.IconContent("Folder Icon");
            Color defaultColor = GUI.color;
            if (_folderList != null)
            {
                for (int i = 0; i < _folderList.Count; i++)
                {
                    string folderPath = _folderList[i];
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    GUILayout.Label(icon, GUILayout.Width(20f), GUILayout.Height(20f));
                    GUILayout.Label(folderPath);
                    GUI.color = Color.red;
                    if (GUILayout.Button("X"))
                    {
                        _folderList.Remove(folderPath);
                        i--;
                    }
                    GUI.color = defaultColor;
                    GUILayout.EndHorizontal();
                }
                GUI.color = defaultColor;
            }
            GUIContent addContent = new GUIContent("Add Folder", icon.image);
            if (GUILayout.Button(addContent, GUILayout.Width(position.width) , GUILayout.Height(20f)))
            {
               string selectedFolder = EditorUtility.OpenFolderPanel("Add Path", Application.dataPath, "");
               if (!string.IsNullOrEmpty(selectedFolder))
               {
                   Uri selectedFolderUri = new Uri(selectedFolder, UriKind.Absolute);
                   Uri assetRootUri = new Uri(Application.dataPath, UriKind.Absolute);
                   Uri relativePath = assetRootUri.MakeRelativeUri(selectedFolderUri);
                   if (_folderList == null)
                   {
                       _folderList = new List<string>();
                   }
                   _folderList.Add(relativePath.ToString());
               }
            }
        }

        private void RenderChangeTargetProperty()
        {
            if (_targetProperty == null)
            {
                return;
            }
            EditorGUILayout.BeginHorizontal();
            if (!_targetProperty.isArray)
            {
                EditorGUIUtility.labelWidth = 1;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.ObjectField(_targetProperty);
                EditorGUI.EndDisabledGroup();
            
                EditorGUIUtility.labelWidth = 0;
            }
            else
            {
                
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void RenderSearchResults()
        {
            if (_searchResultsCache != null)
            {
                EditorGUILayout.LabelField("Search Results " + _searchResultsCache.Length, EditorStyles.boldLabel);
                if (_searchResultsCache.Length > 0)
                {
                    for (int i = 0; i < _searchResultsCache.Length && i < _maxResults; i++)
                    {
                        SearchResult searchResult = _searchResultsCache[i];
                        searchResult.RenderEditorUI();
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField("No Search Results", EditorStyles.boldLabel);
            }
        }

        private bool HasSelectedItems()
        {
            if (_searchResultsCache == null || _searchResultsCache.Length == 0)
            {
                return false;
            }
            return true;
        }
        
        private void DoConfirm()
        {
            if (_targetProperty == null)
            {
                return;
            }
            if (!HasSelectedItems())
            {
                ///TODO implement file naming and creation here.
            }
            else
            {
                if (_targetProperty.isArray)
                {
                    
                }
                else
                {
                    if (_searchResultsCache.Length > 0)
                    {
                        if (!_targetProperty.isArray)
                        {
                            string controlName = GUI.GetNameOfFocusedControl();
                            foreach (SearchResult item in _searchResultsCache)
                            {
                                if (item.HasControlName(controlName))
                                {
                                    _targetProperty.objectReferenceValue = item.Load();
                                    _targetProperty.serializedObject.ApplyModifiedProperties();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            
                        }
                    }
                }
            }
        }
        
        void DoSearch()
        {
            StringBuilder search = new StringBuilder();
            if (_searchType != null)
            {
                search.Append("t:" + _searchType + " ");
            }
            if (_searchToken != null)
            {
                search.Append(" " + _searchToken);
            }
            string[] guids;
            string searchString = search.ToString();
            if (_folderList != null)
            {
                guids = AssetDatabase.FindAssets(searchString, _folderList.ToArray());
            }
            else
            {
                guids = AssetDatabase.FindAssets(searchString);
            }
            _searchResultsCache = new SearchResult[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                _searchResultsCache[i] = new SearchResult(guids[i], _type);
            }
        }
    }

    internal class SearchResult
    {
        private readonly string _assetPath;
        private readonly string _displayName;
        private readonly Type _type;
        private readonly string _controlName;
        
        private SerializedObject _serializedObject;
        private bool _isObjectDisplayed;
        private Editor _inlineEditor;
        
        internal SearchResult(string guid, Type type)
        {
            _assetPath = AssetDatabase.GUIDToAssetPath(guid);
            _displayName = Path.GetFileName(_assetPath);
            _type = type;
            _serializedObject = null;
            _isObjectDisplayed = false;
            _controlName = _displayName;
        }
        
        public bool HasControlName(string name)
        {
            return _controlName == name;
        }
        
        internal Object Load()
        {
            if (_serializedObject == null)
            {
                Object loadedObject = AssetDatabase.LoadAssetAtPath(_assetPath,_type);
                if (loadedObject != null)
                {
                    _serializedObject = new SerializedObject(loadedObject);
                }
                return loadedObject;
            }
            else
            {
                return _serializedObject.targetObject;
            }
        }
        
        public void RenderEditorUI()
        {
            GUI.SetNextControlName(_controlName);
            _isObjectDisplayed = EditorGUILayout.Foldout(_isObjectDisplayed, _displayName);
            if (_isObjectDisplayed)
            {
                if (_serializedObject == null)
                {
                    Load();
                }
                if (_serializedObject != null && _inlineEditor == null)
                {
                    _inlineEditor = Editor.CreateEditor(_serializedObject.targetObjects);
                }
                if (_inlineEditor != null)
                {
                    _inlineEditor.OnInspectorGUI();
                }
            }
        }
    }
}