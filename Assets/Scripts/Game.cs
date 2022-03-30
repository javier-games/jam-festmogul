using System.Collections;
using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public class Game : MonoBehaviour
    {
        [SerializeField] 
        private float delay;

        [SerializeField]
        private TextAsset venuesJson;

        internal static readonly Deck<Venue> VenuesDeck = new(
            (a, b) => a.level.CompareTo(b.level)
        );
        
        internal static readonly Deck<Talent> TalentDeck = new(
            Talent.GetRandomTalent,
            GameDefinitions.TalentDeckCount
        );

        public static Season CurrentSeason;
        
        private List<List<Job>> Jobs = new();
        private List<Player> Players = new();
        private int _firstPlayer;
        private Season _firstSeason;
        private int _yearsCount;

        public IEnumerator Start()
        {
            SetUp();
            var i = 0;
            while (VenuesDeck.CardsAmount > 0)
            {
                Debug.Log($"Round {i}");
                Debug.Log($"Year {_yearsCount} - Season {CurrentSeason}");
                yield return new WaitForSeconds(delay);
            
                Round();
            
                foreach (var player in Players)
                {
                    Debug.Log(player);
                }

                i++;
            }
            
            Debug.Log($"Total Round {i}");
        }


        private void SetUp()
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
            Jobs.Add(new List<Job>());
            for (var i = 0; i < GameDefinitions.VenuesSlots; i++)
            {
                Jobs[0].Add(new VenueAcquisition());
            }
            
            Jobs.Add(new List<Job>());
            for (var i = 0; i < GameDefinitions.TalentSlots.Length; i++)
            {
                for (var j = 0; j < GameDefinitions.TalentSlots[i]; j++)
                {
                    Jobs[1].Add(new TalentAcquisition(i));
                }
            }
            
            Jobs.Add(new List<Job>());
            Jobs[2].Add(new Marketing());
            
            Jobs.Add(new List<Job>());
            Jobs[3].Add(new HumanResources());
            
            Jobs.Add(new List<Job>());
            Jobs[4].Add(new InvestmentAcquisition());
        }

        private void Round()
        {
            // Check year.
            if (CurrentSeason == _firstSeason) { _yearsCount++; }

            // Prepare.
            foreach (var jobCollection in Jobs)
            {
                foreach (var job in jobCollection)
                {
                    job.Prepare();
                }
            }
            
            // Place workers.
            var playerOnTurn = _firstPlayer;
            
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[playerOnTurn];
                
                while (player.WorkersAmount > 0)
                {
                    var jobKind = Random.Range(0, Jobs.Count);
                    var jobIndex = Random.Range(0, Jobs[jobKind].Count);
                    var randomJob = Jobs[jobKind][jobIndex];
                    
                    if (randomJob.HasVacancy(player))
                    {
                        randomJob.PlaceWorker(player);
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
                foreach (var jobKind in Jobs)
                {
                    foreach (var job in jobKind)
                    { 
                        job.RecollectWorkers(player);
                    }
                }
                
                playerOnTurn++;
                CheckIndex(ref playerOnTurn, 0,  Players.Count -1);
            }

            foreach (var player in Players)
            {
                player.PayWorkers();
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