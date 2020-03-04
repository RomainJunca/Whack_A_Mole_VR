using UnityEngine;

public class AttachToMainCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    void Start()
    {
        this.transform.SetParent(mainCamera.transform,false);
    }
}
