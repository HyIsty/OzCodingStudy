using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

#if ENABLE_INPUT_SYSTEM
[RequireComponent(typeof(PlayerInput))]
#endif
public class RayShooter : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask r_interactableMask;
    [SerializeField]
    private LayerMask r_enemyMask;
    [SerializeField]
    private float r_maxDistance = 100f;
    [SerializeField]
    private float interact_maxDistance = 10f;
    [SerializeField]
    private float attackRadius = 1f;


    [Header("Ref")]
    [SerializeField]
    private StarterAssetsInputs _input;
    [SerializeField]
    private Transform _attackPosition;

    [Header("Stat")]
    [SerializeField]
    private int atk = 10;

    private IInteractable currentInteract;

    private bool isInteracting = false;
    private bool isAttacking = false;

    private void Awake()
    {
        _input = GetComponent<StarterAssetsInputs>();
        if(_camera == null)
            _camera = Camera.main;
    }

    private void Update()
    {
        CheckHovering();
    }

    private void CheckHovering()
    {
        if (_camera == null)
            return;

        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = _camera.ScreenPointToRay(screenCenter);

        IInteractable nextHovering = null;

        if (Physics.Raycast(ray, out RaycastHit hit, interact_maxDistance, r_interactableMask))
        {
            nextHovering = hit.collider.gameObject.GetComponent<IInteractable>();

            Debug.DrawLine(ray.origin, hit.point, Color.blue);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * interact_maxDistance, Color.red);
        }

        if (currentInteract == nextHovering)
            return;

        if (currentInteract != null)
            currentInteract.UnHovering();

        currentInteract = nextHovering;

        if(currentInteract != null)
            currentInteract.Hovering();

    }

#if ENABLE_INPUT_SYSTEM
    private void OnInteract()
    {
        if (_input.jump || _input.attack || isInteracting)
            return;

        OnRayFire();
    }

    private void OnAttack()
    {
        if (_input.jump || isAttacking || isInteracting)
            return;

        AttackOverlap();
    }
#endif


    private void OnRayFire()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        isInteracting = true;

        Vector2 screentCenter = new(Screen.width * 0.5f, Screen.height * 0.5f);
        Ray ray = _camera.ScreenPointToRay(screentCenter);

        if (Physics.Raycast(ray, out var hit, r_maxDistance, r_interactableMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"[RayShooter] Ray Hit {hit.collider.name} @ {hit.point}");

            Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }
        else
            Debug.DrawRay(ray.origin, ray.direction * r_maxDistance, Color.yellow, 0.5f);

        StartCoroutine(InteractDelay());
    }

    private IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(3f);

        isInteracting = false;
    }

    Collider[] results = new Collider[3];
    
    private void AttackOverlap()
    {
        isAttacking = true;

        float radius = attackRadius;

        Vector3 center = _attackPosition.position;

        int hitColliders = Physics.OverlapSphereNonAlloc(center, radius, results, r_enemyMask);

        foreach(Collider collider in results)
        {
            if(collider != null)
            {
                Debug.Log($"{collider.gameObject.name}");
                IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
                if(damageable != null)
                    damageable.TakeDamage(atk);

            }
        }

        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.aquamarine;

        Gizmos.DrawWireSphere(_attackPosition.position, attackRadius);
    }

}
