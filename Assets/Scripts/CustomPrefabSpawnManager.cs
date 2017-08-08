using HoloToolkit.Sharing;
using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MARKit
{
    public class CustomPrefabSpawnManager : PrefabSpawnManager
    {

        public DataProvider DataProvider;

        /// <summary>
        /// Deletes a given gameObject
        /// </summary>
        /// <param name="objectToDelete">Object to delete</param>
        public bool Delete(GameObject objectToDelete)
        {
            bool retval = false;

            //looking up the given object
            SyncSpawnedObject tmp = null;
            foreach (var spawnedObject in SyncSource)
            {
                if (spawnedObject.GameObject == objectToDelete)
                {
                    tmp = spawnedObject;
                    break;
                }
            }

            if (tmp != null)
            {
                retval = SyncSource.RemoveObject(tmp);
            }

            return retval;
        }

        /// <summary>
        /// This method should get a prefab to a given dataModel. This class uses on dataModel for one prefab. All other prefab settings are set through the dataProvider.
        /// </summary>
        /// <param name="dataModel"></param>
        /// <param name="baseName"></param>
        /// <returns>The method return a empty gameobject to get the spawn method to work</returns>
        protected override GameObject GetPrefab(SyncSpawnedObject dataModel, string baseName)
        {
            //return empty GameObject to hack the Spawn Method of PrefabSpawnManager
            return new GameObject();
        }

        /// <summary>
        /// Instantiate a new GameObject from network.
        /// </summary>
        /// <param name="spawnedObject">DataModel of the gameObject to spwan</param>
        protected override void InstantiateFromNetwork(SyncSpawnedObject spawnedObject)
        {
            // Find the parent object
            GameObject parent = null;
            if (!string.IsNullOrEmpty(spawnedObject.ParentPath.Value))
            {
                parent = GameObject.Find(spawnedObject.ParentPath.Value);
                if (parent == null)
                {
                    Debug.LogErrorFormat("Parent object '{0}' could not be found to instantiate object.", spawnedObject.ParentPath);
                    return;
                }
            }

            CreatePrefabInstance(spawnedObject, parent);
        }

        /// <summary>
        /// Updates the position of the gameobject with the given id to the given position. Used for updating the position with a marker
        /// </summary>
        /// <param name="id"></param>
        /// <param name="position"></param>
        public void UpdatePosition(long id, Vector3 position)
        {
            var _spawnedObject = GetSpawnedPrefabById(id);

            if (_spawnedObject != null)
            {
                _spawnedObject.transform.position = position;
            }
        }

        /// <summary>
        /// Updates the rotation of the gameobject with the given id to the given rotatin. Used for updating the rotation with a marker
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rotation"></param>
        public void UpdateRotation(long id, Quaternion rotation)
        {
            var _spawnedObject = GetSpawnedPrefabById(id);

            if (_spawnedObject != null)
            {
                _spawnedObject.transform.rotation = rotation;
            }
        }

        /// <summary>
        /// Gets the spawned prefab of a given id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if there is no spawned object of the given id</returns>
        private GameObject GetSpawnedPrefabById(long id)
        {
            foreach (var spawnedObject in SyncSource)
            {
                var _object = spawnedObject as CustomSyncSpawnedObject;
                if (_object != null)
                {
                    if (_object.ID.Value == id)
                    {
                        return _object.GameObject;
                    }
                }
            }
            return null;

        }

        /// <summary>
        /// Checks if a specific marker was already scanned
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsMarkerID(long id)
        {
            foreach (var spawnedObject in SyncSource)
            {
                var _object = spawnedObject as CustomSyncSpawnedObject;
                if (_object != null)
                {
                    if (_object.ID.Value == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Create a prefab instance in the scene, in reaction to data being added to the data model.
        /// </summary>
        /// <param name="dataModel">Object to spawn's data model.</param>
        /// <param name="parentObject">Parent object under which the prefab should be.</param>
        /// <returns></returns>
        protected GameObject CreatePrefabInstance(SyncSpawnedObject dataModel, GameObject parentObject)
        {
            var id = (dataModel as CustomSyncSpawnedObject).ID.Value;
            GameObject instance = DataProvider.GetGameObjectById((ulong)id);
            if (instance == null)
            {
                return null;
            }
            dataModel.Transform.Scale.Value = instance.transform.localScale;
            instance.transform.SetParent(parentObject.transform, false);
            instance.gameObject.name = dataModel.Name.Value;

            dataModel.GameObject = instance;

            // Set the data model on the various ISyncModelAccessor components of the spawned game obejct
            ISyncModelAccessor[] syncModelAccessors = instance.GetComponentsInChildren<ISyncModelAccessor>(true);
            if (syncModelAccessors.Length <= 0)
            {
                // If no ISyncModelAccessor component exists, create a default one that gives access to the SyncObject instance
                ISyncModelAccessor defaultAccessor = instance.EnsureComponent<DefaultSyncModelAccessor>();
                defaultAccessor.SetSyncModel(dataModel);
            }

            for (int i = 0; i < syncModelAccessors.Length; i++)
            {
                syncModelAccessors[i].SetSyncModel(dataModel);
            }

            // Setup the transform synchronization
            TransformSynchronizer transformSynchronizer = instance.EnsureComponent<TransformSynchronizer>();
            transformSynchronizer.TransformDataModel = dataModel.Transform;

            return instance;
        }
    }
}