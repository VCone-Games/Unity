using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSSISDEAD : MonoBehaviour
{
    [Header("0.curcuma, 1.azafran, 2.perejil")]
    [SerializeField] private int defeatedBoss;
    [SerializeField] private GameObject tpToBoss;
    [SerializeField] private GameObject tpAfterBoss;
    void Start()
    {
        string bossName = "";
        switch (defeatedBoss)
        {
            case 0:
                bossName = "Curcuma";
                break;
            case 1:
                bossName = "Azafran";
                break;
            case 2:
                bossName = "Perejil";
                break;

        }

        if (DataBase.Singleton.IsBossDead(bossName))
        {
            tpToBoss.SetActive(false);
            tpAfterBoss.SetActive(true);
        }
        else
        {
            tpToBoss.SetActive(true);
            tpAfterBoss.SetActive(false);
        }
    }

}
