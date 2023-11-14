
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;
    private Transform cameraTransform;
    private Vector3 previosCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        previosCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = (cameraTransform.position.x - previosCameraPosition.x) * parallaxMultiplier;

        float deltaY = (cameraTransform.position.y - previosCameraPosition.y) * parallaxMultiplier;

        transform.Translate(new Vector3(deltaX, deltaY, 0));

        previosCameraPosition = cameraTransform.position;
    }
}