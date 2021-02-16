using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform visual;
    [SerializeField] private AnimatorController animator;
    [SerializeField] private int playerIndex = 0;
    [SerializeField] private Rigidbody body;
    [SerializeField] private Transform front;
    [SerializeField] private bool onFloor = false;
    [SerializeField] private GameObject onFloorGO;
    [SerializeField] private LayerMask floorLayer;
    [SerializeField] private List<Vector2> directions = new List<Vector2>();

    [SerializeField] private float rotationSpeed = 50f;
    //[SerializeField] private float jumpSpeed = 80f;
    [SerializeField] private bool canMove = true;
    //[SerializeField] private float charging = 0.01f;
    //[SerializeField] private float nb = 0f;
    [SerializeField] private Vector3 move = Vector3.zero;
    [SerializeField] private Vector3 look = Vector3.zero;
    [SerializeField] private float maxSpeed = 14f;
    private float turnSmothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;
    private float lookSmothTime = 0.1f;
    [SerializeField] private float lookSmoothVelocity;
    [SerializeField] private float speed = 60f;
    //[SerializeField] private bool jump = false;
    //[SerializeField] private int jumpNumber = 2;
    void Awake()
    {
        directions.Add(new Vector2(0, 1));
        directions.Add(new Vector2(1, 0));
        directions.Add(new Vector2(-1, 0));
        directions.Add(new Vector2(0, -1));

        visual = transform.Find("Visual");
        body = GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
        speed = (speed + body.drag * 17f) * 0.01f;
        front = visual.transform.Find("Front");
    }
    public int GetPlayerIndex()
    {
        return playerIndex;
    }
    public void SetPlayerIndex(int nb)
    {
        playerIndex = nb;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable"))
        {
            Debug.Log("PlayerController, OnCollisionEnter : CollisionGameobjectLayer = " + collision.gameObject.layer);
            onFloor = true;
       };
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        var cols = Physics.OverlapSphere(onFloorGO.transform.position, 0.2f, floorLayer);
        if (cols.Length == 0) onFloor = false;
        else onFloor = true;

        if (!canMove) return;

        if (look.magnitude >= 0.3f)
        {
            float targetLookAngle = Mathf.Atan2(look.x, look.z) * Mathf.Rad2Deg;
            float lookAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetLookAngle, ref lookSmoothVelocity, lookSmothTime);
            transform.rotation = Quaternion.Euler(0f, lookAngle, 0f);
        }
        var moveVectX = 0f;
        var moveVectY = 0f;
        var moveVectZ = 0f;
        if (!onFloor && body.velocity.y < 0f && body.velocity.y > -7f) moveVectY = body.velocity.y * 0.1f;
        if (move.magnitude >= 0.2f)
        {
            moveVectX = move.x * speed  - body.velocity.x * 1f/maxSpeed;
            moveVectZ = move.z * speed - body.velocity.z * 1f/maxSpeed;

            if (look.magnitude < 0.3f)
            {
                float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
        //body.AddForce(new Vector3(moveVectX, moveVectY, moveVectZ), ForceMode.VelocityChange);
        body.velocity += moveVectX * Vector3.right + moveVectY * Vector3.up + moveVectZ * Vector3.forward;
        if (animator != null)
        {
            Debug.Log("PlayerController, Walk Direction : move");
            var mov = FindClosest(new Vector2(move.x, move.z), directions);
            Debug.Log("PlayerController, Walk Direction : dir");
            var dir = FindClosest(new Vector2(look.x, look.z), directions);
            if (move.magnitude >= 0.2f)
            {
                if (mov == dir)
                {
                    Debug.Log("PlayerController, Walk Direction : Forward - Move = " + mov);
                    Debug.Log("PlayerController, Walk Direction : Forward - Dir = " + dir);
                    animator.WalkForward();
                    return;
                }
                if (mov == -dir)
                {
                    Debug.Log("PlayerController, Walk Direction : Backward - Move = " + mov);
                    Debug.Log("PlayerController, Walk Direction : Backward - Dir = " + dir);
                    animator.WalkBackward();
                    return;
                }
                if (CheckRightDir(mov, dir))
                {
                    Debug.Log("PlayerController, Walk Direction : Right - Move = " + mov);
                    Debug.Log("PlayerController, Walk Direction : Right - Dir = " + dir);
                    animator.WalkRight();
                    return;
                }
                if (!CheckRightDir(mov, dir))
                {
                    Debug.Log("PlayerController, Walk Direction : Left - Move = " + mov);
                    Debug.Log("PlayerController, Walk Direction : Left - Dir = " + dir);
                    animator.WalkLeft();
                    return;
                }
            }
            else animator.ToIdle();

        }


    }
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    public bool CheckRightDir(Vector2 move, Vector2 dir)
    {
        if (move == Vector2.right)
        {
            if (dir == Vector2.up) return true;
        }
        if (move == Vector2.up)
        {
            if (dir == -Vector2.right) return true;
        }
        if (move == -Vector2.right)
        {
            if (dir == -Vector2.up) return true;
        }
        if (move == -Vector2.up)
        {
            if (dir == Vector2.right) return true;
        }
        return false;
    }
    public bool GetCanMove()
    {
        return canMove;
    }
    public void SetMove(Vector2 value)
    {
        move = new Vector3(value.x, 0f, value.y);
    }
    public Vector2 GetMove()
    {
       return new Vector2 (move.x, move.z);
    }
    public bool GetOnFloor()
    {
        return onFloor;
    }
    public Vector2 GetLook()
    {
        return new Vector2(look.x, look.z);
    }
    public void SetLook(Vector2 value)
    {
        look = new Vector3(value.x, 0f, value.y);
    }

    public void StopMotion()
    {
        body.velocity = Vector3.zero;
    }
    public void SetFreeze(bool value)
    {
        if (value)
        {
            body.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            body.constraints = RigidbodyConstraints.None;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (onFloorGO) Gizmos.DrawWireSphere(onFloorGO.transform.position, 0.2f);
    }
    private Vector3 FindClosest(Vector2 vect, List<Vector2> vects)
    {
        List<float> range = new List<float>();
        for (var i = 0; i < vects.Count; i++)
        {
            Debug.Log("BoxingManager, FindClosest : vetcs[i] = " + i + " " + vects[i]);
            range.Add(Vector2.Distance(vects[i], vect));
            Debug.Log("BoxingManager, FindClosest : range[i] = " + i + " " + range[i]);
        }
        float closest = Mathf.Infinity;
        int nb = 0;
        for (var i = 0; i < range.Count; i++)
        {
            if (range[i] < closest)
            {
                closest = range[i];
                nb = i;
            }
        }
        return vects[nb];
    }
    
}
