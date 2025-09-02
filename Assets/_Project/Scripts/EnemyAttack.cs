using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private Canvas _gameOverCanvas;

    private bool playerDead = false;

    void Start()
    {
        _gameOverCanvas.enabled = false;
    }

    void Update()
    {
        bool playerHit = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (playerHit && !playerDead)
        {
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        playerDead = true;
        _gameOverCanvas.enabled = true;
    }
}