using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Canvas _gameWinCanvas;
    [SerializeField] private LayerMask _playerLayer;

    void Start()
    {
        _gameWinCanvas.enabled = false;
    }

    void Update()
    {
        bool playerHit = Physics.CheckSphere(transform.position, 1f, _playerLayer);

        if (playerHit)
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        _gameWinCanvas.enabled = true;
    }
}