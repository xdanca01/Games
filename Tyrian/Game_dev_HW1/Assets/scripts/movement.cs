using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] private CapsuleCollider CapCollider;
    [SerializeField]
    private healthPlayer health;
    [SerializeField]
    private float collisionDamage = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
       // maxX = 
    }

    // Update is called once per frame
    void Update()
    {
        if(EnvironmentProps.Instance.alive == true)
        {
            MovementAndroid();
            if (Application.platform == RuntimePlatform.Android)
            {
                MovementAndroid();
            }
            else
            {
                Movement();
            }
        }
    }

    void Awake()
    {
        //CapCollider = GetComponent<CapsuleCollider>();
    }


    void Movement(){
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.W) && transform.position.y <= 10f){
            
            pos.z += speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S)){
            pos.z -= speed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.A)){
            pos.x -= speed * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.D)){
            pos.x += speed * Time.deltaTime;
        }
        pos = EnvironmentProps.Instance.IntoAreaX(pos, CapCollider.radius);
        pos = EnvironmentProps.Instance.IntoAreaZ(pos, CapCollider.height/2);
        transform.position = pos;
    }

    void MovementAndroid()
    {
        Vector3 pos = transform.position;
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 20.0f));
            pos.x = touchPosition.x;
            pos.z = touchPosition.z;
            pos = EnvironmentProps.Instance.IntoAreaX(pos, CapCollider.radius);
            pos = EnvironmentProps.Instance.IntoAreaZ(pos, CapCollider.height / 2);
            transform.position = pos;
        }
    }

        private void OnCollisionEnter(Collision collision)
    {
        health.DealDamage(collisionDamage);
    }


}
