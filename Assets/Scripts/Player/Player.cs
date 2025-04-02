using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] int wisdom;
    [SerializeField] int hunger;
    [SerializeField] int endurance;

    public int Wisdom { get { return wisdom; } set { wisdom = value; } }
    public int Endurance { get { return endurance; } set { endurance = value; } }
    public int Hunger { get { return hunger; } set { hunger = value; } } //Using in DisplayStats

    private void InitStats()
    {
        wisdom = 5;
        hunger = 0;
        endurance = 5;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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

    public void DrinkBeer(int wisdom_decrease, int endurance_increase)
    {
        wisdom -= wisdom > 0 ? wisdom_decrease : 0;
        endurance += endurance_increase;
    }
}
