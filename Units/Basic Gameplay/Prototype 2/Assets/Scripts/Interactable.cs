using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected virtual void UseInteractable(PlayerController player) 
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            UseInteractable(player);
        }
    }
}
