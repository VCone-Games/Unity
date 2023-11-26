using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSceneChange : MonoBehaviour
{
    [SerializeField] private SceneObject changingScene;
    [SerializeField] private int EnterPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneChanger.Instance.ChangeSceneByMoving(changingScene, EnterPosition);
        }
    }
}
