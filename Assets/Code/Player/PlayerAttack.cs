using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerControls _controls;
    PlayerMovements _movements;
    Animator animator;

    public LayerMask HitableLayers;
    public float HitForce = 100f;
    public float HitRate = .75f;

    float lastHitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _movements = GetComponent<PlayerMovements>();
        animator = transform.Find("Renderer").GetComponent<Animator>();

        _controls = _movements.GetControls;

        _controls.Gameplay.Attack.performed += Attack_performed;
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Time.realtimeSinceStartup > lastHitTime + HitRate)
        {
            lastHitTime = Time.realtimeSinceStartup;

            animator.SetTrigger("Attack");

            var boxCenter = new Vector2(_movements.IsFacingRight ? transform.position.x + 0.33f : transform.position.x - 0.33f, transform.position.y);
            var boxSize = new Vector2(_movements.IsFacingRight ? 0.66f : -0.66f, 1.33f);

            var hitableObjects = Physics2D.OverlapBox(boxCenter, boxSize, 0, HitableLayers);
            if (hitableObjects != null)
            {
                Debug.Log("Can hit");

                var hitDirection = hitableObjects.transform.position - transform.position;

                hitableObjects.attachedRigidbody.AddForce(hitDirection.normalized * HitForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var boxCenter = new Vector3(_movements.IsFacingRight ? transform.position.x + 0.33f : transform.position.x - 0.33f, transform.position.y, 1f);
        var boxSize = new Vector3(_movements.IsFacingRight ? 0.66f : -0.66f, 1.33f, 1f);
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
