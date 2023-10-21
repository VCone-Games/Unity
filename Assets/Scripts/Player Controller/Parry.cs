using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Parry : MonoBehaviour
{
    [Header("Input system")]
    [SerializeField] InputActionReference parryReference;

    private GameObject hookedObject;



    // Start is called before the first frame update
    void Start()
    {
        parryReference.action.performed += OnParry;

    }



    // Update is called once per frame
    void Update()
    {

    }


    private void OnParry(InputAction.CallbackContext context)
    {
        if (!GetComponent<Hook>().HookLanded()) return;

        Debug.Log("PARREO");

        hookedObject = GetComponent<Hook>().getHookedObject();

        Vector3 distance = transform.position - hookedObject.transform.position;

        if(distance.magnitude < 3)
        {
            Debug.Log("MUERE");
            Destroy(hookedObject);
        }

    }
}
