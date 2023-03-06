using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiceRoller 
{
    public static int DiceRoll(int diceMax)
    {
        int rollResult = Random.Range(1, diceMax + 1);
        return rollResult;
    }

    public static int RollMultiple(int diceMax, int numOfDice)
    {
        int rollResult = 0;

        for (int i = 0; i < numOfDice; i++)
        {
            rollResult += DiceRoll(diceMax);
        }

        return rollResult;
    }

    public static int D20Check()
    {
        int rollResult = DiceRoll(20);
        return rollResult;
    }

    public static int D20WithAdvantage()
    {
        int rollResult;
        int firstD20 = D20Check();
        int secondD20 = D20Check();

        if (firstD20 > secondD20) rollResult = firstD20;
        else rollResult = secondD20;

        return rollResult;
    }

    public static int D20WithDisadvantage()
    {
        int rollResult;
        int firstD20 = D20Check();
        int secondD20 = D20Check();

        if (firstD20 < secondD20) rollResult = firstD20;
        else rollResult = secondD20;

        return rollResult;
    }
}
