using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    [SerializeField] private Texture2D King_White;
    [SerializeField] private Texture2D King_Black;
    [SerializeField] private Texture2D Queen_White;
    [SerializeField] private Texture2D Queen_Black;
    [SerializeField] private Texture2D Bishop_white;
    [SerializeField] private Texture2D Bishop_Black;
    [SerializeField] private Texture2D Pawn_White;
    [SerializeField] private Texture2D Pawn_Black;
    [SerializeField] private Texture2D Rook_White;
    [SerializeField] private Texture2D Rook_Black;
    [SerializeField] private Texture2D Knight_White;
    [SerializeField] private Texture2D Knight_Black;

    public void King(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), King_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), King_White, style);
        }
    }
    public void Queen(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Queen_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Queen_White, style);
        }
    }
    public void Bishop(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Bishop_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Bishop_white, style);
        }
    }
    public void Pawn(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos + 100/10 - offset, yPos - offset, 100, 100), Pawn_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos + 100 / 10 - offset, yPos - offset, 100, 100), Pawn_White, style);
        }
    }
    public void Rook(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Rook_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Rook_White, style);
        } 
    }
    public void Knight(char color, float xPos, float yPos, GUIStyle style, int offset)
    {
        if (color == 'B')
        {
            GUI.Box(new Rect(xPos- offset, yPos - offset, 100, 100), Knight_Black, style);
        }
        else if (color == 'W')
        {
            GUI.Box(new Rect(xPos - offset, yPos - offset, 100, 100), Knight_White, style);
        }
    }
}

