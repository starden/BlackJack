﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BlackJack.Core;
using BlackJack.Models;
using static BlackJack.Core.GameStates;

namespace BlackJack
{
    class Program
    {
        private static readonly Game _game = new Game(new NewGameState());
        private static bool _isEnd;
        private static bool _isOverflow;
        private static bool _isBlackJack;
        private static bool _isPush;



        static void Main(string[] args)
        {
            Run();
        }
        private static void CheckRules()
        {
            if (/*_game.Croupier.CheckForOverflow() && */!_game.Client.CheckForOverflow())
            {
                _game.Messages.WinPoints();

                _isOverflow = true; // Need set state\
                return;
            }
            if (!_game.Croupier.CheckForOverflow() && _game.Client.CheckForOverflow())
            {
                _game.Messages.LosePoints();

                _isOverflow = true;
                return;
            }

            if (!_game.Croupier.CheckBlackJack() && _game.Client.CheckBlackJack())
            {
                _game.Messages.WinBlackJack();

                _isBlackJack = true;
                return;
            }
            if (_game.Croupier.CheckBlackJack() && !_game.Client.CheckBlackJack())
            {
                _game.Messages.LoseBlackJack();
                _isBlackJack = true;
                return;
            }
            if (_game.Croupier.CheckBlackJack() && _game.Client.CheckBlackJack())
            {
                _game.Messages.Push();
                _isPush = true;
                return;
            }
        }
        private static void Run()
        {
            _game.Messages.Hello();

            _game.Messages.WriteNewGame();

            while (true)
            {
                var userInput = Console.ReadLine();
                if (userInput == UserCommands.NewGame)
                {
                    _game.NewGame();

                    while (true)
                    {
                        _game.Messages.WriteStart();

                        if (Console.ReadLine() == UserCommands.Start)
                        {
                            do
                            {
                                // Start
                                _isEnd = false;
                                _isOverflow = false;
                                _isBlackJack = false;
                                _isPush = false;
                                _game.Messages.GameStarted();

                                Thread.Sleep(1000);
                                _game.Messages.DistributionCards();

                                Thread.Sleep(1500);

                                _game.Client.TakeCard(_game.Croupier.GiveCards(1).First());
                                _game.Client.TakeCard(_game.Croupier.GiveCards(1).First());
                                _game.Messages.ShowUserStartPool(_game.Client); //TODO: #1


                                _game.Croupier.TakeCard(_game.Croupier.GiveCards(1).First());
                                _game.Croupier.TakeCard(_game.Croupier.GiveCards(1).First());

                                CheckRules();
                                //

                                // TakeCard
                                if ((!_isPush) && (!_isOverflow) && (!_isBlackJack))
                                {

                                    do
                                    {
                                        _game.Messages.AskNeedCard();

                                        switch (Console.ReadLine())
                                        {
                                            case "yes":
                                                _game.Client.TakeCard(_game.Croupier.GiveCards(1).First());
                                                _game.Messages.ShowStatusAfterTakeCard(_game.Client);

                                                CheckRules();
                                                if (_isOverflow)
                                                {
                                                    _isEnd = true;
                                                }

                                                if (_isBlackJack)
                                                {
                                                    _isEnd = true;
                                                }
                                                break;
                                            case "no":
                                                _isEnd = true;
                                                break;
                                            default:
                                                _game.Messages.UnknownCommand();

                                                break;
                                        }

                                    } while (!_isEnd);
                                    _isEnd = false;



                                    if ((!_isPush) && (!_isOverflow) && (!_isBlackJack))
                                    {
                                        _game.Messages.CroupierThink();

                                        Thread.Sleep(2000);
                                        _game.Messages.ShowCardsCountCroupierTake(_game.Croupier);

                                        _game.Messages.Calculating();

                                        Thread.Sleep(2000);
                                        _game.Messages.ShowCroupierStatusAfterTakeCard(_game.Croupier);


                                        CheckRules();

                                        if ((!_isPush) && (!_isOverflow) && (!_isBlackJack))
                                        {

                                            switch (_game.Croupier.ComparePoints(_game.Client.CalculatePoints()))
                                            {
                                                case "WIN!":
                                                    _game.Messages.WinPoints();
                                                    break;
                                                case "LOSE!":
                                                    _game.Messages.LosePoints();
                                                    break;
                                                default:
                                                    _game.Messages.Push();
                                                    break;
                                            }
                                        }
                                    }
                                }
                                //

                                // EndGame
                                do
                                {
                                    _game.Messages.AskOneMoreGame();

                                    switch (Console.ReadLine())
                                    {
                                        case "yes":
                                            _game.NewGame();
                                            _isEnd = true;
                                            break;
                                        case "no":
                                            //isEnd = true;
                                            Environment.Exit(0);
                                            break;
                                        default:
                                            _game.Messages.UnknownCommand();

                                            break;
                                    }

                                } while (!_isEnd);
                                //

                            } while (true);
                        }
                        else
                        { _game.Messages.UnknownCommand(); }

                    }



                }
            }
        }
    }
}
