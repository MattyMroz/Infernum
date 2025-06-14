using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Player _player_script;
    private Camera _camera;
    public GameObject _current_map_bounds;

    private Vector2 vel;

    private void Start()
    {
        _player_script = player.GetComponent<Player>();
        _camera = GetComponentInChildren<Camera>();
        _current_map_bounds = _player_script.current_map_bounds;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {

        _current_map_bounds = _player_script.current_map_bounds;

        Vector2 boundsSize = _current_map_bounds.GetComponent<SpriteRenderer>().bounds.size;
        float camHeight = 2f * _camera.orthographicSize;
        float camWidth = camHeight * _camera.aspect;

        float minX = (camWidth / 2f) + _current_map_bounds.transform.position.x - boundsSize.x / 2f;
        float maxX = (_current_map_bounds.transform.position.x + boundsSize.x / 2f) - (camWidth / 2f);

        float minY = (camHeight / 2f) + _current_map_bounds.transform.position.y - boundsSize.y / 2f;
        float maxY = (_current_map_bounds.transform.position.y + boundsSize.y / 2f) - (camHeight / 2f);

        Vector2 boundPosition = new Vector2(
            Mathf.Clamp(player.transform.position.x, minX, maxX),
            Mathf.Clamp(player.transform.position.y, minY, maxY));

        transform.position = boundPosition;
    }
}
