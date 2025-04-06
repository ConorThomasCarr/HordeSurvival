using ISensor;
using ISensor.AISSensorScanModule;
using ISensor.ISensorMesh;
using ISensor.SensorData;
using ISensor.SensorColourData;
using ISensor.SensorScanData;
using UnityEngine.Events;

public interface ISensorSettingModule
{
    public ISensorModule SensorModule { get; set; }

    public ISensorScanModule SensorScanModule { get; set; }

    public ISensorMeshModule SensorMeshModule { get; set; }

    public SensorDataStruct SensorData { get; set; }

    public SensorScanDataStruct SensorScanData { get; set; }

    public SensorColourDataStruct SensorColourData { get; set; }

    public void Awake();

    public void Enable();

    public void Disable();

    public void OnDrawGizmos();
}
