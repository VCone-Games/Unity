using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPhaseAzafran : MonoBehaviour
{
    [Header("TILEMAP")]
    [SerializeField] GameObject tileMapGround;
	[Header("Enemy components")]
	[SerializeField] Animator myAnimator;

    [Header("Second Phase params")]
    [SerializeField] private float advanceTime = 2f;
    [SerializeField] private float spawnStonesTime = 3f;
    [SerializeField] private int stonesPerSpawnPoint = 3;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int maxSteps = 6;
    [SerializeField] private int currentStep = 0;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        tileMapGround = GameObject.FindGameObjectWithTag("Ground");
        myAnimator = GetComponent<Animator>();
        MusicManager.Instance.PhirstPhaseAzafran = false;
        MusicManager.Instance.PlayAzafranSecondPhase();

        InvokeRepeating("Advance", advanceTime, advanceTime);
        InvokeRepeating("SpawnAttack", spawnStonesTime, spawnStonesTime);
    }

    void Advance ()
    {
        Debug.Log("\tAvanzando..");
        
        if (currentStep == maxSteps)
        {
            Debug.Log("Fin del combate");
            canMove = false;
            myAnimator.SetBool("isDead", true);
            CancelInvoke();
		}else
        {
			canMove = !canMove;
            if (canMove) currentStep++;
		}
	}

    void SpawnAttack()
    {
        Debug.Log("\tstones falling");
		GetComponent<PhaseManagerAzafran>().summonFallingStone?.Invoke(this, stonesPerSpawnPoint);
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if (canMove)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
			tileMapGround.GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        } else
        {
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			tileMapGround.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

		}
	}
}
