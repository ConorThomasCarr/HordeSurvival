using UnityEngine;
using UnityEngine.Serialization;

public class CutoutObjects : MonoBehaviour
{ 
    public Transform targetObject;
    
    public LayerMask cutoutLayer;
    
    public Camera mainCamera;

    public Material[] mats;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cutPos = mainCamera.WorldToViewportPoint(targetObject.position);
        cutPos.y /= (Screen.width / Screen.height);
        
        Vector3 offset = targetObject.position - mainCamera.transform.position;
        RaycastHit [] hits = Physics.RaycastAll(mainCamera.transform.position, offset, offset.magnitude, cutoutLayer);

        for (int i = 0; i < hits.Length; ++i)
        {
            mats = hits[i].transform.GetComponent<Renderer>().materials;

            if (mats.Length > 0)
            {
                mats[i].SetVector("_CutoutPosition", cutPos);
            }
            
          
        }
    }
}
