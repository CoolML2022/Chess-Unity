using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chessboard : MonoBehaviour
{
    /////////////////////////////////////////////
    //                                         //
    //   R = Rook   N = Knight   B = Bishop    //
    //   Q = Queen  K = King     P = Pawn      //
    //                                         //
    //   King = 1    Queen = 2    Rook = 3     //
    //   Bishop = 4  Knight = 5   Pawn = 6     //
    //   Black = 0               White = 1     //
    //                                         //
    //   int[,] -> [X-Field, Y-Field] = 10     //
    //   => Black King auf X-Field und Y-Field // 
    //   If nothing on X-Field and Y-Field = 0 //
    /////////////////////////////////////////////
    public bool PlayAsWhite;
    private bool chancePlayer;
    private char PlayerColor;
    private char VPlayerColor;
    private int EnemyColorInt;
    private int PlayerColorInt;
    public Color Color1;
    public Color Color2;
    public int Size = 100;
    private int centerOffsetX;
    private int centerOffsetY;
    public AudioSource MoveCharacterSource;
    public AudioClip MoveCharacter;
    Pieces Figure;
    public string FenString = "/00RB/10NB/20BB/30KB/40QB/50BB/60NB/70RB/01PB/11PB/21PB/31PB/41PB/51PB/61PB/71PB" +
        "/07RW/17NW/27BW/37KW/47QW/57BW/67NW/77RW/06PW/16PW/26PW/36PW/46PW/56PW/66PW/76PW";
    string DeadString = "";
    public Texture test;
    int[,] Fields;
    int[,] PossableFields;
    void Awake()
    {
        PossableFields = new int[8, 8];
        Fields = new int[8, 8];
        selectedFen = new char[5];
        deadFen = new char[5];
        _staticRectTexture = new Texture2D(1, 1);
        _staticRectStyle = new GUIStyle();
        Figure = GetComponent<Pieces>();
        //Chances Values if White or Black
        if (PlayAsWhite)
        {
            PlayerColor = 'W';
            VPlayerColor = 'B';
            EnemyColorInt = 10;
            PlayerColorInt = 11;
        }
        else
        {
            PlayerColor = 'B';
            VPlayerColor = 'W';
            EnemyColorInt = 11;
            PlayerColorInt = 10;
        }
    }
    void Update()
    {

        if (Input.GetMouseButtonUp(0))
        {
            clickCount++;
            print(clickCount);
        }
    }
    public void OnGUI()
    {
        //Chances Values if White or Black if chance Player = true -> Switches from Black to White or White to Black
        if (chancePlayer)
        {
            PlayAsWhite = !PlayAsWhite;
            if (PlayAsWhite)
            {
                PlayerColor = 'W';
                VPlayerColor = 'B';
                EnemyColorInt = 10;
                PlayerColorInt = 11;
            }
            else
            {
                PlayerColor = 'B';
                VPlayerColor = 'W';
                EnemyColorInt = 11;
                PlayerColorInt = 10;
            }
            chancePlayer = false;
        }
        //Calculates centerOffset so that the board is always in the center of the screen
        centerOffsetX = (Screen.width / 2) - this.Size * 4;
        centerOffsetY = (Screen.height / 2) - this.Size * 4;

        //DrawBoard(Color1, Color2, Size);
        CheckMouseRelativeToFigures();
        //DrawFigures(FenString);
        DrawFigureOnMouse();
        float mouseX = Input.mousePosition.x;
        StoreSingleFields(Fields, FenString);
    }

    private static Texture2D _staticRectTexture;
    private static GUIStyle _staticRectStyle;

    public static void GUIDrawRect(int xPos, int yPos, int size, Color color)
    {
        _staticRectTexture.SetPixel(0, 0, color);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
        GUI.Box(new Rect(xPos, yPos, size, size), GUIContent.none, _staticRectStyle);

    }
    public void DrawBoard(Color Color1, Color Color2, int Size)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                bool isSquare = (i + j) % 2 != 0;
                var squareColor = (isSquare) ? this.Color1 : this.Color2;
                GUIDrawRect(j * this.Size + centerOffsetX, i * this.Size + centerOffsetY, this.Size, squareColor);

            }
        }
    }
    public void DrawFigures(string fen)
    {
        Color col = new Color(0, 0, 0, 0);
        _staticRectTexture.SetPixel(0, 0, col);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
        char[] pieces = fen.ToCharArray();
        checkandDrawFigures(pieces, 0, 0, true, 0);
    }
    public float mouseX;
    public float mouseY;
    private bool selected;
    private char[] selectedFen;
    private char[] deadFen;
    private int lastX, lastY;
    private int clickCount = 1;
    public void CheckMouseRelativeToFigures()
    {
        char[] fen = FenString.ToCharArray();
        int XX;
        int YY;
        int fenX;
        int fenY;
        bool clikcedOn = false;
        mouseX = Input.mousePosition.x;
        mouseY = Screen.height - Input.mousePosition.y;
        (XX, YY) = mouseGrid();

        if (Input.GetMouseButtonUp(0) && clickCount == 1)
        {
            // Debug.Log("Hittet Player   PlayerID: " + board[XX, YY] + " at Position: " + XX + " " + YY);

            for (int c = 0; c < fen.Length; c++)
            {
                if (fen[c] == '/')
                {
                    if (fen[c + 4] == PlayerColor)
                    {
                        int.TryParse(fen[c + 1].ToString(), out fenX);
                        int.TryParse(fen[c + 2].ToString(), out fenY);

                        if (fenX == XX && fenY == YY)
                        {
                            //print("Clicked field at x: " + fenX);
                            //print("Clicked field at y: " + fenY);
                            for (int i = 0; i < 5; i++)
                            {
                                selectedFen[i] = fen[c + i];
                                fen[c + i] = '#';
                            }
                            string g = new string(selectedFen);
                            lastX = XX;
                            lastY = YY;
                            //print(g);
                            string s = new string(fen);
                            FenString = s;
                            //print(FenString);
                            selected = true;
                            clikcedOn = true;
                        }

                    }
                }
            }
            if(clikcedOn == false && selected == false)
            {
                print("Nothing clicked on");
                clickCount = 0;
            }
            
        }
        else if (Input.GetMouseButtonUp(0) && clickCount == 2)
        {
            bool allowedToChance = true;
            selected = false;
            if (PossableFields[XX, YY] == 1)
            {

                for (int c = 0; c < fen.Length; c++)
                {
                    if (fen[c] == '/')
                    {
                        int.TryParse(fen[c + 1].ToString(), out fenX);
                        int.TryParse(fen[c + 2].ToString(), out fenY);
                        if (fen[c + 4] == VPlayerColor && fenX == XX && fenY == YY)
                        {
                            //print("Hitted black figure");
                            for (int i = 4; i >= 0; i--)
                            {
                                deadFen[i] = fen[c + i];
                                fen[c + i] = 'D';

                            }
                            string g = new string(deadFen);
                            DeadString += g;
                            print(DeadString);
                            chancePlayer = true;
                        }
                    }
                }
            }
            else
            {
                XX = lastX;
                YY = lastY;
                print("Same position clicked");
                allowedToChance = false;
            }
            for (int c = 0; c < fen.Length; c++)
            {
                if (fen[c] == '#')
                {
                    fen[c] = selectedFen[0];
                    fen[c + 1] = XX.ToString().ToCharArray()[0];
                    fen[c + 2] = YY.ToString().ToCharArray()[0];
                    fen[c + 3] = selectedFen[3];
                    fen[c + 4] = selectedFen[4];
                    string s = new string(fen);
                    FenString = s;
                    if (allowedToChance)
                        chancePlayer = true;

                }
            }

            MoveCharacterSource.PlayOneShot(MoveCharacter);
            clickCount = 0;
        }
        // Moves Figure on Mouse X and Y
        if (selected)
        {

            Color col = new Color(0, 0, 0, 0);
            _staticRectTexture.SetPixel(0, 0, col);
            _staticRectTexture.Apply();
            _staticRectStyle.normal.background = _staticRectTexture;
            if (selectedFen[0] == '/')
            {
                if (selectedFen[3] == 'K')
                {
                    possableKingMoves(lastX, lastY);
                    //Figure.King(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'Q')
                {
                    possabelQueenMoves(lastX, lastY);
                    //Figure.Queen(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'N')
                {
                    posssabelKnightMoves(lastX, lastY);
                    //Figure.Knight(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'P')
                {
                    possablePawnMoves(lastX, lastY);
                    //Figure.Pawn(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'R')
                {
                    possableRookMoves(lastX, lastY);
                    //Figure.Rook(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'B')
                {
                    possableBishopMoves(lastX, lastY);
                    //Figure.Bishop(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
            }
        }
    }

    // clamps mouse X and Y to the field grid
    (int, int) mouseGrid()
    {
        int posX;
        int posY;
        float X;
        float Y;
        X = mouseX - centerOffsetX;
        Y = mouseY - centerOffsetY;
        if (X < 100f)
            X = 0f;
        else if (X > 700f)
            X = 700f;
        if (Y < 100f)
            Y = 0f;
        else if (Y > 700f)
            Y = 700f;
        string g = X.ToString();
        char[] f = g.ToCharArray();
        int.TryParse(f[0].ToString(), out posX);


        //Y += Screen.height;
        if (Y < 100f)
            Y = 0f;

        string gg = Y.ToString();
        char[] ff = gg.ToCharArray();
        int.TryParse(ff[0].ToString(), out posY);
        return (posX, posY);

    }
    //Draws Figures from the Fen String
    public void checkandDrawFigures(char[] pieces, float x, float y, bool normal, int offset)
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == '/')
            {
                if (normal)
                {
                    string X = pieces[i + 1].ToString();
                    string Y = pieces[i + 2].ToString();
                    float.TryParse(X, out x);
                    float.TryParse(Y, out y);
                }
                else
                {
                    centerOffsetX = 0;
                    centerOffsetY = 0;
                }
                if (pieces[i + 3] == 'K')
                {
                    Figure.King(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                if (pieces[i + 3] == 'Q')
                {
                    Figure.Queen(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                if (pieces[i + 3] == 'N')
                {
                    Figure.Knight(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                if (pieces[i + 3] == 'P')
                {
                    Figure.Pawn(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                if (pieces[i + 3] == 'R')
                {
                    Figure.Rook(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                if (pieces[i + 3] == 'B')
                {
                    Figure.Bishop(pieces[i + 4], x * Size + centerOffsetX, y * Size + centerOffsetY, _staticRectStyle, offset);
                }
                i += 4;
            }
        }
    }

    public void DrawFigureOnMouse()
    {
        if (selected)
        {
            if (selectedFen[0] == '/')
            {
                if (selectedFen[3] == 'K')
                {
                    Figure.King(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'Q')
                {
                    Figure.Queen(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'N')
                {
                    Figure.Knight(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'P')
                {
                    Figure.Pawn(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'R')
                {
                    Figure.Rook(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
                if (selectedFen[3] == 'B')
                {
                    Figure.Bishop(selectedFen[4], mouseX, mouseY, _staticRectStyle, 50);
                }
            }
        }
    }
    //
    //Moves that a Pawn can make
    //
    public void possablePawnMoves(int startX, int startY)
    {
        int cC;
        if (PlayAsWhite)
            cC = 1;
        else
            cC = -1;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                PossableFields[i, j] = 0;
            }
        }
        //Draws Selected field on Figures Position
        
        //
        if (startX != 0)
        {
            if (Fields[startX - 1, startY - cC] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY - cC) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY - cC] = 1;
            }
        }
        if (startX != 7)
        {
            if (Fields[startX + 1, startY - cC] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY - cC) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY - cC] = 1;
            }
        }

        if (Fields[startX, startY - cC] == 0)
        {
            GUIDrawRect(startX * 100 + centerOffsetX, (startY - cC) * 100 + centerOffsetY, 100, Color.yellow);
            PossableFields[startX, startY - cC] = 1;
        }
        if ((startY == 6 && cC == 1 && Fields[startX, startY - 1] == 0 && Fields[startX, startY - 2] == 0) || (startY == 1 && cC == -1 && Fields[startX, startY + 1] == 0 && Fields[startX, startY + 2] == 0))
        {
            GUIDrawRect(startX * 100 + centerOffsetX, (startY - cC) * 100 + centerOffsetY, 100, Color.yellow);
            GUIDrawRect(startX * 100 + centerOffsetX, (startY - cC * 2) * 100 + centerOffsetY, 100, Color.yellow);
            PossableFields[startX, startY - cC] = 1;
            PossableFields[startX, startY - cC * 2] = 1;
        }
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);
        PossableFields[startX, startY] = 0;
        Color col = new Color(0, 0, 0, 0);
        _staticRectTexture.SetPixel(0, 0, col);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
    }
    public void possabelQueenMoves(int startX, int startY)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                PossableFields[i, j] = 0;
            }
        }
        
        //Up
        Up(startX, startY);
        //Down
        Down(startX, startY);
        //Right
        Right(startX, startY);
        //Left
        Left(startX, startY);
        //Left Up
        LeftUp(startX, startY);
        //Left Down
        LeftDown(startX, startY);
        //Right Up
        RightUp(startX, startY);
        //Right Down
        RightDown(startX, startY);
        PossableFields[startX, startY] = 0;
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);
        Color col = new Color(0, 0, 0, 0);
        _staticRectTexture.SetPixel(0, 0, col);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
    }

    public void possableBishopMoves(int startX, int startY)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                PossableFields[i, j] = 0;
            }
        }
        //Left Up
        LeftUp(startX, startY);
        //Left Down
        LeftDown(startX, startY);
        //Right Up
        RightUp(startX, startY);
        //Right Down
        RightDown(startX, startY);
        PossableFields[startX, startY] = 0;
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);
        Color col = new Color(0, 0, 0, 0);
        _staticRectTexture.SetPixel(0, 0, col);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
    }
    public void possableRookMoves(int startX, int startY)
    {

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                PossableFields[i, j] = 0;
            }
        }
        
        //Up
        Up(startX, startY);
        //Down
        Down(startX, startY);
        //Right
        Right(startX, startY);
        //Left
        Left(startX, startY);
        PossableFields[startX, startY] = 0;
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);
        Color col = new Color(0, 0, 0, 0);
        _staticRectTexture.SetPixel(0, 0, col);
        _staticRectTexture.Apply();
        _staticRectStyle.normal.background = _staticRectTexture;
    }

    public void posssabelKnightMoves(int startX, int startY)
    {
        //2. Left 1. Up
        if (startX + 2 <= 7 && startY - 1 >= 0)
        {
            if (Fields[startX + 2, startY - 1] == 0 || Fields[startX + 2, startY - 1] == EnemyColorInt)
            {
                GUIDrawRect((startX + 2) * 100 + centerOffsetX, (startY - 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 2, startY - 1] = 1;
            }
        }
        //2. Left 1. Down
        if (startX + 2 <= 7 && startY + 1 <= 7)
        {
            if (Fields[startX + 2, startY + 1] == 0 || Fields[startX + 2, startY + 1] == EnemyColorInt)
            {
                GUIDrawRect((startX + 2) * 100 + centerOffsetX, (startY + 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 2, startY + 1] = 1;
            }
        }
        //2. Right 1. Down
        if (startX - 2 >= 0 && startY + 1 <= 7)
        {
            if (Fields[startX - 2, startY + 1] == 0 || Fields[startX - 2, startY + 1] == EnemyColorInt)
            {
                GUIDrawRect((startX - 2) * 100 + centerOffsetX, (startY + 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 2, startY + 1] = 1;
            }
        }
        //2. Right 1. Up
        if (startX - 2 >= 0 && startY - 1 >= 0)
        {
            if (Fields[startX - 2, startY - 1] == 0 || Fields[startX - 2, startY - 1] == EnemyColorInt)
            {
                GUIDrawRect((startX - 2) * 100 + centerOffsetX, (startY - 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 2, startY - 1] = 1;
            }
        }
        //1. Left 2. Up
        if (startX + 1 <= 7 && startY - 2 >= 0)
        {
            if (Fields[startX + 1, startY - 2] == 0 || Fields[startX + 1, startY - 2] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY - 2) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY - 2] = 1;
            }
        }
        //1. Left 2. Down
        if (startX + 1 <= 7 && startY + 2 <= 7)
        {
            if (Fields[startX + 1, startY + 2] == 0 || Fields[startX + 1, startY + 2] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY + 2) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY + 2] = 1;
            }
        }
        //1. Right 2. Down
        if (startX - 1 >= 0 && startY + 2 <= 7)
        {
            if (Fields[startX - 1, startY + 2] == 0 || Fields[startX - 1, startY + 2] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY + 2) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY + 2] = 1;
            }
        }
        //1. Right 2. Up
        if (startX - 1 >= 0 && startY - 2 >= 0)
        {
            if (Fields[startX - 1, startY - 2] == 0 || Fields[startX - 1, startY - 2] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY - 2) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY - 2] = 1;
            }
        }
        PossableFields[startX, startY] = 0;
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);
    }

    public void possableKingMoves(int startX, int startY)
    {
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.yellow);
        if (startX + 1 <= 7 && startY - 1 >= 0)
        {
            if (Fields[startX + 1, startY - 1] == 0 || Fields[startX + 1, startY - 1] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY - 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY - 1] = 1;
            }
        }
        if (startX + 1 <= 7 && startY + 1 <= 7)
        {
            if (Fields[startX + 1, startY + 1] == 0 || Fields[startX + 1, startY + 1] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY + 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY + 1] = 1;
            }
        }
        if (startX - 1 >= 0 && startY + 1 <= 7)
        {
            if (Fields[startX - 1, startY + 1] == 0 || Fields[startX - 1, startY + 1] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY + 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY + 1] = 1;
            }
        }
        if (startX - 1 >= 0 && startY - 1 >= 0)
        {
            if (Fields[startX - 1, startY - 1] == 0 || Fields[startX - 1, startY - 1] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY - 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY - 1] = 1;
            }
        }
        if (startX - 1 >= 0)
        {
            if (Fields[startX - 1, startY] == 0 || Fields[startX - 1, startY] == EnemyColorInt)
            {
                GUIDrawRect((startX - 1) * 100 + centerOffsetX, (startY) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX - 1, startY] = 1;
            }
        }
        if (startX + 1 <= 7)
        {
            if (Fields[startX + 1, startY] == 0 || Fields[startX + 1, startY] == EnemyColorInt)
            {
                GUIDrawRect((startX + 1) * 100 + centerOffsetX, (startY) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX + 1, startY] = 1;
            }
        }
        if (startY - 1 >= 0)
        {
            if (Fields[startX, startY - 1] == 0 || Fields[startX, startY - 1] == EnemyColorInt)
            {
                GUIDrawRect((startX) * 100 + centerOffsetX, (startY - 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX, startY - 1] = 1;
            }
        }
        if (startY + 1 <= 7)
        {
            if (Fields[startX, startY + 1] == 0 || Fields[startX, startY + 1] == EnemyColorInt)
            {
                GUIDrawRect((startX) * 100 + centerOffsetX, (startY + 1) * 100 + centerOffsetY, 100, Color.yellow);
                PossableFields[startX, startY + 1] = 1;
            }
        }
        PossableFields[startX, startY] = 0;
        GUIDrawRect(startX * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.green);

    }
    public void StoreSingleFields(int[,] fields, string fenString)
    {
        char[] fen = fenString.ToCharArray();
        int x;
        int y;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                fields[i, j] = 0;
            }
        }
        for (int c = 0; c < fen.Length; c++)
        {
            if (fen[c] == '/')
            {
                int.TryParse(fen[c + 1].ToString(), out x);
                int.TryParse(fen[c + 2].ToString(), out y);
                if (fen[c + 4] == 'B')
                {
                    fields[x, y] = 10;
                }
                else if (fen[c + 4] == 'W')
                {
                    fields[x, y] = 11;
                }



            }


        }
        Fields = fields;
    }
    private void Up(int startX, int startY)
    {
        if (startY != 0)
        {
            for (int i = startY; i >= 0; i--)
            {
                if (!(Fields[startX, i] == PlayerColorInt))
                {
                    GUIDrawRect(startX * 100 + centerOffsetX, i * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX, i] = 1;
                    if (Fields[startX, i] == EnemyColorInt)
                    {
                        GUIDrawRect(startX * 100 + centerOffsetX, i * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX, i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void Down(int startX, int startY)
    {
        if (startY != 7)
        {
            for (int i = startY; i < 8; i++)
            {
                if (!(Fields[startX, i] == PlayerColorInt))
                {
                    GUIDrawRect(startX * 100 + centerOffsetX, i * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX, i] = 1;
                    if (Fields[startX, i] == EnemyColorInt)
                    {
                        GUIDrawRect(startX * 100 + centerOffsetX, i * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX, i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void Right(int startX, int startY)
    {
        if (startX != 7)
        {
            for (int i = startX; i < 8; i++)
            {
                if (!(Fields[i, startY] == PlayerColorInt))
                {
                    GUIDrawRect(i * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[i, startY] = 1;
                    if (Fields[i, startY] == EnemyColorInt)
                    {
                        GUIDrawRect(i * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX, i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void Left(int startX, int startY)
    {
        {
            for (int i = startX; i >= 0; i--)
            {
                if (!(Fields[i, startY] == PlayerColorInt))
                {
                    GUIDrawRect(i * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[i, startY] = 1;
                    if (Fields[i, startY] == EnemyColorInt)
                    {
                        GUIDrawRect(i * 100 + centerOffsetX, startY * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[i, startY] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void LeftUp(int startX, int startY)
    {
        if (startX != 0)
        {
            for (int i = 0; i < 8; i++)
            {
                if (startX - i < 0 || startY - i < 0)
                    break;
                if (!(Fields[startX - i, startY - i] == PlayerColorInt))
                {
                    GUIDrawRect((startX - i) * 100 + centerOffsetX, (startY - i) * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX - i, startY - i] = 1;
                    if (Fields[startX - i, startY - i] == EnemyColorInt)
                    {
                        GUIDrawRect((startX - i) * 100 + centerOffsetX, (startY - i) * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX - i, startY - i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void LeftDown(int startX, int startY)
    {
        if (startX != 0)
        {
            for (int i = 0; i < 8; i++)
            {
                if ((startX - i) < 0 || startY + i > 7)
                    break;
                if (!(Fields[startX - i, startY + i] == PlayerColorInt))
                {
                    GUIDrawRect((startX - i) * 100 + centerOffsetX, (startY + i) * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX - i, startY + i] = 1;
                    if (Fields[startX - i, startY + i] == EnemyColorInt)
                    {
                        GUIDrawRect((startX - i) * 100 + centerOffsetX, (startY + i) * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX - i, startY + i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void RightUp(int startX, int startY)
    {
        if (startX != 7)
        {
            for (int i = 0; i < 8; i++)
            {
                if (startX + i > 7 || startY - i < 0)
                    break;
                if (!(Fields[startX + i, startY - i] == PlayerColorInt))
                {
                    GUIDrawRect((startX + i) * 100 + centerOffsetX, (startY - i) * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX + i, startY - i] = 1;
                    if (Fields[startX + i, startY - i] == EnemyColorInt)
                    {
                        GUIDrawRect((startX + i) * 100 + centerOffsetX, (startY - i) * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX + i, startY - i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
    private void RightDown(int startX, int startY)
    {
        if (startX != 7)
        {
            for (int i = 0; i < 8; i++)
            {
                if (startX + i > 7 || startY + i > 7)
                    break;
                if (!(Fields[startX + i, startY + i] == PlayerColorInt))
                {
                    GUIDrawRect((startX + i) * 100 + centerOffsetX, (startY + i) * 100 + centerOffsetY, 100, Color.yellow);
                    PossableFields[startX + i, startY + i] = 1;
                    if (Fields[startX + i, startY + i] == EnemyColorInt)
                    {
                        GUIDrawRect((startX + i) * 100 + centerOffsetX, (startY + i) * 100 + centerOffsetY, 100, Color.yellow);
                        PossableFields[startX + i, startY + i] = 1;
                        break;
                    }
                }
                else
                    break;
            }
        }
    }
}

