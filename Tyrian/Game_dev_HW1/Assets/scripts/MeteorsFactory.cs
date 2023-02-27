using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorsFactory : MonoBehaviour
{
    // reference to prefab
    [SerializeField]
    private GameObject meteorPrefab;
    [SerializeField]
    private float delayMin;
    [SerializeField]
    private float delayMax;

    [SerializeField] private float _meteorRadiusMin, _meteorRadiusMax;
    [SerializeField] private float _meteorSpeed;
    // delay from last spawn
    private float _delay;
    

    // Start is called before the first frame update
    void Start()
    {
        _delay = 0;
    }


    // Update is called once per frame
    void Update()
    {
            // time elapsed from previous frame
        _delay -= Time.deltaTime;
        if (_delay > 0.0f || EnvironmentProps.Instance.killedMeteors >= EnvironmentProps.Instance.MaxMeteors)
            return;
        //choose position for new spawn
        //horizontal
        float _radius = Random.Range(_meteorRadiusMin, _meteorRadiusMax);
        float _speed = _meteorSpeed * 0.3f/_radius;
        float x = Random.Range(
            EnvironmentProps.Instance.minX() + _radius,
            EnvironmentProps.Instance.maxX() - _radius
        );
        //vertical
        float z = EnvironmentProps.Instance.maxZ() + _radius;
        //LATER - set speed and size of meteor
        // set new delay for next spawn
        _delay = Random.Range(delayMin, delayMax);
        var meteorGO = Instantiate(meteorPrefab, new Vector3(x, 0, z), Quaternion.identity);

        var meteorContr = meteorGO.GetComponent<MeteorController>();
        if (meteorContr != null)
        {
            meteorContr.Set(_speed, _radius);
        }
        else
        {
            Debug.LogError("Missing MeteorController component");
        }
    }
}
