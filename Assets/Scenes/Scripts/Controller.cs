using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private List<Planet> selectedPlanets;
    private RaycastHit2D hit;
    private Ray2D ray;
    private Planet planet;
    private Vector2 lastClickedPlanetPosition;
    private GameObject lastClickedPlanet;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CastRay();
        }
    }
    void ReducingShips()
    {
        foreach (Planet planet in selectedPlanets)
        {
            planet.AttackOfShips();
        }
    }
    void CastRay()
    {
        Vector2 origin = Vector2.zero;
        Vector2 dir = Vector2.zero;
        origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(origin, dir);
        if (hit)
        {
            
            if (hit.transform.GetComponent<Planet>() != null)
            {
                planet = hit.transform.GetComponent<Planet>();
                ClickPlanet(planet);
                lastClickedPlanetPosition = planet.transform.position;
            }
        }
    }

    void ClickPlanet(Planet planet)
    {
        switch (planet.planetTeam)
        {
            case Planet.Team.Enemy:
                ClickOthetPlanet();
                break;
            case Planet.Team.Neutral:
                ClickOthetPlanet();
                break;
            case Planet.Team.Player:
                ClickPlayerPlanet();
                break;
        }
    }

    void ClickPlayerPlanet()
    {
        if (!hit.transform.GetComponent<Planet>()._isSelected)
        {
            selectedPlanets.Add(hit.transform.gameObject.GetComponent<Planet>());
        }
        else
        {
            selectedPlanets.Remove(hit.transform.gameObject.GetComponent<Planet>());
        }

        hit.transform.GetComponent<Planet>().SelectPlanet();
    }

    void ClickOthetPlanet()
    {
        if (selectedPlanets.Count > 0)
        {
            lastClickedPlanet = hit.transform.gameObject;
            foreach (Planet planet in selectedPlanets)
            {
                planet.SetPlanetForAttack(lastClickedPlanet);
            }

            
            ReducingShips();
            foreach (Planet planet in selectedPlanets)
            {
                planet.SelectPlanet();
            }
            selectedPlanets.Clear();
        }
       
    }


    public Vector2 GetLastAttackPosition()
    {
        return lastClickedPlanetPosition;
    }


}
