using HoloToolkit.Unity;
using UnityEngine;

namespace MARKit
{
    public abstract class DataProvider: Singleton<DataProvider>
    {
        public abstract GameObject GetGameObjectById(ulong id);
    }
}