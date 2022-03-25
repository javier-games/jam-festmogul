using UnityEngine;

namespace Game
{
    public static class Utils
    {
        
        public const int AttendantsToMoney = 2;
        

        public static bool MakeTest(int threshold)
        {
            return Random.Range(1, 7) <= threshold;
        }
        
        
    }
}