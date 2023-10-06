using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrapling : MonoBehaviour
{
    private InputAction graplingShootAction;
    private InputAction graplingAimAction;
    private InputAction graplingShootAimAction; 

    [Space][SerializeField] private InputActionAsset myActionsAsset;

    private Vector2 mouseAimPoint;


    // Start is called before the first frame update
    void Start()
    {
        graplingAimAction = myActionsAsset.FindAction("Player/Grapling Aim ");
        graplingShootAction = myActionsAsset.FindAction("Player/Grapling Shoot");
        graplingShootAimAction = myActionsAsset.FindAction("Player/Grapling Aim + Shoot");

        graplingAimAction.performed += OnMouseMovement;
        graplingShootAction.performed += OnMouseShoot;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseMovement (InputAction.CallbackContext context)
    {
        mouseAimPoint = context.ReadValue<Vector2>();
    }

    void OnMouseShoot(InputAction.CallbackContext context)
    {
        Vector2 shootDirection = mouseAimPoint - new Vector2(transform.position.x, transform.position.y);   
        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, 3, LayerMask.GetMask("Grabbable"));
        Debug.Log(shootDirection);

        if(hit.collider != null)
        {
            Debug.Log(hit.collider.gameObject.tag);
        }
    }
}
