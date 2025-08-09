using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -7);
    

    void LateUpdate()
    {
        transform.position = playerController.transform.position + offset;
    }
}
