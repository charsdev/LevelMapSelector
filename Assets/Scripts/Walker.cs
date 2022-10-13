using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum Direction
{
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class Walker : MonoBehaviour
{
    [SerializeField] private Path _currentPath;
    [SerializeField] private Waypoint _currentWaypoint;
    public Node InitialNode;
    public float Speed;
    private List<KeyCode> _keybiding = new List<KeyCode>();
    private Dictionary<KeyCode, Direction> _keycodeToDirection = new Dictionary<KeyCode, Direction>();
    public bool facingRight;
    [HideInInspector] public Waypoint lastWaypoint;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("LastWaypoint"))
        {
            string name = PlayerPrefs.GetString("LastWaypoint");
            GameObject lastWaypoint = GameObject.Find(name);
            if (lastWaypoint.TryGetComponent(out Waypoint waypoint))
            {
                InitialNode = waypoint;
            }
        }
    }

    public void Start()
    {
        BindKeys();

        if (InitialNode == null) return;

        transform.position = InitialNode.position;

        if (InitialNode.TryGetComponent(out Waypoint waypoint))
        {
            _currentWaypoint = waypoint;
        }

    }

    private void BindKeys()
    {
        _keybiding.Add(KeyCode.W);
        _keybiding.Add(KeyCode.S);
        _keybiding.Add(KeyCode.D);
        _keybiding.Add(KeyCode.A);
        _keybiding.Add(KeyCode.UpArrow);
        _keybiding.Add(KeyCode.DownArrow);
        _keybiding.Add(KeyCode.LeftArrow);
        _keybiding.Add(KeyCode.RightArrow);
        _keybiding.Add(KeyCode.JoystickButton8);
        _keybiding.Add(KeyCode.JoystickButton7);
        _keybiding.Add(KeyCode.JoystickButton5);
        _keybiding.Add(KeyCode.JoystickButton6);

        _keycodeToDirection.Add(KeyCode.W, Direction.UP);
        _keycodeToDirection.Add(KeyCode.S, Direction.DOWN);
        _keycodeToDirection.Add(KeyCode.A, Direction.LEFT);
        _keycodeToDirection.Add(KeyCode.D, Direction.RIGHT);

        _keycodeToDirection.Add(KeyCode.UpArrow, Direction.UP);
        _keycodeToDirection.Add(KeyCode.DownArrow, Direction.DOWN);
        _keycodeToDirection.Add(KeyCode.LeftArrow, Direction.LEFT);
        _keycodeToDirection.Add(KeyCode.RightArrow, Direction.RIGHT);

        _keycodeToDirection.Add(KeyCode.JoystickButton8, Direction.RIGHT);
        _keycodeToDirection.Add(KeyCode.JoystickButton7, Direction.LEFT);
        _keycodeToDirection.Add(KeyCode.JoystickButton5, Direction.UP);
        _keycodeToDirection.Add(KeyCode.JoystickButton6, Direction.DOWN);
    }

    public void Update()
    {
        foreach (KeyCode keyCode in _keybiding)
        {
            if (!_keycodeToDirection.ContainsKey(keyCode)) return;

            if (Input.GetKeyDown(keyCode))
            {
                var direction = _keycodeToDirection[keyCode];
                /// rethink logic i dont like null comparision
                if (_currentWaypoint != null)
                {
                    _currentPath = _currentWaypoint.GetPath(direction);

                    if (_currentPath != null && _currentPath.Direction == direction)
                    {
                        StopAllCoroutines();
                        StartCoroutine(MoveAlongPath(_currentPath));
                    }
                }
            }
        }
    }

    private IEnumerator MoveAlongPath(Path selectedPath)
    {
        var step = Speed * Time.deltaTime;
        var nodes = selectedPath.Nodes;
        var currentIndex = 0;
        var currentNode = nodes[0];
        var target = nodes[currentIndex];
        var end = nodes[nodes.Count - 1];
        _currentWaypoint = null;
        var initialPosition = transform.position;
        var diff = end.position.x - initialPosition.x;

        if (diff < 0 && facingRight)
        {
            facingRight = false;
            transform.localScale = new Vector3(-0.55f, transform.localScale.y, transform.localScale.z);
        }
        else if (diff > 0 && !facingRight)
        {
            facingRight = true;
            transform.localScale = new Vector3(0.55f, transform.localScale.y, transform.localScale.z);
        }

        while (currentNode != end)
        {
            if (Vector3.Distance(transform.position, target.position) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
            else if (DebugTools.InBounds(currentIndex + 1, nodes))
            {
                currentNode = nodes[currentIndex];
                currentIndex++;
                target = nodes[currentIndex];
            }
            else
            {
                currentNode = nodes[currentIndex];
                _currentWaypoint = (Waypoint)currentNode;
            }

           yield return null;
        }

    }

}
