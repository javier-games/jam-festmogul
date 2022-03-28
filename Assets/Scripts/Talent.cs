namespace BoardGame
{
    public class Talent
    {
        public int Level;
        public int Cost;
        public int Interest;
        
        public static Talent GetRandomTalent()
        {
            var random = new System.Random();
            var maxLevel = GameDefinitions.TalentLevels.Length;
            var level =  random.Next(maxLevel - 1);
            var min = GameDefinitions.TalentLevels[level];
            var max = GameDefinitions.TalentLevels[level + 1];
            
            var talent = new Talent
            {
                Level = level,
                Cost = random.Next(min.Cost, max.Cost),
                Interest = random.Next(min.Interest, max.Interest)
            };

            return talent;
        }
    }
}