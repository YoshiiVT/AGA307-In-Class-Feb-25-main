using UnityEngine;

public class DisableMeshRender : MonoBehaviour
{
 
    void Start()
    {
        if(GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
    }
}
