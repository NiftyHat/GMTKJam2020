using System;
using System.IO;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace NiftyFramework.Attributes
{
    [CustomPropertyDrawer(typeof(AssetFolderReference))]
    public class AssetFolderReferenceDrawer : PropertyDrawer
    {       
        
        private const string FOLDER_ICON = "Folder Icon";
        private const string FOLDER_EMPTY_ICON = "FolderEmpty Icon";
        private const string ERROR_ICON = "console.erroricon.sml";
        private const string WARNING_ICON = "console.warnicon.sml";
        
        
        public AssetFolderReferenceDrawer()
        {
            
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            SerializedProperty subfolderProperty = property.FindPropertyRelative("_subfolderPath");
            string subfolderPath = subfolderProperty.stringValue;

            var buttonRect = new Rect(position.x, position.y, 60, position.height);
            var iconRect = new Rect(buttonRect.xMax + 1, position.y , position.height,position.height);
            var labelRect = new Rect(iconRect.xMax + 1, position.y, position.x + position.width - buttonRect.xMax, position.height);

            string buttonLabel = "Browse";
            string displayText = "";
            GUIContent iconGUIContent = EditorGUIUtility.IconContent(FOLDER_ICON);
            Color textColor = Color.white;

            string fullPath = Application.dataPath;
            
            if (string.IsNullOrEmpty(subfolderPath))
            {
                buttonLabel = "Select";
                displayText = "No folder path selected!";
                textColor = Color.red;
                iconGUIContent = EditorGUIUtility.IconContent(ERROR_ICON);
            }
            else
            {
                fullPath = Path.Combine(Application.dataPath, subfolderPath);
                if (!Directory.Exists(subfolderPath))
                {
                    buttonLabel = "Create";
                    displayText = "Folder doesn't exist " + fullPath;
                    textColor = Color.red;
                    iconGUIContent = EditorGUIUtility.IconContent(ERROR_ICON);
                }
                else
                {
                    buttonLabel = "Browse";
                    displayText = subfolderPath;
                    iconGUIContent = EditorGUIUtility.IconContent(FOLDER_ICON);
                }
            }
            
            if (GUI.Button(buttonRect, buttonLabel))
            {
                string selectedPath = EditorUtility.OpenFolderPanel("Select a folder", fullPath, "New Folder");
                if (IsSubfolderOfAssets(selectedPath))
                {
                    Uri uri = GetRelativePathFromAssets(selectedPath);
                    subfolderProperty.stringValue = uri.ToString();
                }
            }
            EditorGUI.LabelField(labelRect, displayText);
            EditorGUI.LabelField(iconRect, iconGUIContent);
            
            EditorGUI.EndProperty();
        }

        protected bool IsSubfolderOfAssets(string fullPath)
        {
            Uri uriFullPath = new Uri(fullPath, UriKind.Absolute);
            Uri uriAssetPath = new Uri(Application.dataPath, UriKind.Absolute);
            return uriAssetPath.IsBaseOf(uriFullPath);
        }

        protected Uri GetRelativePathFromAssets(string fullPath)
        {
            Uri uriFullPath = new Uri(fullPath, UriKind.Absolute);
            Uri uriAssetPath = new Uri(Application.dataPath, UriKind.Absolute);
            Debug.Log(uriFullPath);
            Debug.Log(uriAssetPath);
            return uriAssetPath.MakeRelativeUri(uriFullPath);
        }
        
    }
}