using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float speed = 10.0f;
    private Vector3 velocity;

    public static Projectile Instantiate(Vector3 pos, Vector3 gunVelocity, Vector3 gutUnitAimingDir, GameObject Prefab)
    {
        // First we create a default sphere GO.
        GameObject projectile = Instantiate(Prefab);
        //GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //Destroy(projectile.GetComponent<SphereCollider>()); // We are not concerned
                                                        // with collisions in this tutorial.
                                                        // Next we add the Projectile component.
        Projectile self = projectile.AddComponent<Projectile>();
        // And we set the position and velocity
        self.transform.position = pos;
        self.velocity = gunVelocity + speed * gutUnitAimingDir;
        return self;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        if (EnvironmentProps.Instance.isOutsideGameArea(transform.position))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
