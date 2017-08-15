using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using HoloToolkit.Unity;
using System;

namespace MARKit
{
    public class GameObjectManager : Singleton<GameObjectManager>
    {

        #region PRIVATE_MEMBER_VARIABLES
        /// <summary>
        /// Stores all spawned objects
        /// </summary>
        private Dictionary<ulong, GameObject> _spawnedObjects;
        
        #endregion

        #region PUBLIC_MEMBER_VARIABLES

        [Tooltip("Forward rotation updates?")]
        public bool UpdateObjectRotation;
        [Tooltip("Forward position updates?")]
        public bool UpdateObjectPosition;
        [Tooltip("Forward marker information if marker was already scanned?")]
        public bool MarkerCanBeScannedMultipleTimes;

        public CustomPrefabSpawnManager spawnManager;
        public GameObject SpawnParent;

        #endregion

        #region PUBLIC_METHODS
        void Start()
        {
            _spawnedObjects = new Dictionary<ulong, GameObject>();
        }


        /// <summary>
        /// Spawns a new object from VuMarkAbstractBehaviour
        /// </summary>
        /// <param name="vumark"></param>
        public void UpdateFromVumark(VuMarkAbstractBehaviour behaviour)
        {
            UpdateFromVumark(behaviour.VuMarkTarget.InstanceId.NumericValue, behaviour.transform.position, behaviour.transform.rotation);
        }

        public void UpdateFromVumark(ulong id, Vector3 position, Quaternion rotation)
        {
            if (MarkerCanBeScannedMultipleTimes || !spawnManager.ContainsMarkerID((long)id))
            {
                SpawnObject(id, position, rotation);
                return;
            }

            if (UpdateObjectRotation)
            {
                position = spawnManager.transform.InverseTransformPoint(position);//SpawnParent.transform.InverseTransformPoint(position); ;
                spawnManager.UpdatePosition((long)id, position);
            }
            if (UpdateObjectPosition)
            {
                spawnManager.UpdateRotation((long)id, rotation);
            }
        }

        /// <summary>
        /// Spawns a new object on a given position and rotation
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void SpawnObject(ulong ID, Vector3 position, Quaternion rotation)
        {
            var dataModel = new CustomSyncSpawnedObject();
            dataModel.ID.Value = (long)ID;

            position = spawnManager.transform.InverseTransformPoint(position);//SpawnParent.transform.InverseTransformPoint(position);
            spawnManager.Spawn(dataModel, position, rotation, SpawnParent, "SpawnObject", true);
        }

        public void DeleteObjectByGameObject(GameObject gameObject)
        {
            spawnManager.Delete(gameObject);
        }

        /// <summary>
        /// checks if a given id is already spawned.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsID(ulong id)
        {
            return _spawnedObjects.ContainsKey(id);
        }


        /// <summary>
        /// Index operator.
        /// </summary>
        /// <param name="key">ID of VuMark</param>
        /// <returns>GameObject to ID</returns>
        public GameObject this[ulong key]
        {
            get
            {
                return _spawnedObjects[key];
            }
            private set
            {
                _spawnedObjects[key] = value;
            }
        }
        #endregion
    }

}


 
