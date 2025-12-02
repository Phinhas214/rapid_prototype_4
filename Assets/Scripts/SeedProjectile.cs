using UnityEngine;

public class SeedProjectile : MonoBehaviour
{
    private float speed = 10f;
    private Vector2 direction;
    private float lifetime = 5f;
    private float spawnTime;
    
    void Start()
    {
        spawnTime = Time.time;
    }
    
    public void SetDirection(float angleDegrees, float seedSpeed)
    {
        speed = seedSpeed;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(angleRadians), Mathf.Sin(angleRadians));
    }
    
    void Update()
    {
        // Move in the set direction
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
        
        // Destroy if lifetime exceeded
        if (Time.time - spawnTime > lifetime)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we hit a target
        Target target = other.GetComponent<Target>();
        if (target != null)
        {
            target.OnHit();
            Destroy(gameObject);
        }
    }
    
    void OnBecameInvisible()
    {
        // Destroy when off-screen
        Destroy(gameObject);
    }
}

