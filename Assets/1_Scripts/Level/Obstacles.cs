using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public PlayerState ps;
    public PlayerController pc;

    private void Awake()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (ps == PlayerState.Whatever)
            {
                Debug.Log("Whatever loseee");
            }
            else if (col.gameObject.GetComponent<PlayerController>().playerState != ps)
            {
                Debug.Log("Player loseee");
            }
        }
    }
}
