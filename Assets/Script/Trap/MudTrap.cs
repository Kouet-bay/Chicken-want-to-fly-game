using System.Collections;
using UnityEngine;

public class MudTrap : MonoBehaviour
{
    [Header("Mud Trap Settings")]
    [Range(0.1f, 0.9f)] [SerializeField] private float speedMultiplier = 0.4f;
    [Range(0.1f, 0.9f)] [SerializeField] private float dashMultiplier = 0.5f;
    [SerializeField] private float exitGraceTime = 0.1f;

    private Coroutine exitRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (exitRoutine != null)
            {
                StopCoroutine(exitRoutine);
                exitRoutine = null;
            }

            player.EnterMud(speedMultiplier, dashMultiplier);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            if (exitRoutine != null)
            {
                StopCoroutine(exitRoutine);
            }
            exitRoutine = StartCoroutine(DelayedExit(player));
        }
    }

    private IEnumerator DelayedExit(Player player)
    {
        yield return new WaitForSeconds(exitGraceTime);
        if (player != null)
        {
            player.ExitMud();
        }
        exitRoutine = null;
    }
}