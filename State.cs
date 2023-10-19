using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tic_Tac_Toe.Config;

namespace Tic_Tac_Toe
{
    internal class State
    {
        public space[,] board;
        Config config;
        public State() {
            config = new Config();
            board = new space[config.numberOfColumns, config.numberOfRows];
        }
        public State(State s)
        {
            config = new Config();
            board = new space[config.numberOfColumns, config.numberOfRows];
            for (int i = 0; i < config.numberOfColumns; i++)
                for (int j = 0; j < config.numberOfRows; j++)
                    board[i, j] = s.board[i, j];
        }
    }
}
