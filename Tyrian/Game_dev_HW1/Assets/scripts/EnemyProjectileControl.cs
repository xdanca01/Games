using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileControl : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 4.0f;
    private float _radius = 0.25f;
    // Start is called before the first frame update

    public bool EscapedDown(Vector3 pos, float dz)
    {
        return pos.z - dz < EnvironmentProps.Instance.minZ();
    }

    // Update is called once per frame
    void Update()
    {
        // move projectile down
        transform.position -= new Vector3(0, 0, _speed * Time.deltaTime);
        // destroy it on border
        if (EscapedDown(transform.position, _radius) || EnvironmentProps.Instance.start == true)
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
        Destroy(this.gameObject);
    }
}
