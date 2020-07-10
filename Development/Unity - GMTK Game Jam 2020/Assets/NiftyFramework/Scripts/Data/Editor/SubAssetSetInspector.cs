using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace NiftyFramework.Data
{
    //[CustomEditor(typeof(SubAssetSet<TAsset>))]
    public class SubAssetSetInspector<TAsset> : Editor where TAsset : UnityEngine.Object
    {
        private SubAssetSet<TAsset> _assetSet;
        private ReorderableList _reorderableList;
        private GenericMenu _menuAdd;
        private GenericMenu _menuRemove;

        protected void OnEnable()
        {
            _assetSet = target as SubAssetSet<TAsset>;
            _reorderableList = new ReorderableList(_assetSet.References, typeof(TAsset));
            _reorderableList.onAddCallback = HandleAdd;
            _reorderableList.onRemoveCallback = HandleRemove;
            
            _menuAdd = new GenericMenu();
            _menuAdd.AddItem(new GUIContent("Create New"), false, HandleCreateNew);
            _menuAdd.AddItem(new GUIContent("Add Existing"), false, HandleAddExisting);
        }

        private void HandleCreateNew()
        {
            //Object.Instantiate()
            //AssetDatabase.AddObjectToAsset(newAsset, this);
        }

        private void HandleAddExisting()
        {
            
        }
        
        private void HandleRemove(ReorderableList list)
        {
            _menuRemove.ShowAsContext();
        }

        protected void HandleAdd(ReorderableList list)
        {
            if (_menuAdd != null)
            {
                _menuAdd.ShowAsContext();
            }
            else
            {
                
            }
        }
    }
}