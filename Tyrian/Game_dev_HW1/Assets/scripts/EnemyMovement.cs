using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 20.0f;
    private float _width;
    private bool right = true;
    private bool down;
    [SerializeField]
    private Health health;
    [SerializeField]
    private float collisionDamage = 40.0f;
    // Start is called before the first frame update

    public bool EscapedDown(Vector3 pos, float dz)
    {
        return pos.z - dz < EnvironmentProps.Instance.minZ();
    }

    public bool MoveDown(Vector3 pos, float dx) {
        if(pos.x + dx >= EnvironmentProps.Instance.maxX() || pos.x - dx <= EnvironmentProps.Instance.minX())
            return true;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector3 move = new Vector3(_speed * Time.deltaTime, 0, 0);
        //if right is false move to left (-x)
        if(!right){
            move = -move;
        }
        // move enemy to side
        pos += move;
        if(down){
            pos -= new Vector3(0, 0, _width);
            down = false;
        }
        transform.position = EnvironmentProps.Instance.IntoAreaX(pos, _width/2);
        down = MoveDown(transform.position, _width/2);
        if(down){
            right = !right;
        }
        // destroy it on border
        if (EscapedDown(transform.position, _width/2) || EnvironmentProps.Instance.start == true)
        {
            Destroy(this.gameObject);
        }

    }

    public void Set(float speed, float width)
    {
        _speed = speed;
        _width = width;
        transform.localScale = new Vector3(width, width, width);
    }

    void OnDestroy()
    {
        --EnvironmentProps.Instance.enemyCount;
        ++EnvironmentProps.Instance.killedEnemies;
    }

    private void OnCollisionEnter(Collision collision)
    {
        health.DealDamage(collisionDamage);
        //Projectile layer
        if (health.IsAlive() == false && collision.gameObject.layer == 9)
        {
            EnvironmentProps.Instance.AddScore(3);
        }
    }
}
