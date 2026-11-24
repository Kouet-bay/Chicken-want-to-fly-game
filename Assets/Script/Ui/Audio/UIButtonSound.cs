using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Button Sound Clips")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"[UIButtonSound] Hover on {gameObject.name} | Instance: {AudioManager.Instance != null} | Clip: {hoverClip != null}");
        if (hoverClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(hoverClip);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"[UIButtonSound] Click on {gameObject.name} | Instance: {AudioManager.Instance != null} | Clip: {clickClip != null}");
        if (clickClip != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(clickClip);
        }
    }
}