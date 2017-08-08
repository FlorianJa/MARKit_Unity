using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using HoloToolkit.Unity;
using System;
using UnityEditor;

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

        [Tooltip("The Prefab to spawn")]
        public GameObject PrefabToSpawn;

        [SerializeField][Tooltip("Oject which implements the IDataProvider Interface.")]
        public LocalDataProvider DataProvider;
        #endregion

        #region PUBLIC_METHODS
        void Start()
        {
            if (PrefabToSpawn == null)
            {
                Debug.LogError("No Prefab selected.");
            }
            _spawnedObjects = new Dictionary<ulong, GameObject>();
        }

        /// <summary>
        ///  Spawns an object on the position of the vumar
        /// </summary>
        /// <param name="vumark"></param>
        /// <param name="addToDictenary"></param>
        public void SpawnObject(VuMarkAbstractBehaviour vumark, bool addToDictenary = true)
        {
            SpawnObject(vumark.VuMarkTarget.InstanceId.NumericValue, vumark.transform.position, vumark.transform.rotation, addToDictenary);
        }

        /// <summary>
        /// Spawns an object on a given position with a given rotation
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="addToDictenary"></param>
        public void SpawnObject(ulong ID, Vector3 position, Quaternion rotation, bool addToDictenary = true)
        {
            if (PrefabToSpawn == null)
            {
                return;
            }

            //Inistaniate new gameobject
            GameObject go = DataProvider.GetGameObjectById(ID);

            go.transform.position = position;
            go.transform.rotation = rotation;

            if (addToDictenary)
            {
                _spawnedObjects.Add(ID, go);
            }
        }


        public bool ContainsID(ulong id)
        {
            return _spawnedObjects.ContainsKey(id);
        }

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


 
