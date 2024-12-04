using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{ 
    enum JewelKind
    {
        Empty,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet
    }
    enum MoveDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    struct Move
    {
        public int x;
        public int y;
        public MoveDirection direction;
    }
    int GetWidth() { return 8; }
    int GetHeight() { return 8; }
    JewelKind GetJewel(int x, int y) { return JewelKind.Empty; }
    void SetJewel(int x, int y, JewelKind kind) { }

    /*
     STEPS TO TAKE:
     - Iterate through all jewels in the board, row by row
     - Check jewels below for all rows other than the last three, then jewels to left and right of jewel's new position for 'down' moves:
         - While (currentKind == GetJewel(x, y + currentOffset) && y + currentOffset < GetHeight() - 1)
             - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)
         - While (currentKind == GetJewel(x +/- currentOffset && x +/- currentOffset < GetWidth() - 1 AND >= 0)
             - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)
     
     - Check jewels to the right for all columns other than the last three, then jewels above and below jewel's new position for 'right' moves:
         - While (currentKind == GetJewel(x + currentOffset, y) && x + currentOffset < GetWidth() - 1)
             - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)
         - While (currentKind == GetJewel(x, y +/- currentOffset) && y +/- currentOffset < GetHeight() - 1 AND >= 0)
                 - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)

    - Check jewels in the bottom row for 'up' moves:
        - While (currentKind == GetJewel(x +/- currentOffset, y - 1) && currentOffset < GetWidth() - 1 AND >= 0)
             - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)
    - Check jewels in the rightmost column for 'left' moves:
        - While (currentKind == GetJewel(x - 1, y +/- currentOffset) && currentOffset < GetHeight() - 1 AND >= 0)
             - Increment score counter, update best score and store move details if this move's score is higher AND if current score >= 3 (invalid otherwise for match 3 game)
     */

    Move CalculateBestMoveForBoard()
    {
        Move bestMove = default;
        int currentBestScore = 0;

        // Iterate through elements in row-major order:
        for (int y = 0; y < GetHeight(); ++y)
        {
            for (int x = 0; x < GetWidth(); ++x)
            {
                if (y < GetHeight() - 1)
                    CheckDownMove(x, y, ref currentBestScore, ref bestMove);

                if (x < GetWidth() - 1)
                    CheckRightMove(x, y, ref currentBestScore, ref bestMove);

                if (x == GetWidth() - 1)
                    CheckLeftMove(x, y, ref currentBestScore, ref bestMove);

                if (y == GetHeight() - 1)
                    CheckUpMove(x, y, ref currentBestScore, ref bestMove);
            }
        }
        return bestMove;
    }

    private void CheckDownMove(int x, int y, ref int currentBestScore, ref Move bestMove)
    {
        JewelKind currentKind = GetJewel(x, y);
        int moveScore = 0;

        // Early-out if jewels being swapped are the same, as this move won't score any points:
        if (currentKind == GetJewel(x, y + 1))
            return;

        // Skip checking below jewels if there aren't enough to make a line of three:
        if (y <= GetHeight() - 4)
        {
            int currentOffsetY = 2; // How many positions down the board we're searching for matching jewels
                                    // (starts two positions below the current jewel for a 'down' move)

            // Check jewels below jewel's new position:
            while (y + currentOffsetY < GetHeight() - 1 && currentKind == GetJewel(x, y + currentOffsetY))
            {
                ++currentOffsetY;
                ++moveScore;
            }

            // If this move is valid and gets a higher score than the current best move, mark it as the new best move:
            if (moveScore >= 3 && moveScore > currentBestScore)
            {
                currentBestScore = moveScore;
                bestMove.x = x;
                bestMove.y = y;
                bestMove.direction = MoveDirection.Down;
            }
        }

        // As above, but for checking jewels to the left and right:
        int currentOffsetX = 1;
        moveScore = 0;
        while (x - currentOffsetX >= 0 && currentKind == GetJewel(x - currentOffsetX, y + 1))               // Left
        {
            --currentOffsetX;
            ++moveScore;
        }
        currentOffsetX = 1;
        while (x + currentOffsetX < GetWidth() - 1 && currentKind == GetJewel(x + currentOffsetX, y + 1))   // Right
        {
            ++currentOffsetX;
            ++moveScore;
        }

        // Perform same score check as above to ensure that this move is still regarded as the best:
        if (moveScore >= 3 && moveScore > currentBestScore)
        {
            currentBestScore = moveScore;
            bestMove.x = x;
            bestMove.y = y;
            bestMove.direction = MoveDirection.Down;
        }
    }

    private void CheckRightMove(int x, int y, ref int currentBestScore, ref Move bestMove)
    {
        JewelKind currentKind = GetJewel(x, y);
        int moveScore = 0;

        // Early-out if jewels being swapped are the same, as this move won't score any points:
        if (currentKind == GetJewel(x + 1, y))
            return;

        // Skip checking jewels to the right if there aren't enough to make a line of three:
        if (x <= GetWidth() - 4)
        {
            int currentOffsetX = 2; // How many positions along the board we're searching for matching jewels
                                    // (starts two positions to the right for a 'right' move)

            // Check jewels to the right of jewel's new position:
            while (x + currentOffsetX < GetWidth() - 1 && currentKind == GetJewel(x + currentOffsetX, y))
                ++moveScore;

            // If this move is valid and gets a higher score than the current best move, mark it as the new best move:
            if (moveScore >= 3 && moveScore > currentBestScore)
            {
                currentBestScore = moveScore;
                bestMove.x = x;
                bestMove.y = y;
                bestMove.direction = MoveDirection.Right;
            }
        }

        // As above, but for checking jewels above and below:
        int currentOffsetY = 1;
        moveScore = 0;
        while (y - currentOffsetY >= 0 && currentKind == GetJewel(x + 1, y - currentOffsetY))               // Above
        {
            --currentOffsetY;
            ++moveScore;
        }
        currentOffsetY = 1;
        while (y + currentOffsetY < GetHeight() - 1 && currentKind == GetJewel(x + 1, y + currentOffsetY))   // Below
        {
            ++currentOffsetY;
            ++moveScore;
        }

        // Perform same score check as above to ensure that this move is still regarded as the best:
        if (moveScore >= 3 && moveScore > currentBestScore)
        {
            currentBestScore = moveScore;
            bestMove.x = x;
            bestMove.y = y;
            bestMove.direction = MoveDirection.Right;
        }
    }

    private void CheckUpMove(int x, int y, ref int currentBestScore, ref Move bestMove)
    {
        JewelKind currentKind = GetJewel(x, y);
        int moveScore = 0;

        // Early-out if jewels being swapped are the same, as this move won't score any points:
        if (currentKind == GetJewel(x, y - 1))
            return;

        // Only check jewels to the left and right of the jewel's new position, as vertical
        // lines of jewels have already been checked during the 'down' move checks:
        int currentOffsetX = 1;
        while (x - currentOffsetX >= 0 && currentKind == GetJewel(x - currentOffsetX, y - 1))               // Left
        {
            --currentOffsetX;
            ++moveScore;
        }
        while (x + currentOffsetX < GetWidth() - 1 && currentKind == GetJewel(x + currentOffsetX, y - 1))   // Right
        {
            ++currentOffsetX;
            ++moveScore;
        }
        // If this move is valid and gets a higher score than the current best move, mark it as the new best move:
        if (moveScore >= 3 && moveScore > currentBestScore)
        {
            currentBestScore = moveScore;
            bestMove.x = x;
            bestMove.y = y;
            bestMove.direction = MoveDirection.Up;
        }
    }

    private void CheckLeftMove(int x, int y, ref int currentBestScore, ref Move bestMove)
    {
        JewelKind currentKind = GetJewel(x, y);
        int moveScore = 0;

        // Early-out if jewels being swapped are the same, as this move won't score any points:
        if (currentKind == GetJewel(x - 1, y))
            return;

        // Only check jewels above and below the jewel's new position, as horizontal
        // lines of jewels have already been checked during the 'right' move checks:
        int currentOffsetY = 1;
        while (y - currentOffsetY >= 0 && currentKind == GetJewel(x - 1, y - currentOffsetY))               // Above
        {
            --currentOffsetY;
            ++moveScore;
        }
        while (y + currentOffsetY < GetHeight() - 1 && currentKind == GetJewel(x - 1, y + currentOffsetY))  // Below
        {
            ++currentOffsetY;
            ++moveScore;
        }
        // If this move is valid and gets a higher score than the current best move, mark it as the new best move:
        if (moveScore >= 3 && moveScore > currentBestScore)
        {
            currentBestScore = moveScore;
            bestMove.x = x;
            bestMove.y = y;
            bestMove.direction = MoveDirection.Left;
        }
    }
}
