using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference;

    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed = 10f;

    private float horizontalInput;    
    private float verticalInput;    
    // private Vector2 playerInput;


    //private void Start()
    //{
    //    actionReference.ToInputAction().performed += PlayerController_performed;
    //    actionReference.ToInputAction().canceled += (obj) => playerInput = obj.ReadValue<Vector2>().normalized;
    //}
    //private void PlayerController_performed(InputAction.CallbackContext obj)
    //{
    //    playerInput = obj.ReadValue<Vector2>().normalized;        
    //}
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        // Moves the car forwad based on vertical input
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        // Rotates the cards based on horizontal input
        transform.Rotate(Vector3.up, Time.deltaTime * speed * horizontalInput * 5);

        // Moves the card forward using input actions
        //transform.Translate(Vector3.forward * Time.deltaTime * speed * playerInput.y);
        // Rotates the card using input actions
        //transform.Rotate(Vector3.up, Time.deltaTime * speed * playerInput.x * 5);
    }
}
