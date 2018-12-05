using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Windsmoon
{
    [CustomEditor(typeof(SceneMonobehaviour))]
    public class Raycaster : UnityEditor.Editor
    {
        #region fields
        private static Dictionary<GameObject, GameObject> gameObjectDict = new Dictionary<GameObject, GameObject>();
        #endregion

        #region unity methods
        private void OnSceneGUI()
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Event currentEvent = Event.current;

            if (currentEvent.type != EventType.MouseDown)
            {
                return;
            }

            if (currentEvent.button == 0 && currentEvent.control) // 0 means left mouse button pressed, control measn control button pressed
            {
                DestroyCube(currentEvent);
            }

            else if (currentEvent.button == 0) // 0 means left mouse button pressed
            {
                CreateCube(currentEvent);
            }
        }

        private void CreateCube(Event currentEvent)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (gameObjectDict.ContainsKey(raycastHit.collider.gameObject))
                {
                    return;
                }

                CreateCubeCommand createCubeCommand = new CreateCubeCommand(CreateCubeImpl, DestroyCubeImpl, raycastHit);
                UndoRedoManager.CommandPool.Register(createCubeCommand);
                createCubeCommand.Excute();
            }
        }

        private void DestroyCube(Event currentEvent)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (gameObjectDict.ContainsKey(raycastHit.collider.gameObject))
                {
                    DestoryCubeCommand destoryCubeCommand = new DestoryCubeCommand(DestroyCubeImpl, CreateCubeImpl, raycastHit);
                    UndoRedoManager.CommandPool.Register(destoryCubeCommand);
                    destoryCubeCommand.Excute();
                }
            }
        }

        private void CreateCubeImpl(RaycastHit raycastHit)
        {
            GameObject hitGameObject = raycastHit.collider.gameObject;
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject.DestroyImmediate(cube.GetComponent<Collider>());
            cube.transform.SetParent(hitGameObject.transform, false);
            gameObjectDict[hitGameObject] = cube;
        }

        private void DestroyCubeImpl(RaycastHit raycastHit)
        {
            GameObject hitGameObject = raycastHit.collider.gameObject;
            GameObject.DestroyImmediate(gameObjectDict[hitGameObject]);
            gameObjectDict.Remove(hitGameObject);
        }
        #endregion
    }
}

