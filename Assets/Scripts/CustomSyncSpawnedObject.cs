using HoloToolkit.Sharing.Spawning;
using HoloToolkit.Sharing.SyncModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SyncDataClass]
public class CustomSyncSpawnedObject : SyncSpawnedObject
{
    [SyncData] public SyncLong ID;
}
