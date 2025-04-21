using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;
    public int peopleHelped;
    public int maxPeopleToHelp;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;
        peopleHelped = 0;
        maxPeopleToHelp = 7;
    }
}
