using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 1f;
    private bool canDamage = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (canDamage && collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}

