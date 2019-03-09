using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalPolicy
{
    class Program
    {
        private static int n = 3, m = 4;

        private static float[,] initialState = new float[,]
        {
            { 0, 0,         0, 0},
            { 0, float.NaN, 0, -1},
            { 0, 0,         0, 1}
        };
        private static bool[,] cannotBeChanged = new bool[,]
        {
            { false, false, false, false},
            { false, true , false, true},
            { false, false, false, true}
        };

        //private static int startX = 0, startY = 0;
        private static float probUp = 0.8f, probDown = 0, probLeft = 0.1f, probRight = 0.1f;
        private static float stepReward = -2;



        private static float epsilon = 0.1f;


        static void Main(string[] args)
        {
            float[,] statePrevious;
            float[,] stateNext = initialState;
            float error = 100;
            while (Math.Abs(error) > epsilon)
            {
                statePrevious = (float[,])stateNext.Clone();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (cannotBeChanged[i, j])
                        {
                            continue;
                        }
                        int iUp = ((i + 1) < n) && !float.IsNaN(statePrevious[i + 1, j]) ? i + 1 : i;
                        int iDown = ((i - 1) >= 0) && !float.IsNaN(statePrevious[i - 1, j]) ? i - 1 : i;
                        int jRight = ((j + 1) < m) && !float.IsNaN(statePrevious[i, j + 1]) ? j + 1 : j;
                        int jLeft = ((j - 1) >= 0) && !float.IsNaN(statePrevious[i, j - 1]) ? j - 1 : j;

                        stateNext[i, j] = stepReward + 
                            Math.Max
                            (
                                Math.Max
                                (
                                    probUp * statePrevious[iUp, j] +
                                    probDown * statePrevious[iDown, j] +
                                    probRight * statePrevious[i, jRight] +
                                    probLeft * statePrevious[i, jLeft],

                                    probUp * statePrevious[i, jRight] +
                                    probDown * statePrevious[i, jLeft] +
                                    probRight * statePrevious[iDown, j] +
                                    probLeft * statePrevious[iUp, j]
                                ),
                                Math.Max
                                (
                                    probUp * statePrevious[iDown, j] +
                                    probDown * statePrevious[iUp, j] +
                                    probRight * statePrevious[i, jLeft] +
                                    probLeft * statePrevious[i, jRight],

                                    probUp * statePrevious[i, jLeft] +
                                    probDown * statePrevious[i, jRight] +
                                    probRight * statePrevious[iUp, j] +
                                    probLeft * statePrevious[iDown, j]
                                )
                            );

                    }
                }

                error = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        if (!float.IsNaN(stateNext[i, j]))
                        {
                            error = Math.Max(error,stateNext[i, j] - statePrevious[i, j]);

                        }
                        //Console.Write(stateNext[i, j] + " ");
                    }
                    //Console.Write("\n");
                }
                //Console.Write("\n");
            }
            for (int i = n - 1; i >= 0; i--)
            {
                for (int j = 0; j < m; j++)
                {
                    if (float.IsNaN(stateNext[i, j]))
                    {
                        Console.Write("  ");
                        continue;
                    }
                    int iUp = ((i + 1) < n) && !float.IsNaN(stateNext[i + 1, j]) ? i + 1 : i;
                    int iDown = ((i - 1) >= 0) && !float.IsNaN(stateNext[i - 1, j]) ? i - 1 : i;
                    int jRight = ((j + 1) < m) && !float.IsNaN(stateNext[i, j + 1]) ? j + 1 : j;
                    int jLeft = ((j - 1) >= 0) && !float.IsNaN(stateNext[i, j - 1]) ? j - 1 : j;

                    float up = probUp * stateNext[iUp, j] +
                                    probDown * stateNext[iDown, j] +
                                    probRight * stateNext[i, jRight] +
                                    probLeft * stateNext[i, jLeft];
                    float down = probUp * stateNext[iDown, j] +
                                    probDown * stateNext[iUp, j] +
                                    probRight * stateNext[i, jLeft] +
                                    probLeft * stateNext[i, jRight];
                    float right = probUp * stateNext[i, jRight] +
                                    probDown * stateNext[i, jLeft] +
                                    probRight * stateNext[iDown, j] +
                                    probLeft * stateNext[iUp, j];
                    float left = probUp * stateNext[i, jLeft] +
                                    probDown * stateNext[i, jRight] +
                                    probRight * stateNext[iUp, j] +
                                    probLeft * stateNext[iDown, j];

                    if (up >= down && up >= right && up >= left)//only if possible to go up
                    {
                        Console.Write("^ ");
                    }
                    else if (down > up && down >= right && down >= left)
                    {
                        Console.Write("v ");
                    }
                    else if (left > down && left >= right && left > up)
                    {
                        Console.Write("< ");
                    }
                    if (right > down && right > up && right > left)
                    {
                        Console.Write("> ");
                    }
                }
                Console.Write("\n");

            }
            Console.Read();
        }
    }
}