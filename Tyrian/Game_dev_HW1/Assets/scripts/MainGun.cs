using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGun : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab, ship;
    [SerializeField]
    private float _projectileRadius;
    [SerializeField]
    private float _projectileSpeed = 20f;
    [SerializeField] private float _delaySettings;
    // delay from last spawn
    [SerializeField] private CapsuleCollider CapCollider;
    private float _delay;
    private AudioSource source;
    public AudioClip laserSound;
    // Start is called before the first frame update
    void Start()
    {
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        _delay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (EnvironmentProps.Instance.alive == false)
        {
            return;
        }
        _delay -= Time.deltaTime;
        if (_delay > 0.0f || (!Input.GetKey(KeyCode.Space) && Application.platform != RuntimePlatform.Android))
            return;
        _delay = _delaySettings;
        Vector3 shipPos = ship.transform.position;

        var projectileGO = Instantiate(projectilePrefab, shipPos + new Vector3(0, 0, CapCollider.height/2 + _projectileRadius), Quaternion.identity);

        var projectileContr = projectileGO.GetComponent<ProjectileController>();
        if (projectileContr != null)
        {
            projectileContr.Set(_projectileSpeed, _projectileRadius);
            
            source.PlayOneShot(laserSound, 0.5f);
        }
        else
        {
            Debug.LogError("Missing projectileController component");
        }
    }
}
