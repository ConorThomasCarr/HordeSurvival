using System;
using UnityEngine;

public class SensorMaster : MonoBehaviour
{
    private ISensorSettingModule _sensorSettingModule;

    private void Awake()
    {
        _sensorSettingModule = GetComponent<ISensorSettingModule>();
    }

    private void OnEnable()
    {
        _sensorSettingModule.Enable();
    }

    private void OnDisable()
    {
        _sensorSettingModule.Disable();
    }
}
