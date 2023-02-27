using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    // reference to prefab
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float delayMin;
    [SerializeField]
    private float delayMax;
    private float _width;
    [SerializeField]
    private uint enemyMax;

    [SerializeField] private float _enemySpeed;
    // delay from last spawn
    private float _delay;
    

    // Start is called before the first frame update
    void Start()
    {
        _delay = 0;
        _width = 1;
        EnvironmentProps.Instance.enemyCount = 0;
    }


    // Update is called once per frame
    void Update()
    {
        //if there is max amount of enemies don't create new ones
        if (EnvironmentProps.Instance.enemyCount >= enemyMax) return;
        // time elapsed from previous frame
        _delay -= Time.deltaTime;
        if (_delay > 0.0f || EnvironmentProps.Instance.killedEnemies >= EnvironmentProps.Instance.MaxEnemies)
            return;
        //choose position for new spawn
        //horizontal
        float _speed = _enemySpeed;
        float x = Random.Range(
            EnvironmentProps.Instance.minX() + _width/2,
            EnvironmentProps.Instance.maxX() - _width/2
        );
        //vertical
        float z = EnvironmentProps.Instance.maxZ() - _width/2;
        //LATER - set speed and size of meteor
        // set new delay for next spawn
        _delay = Random.Range(delayMin, delayMax);
        var enemyGO = Instantiate(enemyPrefab, new Vector3(x, 0, z), Quaternion.identity);

        var enemyContr = enemyGO.GetComponent<EnemyMovement>();
        if (enemyContr != null)
        {
            enemyContr.Set(_speed, _width);
            ++EnvironmentProps.Instance.enemyCount;
        }
        else
        {
            Debug.LogError("Missing EnemyMovement component");
        }
    }
}
