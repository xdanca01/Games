using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGun : MonoBehaviour
{
    // delay from last shot
    private float _delay, _interval;

    [SerializeField]
    private GameObject prefab;
    private float width = 1f;
    private AudioSource source;
    public AudioClip clip;


    void Start()
    {
        _delay = 0;
        _interval = Random.Range(EnvironmentProps.Instance.ShootingInterval.x, EnvironmentProps.Instance.ShootingInterval.y);
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    void Update()
    {
        _delay += Time.deltaTime;
        
        if (_delay < _interval)
            return;
        //note - with floats parameters, Random.Range() is inclusive...
        //but for integers - check yourself
        //instantiate prefab, with same orientation as gun itself
        Instantiate(prefab, transform.position - new Vector3(0, 0, width), transform.rotation);
        //reset delay
        _delay = 0.0f;
        _interval = Random.Range(EnvironmentProps.Instance.ShootingInterval.x, EnvironmentProps.Instance.ShootingInterval.y);
        source.PlayOneShot(clip, 0.7f);
    }

}
