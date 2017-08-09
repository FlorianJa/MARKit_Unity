using HoloToolkit.Sharing.Tests;
using HoloToolkit.Unity;
using HUX.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MARKit
{
    public class StateManager : Singleton<StateManager>
    {

        // Use this for initialization
        void Start()
        {

            LoadingDialog.Instance.Open(LoadingDialog.IndicatorStyleEnum.AnimatedOrbs, LoadingDialog.ProgressStyleEnum.None, LoadingDialog.MessageStyleEnum.Visible, "Waiting for server");

            ImportExportAnchorManager.Instance.AnchorUploaded += Instance_AnchorUploaded;
#if UNITY_WSA && !UNITY_EDITOR
            ImportExportAnchorManager.Instance.AnchorLoaded += Instance_AnchorLoaded;
#endif
        }

        private void Instance_AnchorLoaded()
        {
            LoadingDialog.Instance.Close();
        }

        private void Instance_AnchorUploaded(bool obj)
        {
            LoadingDialog.Instance.Close();
        }

    }

}