using UnityEngine;

public class WallDetection : MonoBehaviour
{
    [Header("Wall Detection Settings")]

    public float wallDetectionDistance = 0.5f;

    public LayerMask wallLayer;

    public bool isTouchingWallLeft;

    public bool isTouchingWallRight;

    public bool isTouchingWallBottom;

    public bool isTouchingWallTop;

    public Rigidbody2D rb;
 
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Update()
    {

        DetectWalls();

    }

    void DetectWalls()
    {
        isTouchingWallLeft = Physics2D.Raycast(transform.position, Vector2.left, wallDetectionDistance, wallLayer);
        isTouchingWallTop = Physics2D.Raycast(transform.position, Vector2.up, wallDetectionDistance, wallLayer);
        isTouchingWallBottom = Physics2D.Raycast(transform.position, Vector2.down, wallDetectionDistance, wallLayer);
        isTouchingWallRight = Physics2D.Raycast(transform.position, Vector2.right, wallDetectionDistance, wallLayer);
    }
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallDetectionDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallDetectionDistance);
    }
}
