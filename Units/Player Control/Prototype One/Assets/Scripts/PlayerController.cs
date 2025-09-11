using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 20.0f;
    public float turnSpeed = 45.0f;
    

    private Vector2 InputMovement;

    public InputActionReference move;

    private Rigidbody rb;
    Collider left_wheel;

    enum InputDirections
    {
        Horizontal=99,
        Vertical
    }
    enum GrowthCycle
    {
        barren,
        grow1,
        grow2,
        havrest,
        seedling,
        sprout,        
    }
    private GrowthCycle growthCycle;
    [SerializeField] private Sprite[] growthImages;
    private Sprite currentImages;

    enum boolean
    {
        False,
        True,
    }

    private void Awake()
    {
        if (growthCycle == GrowthCycle.grow1)
        {
            currentImages = growthImages[(int)growthCycle];
        }

        rb = GetComponent<Rigidbody>();
        left_wheel = GetComponentsInChildren<Collider>().SingleOrDefault(q => q.name == "Wheel_fl");
    }

    // Update is called once per frame
    void Update()
    {
        //InputMovement = new Vector2(Input.GetAxis(InputDirections.Horizontal.ToString()), Input.GetAxis(InputDirections.Vertical.ToString()));

        InputMovement = move.action.ReadValue<Vector2>();

        // Move the vehicle forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed * InputMovement.y);
        // Turns the vehicle
        transform.Rotate(Vector3.up, turnSpeed * InputMovement.x * Time.deltaTime);
    }
}
