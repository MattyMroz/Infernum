using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{

    [Header("Move Speed")]
    [SerializeField] float move_speed = 10f;

    [Header("Keycodes")]
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    private Rigidbody2D _rb;
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _last_velocity;

    [Header("Animation")]
    [SerializeField] Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _last_velocity = _velocity;
    }

    // Update is called once per frame
    void Update()
    {
        float x_dir = 0f, y_dir = 0f;

        if (Input.GetKey(up))
            y_dir = 1f;
        if (Input.GetKey(down))
            y_dir = -1f;
        if (Input.GetKey(left))
            x_dir = -1f;
        if (Input.GetKey(right))
            x_dir = 1f;

        _velocity = new Vector2(x_dir, y_dir).normalized;

        if (x_dir != 0f || y_dir != 0f)
        {
            animator.SetBool("Moving", true);
            _last_velocity = _velocity;
        }
        else
            animator.SetBool("Moving", false);

        // Rotate player towards movement direction
        float angle = Mathf.Atan2(_last_velocity.y, _last_velocity.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10f * UnityEngine.Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _velocity * move_speed;
    }
}
