using ISensor;
using ISensor.AISSensorScanModule;
using ISensor.ISensorTargeting;
using ISensor.SensorColourData;
using ISensor.SensorData;
using ISensor.SensorScanData;
using ISensor.ISensorMemory;
using UnityEngine;

public class ZombieVisionSensorModule : MonoBehaviour, ISensorModule
{
    public ISensorScanModule ScanModule { get;  set; }

    public GameObject SensorObject { get; set; }
    public SensorDataStruct SensorData { get; set; }
    
    public SensorScanDataStruct SensorScanData { get; set; }
    
    public SensorColourDataStruct SensorColourData { get; set; }

    public ISensorTargetingModule SensorTargetingModule { get; set; }
    
    private ISensorDetectionModule _sensorDetectionModule;
    
    private float _scanTimer;
    
    private AIMemory _bestMemory;
    
    public void Awake()
    {
        ScanModule = GetComponent<ZombieVisionSensorScanModule>();
        SensorTargetingModule = GetComponent<ZombieVisionSensorTargetingModule>();
        _sensorDetectionModule = gameObject.GetComponent<ISensorDetectionModule>();
        
        SensorTargetingModule.InitializeTargetingModule(this);

        SensorObject = this.gameObject;
    }
    
    public void Enable()
    {
        //Debug.Log("Vision Sensor Setting Module Enable: " + gameObject.name);

        SensorTargetingModule.Enable();
        _sensorDetectionModule.Enable();
        
        enabled = true;
    }

    public void Disable()
    {
        //Debug.Log("Vision Sensor Setting Module Disable: " + gameObject.name);
        
        SensorTargetingModule.Disable();
        _sensorDetectionModule.Disable();
        
        enabled = false;
    }

    public void Start()
    {
        _scanTimer = SensorScanData.ScanInterval;
    }

    public void Update()
    {
        _scanTimer -= Time.deltaTime;

        if (_scanTimer <= 0)
        {
            ScanModule.Scan();
            
            SensorTargetingModule.UpdateMemory();
            
            _scanTimer = SensorScanData.ScanInterval;
            
            if (SensorTargetingModule.BestMemory != null)
            {
                if (IsInRange(SensorTargetingModule.BestMemory.gameObject))
                {
                    _sensorDetectionModule.CombatAlert?.Invoke(SensorTargetingModule.BestMemory.gameObject);
                }
            }
            
        }
        
     
    }

    private bool IsInRange(GameObject obj)
    {
        var origin = transform.position;
        var dest = obj.transform.position;

        if(Vector3.Distance(origin, dest) <= 20f)
        {
            return true;
        }
        
        if(Vector3.Distance(origin, dest) >= 21f && Vector3.Distance(origin, dest) <= 50f )
        {
            _sensorDetectionModule.VisionAlert?.Invoke(SensorTargetingModule.BestMemory.position);
            return false;
        }

        return false;
    }

    public void InitializeSensorModule(SensorDataStruct sensorData, SensorScanDataStruct sensorScanData, SensorColourDataStruct sensorColourData)
    {
        SensorData = sensorData;
        SensorScanData = sensorScanData;
        SensorColourData = sensorColourData;
    }

    public int Filter(GameObject[] buffer, string layerName)
    {
        int layer = LayerMask.NameToLayer(layerName);
        int count = 0;

        foreach (var obj in ScanModule.InRangeObjects)
        {
            if (obj.layer == layer && obj.CompareTag("Player"))
            {
                buffer[count++] = obj;
            }

            if (buffer.Length == count)
            {
                break;
            }
        }
            
        return count;
    }

    public void OnDrawGizmos()
    {
        if (SensorTargetingModule != null)
        {
            var maxScore = float.MinValue;

            foreach (var memory in SensorTargetingModule.Memory.memories)
            {
                maxScore = Mathf.Max(maxScore, memory.score);
            }

            foreach (var memory in SensorTargetingModule.Memory.memories)
            {
                var color = SensorColourData.inMemoryColor;

                if (memory ==  SensorTargetingModule.BestMemory)
                {
                    color = SensorColourData.bestMemoryColor;
                }

                color.a = memory.score / maxScore;
                Gizmos.color = color;
                Gizmos.DrawSphere(memory.position, 1f);
            }
        }
    }

}
