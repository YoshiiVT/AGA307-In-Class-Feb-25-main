using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticles;

    public int damage;

    void Start()
    {
        Invoke("DestroyProjectile", 3);
    }

    public void DestroyProjectile() 
    {
        Destroy(gameObject);
        GameObject particles = Instantiate(hitParticles, transform.position, transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<Enemy>() != null) //Sanity Backup
                collision.gameObject.GetComponent<Enemy>().Hit(damage);
        }    
        DestroyProjectile(); 
    }

   // private void Explosion()
    
        
      //Instantiate(gameObject);
        

        
    
}
