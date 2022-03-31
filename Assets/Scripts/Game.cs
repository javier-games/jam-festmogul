using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Monogum.BricksBucket.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoardGame
{
    public class Game : MonoSingleton<Game>
    {
        [SerializeField] 
        private float delay;

        [SerializeField]
        private TextAsset venuesJson;

        public readonly Deck<Venue> VenuesDeck = new(
            (a, b) => a.level.CompareTo(b.level)
        );
        
        public readonly Deck<Talent> TalentDeck = new();

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
                Debug.Log($"Venues Deck {VenuesDeck.CardsAmount}");
                Debug.Log($"Talend Deck {TalentDeck.CardsAmount}");
                
                yield return new WaitForSeconds(delay);
                Round();
                i++;
            }
            
            Debug.Log($"Total Round {i}");
        }


        private void SetUp()
        {
            // Seasons.
            _firstSeason = (Season) Random.Range(0, 4);
            CurrentSeason = _firstSeason;
            
            // GetDeck.
            VenuesDeck.AddCardsFromFile(venuesJson);
            //VenuesDeck.AddCardsFromFactory(Venue.GetRandomVenue, GameDefinitions.VenuesDeckCount);
            TalentDeck.AddCardsFromFactory(Talent.GetRandomTalent, GameDefinitions.TalentDeckCount);

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
                for (var j = 0; j < player.TotalWorkersAmount; j++)
                {
                    var foundJob = false;
                    Shuffle(Jobs);
                    for (var k = 0; k < Jobs.Count && !foundJob; k++)
                    {
                        for (int l = 0; l < Jobs[k].Count && !foundJob; l++)
                        {
                            var randomJob = Jobs[k][l];
                            if (!randomJob.HasVacancy(player)) continue;
                            
                            Debug.Log(player.ColoredText($"Placed on {randomJob}"));
                            randomJob.PlaceWorker(player);
                            foundJob = true;
                        }
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
                player.PayWorkers();
                Debug.Log(player);
                
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
        
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static System.Random Local;

        public static System.Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new System.Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }
}