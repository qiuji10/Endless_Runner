using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingChunk : MonoBehaviour
{
    [SerializeField] private float speed;
    private float actualSpeed;

    PoolerManager poolerManager;
    Vector3 spawnPos = new Vector3(0, 0, 85);

    void Awake()
    {
        poolerManager = FindObjectOfType<PoolerManager>();
        GameManager.OnGameStart += StartMove;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStart -= StartMove;
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
            Transform newChunk = poolerManager.GetFromPool();
            newChunk.gameObject.SetActive(true);
            newChunk.position = spawnPos;
        }
    }

    void StartMove()
    {
        StartCoroutine(DelayMove());
    }

    IEnumerator DelayMove()
    {
        yield return new WaitForSeconds(4);
        actualSpeed = speed;
    }
}
