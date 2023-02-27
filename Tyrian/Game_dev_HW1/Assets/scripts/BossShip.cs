using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShip : MonoBehaviour
{
    enum State
    {
        ENTER_GAME_ZONE,
        ATTACK_LONG,
        ATTACK_SHORT,
        ATTACK_POWER,
        RETREAT
    }
    private State activeState = State.ENTER_GAME_ZONE;

    public float minReactionDelay = 0.1f;
    public float maxReactionDelay = 0.2f;
    private float reactionDelay = 0.0f;
    private bool gameZoneEntered = false;
    public int NumShotsToCooldown = 5;
    private int numShots = 0;
    public float maxAccel = 20.0f;
    public float maxSpeed = 5.0f;
    private Vector3 velocity = Vector3.zero;
    public float firePointShiftZ_long = 9.0f;
    public float firePointShiftZ_power = 7.0f;
    public float firePointShiftZ_short = 4.0f;
    private Vector3 enemyPosition = Vector3.zero;
    [SerializeField] private GameObject playerShip;
    [SerializeField] public GameObject bossProjectile;
    public float ReloadSeconds = 0.5f;
    private float reload = 0.0f;
    private float reload_short = 0.0f;
    private float reload_power = 0.0f;
    public float ReloadSeconds_short = 2.0f;
    public float ReloadSeconds_power = 1.5f;
    public float CooldownSeconds = 4.0f;
    public float CooldownSecondsPower = 8.0f;
    private float cooldown;
    private float cooldownPower;
    private Transform gun;
    private bool PowerShot = false;
    private AudioSource source;
    public AudioClip clip;
    [SerializeField] private Health health;
    private float collisionDamage = 40.0f;

    // Start is called before the first frame update
    void Start()
    {
        gun = transform.Find("Gun");
        cooldown = CooldownSeconds;
        cooldownPower = CooldownSecondsPower;
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EnvironmentProps.Instance.start == true)
        {
            reactionDelay -= Time.deltaTime;
            if (reactionDelay <= 0.0f)
            {
                reactionDelay = Random.Range(minReactionDelay, maxReactionDelay);
                ScanEnvironment();
                SelectState();
            }
            ProcessGunTimers();
            switch (activeState)
            {
                case State.ENTER_GAME_ZONE:
                    Process_ENTER_GAME_ZONE();
                    break;
                case State.ATTACK_LONG:
                    Process_ATTACK_LONG();
                    break;
                case State.ATTACK_SHORT:
                    Process_ATTACK_SHORT();
                    break;
                case State.ATTACK_POWER:
                    Process_ATTACK_POWER();
                    break;
                case State.RETREAT:
                    Process_RETREAT();
                    break;
                default: Debug.Assert(false); break;
            }
        }
    }

    private void SelectState()
    {
        if (!gameZoneEntered)
            activeState = State.ENTER_GAME_ZONE;
        else if(PowerShot == true)
            activeState = State.ATTACK_POWER;
        else if (numShots < NumShotsToCooldown)
            activeState = State.ATTACK_LONG;
        else
            activeState = State.ATTACK_SHORT;
    }

    private void Process_ENTER_GAME_ZONE()
    {
        EnvironmentProps env = EnvironmentProps.Instance;
        Vector3 target = new Vector3(0.5f * (env.minX() + env.maxX()),0.0f,env.minZ() + 0.75f * (env.maxZ() - env.minZ()));
        velocity = EnvironmentProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel,target, Time.deltaTime);
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        if ((target - transform.position).sqrMagnitude < 1.0f)
        {
            gameZoneEntered = true;
        }
    }
    private void Process_ATTACK_LONG()
    {
        velocity = EnvironmentProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel,enemyPosition + new Vector3(0, 0, firePointShiftZ_long),Time.deltaTime);
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.positionClippedIntoGameArea(transform.position);
        shoot();
        reload_short = 1.0f;
    }

    private void Process_ATTACK_SHORT()
    {
        velocity = EnvironmentProps.ComputeSeekVelocity(transform.position, velocity, maxSpeed, maxAccel, enemyPosition + new Vector3(0, 0, firePointShiftZ_short), Time.deltaTime);
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.positionClippedIntoGameArea(transform.position);
        shootShort();
        ReloadSeconds = 0.5f;
    }

    private void Process_ATTACK_POWER()
    {
        velocity = EnvironmentProps.ComputeSeekVelocity(transform.position, velocity, maxSpeed, maxAccel, enemyPosition + new Vector3(0, 0, firePointShiftZ_power), Time.deltaTime);
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.positionClippedIntoGameArea(transform.position);
        shootPower();
        ReloadSeconds = 0.5f;
    }


    private void Process_RETREAT()
    {
        Vector3 target = new Vector3(EnvironmentProps.Instance.minX() +(enemyPosition.x < transform.position.x ? 0.8f : 0.2f) *(EnvironmentProps.Instance.maxX() - EnvironmentProps.Instance.minX()),0,EnvironmentProps.Instance.minZ() +0.8f *(EnvironmentProps.Instance.maxZ() - EnvironmentProps.Instance.minZ()));
        velocity = EnvironmentProps.ComputeSeekVelocity(transform.position, velocity,maxSpeed, maxAccel,target, Time.deltaTime);
        transform.position = EnvironmentProps.ComputeEulerStep(transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.positionClippedIntoGameArea(transform.position);
    }

    private void ScanEnvironment()
    {
        enemyPosition = playerShip.transform.position;
    }

    private void ProcessGunTimers()
    {
        if (numShots == NumShotsToCooldown)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                cooldown = CooldownSeconds;
                reload = 0.0f;
                numShots = 0;
            }
        }
        else if (reload > 0.0f)
            reload -= Time.deltaTime;
        if (reload_short > 0.0f)
        {
            reload_short -= Time.deltaTime;
        }
        cooldownPower -= Time.deltaTime;
        if(cooldownPower <= 0.0f)
        {
            PowerShot = true;
            reload_power -= Time.deltaTime;
        }
    }

    private void shoot()
    {
        if (reload <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Projectile.Instantiate(gun.position,horizontalVelocity,Matrix4x4.Rotate(gun.rotation).MultiplyVector(new Vector3(0, 0, 1)), bossProjectile);
            ++numShots;
            reload = ReloadSeconds;
            source.PlayOneShot(clip, 0.7f);
        }
    }

    private void shootShort()
    {
        if (reload_short <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(gun.rotation).MultiplyVector(new Vector3(0, 0, 1)), bossProjectile);
            reload_short = ReloadSeconds_short;
            source.PlayOneShot(clip, 0.7f);
        }
    }

    private void shootPower()
    {
        if(reload_power <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(Quaternion.Euler(0.0f, 45.0f, 0.0f)).MultiplyVector(new Vector3(0, 0, -1)), bossProjectile);
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(Quaternion.Euler(0.0f, 22.5f, 0.0f)).MultiplyVector(new Vector3(0, 0, -1)), bossProjectile);
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(Quaternion.Euler(0.0f, 0.0f, 0.0f)).MultiplyVector(new Vector3(0, 0, -1)), bossProjectile);
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(Quaternion.Euler(0.0f, -22.5f, 0.0f)).MultiplyVector(new Vector3(0, 0, -1)), bossProjectile);
            Projectile.Instantiate(gun.position, horizontalVelocity, Matrix4x4.Rotate(Quaternion.Euler(0.0f, -45.0f, 0.0f)).MultiplyVector(new Vector3(0, 0, -1)), bossProjectile);
            reload_short = ReloadSeconds_short;
            PowerShot = false;
            cooldownPower = CooldownSecondsPower;
            reload_power = ReloadSeconds_power;
            source.PlayOneShot(clip, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        health.DealDamage(collisionDamage);
        //Projectile layer
        if (health.IsAlive() == false && collision.gameObject.layer == 9)
        {
            EnvironmentProps.Instance.AddScore(100);
        }
    }

}
