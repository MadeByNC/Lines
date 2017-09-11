﻿using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour {
    [SerializeField] private int requiredTilesInLine = 3;
    private enum tilesCompareState { EMPTY, DIFFERENT, SAME }   //Used to prettier comparison in looking for points
    private ArenaTile[,] _tile;
    private List<ArenaTile> tileList;   //Used to found empty tiles
    private int maxX, maxY;
    
    public ArenaTile[,] tile { get { return _tile; } private set { _tile = value; } }

	void Start () {
        maxX = transform.GetChild(0).childCount;
        maxY = transform.childCount;
        tile = new ArenaTile[maxX, maxY];
        tileList = new List<ArenaTile>();

        for(int y = 0; y < maxY; ++y) {
            for(int x = 0; x < maxX; ++x) {
                tile[x, y] = transform.GetChild(y).GetChild(x).GetComponent<ArenaTile>();
                tileList.Add(tile[x, y]);
            }
        }
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.C)) checkPoints();
    }

    public List<ArenaTile> getEmptyTiles() {
        Debug.Log(string.Format("Found {0} empty tiles.", tileList.FindAll(b => b.empty).Count));

        return tileList.FindAll(b => b.empty);
    }

    public void checkPoints() {
        checkPointsRow();
        checkPointsColumn();
    }

    private void checkPointsRow() {
        for (int y = 0; y < maxY; ++y) {
            int sameColor = 1, start = 0;

            for (int x = 1; x < maxX; ++x) {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x - 1, y]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if(x == maxX - 1) {
                        /*if (sameColor >= requiredTilesInLine) {
                            Debug.Log(string.Format("row: {0} start: {1} end: {2}", y + 1, start + 1, x + 1));
                        }*/
                    }
                }
                else {
                    /*if (sameColor >= requiredTilesInLine) {
                        Debug.Log(string.Format("row: {0} start: {1} end: {2}", y + 1, start + 1, x));
                    }*/

                    sameColor = 1;
                    start = x;
                }
            }
        }
    }

    private void checkPointsColumn() {
        for (int x = 0; x < maxX; ++x) {
            int sameColor = 1, start = 0;

            for (int y = 1; y < maxY; ++y) {
                tilesCompareState compState = compareTiles(tile[x, y], tile[x, y - 1]);

                if (compState == tilesCompareState.SAME) {
                    ++sameColor;

                    if (y == maxY - 1) {
                        /*if (sameColor >= requiredTilesInLine) {
                            Debug.Log(string.Format("column: {0} start: {1} end: {2}", x + 1, start + 1, y + 1));
                        }*/
                    }
                }
                else {
                    /*if (sameColor >= requiredTilesInLine) {
                        Debug.Log(string.Format("column: {0} start: {1} end: {2}", x + 1, start + 1, y));
                    }*/

                    sameColor = 1;
                    start = y;
                }
            }
        }
    }

    private tilesCompareState compareTiles(ArenaTile A, ArenaTile B) {
        if (A.empty || B.empty) return tilesCompareState.EMPTY;
        else if (A.tile.color == B.tile.color) return tilesCompareState.SAME;
        else return tilesCompareState.DIFFERENT;
    }

    /*private void removeRow(int row, int start, int end) {
        Debug.Log(string.Format("Remove row: {0}, start: {1}, end: {2}, total tiles: {3}", row + 1, start + 1, end + 1, (end + 1) - (start + 1)));
    }*/



    /*private void removeColumn(int col, int start, int end) {
        Debug.Log(string.Format("Remove column {0}", col + 1));
    }*/
}
