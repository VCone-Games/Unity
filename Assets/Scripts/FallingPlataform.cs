using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlataform : MonoBehaviour
{
    [Header("Falling params")]
    [SerializeField] private float FallingTime = 2.0f;
    [SerializeField] private bool isFalling;
    [SerializeField] private float RespawnTime = 10.0f;
	[SerializeField] private bool isRespawning;

	[Header("Own Components")]
    [SerializeField] private Animator myAnimator;

    [Header("Control variables")]
    [SerializeField] private float fallingTimer;
    [SerializeField] private float respawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();    
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
			if (!isFalling) fallingTimer = FallingTime;
			isFalling = true;
        }
    }
    void InAnimation_EndFallingPlataform()
    {
        myAnimator.SetBool("isFalling", false);
        isFalling = false;
        isRespawning = true;
		respawnTimer = RespawnTime;
	}

    void InAnimation_EndRespawningPlataform()
    {
        myAnimator.SetBool("isRespawning", false);
        isRespawning = false;
    }
	// Update is called once per frame
	void FixedUpdate()
    {
        if (isFalling)
        {
            if (fallingTimer < 0)
            {
                myAnimator.SetBool("isFalling", true);
            } else
            {
				fallingTimer -= Time.deltaTime;
			}
		}

        if (isRespawning)
        {
			if (respawnTimer < 0)
			{
				myAnimator.SetBool("isRespawning", true);
			}
			else
			{
				respawnTimer -= Time.deltaTime;
			}
		}
    }
}
