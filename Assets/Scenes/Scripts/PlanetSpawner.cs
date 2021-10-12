using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject planet;
    [SerializeField] 
    private GameObject plane;
    private float _planetRadius;
    [SerializeField] 
    private int countSpawn;
    private Vector2 _minScreenPos;
    private Vector2 _maxScreenpos;
    [SerializeField] 
    private LayerMask _layerMask;
    public List<GameObject> planetList;
    void Awake()
    {
        float scl = planet.transform.localScale.x;
        _planetRadius = planet.GetComponent<CircleCollider2D>().radius * scl;
        MeshCollider mc = plane.GetComponent<MeshCollider>();
        _maxScreenpos.y = mc.bounds.max.y;
        _maxScreenpos.x = mc.bounds.max.x;
        _minScreenPos.y = mc.bounds.min.y;
        _minScreenPos.x = mc.bounds.min.x;
        SpawnPlanets();
    }
   
    void SpawnPlanets()
    {
        float screenX, screenY;
        Vector2 spawnPos;

        for (int i = 0; i < countSpawn; i++)
        {
            screenX = Random.Range(_minScreenPos.x, _maxScreenpos.x);
            screenY = Random.Range(_minScreenPos.y, _maxScreenpos.y);
            spawnPos = new Vector2(screenX, screenY);

            GameObject newPlanet = (GameObject)Instantiate(planet, CheckPosPlanet(spawnPos), planet.transform.rotation);

            if (i < 2)
            {
                if (i == 0)
                    newPlanet.GetComponent<Planet>().CreatePlayerPlanet();
                else
                    newPlanet.GetComponent<Planet>().CreateEnemyPlanet();
            }
            planetList.Add(newPlanet);
        }
    }
    Vector2 CheckPosPlanet(Vector2 pos)
    {
        Vector2 newSpawnPosition = pos;
        bool check = false;

        while (!check)
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(newSpawnPosition, _planetRadius * 4f, _layerMask);
            
            if (collider2D.Length > 0)
            {
                newSpawnPosition = new Vector2(Random.Range(_minScreenPos.x, _maxScreenpos.x),
                                               Random.Range(_minScreenPos.y, _maxScreenpos.y));
            }
            else
            {
                check = true;
                break;
            }
        }
        return newSpawnPosition;
    }
}
