using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private HorizontalMovement horizontalMovementComponent;

    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRoationTime;

    private Coroutine turnCoroutine;

    private void Update()
    {

        transform.position = playerTransform.position;
    }

    public void CallTurn(bool isFacingRight)
    {
        turnCoroutine = StartCoroutine(FlipYLerp(isFacingRight));
    }

    private IEnumerator FlipYLerp(bool isFacingRight)
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotation = isFacingRight ? 0f : 180f;
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < flipYRoationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotation, (elapsedTime / flipYRoationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

}
