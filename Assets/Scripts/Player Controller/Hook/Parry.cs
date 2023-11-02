using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{

    [SerializeField] private InputActionReference parryReference;
    [SerializeField] private InputActionReference aimReference;


    [SerializeField] private float parryDistance;

    [SerializeField] private GameObject hookedObject;

    // Start is called before the first frame update
    void Start()
    {
        parryReference.action.performed += OnParry;
        parryReference.action.Disable();
    }

    private void OnParry(InputAction.CallbackContext context)
    {
        Vector3 distance = transform.position - hookedObject.transform.position;
        if (distance.magnitude < parryDistance)
        {
            Debug.Log("parried");
            hookedObject.GetComponent<IHookable>().Unhook();
            hookedObject.GetComponent<Rigidbody2D>().velocity = - distance * 2;
        }
    }

    public void SetHookedObject(GameObject hooked)
    {
        hookedObject = hooked;
    }

    public void DisableParry()
    {
        parryReference.action.Disable();
    }

    public void EnableParry()
    {
        parryReference.action.Enable();
    }


}
