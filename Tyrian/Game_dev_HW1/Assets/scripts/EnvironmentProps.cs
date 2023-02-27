using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EnvironmentProps : MonoBehaviour
{
    //delay between two shots from of geometry gun
    [SerializeField]
    private Vector2 shootingInterval;
    [SerializeField] private GameObject ScoreUI;
    [SerializeField] private GameObject PanelWonDied;
    [SerializeField] private GameObject Boss;

    public bool alive = true;

    [SerializeField] public int MaxMeteors;
    [SerializeField] public int MaxEnemies;
    public int killedEnemies = 0;
    public int killedMeteors = 0;
    public bool start = false;

    public Vector2 ShootingInterval { get { return shootingInterval; } }
    public static EnvironmentProps Instance { get; private set; }
    public uint enemyCount;
    [SerializeField] float sizeX, sizeZ;
    public float minX() { return -sizeX / 2.0f; }
    public float maxX() { return sizeX / 2.0f; }
    public float minZ() { return -sizeZ / 2.0f; }
    public float maxZ() { return sizeZ / 2.0f; }

    static int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        AddScore(0);
        PanelWonDied.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(killedMeteors >= MaxMeteors && killedEnemies >= MaxEnemies)
        {
            start = true;
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                YouWon();
                StartCoroutine(ExampleCoroutine());
                
            }
            else
            {
                if(Boss == null || Boss.GetComponent<Health>().IsAlive() == false)
                {
                    YouWon();
                }
            }
        }
    }

    void Awake() {
        if(Instance == null){
            Instance = this;
        }
        else if(Instance != this){
            Destroy(this.gameObject);
        }

    }

    public Vector3 IntoAreaX(Vector3 pos, float dx)
    {
        Vector3 result = pos;
        result.x = result.x - dx < minX() ? minX() + dx : result.x;
        result.x = result.x + dx > maxX() ? maxX() - dx : result.x;
        return result;
    }

    public Vector3 IntoAreaZ(Vector3 pos, float dz)
    {
        Vector3 result = pos;
        result.z = result.z - dz < minZ() ? minZ() + dz : result.z;
        result.z = result.z + dz > maxZ() ? maxZ() - dz : result.z;
        return result;
    }

    public bool EscapedBelow(Vector3 pos, float dz)
    {
        return pos.z + dz < minZ();
    }

    public void AddScore(int addition)
    {
        score = Mathf.Max(0, score + addition);
        TextMeshProUGUI text = ScoreUI.GetComponent<TextMeshProUGUI>();
        text.text = score.ToString();
    }

    public void YouWon()
    {
        PanelWonDied.SetActive(true);
        Transform Died = PanelWonDied.transform.GetChild(0);
        Died.gameObject.SetActive(false);
        Transform Won = PanelWonDied.transform.GetChild(1);
        Won.gameObject.SetActive(true);
    }

    public void YouDied()
    {
        PanelWonDied.SetActive(true);
        Transform Died = PanelWonDied.transform.GetChild(0);
        Died.gameObject.SetActive(true);
        Transform Won  = PanelWonDied.transform.GetChild(1);
        Won.gameObject.SetActive(false);
    }

    public Vector3 positionClippedIntoGameArea(Vector3 pos)
    {
        Vector3 result = pos;
        result.x = result.x < minX() ? minX() : result.x;
        result.x = result.x > maxX() ? maxX() : result.x;
        result.z = result.z < minZ() ? minZ() : result.z;
        result.z = result.z > maxZ() ? maxZ() : result.z;
        return result;
    }
    public bool isOutsideGameArea(Vector3 pos)
    {
        return pos.z < minZ() || pos.z > maxZ();
    }

    public static Vector3 ComputeSeekAccel(Vector3 pos, float maxAccel, Vector3 targetPos)
    {
        Vector3 dir = targetPos - pos;
        // NOTE: We add 1e-6f to handle the case when pos == targetPos.
        return (maxAccel / (dir.magnitude + 1e-6f)) * dir;
    }

    public static Vector3 ComputeSeekVelocity(Vector3 pos, Vector3 velocity, float maxSpeed, float maxAccel,Vector3 targetPos, float dt)
    {
        Vector3 seek_accel = ComputeSeekAccel(pos, maxAccel, targetPos);
        velocity = ComputeEulerStep(velocity, seek_accel, dt);
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            velocity *= (maxSpeed / velocity.magnitude);
        return velocity;
    }

    public static Vector3 ComputeEulerStep(Vector3 x0, Vector3 dx_dt, float delta_t)
    {
        return x0 + delta_t * dx_dt;
    }

    IEnumerator ExampleCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
