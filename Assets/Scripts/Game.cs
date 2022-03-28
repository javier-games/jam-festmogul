using System.Collections;
using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private float delay;

        internal static readonly Deck<Venue> VenuesDeck = new(
            Venue.GetRandomVenue,
            GameDefinitions.VenuesDeckCount,
            (a, b) => a.Level.CompareTo(b.Level)
        );
        
        internal static readonly Deck<Talent> TalentDeck = new(
            Talent.GetRandomTalent,
            GameDefinitions.TalentDeckCount
        );

        private static readonly List<Job> Jobs = new();
        private static readonly List<Player> Players = new();
        private static int _firstPlayer;
        private static Season _firstSeason;
        public static Season CurrentSeason;
        private static int _yearsCount;

        public IEnumerator Start()
        {
            SetUp();
            var i = 0;
            while (true)
            {
                Debug.Log($"{i} -------");
                yield return new WaitForSeconds(delay);
            
                Round();
            
                Debug.Log($"Year {_yearsCount} - Season {CurrentSeason}");
                foreach (var player in Players)
                {
                    Debug.Log(player);
                }

                i++;
            }
        }


        private static void SetUp()
        {
            // Seasons.
            
            _firstSeason = (Season) Random.Range(0, 4);
            CurrentSeason = _firstSeason;
            
            // Players Setup. 
            
            Players.Clear();
            var playersCount = Random.Range(
                GameDefinitions.MinPlayers,
                GameDefinitions.MaxPlayers + 1
            );

            for (var i = 0; i < playersCount; i++)
            {
                Players.Add(new Player());
            }
            _firstPlayer = Random.Range(0, Players.Count);

            // Jobs
            
            Jobs.Clear();
            for (var i = 0; i < GameDefinitions.VenuesSlots; i++)
            {
                Jobs.Add(new VenueAcquisition());
            }
            
            for (var i = 0; i < GameDefinitions.TalentSlots.Length; i++)
            {
                for (var j = 0; j < GameDefinitions.TalentSlots[i]; j++)
                {
                    Jobs.Add(new TalentAcquisition(i));
                }
            }
            
            Jobs.Add(new Marketing());
            Jobs.Add(new HumanResources());
            Jobs.Add(new InvestmentAcquisition());
        }

        private static void Round()
        {
            // Check year.
            if (CurrentSeason == _firstSeason) { _yearsCount++; }
            
            // Prepare.
            foreach (var job in Jobs)
            {
                job.Prepare();
            }
            
            // Place workers.
            var playerOnTurn = _firstPlayer;
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[playerOnTurn];
                var shuffleList = Jobs.GetSample(Jobs.Count, true);
                for (var j = 0; j < shuffleList.Length && player.WorkersCount != 0 ; j++)
                {
                    var job = shuffleList[j];
                    if (job.HasVacancy(player)) 
                    { 
                        job.PlaceWorker(player);
                    }
                }

                playerOnTurn++;
                CheckIndex(ref playerOnTurn, 0, Players.Count -1);
            }
            
            // Recollect
            playerOnTurn = _firstPlayer;
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[playerOnTurn];
                foreach (var job in Jobs)
                {
                    job.RecollectWorkers(player);
                }
                //player.PayWorkers();
                
                playerOnTurn++;
                CheckIndex(ref playerOnTurn, 0,  Players.Count -1);
            }

            _firstPlayer--;
            CheckIndex(ref _firstPlayer, 0, Players.Count -1);
            
            var currentSeason = (int) CurrentSeason;
            currentSeason++;
            CheckIndex(ref currentSeason, 0, 3);
            CurrentSeason = (Season) currentSeason;
        }

        private static void CheckIndex(ref int index, int min, int max)
        {
            if (index < 0)
            {
                index = max;
            }

            if (index > max)
            {
                index = min;
            }
        }
    }
}
