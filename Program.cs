using System;

class GFG
{
    public static void Main(string[] args)
    {

        int[,] grid = new int[2, 3] {
    { 4,6,3}, { 1,3,2}
    }; // table
        int[] supply
        = new int[2] { 110,70 }; // supply
        int[] demand
        = new int[3] {  110,60,50}; // demand

        int startR = 0; // start row
        int startC = 0; // start col
        int ans = 0;

        // loop runs until it reaches the bottom right
        // corner
        while (startR != grid.GetLength(0)
            && startC != grid.GetLength(1))
        {

            // if demand is greater than supply
            if (supply[startR] <= demand[startC])
            {
                ans += supply[startR]
                * grid[startR, startC];

                // subtract the value of supply from the
                // demand
                demand[startC] -= supply[startR];
                startR++;
            }

            // if supply is greater than demand
            else
            {
                ans += demand[startC]
                * grid[startR, startC];

                // subtract the value of demand from the
                // supply
                supply[startR] -= demand[startC];
                startC++;
            }
        }

        Console.WriteLine(
        "The initial feasible basic solution is "
        + ans);
    }
}

