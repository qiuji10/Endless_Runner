using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.UpdateCoin();
            gameObject.SetActive(false);
        }
    }
}
