using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{

    private float _speed;
    private float _radius;
    [SerializeField]
    private Health health;
    [SerializeField]
    private float collisionDamage = 40.0f;
    private AudioSource source;
    public AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // move meteor down
        transform.position += new Vector3(0, 0, -_speed * Time.deltaTime);
        // destroy it on border
        if (EnvironmentProps.Instance.EscapedBelow(transform.position, _radius) || EnvironmentProps.Instance.start == true)
        {
            Destroy(this.gameObject);
        }
    }

    public void Set(float speed, float radius)
    {
        _speed = speed;
        _radius = radius;
        transform.localScale = new Vector3(2 * _radius, 2 * _radius, 2 * _radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        health.DealDamage(collisionDamage);
        //Projectile layer
        if (health.IsAlive() == false && collision.gameObject.layer == 9)
        {
            source.PlayOneShot(clip, 1f);
            EnvironmentProps.Instance.AddScore(5);
            ++EnvironmentProps.Instance.killedMeteors;
        }
    }
}
