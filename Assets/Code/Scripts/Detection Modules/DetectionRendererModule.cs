using System;
using System.Collections;
using UnityEngine;


public class DetectionRendererModule : MonoBehaviour
{
    private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
    private IEnumerator _detectionCoroutine;
    
    private IEnumerator _showMeshCoroutine;
    private IEnumerator _hideMeshCoroutine;
    
    private Material _characterMaterial;
    
    private float _detectionAge;
    
    private bool _isShowingMesh;
    
    public void Awake()
    {
        EventManager.AddListener<ShowMesh>(OnShowMesh);
        EventManager.AddListener<HideMesh>(OnHideMesh);

        _detectionCoroutine = DetectionCoroutine();

        _showMeshCoroutine = ShowMeshCoroutine();
        _hideMeshCoroutine = HideMeshCoroutine();
        
        _characterMaterial = GetComponentInChildren<Renderer>().material;
    }
    

    private void OnShowMesh(ShowMesh showMesh)
    {
        if (showMesh.ReceiverName == gameObject.name && !_isShowingMesh)
        {
            _detectionAge = showMesh.DetectionAge;
            StartCoroutine(_detectionCoroutine);
            StartCoroutine(_showMeshCoroutine);

            _isShowingMesh = true;
        }
    }
    
    private void OnHideMesh(HideMesh hideMesh)
    {
        if (hideMesh.ReceiverName == gameObject.name)
        {
            StopCoroutine(_detectionCoroutine);
            StartCoroutine(_hideMeshCoroutine);
            _isShowingMesh = false;
        }
    }

    private IEnumerator DetectionCoroutine()
    {
        while (true)
        {
           yield return new WaitForSeconds(_detectionAge);

           var evtMeshIsDetected = DetectionEvent.MeshIsDetected;
           evtMeshIsDetected.ReceiverName = gameObject.name;
           evtMeshIsDetected.ReceiverObject = gameObject;
           
           EventManager.Broadcast(evtMeshIsDetected);
        }
        
        // ReSharper disable once IteratorNeverReturns
    }
    
    
    private IEnumerator ShowMeshCoroutine()
    {
        while (true)
        {
           yield return new WaitForSeconds(0.1f);

           float t = 1f;

           while (t > 0f)
           {
               t -= Time.deltaTime / 2f;
               
               _characterMaterial.SetFloat(Dissolve, t);

               yield return null;
           }
           
           StopCoroutine(_showMeshCoroutine);
        }
        
        
        // ReSharper disable once IteratorNeverReturns
    }
    
    private IEnumerator HideMeshCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / 2f;
               
                _characterMaterial.SetFloat(Dissolve, t);

                yield return null;
            }
           
            StopCoroutine(_hideMeshCoroutine);
        }
        
        
        // ReSharper disable once IteratorNeverReturns
    }
}