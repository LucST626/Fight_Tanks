using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("CoolDowns")]
    [SerializeField] float movementCD = 0.2f;
    private float movementTimer;

    [Header("Controls")]
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode sprint;
    [SerializeField] KeyCode shoot;

    [Header("Sprint")]
    [SerializeField] float maxEnergy = 5f;
    [SerializeField] float sprintCost = 1f;
    [SerializeField] float sprintMultiplier = 2f;
    private float currentEnergy;

    [Header("Speed")]
    [SerializeField] Vector3 movLateral = new Vector2(1, 0);
    [SerializeField] Vector3 movLateralUp = new Vector2(0, 1);

    [Header("Shooting")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint;
    [SerializeField] float projectileSpeed = 10f;

    private Vector3 lastDirection = Vector3.zero;

    void Start()
    {
        movementTimer = 0f;
        currentEnergy = maxEnergy;
    }

    void Update()
    {
        if (movementTimer > 0)
        {
            movementTimer -= Time.deltaTime;
            return;
        }

        Vector3 moveDirection = Vector3.zero;
        float currentSpeed = 1f;

        if (Input.GetKey(right))
        {
            moveDirection = movLateral;
        }

        else if (Input.GetKey(left))
        {
            moveDirection = -movLateral;
        }

        else if (Input.GetKey(up))
        {
            moveDirection = movLateralUp;
        }

        else if (Input.GetKey(down))
        {
            moveDirection = -movLateralUp;
        }

        if (Input.GetKey(sprint) && currentEnergy > 0)
        {
            currentSpeed = sprintMultiplier;
            currentEnergy -= sprintCost * Time.deltaTime;
        }
        else if (currentEnergy < maxEnergy)
        {
            currentEnergy += sprintCost * Time.deltaTime;
        }

        if (moveDirection != Vector3.zero)
        {
            transform.position += moveDirection * currentSpeed;
            lastDirection = moveDirection;
            movementTimer = movementCD / currentSpeed;

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        if (Input.GetKeyDown(shoot))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (lastDirection == Vector3.zero) return;
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = lastDirection * projectileSpeed;
        Destroy(projectile, 5);
    }

    public Vector3 GetLastDirection()
    {
        return lastDirection;
    }
}
