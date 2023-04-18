using System;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public float timeScale = 1.0f;
    public DateTime currentDate;
    public int currentYear;
    public int currentMonth;
    public int currentDay;
    public int currentHour;
    public int currentMinute;
    public int currentSecond;

    public DateTime startDate;

    private float timeSinceLastUpdate;

    // Initialize the current date and time
    private void Start()
    {
        DateTime mytime = DateTime.Now;
        currentDate = new DateTime(mytime.Year, mytime.Month, mytime.Day,0,0,0);
        startDate = currentDate; 
        currentYear = currentDate.Year;
        currentMonth = currentDate.Month;
        currentDay = currentDate.Day;
        currentHour = currentDate.Hour;
        currentMinute = currentDate.Minute;
        currentSecond = currentDate.Second;
    }

    // Update the current time based on the time scale and the real time elapsed since last update
    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
        float timePerSecond = 1.0f / timeScale;
        while (timeSinceLastUpdate >= timePerSecond)
        {
            timeSinceLastUpdate -= timePerSecond;
            currentDate = currentDate.AddSeconds(1);
            currentYear = currentDate.Year;
            currentMonth = currentDate.Month;
            currentDay = currentDate.Day;
            currentHour = currentDate.Hour;
            currentMinute = currentDate.Minute;
            currentSecond = currentDate.Second;
        }
    }
    
    public int GetDayNumber()
    {
        // Calculate the time difference between the dates
        TimeSpan timeDifference = currentDate - startDate;

        // Get the number of days between the dates
        int daysPassed = timeDifference.Days;
        return daysPassed;
    }
}
