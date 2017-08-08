using HoloToolkit.Unity;
using UnityEngine;

namespace MARKit
{
    public class LocalDataProvider : DataProvider
    {

        #region PUBLIC_MEMBER_VARIABLES

        [Tooltip("Prefab to spawn")]
        public GameObject PrefabToSpawn;
        [Tooltip("Color of the spawned prefab")]
        public Material[] Colors;

        #endregion

        private void Start()
        {
            if (PrefabToSpawn == null)
            {
                Debug.LogError("No Prefab selected.");
            }
        }

        public override GameObject GetGameObjectById(ulong id)
        {
            if (PrefabToSpawn == null)
            {
                Debug.LogError("Trying to spawn a gameobject which setting a prefab tio spawn.");
                return null;
            }

            //Instantiate new gameobject
            GameObject go;
            go = Instantiate(PrefabToSpawn, Vector3.zero, Quaternion.identity) as GameObject;

            //get cubecontroller
            var cubeController = go.GetComponent<CubeController>();
            //save id in cubecontroller
            cubeController.ID = id;


            //get renderer of the cube
            var renderer = go.GetComponent<Renderer>();

            if (renderer != null)
            {
                //Color the gameobject
                renderer.material = Colors[(int)id % Colors.Length];
                cubeController.Color = Colors[(int)id % Colors.Length].name;
            }
            return go;

        }
    }
}




