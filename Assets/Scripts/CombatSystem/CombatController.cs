using UnityEngine;

public class CombatController : MonoBehaviour
{
    public Camera cam1;
    [SerializeField] LayerMask enemyslayer;
    EnemyController targetEnemy;
    PlayerController playerController;
    GameObject hitObject;
    public EnemyController TargetEnemy
    {
        get => targetEnemy;
        set
        {
            targetEnemy = value;

            if (targetEnemy == null)
                CombatMode = false;
        }
    }

    bool combatMode;

    public bool CombatMode
    {
        get => combatMode;
        set {  
            combatMode = value;

            if (TargetEnemy ==  null)
                combatMode = false;

            animator.SetBool("combatMode",combatMode);
        
        }
    }

    MeleeFighter meleeFighter;
    Animator animator;
    CameraController cam;
    private void Awake()
    {
        meleeFighter = GetComponent<MeleeFighter>();
        animator = GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    private void Start()
    {
        meleeFighter.OnGotHit += (MeleeFighter attacker) =>
        {
            if (CombatMode && attacker != TargetEnemy.Fighter)
                TargetEnemy = attacker.GetComponent<EnemyController>();
        };
        
    }

    private void Update()
    {
        if (Input.GetButtonDown("Attack") && !meleeFighter.IsTakingHit && meleeFighter.Health > 0)
        {
            //CounterAttack
            var enemy = EnemyManager.i.GetAttackingEnemy();

            if (enemy != null && enemy.Fighter.IsCounterable && !meleeFighter.InAction)
            {
                StartCoroutine(meleeFighter.PerformCounterAttack(enemy));
            }
            else
            {
                Ray ray = cam1.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, enemyslayer);

                hitObject = null;

                foreach (RaycastHit hit in hits)
                {
                    //Debug.DrawLine(ray.origin, hit.point, Color.green);

                    if (hit.collider.CompareTag("VisionSensor")) // Vision Sensor'ü atla
                    {
                        continue;
                    }

                    hitObject = hit.collider.gameObject;

                    //Debug.Log("Ray hit object: " + hitObject);
                    break;
                }

                if (hitObject != null)
                {
                    var enemyToAttack = hitObject.GetComponent<EnemyController>().Fighter;
            
                    if (enemyToAttack != null)
                    {
                        meleeFighter.TryToAttack(enemyToAttack);
                    }
                    else
                    {
                        Debug.LogWarning("Hit object is not a valid enemy!");
                    }
                }
                else
                {
                    Debug.LogWarning("No valid enemy detected by Raycast!");
                }

                CombatMode = true;
            }
        }
        if (Input.GetButtonDown("LockOn") || JoystickHelper.i.GetAxisDown("LockOnTrigger"))
        {
            CombatMode = !CombatMode;
        }
    }

    private void OnAnimatorMove()
    {
        if (!meleeFighter.InCounter)
            transform.position += animator.deltaPosition;
        transform.rotation *= animator.deltaRotation;
        
    }

    public Vector3 GetTargetDir()
    {
        if (!CombatMode)
        {

            var vecFromCam = transform.position - cam.transform.position;
            vecFromCam.y = 0;
            return vecFromCam.normalized;

        }
        else
        {
            return transform.forward;
        }
    }
}
