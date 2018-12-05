using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Windsmoon
{
    public static class UndoRedoManager
    {
        #region fields
        private static CommandPool commandPool;
        #endregion

        #region properties
        public static CommandPool CommandPool
        {
            get
            {
                return commandPool;
            }
        }
        #endregion

        #region methods
        [MenuItem("UndoRedo/Gen Scene")]
        private static void Gen()
        {
            commandPool = new CommandPool(100);
            GameObject scene = new GameObject("Scene");
            scene.AddComponent<SceneMonobehaviour>();

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 10; ++j)
                {
                    GameObject gameObject = new GameObject(i + "x" + j);
                    gameObject.transform.parent = scene.transform;
                    gameObject.transform.position = new Vector3(i, 0, -j);
                    BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                    boxCollider.size = new Vector3(1, 0.01f, 1);
                }
            }
        }

        [MenuItem("UndoRedo/Undo")]
        private static void Undo()
        {
            string info = commandPool.GetNextUndoCommandInfo();
            Debug.Log(info);
            commandPool.Undo();
            Debug.Log(commandPool.ToString());
        }

        [MenuItem("UndoRedo/Redo")]
        private static void Redo()
        {
            string info = commandPool.GetNextRedoCommandInfo();
            Debug.Log(info);
            commandPool.Redo();
            Debug.Log(commandPool.ToString());
        }
        #endregion
    }
}
