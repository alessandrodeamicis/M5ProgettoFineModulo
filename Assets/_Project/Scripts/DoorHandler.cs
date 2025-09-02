using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class DoorHandler : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private NavMeshSurface _surface;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _panelUI;

    private Vector3 openPosition;
    private bool isOpen = false;
    private bool hasOpened = false;

    void Start()
    {
        openPosition = _door.transform.position + _door.transform.forward * 4f;
    }

    void Update()
    {
        float distance = Vector3.Distance(_player.position, transform.position);

        if (_panelUI != null && !hasOpened)
            _panelUI.SetActive(distance <= 2f);

        if (!hasOpened && distance <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
            hasOpened = true;
            _panelUI.SetActive(false);
        }

        if (isOpen)
        {
            _door.transform.position =
                Vector3.Lerp(_door.transform.position, openPosition, Time.deltaTime * 2f);

            if (_surface != null)
                _surface.BuildNavMesh();
        }
    }

}