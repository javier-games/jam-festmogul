
using System.Collections.Generic;
using Monogum.BricksBucket.Core;
using UnityEngine;

namespace BoardGame
{
    public class Game
    {

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
        private static int firstPlayer = 0;

        public void SetUp()
        {
            Players.Clear();
            var playersCount = Random.Range(
                GameDefinitions.MinPlayers,
                GameDefinitions.MaxPlayers + 1
            );

            for (var i = 0; i < playersCount; i++)
            {
                Players.Add(new Player());
            }
            firstPlayer = Random.Range(0, Players.Count);

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
        }


        public Game()
        {
            // Prepare.
            foreach (var job in Jobs)
            {
                job.Prepare();
            }
            
            // Place workers.
            var playerOnTurn = firstPlayer;
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[playerOnTurn];
                while (player.Workers.Count > 0)
                {
                    var jobIndex = Random.Range(0, Jobs.Count);
                    if (Jobs[jobIndex].HasVacancy(player))
                    {
                        Jobs[jobIndex].PlaceWorker(player);
                    }
                }
                playerOnTurn.Loop(0, Players.Count - 1);
            }
            
            // Recollect
            for (var i = 0; i < Players.Count; i++)
            {
                var player = Players[playerOnTurn];
                foreach (var job in Jobs)
                {
                    job.RecollectWorkers(player);
                }
                playerOnTurn.Loop(0, Players.Count - 1);
            }

            firstPlayer.Loop(0, Players.Count, -1);
            GameDefinitions.NextSeason();
        }

        public void Round()
        {
            
        }
        

    }
}
