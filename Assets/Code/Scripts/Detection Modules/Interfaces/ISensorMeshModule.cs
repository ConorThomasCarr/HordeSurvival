using ISensor.MeshData;

using UnityEngine;

namespace ISensor.ISensorMesh
{
    public interface ISensorMeshModule
    {
        public Mesh mesh { get; set; }

        public MeshDataStruct MeshData { get; set; }

        public Mesh CreateWedgeMesh();

        public void InitializeMeshModule(MeshDataStruct meshData);
        
        public void Enable();

        public void Disable();
    }
}



