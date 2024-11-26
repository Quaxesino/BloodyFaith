using System.Collections;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;

    Quaternion targetRotation;

    public Vector3 InputDir { get; private set; }

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;
    MeleeFighter meleeFighter;
    CombatController combatController;
    public static PlayerController i { get; private set; }

    [Header("Dash Info")]
    public float dashingPower = 12f; // Dash sýrasýnda hýz
    [SerializeField] private float dashingTime = 0.12f; // Dash süresi
    [SerializeField] private float dashingCooldown = 1f; // Dash sonrasý bekleme süresi
    [SerializeField] private TrailRenderer trail; // Dash efekti için (opsiyonel)

    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        meleeFighter = GetComponent<MeleeFighter>();
        combatController = GetComponent<CombatController>();
        i = this;
        trail.emitting = false;
    }

    private void Update()
    {
        Debug.Log("Speed:" + moveSpeed);
        Debug.Log("Dash Power:" + dashingPower);

        if (Input.GetKeyDown(KeyCode.Space) && canDash && meleeFighter.Health > 0) // Space tuþuna basýldýðýnda dash yap
        {
            StartCoroutine(Dash());
        }

        if (meleeFighter.InAction || meleeFighter.Health <= 0)
        {
            targetRotation = transform.rotation;
            animator.SetFloat("forwardSpeed", 0f);
              return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3 (h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;
        InputDir = moveDir;

        GroundCheck();
        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveInput * moveSpeed;


        if (combatController.CombatMode)
        {
            velocity /= 2f;

            //Rotate and face the target enemy
            var targetVec = combatController.TargetEnemy.transform.position - transform.position;
            targetVec.y = 0; //for only horizontal

            if (moveAmount > 0)
            {
                targetRotation = Quaternion.LookRotation(targetVec); //Buraya ileride mouse position yazýlcak.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);
            }

            //Split the velocity into its forward and sideward component and set it into the forwardSpeed and strafeSpeed 

            float forwardSpeed = Vector3.Dot(velocity, transform.forward);
            animator.SetFloat("forwardSpeed", forwardSpeed / moveSpeed, 0.2f, Time.deltaTime);
            float angle = Vector3.SignedAngle(transform.forward, velocity, Vector3.up);
            float strafeSpeed = Mathf.Sin(angle * Mathf.Deg2Rad);
            animator.SetFloat("strafeSpeed", strafeSpeed, 0.2f, Time.deltaTime);
        }
        else
        {
            if (moveAmount > 0)
            {
                targetRotation = Quaternion.LookRotation(moveInput);
            }


            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotationSpeed * Time.deltaTime);

            animator.SetFloat("forwardSpeed", moveAmount, 0.2f, Time.deltaTime);
        }

        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

    }

    void GroundCheck()
    {
        
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public Vector3 GetIntentDirection()
    {
        return InputDir != Vector3.zero ? InputDir : transform.forward;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }

    private IEnumerator Dash()
    {
        // Dash iþlemi baþlýyor
        isDashing = true;
        canDash = false;

        // Trail efekti etkinleþtirilir
        if (trail != null)
        {
            trail.emitting = true;
        }

        // Dash sýrasýnda hareket
        float timer = 0f;
        Vector3 dashDirection = transform.forward; // Oyuncunun baktýðý yön
        while (timer < dashingTime)
        {
            characterController.Move(dashDirection * dashingPower * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null; // Her çerçevede devam et
        }

        // Dash iþlemi bitti
        if (trail != null)
        {
            trail.emitting = false;
        }
        isDashing = false;

        // Dash sonrasý cooldown
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; // Dash tekrar kullanýlabilir
    }



}
