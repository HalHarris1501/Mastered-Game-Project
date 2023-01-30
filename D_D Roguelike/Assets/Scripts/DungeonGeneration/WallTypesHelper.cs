using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTypesHelper
{
    public static HashSet<int> wallTop = new HashSet<int>
    {
        0b0010
    };

    public static HashSet<int> wallSideLeft = new HashSet<int>
    {
        0b0100
    };

    public static HashSet<int> wallSideRight = new HashSet<int>
    {
        0b0001
    };

    public static HashSet<int> wallBottom = new HashSet<int>
    {
        0b1000
    };

    public static HashSet<int> wallInnerCornerDownLeft = new HashSet<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b11010001,
        0b01100001,
        0b11010000,
        0b01110001,
        0b00010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001
    };

    public static HashSet<int> wallInnerCornerDownRight = new HashSet<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b01000011,
        0b10000101,
        0b01000100,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010,
        0b10000010

    };

    public static HashSet<int> wallOneWideEdgeRight = new HashSet<int>
    {
        0b1011
    };

    public static HashSet<int> wallOneWideEdgeLeft = new HashSet<int>
    {
        0b1110
    };

    public static HashSet<int> wallOneWideEdgeUp = new HashSet<int>
    {
        0b1101
    };

    public static HashSet<int> wallOneWideEdgeDown = new HashSet<int>
    {
        0b0111
    };

    public static HashSet<int> wallOneWideVertical = new HashSet<int>
    {
        0b0101
    };

    public static HashSet<int> wallOneWideHorizontal = new HashSet<int>
    {
        0b1010
    };

    public static HashSet<int> wallDiagonalCornerDownLeft = new HashSet<int>
    {
        0b01000000
    };

    public static HashSet<int> wallDiagonalCornerDownRight = new HashSet<int>
    {
        0b00000001
    };

    public static HashSet<int> wallDiagonalCornerUpLeft = new HashSet<int>
    {
        0b00010000,
        0b01010000,
    };

    public static HashSet<int> wallDiagonalCornerUpRight = new HashSet<int>
    {
        0b00000100,
        0b00000101
    };

    public static HashSet<int> wallFull = new HashSet<int>
    {
        0b1111
    };

    public static HashSet<int> wallFullEightDirections = new HashSet<int>
    {
        0b10101010,
        0b11101010,
        0b11111010,
        0b11111110,
        0b11111111,
        0b11101010,
        0b10111010,
        0b10101110,
        0b10101011,
        0b10101011,
        0b10101111,
        0b10111111
    };

    public static HashSet<int> wallBottmEightDirections = new HashSet<int>
    {
        0b01000001
    };

    public static HashSet<int> wallInnerCornerUpLeft = new HashSet<int>
    {
        0b01111000,
        0b01111100,
        0b00111000,
        0b00111100,
        0b00110100,
        0b01010100,
        0b01111001,
        0b00110100,
        0b01011000,
        0b01011100,
        0b01101000,
        0b01101100
    };

    public static HashSet<int> wallInnerCornerUpRight = new HashSet<int>
    {
        0b00001110,
        0b00011110,
        0b00001111,
        0b00011111,
        0b00011001,
        0b00010010,
        0b00010101,
        0b00010110,
        0b00001101,
        0b00001001,
        0b00011101,
        0b00001011,
        0b00011011
    };
}