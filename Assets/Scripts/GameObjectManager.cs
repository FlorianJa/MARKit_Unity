using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using HoloToolkit.Unity;

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

        [Tooltip("Oject which implements the IDataProvider Interface.")]
        public DataProvider DataProvider;
        #endregion

        #region PUBLIC_METHODS
        void Start()
        {
            _spawnedObjects = new Dictionary<ulong, GameObject>();
        }

        /// <summary>
        ///  Spawns an object on the position of the VuMark
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
        /// <param name="addToDictionary"></param>
        public void SpawnObject(ulong ID, Vector3 position, Quaternion rotation, bool addToDictionary = true)
        {
            //Inistaniate new gameobject
            GameObject go = DataProvider.GetGameObjectById(ID);

            go.transform.position = position;
            go.transform.rotation = rotation;

            if (addToDictionary)
            {
                _spawnedObjects.Add(ID, go);
            }
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


 
