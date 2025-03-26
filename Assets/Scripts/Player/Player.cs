using UnityEngine;


struct BeerStats {

    public int wisdom_decrease;
    public int endurance_increase;
}

public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int wisdom;
    [SerializeField] int hunger;
    [SerializeField] int endurance;
    [SerializeField] BeerStats beer_stats;

    public int Wisdom { get { return wisdom; } set { wisdom = value; } }
    public int Endurance { get { return endurance; } set { endurance = value; } }

    private void InitBeer()
    {
        beer_stats.wisdom_decrease = 5;
        beer_stats.endurance_increase = 10;
    }

    private void InitStats()
    {
        wisdom = 5;
        hunger = 0;
        endurance = 5;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitBeer();
        InitStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseWisdom(int amount)
    {
        wisdom += amount;
    }

    public void IncreaseEndurance(int amount)
    {
        endurance += amount;
    }

    public void DecreaseWisdom(int amount)
    {
        wisdom -= wisdom > 0 ? amount : 0;
    }

    public void DecreaseEndurance(int amount)
    {
        endurance -= endurance > 0 ? amount : 0;
    }

    public void DrinkBeer()
    {
        wisdom -= wisdom > 0 ? beer_stats.wisdom_decrease : 0;
        endurance += beer_stats.endurance_increase;
    }
}
