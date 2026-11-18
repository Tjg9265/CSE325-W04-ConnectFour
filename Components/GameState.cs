
public class GameState
{
    public enum WinState
    {
        NoWinner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    private readonly byte[,] board = new byte[6, 7];

    public byte PlayerTurn { get; private set; } = 1;
    public int CurrentTurn { get; private set; }

    public void ResetBoard()
    {
        Array.Clear(board, 0, board.Length);
        PlayerTurn = 1;
        CurrentTurn = 0;
    }

    public byte PlayPiece(byte col)
    {
        if (col >= 7)
            throw new ArgumentException("Invalid column.");

        if (CheckForWin() != WinState.NoWinner)
            throw new ArgumentException("The game is over.");

        for (int row = 5; row >= 0; row--)
        {
            if (board[row, col] == 0)
            {
                board[row, col] = PlayerTurn;
                CurrentTurn++;
                PlayerTurn = (byte)(PlayerTurn == 1 ? 2 : 1);
                return (byte)row;
            }
        }

        throw new ArgumentException("This column is full.");
    }

    public WinState CheckForWin()
    {
        int[,] directions =
        {
            { 0, 1 },
            { 1, 0 },
            { 1, 1 },
            { 1, -1 }
        };

        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                byte player = board[row, col];

                if (player == 0)
                    continue;

                for (int d = 0; d < 4; d++)
                {
                    int dr = directions[d, 0];
                    int dc = directions[d, 1];
                    bool won = true;

                    for (int i = 1; i < 4; i++)
                    {
                        int r = row + dr * i;
                        int c = col + dc * i;

                        if (r < 0 || r >= 6 ||
                            c < 0 || c >= 7 ||
                            board[r, c] != player)
                        {
                            won = false;
                            break;
                        }
                    }

                    if (won)
                    {
                        return player == 1
                            ? WinState.Player1_Wins
                            : WinState.Player2_Wins;
                    }
                }
            }
        }

        return CurrentTurn == 42
            ? WinState.Tie
            : WinState.NoWinner;
    }
}
