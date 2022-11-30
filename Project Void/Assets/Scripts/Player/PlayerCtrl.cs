using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    public float speedMod = 3.0f;

    private PlayerInput playerInput;
    private InputAction moveAction;

    Rigidbody2D rbody;
    Animator anim;
    Vector2 lookDir = new Vector2(1, 0);

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        Vector2 moveDir = moveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(moveDir.x, 0.0f))
        {
            lookDir.Set(moveDir.x, 0);
            lookDir.Normalize();
        }

        anim.SetFloat("Direction X", lookDir.x);
        anim.SetFloat("Speed", moveDir.magnitude);
    }

    void FixedUpdate()
    {
        rbody.velocity = moveAction.ReadValue<Vector2>() * speedMod;
    }
}
