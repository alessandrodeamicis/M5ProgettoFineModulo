using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class ButtonDoorController : MonoBehaviour
{
    [SerializeField] private GameObject doorMesh;
    [SerializeField] private NavMeshSurface surface;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject interactionUI;

    private Vector3 openPosition;
    private bool isOpen = false;
    private bool hasOpened = false;
    void Start()
    {
        openPosition = doorMesh.transform.localPosition + doorMesh.transform.forward * 6f;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (interactionUI != null)
            interactionUI.SetActive(distance <= 2f);

        if (!hasOpened && distance <= 2f && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
            hasOpened = true;

            if (surface != null)
                surface.BuildNavMesh();
        }

        if (isOpen)
        {
            doorMesh.transform.localPosition =
                Vector3.Lerp(doorMesh.transform.localPosition, openPosition, Time.deltaTime * 2f);
        }
    }

}