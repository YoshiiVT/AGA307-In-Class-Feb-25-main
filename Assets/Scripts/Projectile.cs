using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public GameObject hitParticles;

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
            //Change the colour of the collided object
            collision.gameObject.GetComponentInChildren<Renderer>().material.color = Color.red;
            //Destroy the collision object
            Destroy(collision.gameObject, 1);
        }    
        DestroyProjectile(); 
    }

   // private void Explosion()
    
        
      //Instantiate(gameObject);
        

        
    
}
