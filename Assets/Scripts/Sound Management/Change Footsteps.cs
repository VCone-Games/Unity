using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFootsteps : MonoBehaviour
{
    [SerializeField] private int footsteps;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MusicManager.Instance.ChangeFootstepsClip(footsteps);
    }
}
