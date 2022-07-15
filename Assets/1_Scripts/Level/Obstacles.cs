using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public PlayerState ps;
    public PlayerCore pc;
    public AudioData bangSfx;
    private GameManager gm;
    private MovingChunk mc;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        pc = FindObjectOfType<PlayerCore>();
        mc = GetComponentInParent<MovingChunk>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (pc.IsShield && col.gameObject.GetComponent<PlayerCore>().playerState != ps)
            {
                AudioManager.instance.PlaySFX(bangSfx, "bang");
                Transform emptyChunk = PoolerManager.instance.GetEmptyChunk();
                emptyChunk.position = mc.gameObject.transform.position;
                emptyChunk.gameObject.SetActive(true);
                mc.gameObject.SetActive(false);
                pc.DisableShield();
                return;
            }
            else if (ps == PlayerState.Whatever)
            {
                AudioManager.instance.PlaySFX(bangSfx, "bang");
                pc.Anim.SetTrigger("isDie");
                gm.isPlaying = false;
                gm.Lose();
            }
            else if (col.gameObject.GetComponent<PlayerCore>().playerState != ps)
            {
                AudioManager.instance.PlaySFX(bangSfx, "bang");
                pc.Anim.SetTrigger("isDie");
                gm.isPlaying = false;
                gm.Lose();
            }
        }
    }
}
