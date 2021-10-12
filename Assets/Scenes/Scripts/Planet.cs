using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class Planet : MonoBehaviour
{
    [SerializeField] 
    private GameObject ship;
    [SerializeField] 
    private int countAttakShips, _maxShips = 300;
    public bool _isSelected;
    public int countOfShips = 0;
    private float _planetRadius;
    [SerializeField] 
    private GameObject sprite, selectLight;
    [SerializeField] 
    private Material playerMaterial, enemyMaterial, neutralMaterial;
    [SerializeField] 
    private TextMeshPro text;
    private GameObject attackPlanet;


    
    public enum Team
    {
        Player,
        Neutral,
        Enemy
    }
    public Team planetTeam;

    void Start()
    {
        float scl = transform.localScale.x;
        _planetRadius = GetComponent<CircleCollider2D>().radius * scl;
        if (countOfShips == 0)
        {
            countOfShips = 50;
            text.text = countOfShips.ToString();
        }
        _isSelected = false;
        SetTeam();
        StartCoroutine(IncreaseShips());
    }
    void SetTeam()
    {
        switch (planetTeam)
        {
            case Team.Player:
                sprite.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case Team.Enemy:
                sprite.GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case Team.Neutral:
                sprite.GetComponent<SpriteRenderer>().color = Color.gray;
                break;
        }
    }
    public void CreatePlayerPlanet()
    {
        countOfShips = 50;
        planetTeam = Team.Player;
        SetTeam();
    }
    public void CreateEnemyPlanet()
    {
        countOfShips = 50;
        planetTeam = Team.Enemy;
        SetTeam();
    }

    public void AttackOfShips()
    {
        countAttakShips = countOfShips / 2;
        countOfShips -= countAttakShips;
        text.text = countOfShips.ToString();
        SpawnShips();


    }
    void SpawnShips()
    {
        List<Vector2> positionList = new List<Vector2>();

        List<Vector2> shipsList = new List<Vector2>();

        for (int i = 0; i < countAttakShips; i++)
        {
            float angle = i * (360f / countAttakShips);
            Vector2 pos2d = transform.position + new Vector3(Mathf.Sin(angle) * _planetRadius, Mathf.Cos(angle) * _planetRadius);

            if (planetTeam != Team.Neutral)
            {
                GameObject newShip = (GameObject)Instantiate(ship, pos2d, ship.transform.rotation);
                
                    newShip.GetComponent<Ship>().SetShip(attackPlanet, this);
                
                
            }    
        }
    }
    public void SelectPlanet()
    {
        _isSelected = !_isSelected;
        selectLight.SetActive(_isSelected);
    }
    IEnumerator IncreaseShips()
    {
        while (planetTeam!=Team.Neutral)
        {
            if (countOfShips < _maxShips)
            {
                countOfShips += 5;
                text.text = countOfShips.ToString();
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void TakeDamage(Ship.Status shipStatus)
    {
        if (countOfShips > 0)
        {
            countOfShips--;
            text.text = countOfShips.ToString();
        }
        else
        {
            if (shipStatus == Ship.Status.Player)
                planetTeam = Team.Player;
            else
            {
                planetTeam = Team.Enemy;
                selectLight.SetActive(false);
            }
                

            SetTeam();
            StartCoroutine(IncreaseShips());
        }
    }
    public void SetPlanetForAttack(GameObject planetForAttack)
    {
        attackPlanet = planetForAttack;
    }
    
    public void TakeComrads()
    {
        countOfShips++;
        text.text = countOfShips.ToString();
    }
}
