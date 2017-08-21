﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJack.Models;

namespace BlackJack.Core
{
    public class Game
    {

        public IGameState State { get; set; }
        private static readonly List<Card> _decks = new List<Card>();
        private static readonly Random _rnd = new Random();
        public Croupier Croupier { get; } = new Croupier(_rnd.Next(0, 3));
        public Client Client { get; } = new Client();
        public int DecksCount { get; set; }
        public Messages Messages { get; } = new Messages();
        public static Random Rnd => _rnd;
        public static List<Card> Decks => _decks;

        private List<Card> CreateDeck()
        {
            var deck = new List<Card>();
            var valuesIndex = 2;
            var cardsWithNumericNamesCount = 36;
            var cardNameTypesCount = 13;

            var aceSuitIndex = 0;
            var suitIndex = 0;


            for (int i = 0; i < cardNameTypesCount; i++)
            {
                for (int j = 0; j < Enum.GetNames(typeof(SuitEnum)).Length; j++)
                {
                    if (cardsWithNumericNamesCount > deck.Count)
                    {
                        deck.Add(
                       new Card()
                       {
                           Name = valuesIndex.ToString(),
                           Suit = ((SuitEnum)j).ToString(),
                           Value = valuesIndex

                       }
                       );
                        continue;
                    }

                    if (((CardNamesEnum)j) != CardNamesEnum.Ace)
                    {
                        deck.Add(new Card()
                        {
                            Name = ((CardNamesEnum)j).ToString(),
                            Suit = ((SuitEnum)suitIndex).ToString(),
                            Value = 10

                        });
                        continue;
                    }

                    deck.Add(new Card()
                    {
                        Name = CardNamesEnum.Ace.ToString(),
                        Suit = ((SuitEnum)suitIndex).ToString(),
                        Value = 11

                    });

                    suitIndex++;
                }
                valuesIndex++;


            }

            return deck;
        }

        private List<Card> CreateDeckList()
        {
            var deckList = new List<Card>();

            for (int i = 0; i < DecksCount; i++)
            {
                deckList.AddRange(CreateDeck());
            }

            return deckList;
        }

        public Game(IGameState gameState)
        {
            State = gameState;
        }
        public void NewGame1()
        {
            if (_decks.Any())
            {
                _decks.Clear();
                Croupier.CardPool.Clear();
                Client.CardPool.Clear();
            }

            _decks.AddRange(CreateDeckList().OrderBy(d => _rnd.Next()));
            Decks.AddRange(_decks);

        }

        public void Run()
        {

        }



        public void NewGame()
        {
            
            State.NewGame(this);
        }

        public void Start() {State.Start(this); }

        public void TakeCard() { State.TakeCard(this);}



    }
    class NewGame : IGameState
    {
        void IGameState.NewGame(Game game)
        {
            game.State = new StartState();
        }

        public void Start(Game game)
        {
            throw new NotImplementedException();
        }

        public void TakeCard(Game game)
        {
            throw new NotImplementedException();
        }

        public void EndGame(Game game)
        {
            throw new NotImplementedException();
        }
    }

    class StartState:IGameState
    {
        public void NewGame(Game game)
        {
            throw new NotImplementedException();
        }

        public void Start(Game game)
        {
            game.State = new TakeCardState();
        }

        public void TakeCard(Game game)
        {
            throw new NotImplementedException();
        }

        public void EndGame(Game game)
        {
            throw new NotImplementedException();
        }
    }
    class TakeCardState:IGameState
    {
        public void NewGame(Game game)
        {
            throw new NotImplementedException();
        }

        public void Start(Game game)
        {
            throw new NotImplementedException();
        }

      public  void TakeCard(Game game)
      {
          game.State = new EndGameState();
      }

        public void EndGame(Game game)
        {
            throw new NotImplementedException();
        }
    }

    class EndGameState: IGameState
    {
        public void NewGame(Game game)
        {
            throw new NotImplementedException();
        }

        public void Start(Game game)
        {
            throw new NotImplementedException();
        }

        public void TakeCard(Game game)
        {
            throw new NotImplementedException();
        }

        public void EndGame(Game game)
        {
            game.State = new NewGame();
        }
    }
}
