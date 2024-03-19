using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _lifeTime = 5f;
    private int _damage;
    private Notifications _notifications;

    public void Init(Vector3 velocity, int damage = 0, Notifications notifications = null)
    {
        _damage = damage;

        _rigidbody.velocity = velocity;

        _notifications = notifications;

        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<EnemyCharacter>(out EnemyCharacter enemy))
        {
            enemy.ApplyDamage(_damage);
        }
        else if(collision.collider.tag == "Head")
        {
            collision.collider.GetComponentInParent<EnemyCharacter>().ApplyDamage(_damage * 2);
            _notifications.RunHeadshot();
        }

        Destroy();
    }
}
