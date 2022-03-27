    using UnityEngine;
namespace BoardGame
{
    public class Talent
    {
        public int Level;
        public int Cost;
        public int Interest;
        
        public static Talent GetRandomTalent()
        {
            var maxLevel = GameDefinitions.TalentLevels.Length;
            var level = Random.Range(0, maxLevel);
            var min = GameDefinitions.TalentLevels[level];
            var max = GameDefinitions.TalentLevels[level + 1];
            
            var talent = new Talent
            {
                Level = level,
                Cost = Random.Range(min.Cost, max.Cost),
                Interest = Random.Range(min.Interest, max.Interest)
            };

            return talent;
        }
    }
}