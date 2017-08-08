using UnityEngine;

namespace MARKit
{
    public interface IDataProvider
    {
        GameObject GetGameObjectById(ulong id);
    }
}