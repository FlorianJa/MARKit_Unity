using Vuforia;
using HoloToolkit.Unity;
using UnityEngine;

namespace MARKit
{
    public class VuMarkHandler : Singleton<VuMarkHandler>
    {

        #region PRIVATE_MEMBER_VARIABLES

        private VuMarkManager _vuMarkManager;
        private GameObjectManager _gameObjectManager;

        #endregion // PRIVATE_MEMBER_VARIABLES

        #region PUBLIC_MEMBER_VARIABLES
        [Tooltip("Forward rotation updates?")]
        public bool UpdateObjectRotation;
        [Tooltip("Forward position updates?")]
        public bool UpdateObjectPosition;
        [Tooltip("Forward marker information if marker was already scanned?")]
        public bool MarkerCanBeScannedMultipleTimes;

        #endregion // PUBLIC_MEMBER_VARIABLES

        #region UNTIY_MONOBEHAVIOUR_METHODS

        void Start()
        {
            // register callbacks to VuMark Manager
            _vuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
            _vuMarkManager.RegisterVuMarkDetectedCallback(OnVuMarkDetected);
            _vuMarkManager.RegisterVuMarkLostCallback(OnVuMarkLost);

            _gameObjectManager = GameObjectManager.Instance;
        }

        void Update()
        {
            ulong id;
            foreach (var behaviour in _vuMarkManager.GetActiveBehaviours())
            {
                id = behaviour.VuMarkTarget.InstanceId.NumericValue;
                if (!_gameObjectManager.ContainsID(id) || MarkerCanBeScannedMultipleTimes)
                {
                    _gameObjectManager.SpawnObject(behaviour, !MarkerCanBeScannedMultipleTimes);

                }
                else
                {
                    if (UpdateObjectRotation)
                    {
                        _gameObjectManager[id].transform.rotation = behaviour.transform.rotation;
                    }

                    if (UpdateObjectPosition)
                    {
                        _gameObjectManager[id].transform.position = behaviour.transform.position;
                    }
                }

            }
        }

        new void OnDestroy()
        {
            base.OnDestroy();
            // unregister callbacks from VuMark Manager
            _vuMarkManager.UnregisterVuMarkDetectedCallback(OnVuMarkDetected);
            _vuMarkManager.UnregisterVuMarkLostCallback(OnVuMarkLost);
        }

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS


        #region PUBLIC_METHODS

        /// <summary>
        /// This method will be called whenever a new VuMark is detected
        /// </summary>
        public void OnVuMarkDetected(VuMarkTarget target)
        {
            Debug.Log("New VuMark: " + GetVuMarkDataAsString(target));
        }

        /// <summary>
        /// This method will be called whenever a tracked VuMark is lost
        /// </summary>
        public void OnVuMarkLost(VuMarkTarget target)
        {
            Debug.Log("Lost VuMark: " + GetVuMarkDataAsString(target));
        }

        #endregion // PUBLIC_METHODS


        #region PRIVATE_METHODS

        /// <summary>
        /// Returns data typ of vumark
        /// </summary>
        /// <param name="vumark"></param>
        /// <returns></returns>
        private string GetVuMarkDataType(VuMarkTarget vumark)
        {
            switch (vumark.InstanceId.DataType)
            {
                case InstanceIdType.BYTES:
                    return "Bytes";
                case InstanceIdType.STRING:
                    return "String";
                case InstanceIdType.NUMERIC:
                    return "Numeric";
            }
            return "";
        }

        /// <summary>
        /// Returns the data of a vumark as string.
        /// </summary>
        /// <param name="vumark"></param>
        /// <returns></returns>
        private string GetVuMarkDataAsString(VuMarkTarget vumark)
        {
            switch (vumark.InstanceId.DataType)
            {
                case InstanceIdType.BYTES:
                    return vumark.InstanceId.HexStringValue;
                case InstanceIdType.STRING:
                    return vumark.InstanceId.StringValue;
                case InstanceIdType.NUMERIC:
                    return vumark.InstanceId.NumericValue.ToString();
            }
            return "";
        }

        #endregion // PRIVATE_METHODS
    }
}