using System;
using Unity.Services.Core;
using UnityEngine;

namespace Managers
{
    public class RelayManager: MonoBehaviour
    {
        private void Awake()
        {
            await UnityServices.InitializeAsync();
        }
    }
}