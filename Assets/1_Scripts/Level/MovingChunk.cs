using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingChunk : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isInstantMove;
    private float actualSpeed;

    PoolerManager poolerManager;
    public Transform coinsParent;
    Vector3 spawnPos = new Vector3(0, 0, 85);

    void Awake()
    {
        poolerManager = FindObjectOfType<PoolerManager>();
        GameManager.OnGameStart += StartMove;
        GameManager.OnGameEnd += StopMove;
    }

    private void Start()
    {
        if (isInstantMove)
        {
            actualSpeed = speed;
        }
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartMove;
        GameManager.OnGameEnd -= StopMove;
    }

    void Update()
    {
        transform.Translate(Vector3.back * actualSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Disabler"))
        {
            gameObject.SetActive(false);
            gameObject.transform.position = spawnPos;
            Transform newChunk = poolerManager.GetFromPool();
            newChunk.position = spawnPos;
            newChunk.gameObject.SetActive(true);
            

            if (coinsParent != null)
            {
                foreach (Transform coin in coinsParent)
                {
                    coin.gameObject.SetActive(true);
                }
            }
        }
    }

    void StartMove()
    {
        StartCoroutine(DelayMove());
    }

    void StopMove()
    {
        actualSpeed = 0;
    }

    IEnumerator DelayMove()
    {
        yield return new WaitForSeconds(4);
        actualSpeed = speed;
    }
}
