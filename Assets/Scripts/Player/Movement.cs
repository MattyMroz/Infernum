using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = _velocity * move_speed;
    }
}
