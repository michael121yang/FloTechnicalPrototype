using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Stats {

    private static int moves, curMoves;

	public static int Moves
    {
        get { return moves; }
        set { moves = value; }
    }

    public static int CurMoves
    {
        get { return curMoves; }
        set { curMoves = value; }
    }
}
