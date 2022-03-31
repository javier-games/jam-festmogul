namespace BoardGame
{
    [System.Serializable]
    public class Talent
    {
        public int level;
        public int cost;
        public int interest;
        
        /*public static Talent GetRandomTalent()
        {
            var random = new System.Random();
            var maxLevel = GameDefinitions.TalentLevels.Length;
            var level =  random.Next(maxLevel - 1);
            var min = GameDefinitions.TalentLevels[level];
            var max = GameDefinitions.TalentLevels[level + 1];
            
            var talent = new Talent
            {
                level = level,
                cost = random.Next(min.cost, max.cost),
                interest = random.Next(min.interest, max.interest)
            };

            return talent;
        }*/
    }
}