using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPPLAYERBACK : MonoBehaviour
{
    [SerializeField] private GameObject DENSE_FOG1;
    [SerializeField] private GameObject DENSE_FOG2;
    [SerializeField] private GameObject DENSE_FOG3;
    [SerializeField] private GameObject triggerScene;
    [SerializeField] private GameObject TO_GENGIBRE;
    private float tpTimer;

    private void FixedUpdate()
    {
        if (tpTimer > 0)
        {
            tpTimer -= Time.fixedDeltaTime;

            if (tpTimer < 4)
            {
                DENSE_FOG1.SetActive(true);
            }

            if (tpTimer < 2.5)
            {
                DENSE_FOG2.SetActive(true);
            }
            if (tpTimer < 1)
            {
                DENSE_FOG3.SetActive(true);
            }

            if (tpTimer < 0)
            {
                if (DatabaseMetrics.Singleton.IsBossDead("Azafran") && DatabaseMetrics.Singleton.IsBossDead("Curcuma") && TO_GENGIBRE != null)
                {
                    TO_GENGIBRE.SetActive(true);
                }
                else
                {
                    triggerScene.SetActive(true);
                }

            }
        }

    }

    public void TeleportBack()
    {
        tpTimer = 5f;
    }
}
