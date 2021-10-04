using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private BeerConfig beerConfig;

    [SerializeField]
    private float startLevelDelay = 1f;


    private int beersPickedUp = 0;
    public int BeersPickedUp { get { return beersPickedUp; } }

    private float beerAmount = 0f;
    private int score = 0;

    public static GameManager Main
    {
        get; private set;
    }

    void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Main = this;
        }
    }

    public void StartGame()
    {
        UIFullscreenFade.main.FadeIn(delegate
        {
            Invoke("StartLevel", startLevelDelay);
        });

        beerAmount = beerConfig.BeerTotal;
    }

    public void StartLevel()
    {
        BiomeManager.main.Begin();
        PowerupManager.main.Begin();
        GenerateRoad.main.Begin();
        MoveAlongRoad.main.SetInitialPosition();
        UIFullscreenFade.main.FadeOut(delegate
        {
            CameraManager.main.SetUp(delegate
            {
                MoveAlongRoad.main.Begin();
                SidewaysMovement.main.Begin();
            });
        });
    }

    public void Update()
    {
        BeerMeter.main.SetAmount(beerAmount / beerConfig.BeerTotal);
    }

    public float Drink()
    {
        beerAmount -= beerConfig.BeerSipAmount;
        int addedScore = (int)(beerConfig.BeerSipScore * Mathf.Lerp(
            beerConfig.MinBeerScoreModifier,
            beerConfig.MaxBeerScoreModifier,
            beerAmount / beerConfig.BeerTotal
        ));
        AddScore(addedScore);
        return beerAmount / beerConfig.BeerTotal;
    }

    public void AddScore(int addedScore)
    {
        ScoreUI.main.AddValueAnimated(addedScore);
        score += addedScore;
    }

    public float Pickup(int beerAdded) {
        beerAmount = Mathf.Min(beerAmount + beerAdded, beerConfig.BeerTotal);
        beersPickedUp++;
        return beerAmount;
    }

    public int GetScore()
    {
        return score;
    }

    // Menu-related functions
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
