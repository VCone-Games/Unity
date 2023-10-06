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
    [SerializeField] private float graplingRange;

    private Vector2 mousePositionInScreen;
    private Vector2 mousePositionInWorld;

    private GameObject grappledObject;
    private bool isGrappling;

    [SerializeField] private float grapplerSpeed = 10;


    // Start is called before the first frame update
    void Start()
    {
        graplingRange = transform.GetChild(0).transform.lossyScale.x * 0.5f;

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

    private void FixedUpdate()
    {
        if(isGrappling && grappledObject != null)
        {
            Vector2 direction = new Vector2( transform.position.x - grappledObject.transform.position.x, transform.position.y - grappledObject.transform.position.y );
            if(direction.magnitude < 1 || direction.magnitude > graplingRange)
            {
                isGrappling = false;
                grappledObject = null;
            }
            else
            {
                direction.Normalize();
                grappledObject.GetComponent<Rigidbody2D>().velocity = new Vector3(direction.x, direction.y, 0) * grapplerSpeed;
            }
            
        }
    }

    void OnMouseMovement(InputAction.CallbackContext context)
    {
        mousePositionInScreen = context.ReadValue<Vector2>();
        mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePositionInScreen);
    }

    void OnMouseShoot(InputAction.CallbackContext context)
    {
        //Debug.Log("Mouse en pantalla: "+mousePositionInScreen);
        //Debug.Log("Mouse en Mundo: " + mousePositionInWorld);
        //Debug.Log("Personaje en " + transform.position);

        if (isGrappling) return;

        Vector2 shootDirection = mousePositionInWorld - new Vector2(transform.position.x, transform.position.y);
        shootDirection.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(transform.position, shootDirection, graplingRange, ~LayerMask.GetMask("Player"));


        if (hit.collider != null && hit.collider.gameObject.tag == "Grabbable Object")
        {
            Debug.Log(hit.collider.gameObject.tag);
            grappledObject = hit.collider.gameObject;
            isGrappling = true;
        }
    }
}
