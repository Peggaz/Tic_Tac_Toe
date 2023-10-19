using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Expando;
using System.Text;
using System.Threading.Tasks;

namespace Tic_Tac_Toe
{
    internal class Game
    {
        State state;
        Config config;
        public Game() { 
            state = new State();
            config = new Config();
        }

        public void Start()
        {
            while (true)
            {
                PrintBoard();
                if (UserMove())
                {
                    System.Console.WriteLine("Wygral gracz");
                    PrintBoard();
                    break;
                }
                PrintBoard();
                if (AiMove())
                {
                    System.Console.WriteLine("Wygral komputer");
                    PrintBoard();
                    break;
                    
                }
            }
        }
        private bool UserMove()
        {
            // Zdeklaruj zmienne do przechowywania wiersza i kolumny
            int i;
            int j;

            Console.WriteLine("Podaj wiersz: ");
            i = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj kolumne: ");
            j = int.Parse(Console.ReadLine());
            if (state.board[i, j] != Config.space.EMPTY)
            {   
                System.Console.WriteLine("Pole zajęte");
            }
            state.board[i, j] = Config.space.USER;
            if (IsWin(state, Config.space.USER))
            {
                return true;
            }
            return false;
        }
        private bool AiMove()
        {
            int level = 1;
            List<State> states = ExpandStatus(state, level);
            int best = int.MinValue;
            int best_index = -1;

            for(int i = 0; i < states.Count(); i++)
            {
                State s = states[i];
                int value = MinMax(s, level);
                if(value > best)
                {
                    best = value;
                    best_index = i;
                }
            }
            state = states[best_index];
            if(IsWin(state, Config.space.AI))
                return true;
            return false;
        }

        int MinMax(State s, int level)
        {
            if (level >= config.maxLevel) return RatingsState(s);
            int best = 0;
            if (level % 2 == 0)
                best = int.MinValue;
            else 
                best = int.MaxValue;
            List<State> states = ExpandStatus(s, level + 1);
            foreach(State it_state in states)
            {
                int value = MinMax(it_state, level + 1);
                if (level % 2 == 0 && value > best)
                {
                    best = value;
                }
                else if (level % 2 != 0 && value < best)
                {
                    best = value;
                }
            }

            return best;
        }

        int RatingsState(State s)
        {
            if (IsWin(s, Config.space.AI))
                return int.MaxValue;
            else if (IsWin(s, Config.space.USER))
                return int.MinValue;
            int ret = 0;
            int user_line1 = 0;
            int user_line2 = 0;
            int ai_line1 = 0;
            int ai_line2 = 0;
            for (int i = 0; i < config.numberOfRows; i++)
            {
                if (s.board[i, i] == Config.space.AI)
                    ai_line1++;
                else if (s.board[i, i] == Config.space.USER)
                    user_line1++;
                if (s.board[i, (config.numberOfColumns - 1) - i] != Config.space.AI)
                    ai_line2++;
                else if (s.board[i, (config.numberOfColumns - 1) - i] != Config.space.AI)
                    user_line2++;
            }
            if (user_line1 >= config.numberOfColumns/2 && ai_line1 == 0)
                ret += config.numberOfColumns * -10;
            if (user_line2 >= config.numberOfColumns / 2 && ai_line2 == 0)
                ret += config.numberOfColumns * -10; ;
            if (ai_line1 >= config.numberOfColumns / 2 && user_line1 == 0)
                ret += config.numberOfColumns * 10; ;
            if (ai_line2 >= config.numberOfColumns / 2 && user_line2 == 0)
                ret += config.numberOfColumns * 10;

            for (int i = 0; i < config.numberOfColumns; i++)
            {
                for (int j = 0; j < config.numberOfRows; j++)
                {
                    if (s.board[i, j] == Config.space.USER)
                        ret += -10;
                    else if (s.board[i, j] == Config.space.AI)
                        ret += 10;
                    if (s.board[j, i] != Config.space.USER)
                        ret += -10;
                    else if (s.board[j, i] != Config.space.AI)
                        ret += 10;
                }
            }
            return ret;
        }

        private List<State> ExpandStatus(State s, int level)
        {
            List<State> states = new List<State>();
            for (int i = 0; i < config.numberOfColumns; i++) {
                for (int j = 0; j < config.numberOfRows; j++)
                {
                    if (s.board[i,j] == Config.space.EMPTY)
                    {
                        State newState = new State(s);
                        if (level % 2 == 0) newState.board[i, j] = Config.space.USER;
                        else newState.board[i, j] = Config.space.AI;
                        states.Add(newState);
                    }
                }
            }
            return states;
        }

        private bool IsWin(State s, Config.space move)
        {
            //przękątna
            if (s.board[0, 0] == move || s.board[0, config.numberOfColumns-1] == move)
            {
                bool ok = true;
                bool ok2 = true;
                for (int i = 0; i < config.numberOfRows; i++)
                {
                    if (s.board[i, i] != move)
                        ok = false;
                    if (s.board[i, (config.numberOfColumns - 1) - i] != move)
                        ok2 = false;

                }
                if (ok || ok2)
                    return true;
            }
            
            for (int i = 0; i < config.numberOfColumns; i++)
            {
                bool ok = true;
                bool ok2 = true;
                for (int j = 0; j<  config.numberOfRows; j++)
                {
                    if (s.board[i, j] != move)
                        ok = false;
                    if (s.board[j, i] != move)
                        ok2 = false;
                }
                if (ok || ok2) return true;
            }
            return false;
        }

        private void PrintBoard()
        {
            for (int i = 0; i < config.numberOfColumns; i++)
            {
                System.Console.Write("\n-----------------------\n");
                for (int j = 0; j < config.numberOfRows; j++)
                {
                    switch (state.board[i, j])
                    {
                        case Config.space.AI:
                            System.Console.Write('X');
                            break;
                        case Config.space.USER:
                            System.Console.Write('O');
                            break;
                        default:
                            System.Console.Write(' ');
                            break;
                    }
                    System.Console.Write('|');
                }
            }
            System.Console.Write("\n-----------------------\n");
        }

    }

}
