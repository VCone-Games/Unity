using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGrapling : MonoBehaviour
{
    private InputAction graplingShootAction;
    private InputAction graplingAimAction;
    private InputAction graplingShootAimAction;

    [Header("Input Actions")][SerializeField] private InputActionAsset myActionsAsset;

    private Vector2 mousePositionInScreen;
    private Vector2 mousePositionInWorld;

    private GameObject grappledObject;
    public bool isGrappling;

	[Header("Grab Params")]
	[SerializeField] float graplingRange;
	[SerializeField] private float grapplerSpeed = 10;
    [SerializeField] AnimationCurve graplingAnimationCurve;
    [SerializeField] float graplingDuration = 2f;
    private float graplingElapsedTime;
    private float graplingPercentageComplete;

    private Rigidbody2D grappledObjectRigidbody;
    private Rigidbody2D myRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

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
        if (isGrappling && grappledObject != null)
        {
            Vector2 distance = new Vector2(transform.position.x - grappledObject.transform.position.x, transform.position.y - grappledObject.transform.position.y);
            if (distance.magnitude < 1 || distance.magnitude > graplingRange)
            {
                isGrappling = false;
                if (grappledObjectRigidbody != null)
                {
                    grappledObjectRigidbody.velocity = Vector2.zero;
                    grappledObjectRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                }

                myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
                grappledObject = null;
                grappledObjectRigidbody = null;
                graplingElapsedTime = 0;
                graplingPercentageComplete = 0;
            }
            else
            {
                distance.Normalize();
                graplingElapsedTime += Time.deltaTime;
                graplingPercentageComplete = graplingElapsedTime / graplingDuration;
                if (grappledObjectRigidbody == null)
                    myRigidbody.velocity = new Vector3(-distance.x, -distance.y, 0) * graplingAnimationCurve.Evaluate(graplingPercentageComplete) * grapplerSpeed;
                else
                {
                    switch (grappledObjectRigidbody.mass)
                    {
                        case 10:
                            myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
                            grappledObjectRigidbody.velocity = new Vector3(distance.x, distance.y, 0) * graplingAnimationCurve.Evaluate(graplingPercentageComplete) * grapplerSpeed;
                            break;
                        case 15:
                            grappledObjectRigidbody.velocity = new Vector3(distance.x, distance.y, 0) * graplingAnimationCurve.Evaluate(graplingPercentageComplete) * grapplerSpeed;
                            myRigidbody.velocity = new Vector3(-distance.x, -distance.y, 0) * graplingAnimationCurve.Evaluate(graplingPercentageComplete) * grapplerSpeed;
                            break;
                        case 20:
                            if (grappledObjectRigidbody != null)
                                grappledObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                            myRigidbody.velocity = new Vector3(-distance.x, -distance.y, 0) * graplingAnimationCurve.Evaluate(graplingPercentageComplete) * grapplerSpeed;
                            break;

                    }
                }


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


        if (hit.collider != null && (hit.collider.gameObject.tag == "Grabbable Object" || hit.collider.gameObject.tag == "Grabbable Spot"))
        {
            Debug.Log(hit.collider.gameObject.tag);
            grappledObject = hit.collider.gameObject;
            if (grappledObject.GetComponent<Rigidbody2D>() != null)
                grappledObjectRigidbody = grappledObject.GetComponent<Rigidbody2D>();
            isGrappling = true;
        }
    }
}
