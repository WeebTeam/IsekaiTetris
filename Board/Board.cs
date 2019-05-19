using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    /// <summary>
    /// class whereby all the information about the blocks are implemented here
    /// </summary>
	public class Board
    {
        public enum BoardPosition
        {
            Left, //player1
            Center, //single player
            Right //player 2
        };
        protected enum FieldState
        {
            Free, //empty?
            Static, //has placed block?
            Dynamic //moving block?
        };

        protected Vector2 _absoluteStartPos = Vector2.Zero; //set start x, y position to draw the board
        protected BoardPosition _boardPosition;
        protected Texture2D _textures;
        protected Rectangle[] _rectangles;
        protected FieldState[,] _boardFields; // field to hold all the pieces etc
        protected Vector2[,,] _figures;
        protected readonly Vector2 _absoluteStartPositionForNewFigure = new Vector2(3, 0);
        protected Vector2 _positionForDynamicFigure;
        protected Vector2[] _dynamicFigure = new Vector2[_blocksCountInFigure];
        protected Random _random = new Random();
        protected int[,] _boardColor;
        protected const int _height = 20;
        protected const int _width = 10;
        protected const int _blocksCountInFigure = 4;
        protected int _dynamicFigureNumber;
        protected int _dynamicFigureModificationNumber;
        protected int _dynamicFigureColor;
        protected bool _blockLine;
        protected bool _showNewBlock;
        protected float _movement;
        protected float _speed;
        protected Queue<int> _nextFigures = new Queue<int>();
        protected Queue<int> _nextFiguresModification = new Queue<int>();

        public virtual BoardPosition BoardPlacement
        {
            set { _boardPosition = value; }
            get { return _boardPosition; }
        }

        public virtual float Movement
        {
            set { _movement = value; }
            get { return _movement; }
        }

        public virtual float Speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        public Board(ref Texture2D textures, Rectangle[] rectangles)
        {
            // Load textures for blocks
            _textures = textures;

            // Rectangles of each figure
            _rectangles = rectangles;

            // Create tetris board
            _boardFields = new FieldState[_width, _height];
            _boardColor = new int[_width, _height];

            #region Creating figures
            // figures[inxed of figure in array, figure's rotation, figure's block number] = Vector2
            // At all figures is 7, every has 4 modifications (for cube all modifications the same)
            // and every figure consists from 4 blocks
            _figures = new Vector2[7, 4, 4];
            // O-figure
            for (int i = 0; i < 4; i++)
            {
                _figures[0, i, 0] = new Vector2(1, 0);
                _figures[0, i, 1] = new Vector2(2, 0);
                _figures[0, i, 2] = new Vector2(1, 1);
                _figures[0, i, 3] = new Vector2(2, 1);
            }
            // I-figures
            for (int i = 0; i < 4; i += 2)
            {
                _figures[1, i, 0] = new Vector2(0, 0);
                _figures[1, i, 1] = new Vector2(1, 0);
                _figures[1, i, 2] = new Vector2(2, 0);
                _figures[1, i, 3] = new Vector2(3, 0);
                _figures[1, i + 1, 0] = new Vector2(1, 0);
                _figures[1, i + 1, 1] = new Vector2(1, 1);
                _figures[1, i + 1, 2] = new Vector2(1, 2);
                _figures[1, i + 1, 3] = new Vector2(1, 3);
            }
            // J-figures
            _figures[2, 0, 0] = new Vector2(0, 0);
            _figures[2, 0, 1] = new Vector2(1, 0);
            _figures[2, 0, 2] = new Vector2(2, 0);
            _figures[2, 0, 3] = new Vector2(2, 1);
            _figures[2, 1, 0] = new Vector2(2, 0);
            _figures[2, 1, 1] = new Vector2(2, 1);
            _figures[2, 1, 2] = new Vector2(1, 2);
            _figures[2, 1, 3] = new Vector2(2, 2);
            _figures[2, 2, 0] = new Vector2(0, 0);
            _figures[2, 2, 1] = new Vector2(0, 1);
            _figures[2, 2, 2] = new Vector2(1, 1);
            _figures[2, 2, 3] = new Vector2(2, 1);
            _figures[2, 3, 0] = new Vector2(1, 0);
            _figures[2, 3, 1] = new Vector2(2, 0);
            _figures[2, 3, 2] = new Vector2(1, 1);
            _figures[2, 3, 3] = new Vector2(1, 2);
            // L-figures
            _figures[3, 0, 0] = new Vector2(0, 0);
            _figures[3, 0, 1] = new Vector2(1, 0);
            _figures[3, 0, 2] = new Vector2(2, 0);
            _figures[3, 0, 3] = new Vector2(0, 1);
            _figures[3, 1, 0] = new Vector2(2, 0);
            _figures[3, 1, 1] = new Vector2(2, 1);
            _figures[3, 1, 2] = new Vector2(1, 0);
            _figures[3, 1, 3] = new Vector2(2, 2);
            _figures[3, 2, 0] = new Vector2(0, 1);
            _figures[3, 2, 1] = new Vector2(1, 1);
            _figures[3, 2, 2] = new Vector2(2, 1);
            _figures[3, 2, 3] = new Vector2(2, 0);
            _figures[3, 3, 0] = new Vector2(1, 0);
            _figures[3, 3, 1] = new Vector2(2, 2);
            _figures[3, 3, 2] = new Vector2(1, 1);
            _figures[3, 3, 3] = new Vector2(1, 2);
            // S-figures
            for (int i = 0; i < 4; i += 2)
            {
                _figures[4, i, 0] = new Vector2(0, 1);
                _figures[4, i, 1] = new Vector2(1, 1);
                _figures[4, i, 2] = new Vector2(1, 0);
                _figures[4, i, 3] = new Vector2(2, 0);
                _figures[4, i + 1, 0] = new Vector2(1, 0);
                _figures[4, i + 1, 1] = new Vector2(1, 1);
                _figures[4, i + 1, 2] = new Vector2(2, 1);
                _figures[4, i + 1, 3] = new Vector2(2, 2);
            }
            // Z-figures
            for (int i = 0; i < 4; i += 2)
            {
                _figures[5, i, 0] = new Vector2(0, 0);
                _figures[5, i, 1] = new Vector2(1, 0);
                _figures[5, i, 2] = new Vector2(1, 1);
                _figures[5, i, 3] = new Vector2(2, 1);
                _figures[5, i + 1, 0] = new Vector2(2, 0);
                _figures[5, i + 1, 1] = new Vector2(1, 1);
                _figures[5, i + 1, 2] = new Vector2(2, 1);
                _figures[5, i + 1, 3] = new Vector2(1, 2);
            }
            // T-figures
            _figures[6, 0, 0] = new Vector2(0, 1);
            _figures[6, 0, 1] = new Vector2(1, 1);
            _figures[6, 0, 2] = new Vector2(2, 1);
            _figures[6, 0, 3] = new Vector2(1, 0);
            _figures[6, 1, 0] = new Vector2(1, 0);
            _figures[6, 1, 1] = new Vector2(1, 1);
            _figures[6, 1, 2] = new Vector2(1, 2);
            _figures[6, 1, 3] = new Vector2(2, 1);
            _figures[6, 2, 0] = new Vector2(0, 0);
            _figures[6, 2, 1] = new Vector2(1, 0);
            _figures[6, 2, 2] = new Vector2(2, 0);
            _figures[6, 2, 3] = new Vector2(1, 1);
            _figures[6, 3, 0] = new Vector2(2, 0);
            _figures[6, 3, 1] = new Vector2(2, 1);
            _figures[6, 3, 2] = new Vector2(2, 2);
            _figures[6, 3, 3] = new Vector2(1, 1);
            #endregion

            _nextFigures.Enqueue(_random.Next(7));
            _nextFigures.Enqueue(_random.Next(7));
            _nextFigures.Enqueue(_random.Next(7));
            _nextFigures.Enqueue(_random.Next(7));

            _nextFiguresModification.Enqueue(_random.Next(4));
            _nextFiguresModification.Enqueue(_random.Next(4));
            _nextFiguresModification.Enqueue(_random.Next(4));
            _nextFiguresModification.Enqueue(_random.Next(4));
        }

        public virtual void Initialize()
        {
            _showNewBlock = true;
            _movement = 0;
            _speed = 0.0167f; //assuming 60fps so 1/60

            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    ClearBoardField(i, j);
        }

        public virtual void FindDynamicFigure()
        {
            int BlockNumberInDynamicFigure = 0;
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    if (_boardFields[i, j] == FieldState.Dynamic)
                        _dynamicFigure[BlockNumberInDynamicFigure++] = new Vector2(i, j);
        }

        /// <summary>
        /// Find, destroy and save lines's count
        /// </summary>
        /// <returns>Number of destoyed lines</returns>
        public virtual int DestroyLines()
        {
            // Find total lines
            int _blockLineCount = 0;
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                    if (_boardFields[i, j] == FieldState.Static)
                        _blockLine = true;
                    else
                    {
                        _blockLine = false;
                        break;
                    }
                //Destroy total lines
                if (_blockLine)
                {
                    // Save number of total lines
                    _blockLineCount++;
                    for (int l = j; l > 0; l--)
                        for (int k = 0; k < _width; k++)
                        {
                            _boardFields[k, l] = _boardFields[k, l - 1];
                            _boardColor[k, l] = _boardColor[k, l - 1];
                        }
                    for (int l = 0; l < _width; l++)
                    {
                        _boardFields[l, 0] = FieldState.Free;
                        _boardColor[l, 0] = -1;
                    }
                }
            }
            return _blockLineCount;
        }

        /// <summary>
        /// Create new shape in the game, if need it
        /// </summary>
        public virtual bool CreateNewFigure()
        {
            if (_showNewBlock)
            {
                // Generate new figure's shape
                _dynamicFigureNumber = _nextFigures.Dequeue();
                _nextFigures.Enqueue(_random.Next(7));

                _dynamicFigureModificationNumber = _nextFiguresModification.Dequeue();
                _nextFiguresModification.Enqueue(_random.Next(4));

                _dynamicFigureColor = _dynamicFigureNumber;

                // Position and coordinates for new dynamic figure
                _positionForDynamicFigure = _absoluteStartPositionForNewFigure;
                for (int i = 0; i < _blocksCountInFigure; i++)
                    _dynamicFigure[i] = _figures[_dynamicFigureNumber, _dynamicFigureModificationNumber, i] +
                    _positionForDynamicFigure;

                if (!DrawFigureOnBoard(_dynamicFigure, _dynamicFigureColor))
                    return false;

                _showNewBlock = false;
            }
            return true;
        }

        protected virtual bool DrawFigureOnBoard(Vector2[] vector, int color)
        {
            if (!TryPlaceFigureOnBoard(vector))
                return false;
            for (int i = 0; i <= vector.GetUpperBound(0); i++)
            {
                _boardFields[(int)vector[i].X, (int)vector[i].Y] = FieldState.Dynamic;
                _boardColor[(int)vector[i].X, (int)vector[i].Y] = color;
            }
            return true;
        }

        // check if figure fits on the current board
        protected virtual bool TryPlaceFigureOnBoard(Vector2[] vector)
        {
            for (int i = 0; i <= vector.GetUpperBound(0); i++)
                if ((vector[i].X < 0) || (vector[i].X >= _width) ||
            (vector[i].Y >= _height))
                    return false;
            for (int i = 0; i <= vector.GetUpperBound(0); i++)
                if (_boardFields[(int)vector[i].X, (int)vector[i].Y] == FieldState.Static)
                    return false;
            return true;
        }

        public virtual void MoveFigureLeft()
        {
            // Sorting blocks fro dynamic figure to correct moving
            SortingVector2(ref _dynamicFigure, true, _dynamicFigure.GetLowerBound(0), _dynamicFigure.GetUpperBound(0));

            // Check colisions
            for (int i = 0; i < _blocksCountInFigure; i++)
            {
                if ((_dynamicFigure[i].X == 0))
                    return;
                if (_boardFields[(int)_dynamicFigure[i].X - 1, (int)_dynamicFigure[i].Y] == FieldState.Static)
                    return;
            }
            // Move figure on board
            for (int i = 0; i < _blocksCountInFigure; i++)
            {
                _boardFields[(int)_dynamicFigure[i].X - 1, (int)_dynamicFigure[i].Y] =
            _boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y];
                _boardColor[(int)_dynamicFigure[i].X - 1, (int)_dynamicFigure[i].Y] =
            _boardColor[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y];
                ClearBoardField((int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y);
                // Change position for blocks in _dynamicFigure
                _dynamicFigure[i].X = _dynamicFigure[i].X - 1;
            }
            // Change position vector
            //if (_positionForDynamicFigure.X > 0)
            _positionForDynamicFigure.X--;
        }

        public virtual void MoveFigureRight()
        {
            // Sorting blocks fro dynamic figure to correct moving
            SortingVector2(ref _dynamicFigure, true, _dynamicFigure.GetLowerBound(0), _dynamicFigure.GetUpperBound(0));

            // Check colisions
            for (int i = 0; i < _blocksCountInFigure; i++)
            {
                if ((_dynamicFigure[i].X == _width - 1))
                    return;
                if (_boardFields[(int)_dynamicFigure[i].X + 1, (int)_dynamicFigure[i].Y] == FieldState.Static)
                    return;
            }
            // Move figure on board
            for (int i = _blocksCountInFigure - 1; i >= 0; i--)
            {
                _boardFields[(int)_dynamicFigure[i].X + 1, (int)_dynamicFigure[i].Y] =
            _boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y];
                _boardColor[(int)_dynamicFigure[i].X + 1, (int)_dynamicFigure[i].Y] =
            _boardColor[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y];
                ClearBoardField((int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y);
                // Change position for blocks in _dynamicFigure
                _dynamicFigure[i].X = _dynamicFigure[i].X + 1;
            }
            // Change position vector
            //if (_positionForDynamicFigure.X < _width - 1)
            _positionForDynamicFigure.X++;
        }

        public virtual bool MoveFigureDown()
        {
            // Sorting blocks for dynamic figure to correct moving
            SortingVector2(ref _dynamicFigure, false, _dynamicFigure.GetLowerBound(0), _dynamicFigure.GetUpperBound(0));

            // Check colisions
            for (int i = 0; i < _blocksCountInFigure; i++) //cycle through every block in a piece (1 piece = 4 blocks)
            {
                if ((_dynamicFigure[i].Y == _height - 1)) //if one block of the piece touches the floor
                {
                    for (int k = 0; k < _blocksCountInFigure; k++)
                        _boardFields[(int)_dynamicFigure[k].X, (int)_dynamicFigure[k].Y] = FieldState.Static; //change this piece to static (count as dropped)
                    _showNewBlock = true;
                    return true; //return true if we collided
                }
                if (_boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y + 1] == FieldState.Static) //if one block of the piece touches a Static field/a placed piece
                {
                    for (int k = 0; k < _blocksCountInFigure; k++)
                        _boardFields[(int)_dynamicFigure[k].X, (int)_dynamicFigure[k].Y] = FieldState.Static; //change this piece to static (count as dropped)
                    _showNewBlock = true;
                    return true; //return true if we collided
                }
            }

            // Move figure on board
            for (int i = _blocksCountInFigure - 1; i >= 0; i--) //cycle through each block
            {
                _boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y + 1] = _boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y]; //copy current block value to next block
                _boardColor[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y + 1] = _boardColor[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y];
                ClearBoardField((int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y); //change previous board location to free (as we moved down alr)
                // Change position for blocks in _dynamicFigure
                _dynamicFigure[i].Y = _dynamicFigure[i].Y + 1; //move down
            }
            // Change position vector
            //if (_positionForDynamicFigure.Y < _height - 1)
            _positionForDynamicFigure.Y++;
            return false; //return false if theres no collision
        }

        public virtual void HardDrop()
        {
            while (true)
            {
                if (MoveFigureDown()) break; //move the piece down until it hits something
            }
        }

        public virtual void RotateFigure()
        {
            // Check colisions for next modification
            Vector2[] TestDynamicFigure = new Vector2[_dynamicFigure.GetUpperBound(0) + 1];
            for (int i = 0; i < _blocksCountInFigure; i++)
                TestDynamicFigure[i] = _figures[_dynamicFigureNumber, (_dynamicFigureModificationNumber + 1) % 4, i] + _positionForDynamicFigure;

            // Make sure that figure can rotate if she stand near left and right borders
            SortingVector2(ref TestDynamicFigure, true, TestDynamicFigure.GetLowerBound(0), TestDynamicFigure.GetUpperBound(0));
            int leftFigureBound;
            int rightFigureBound;
            if ((leftFigureBound = (int)TestDynamicFigure[0].X) < 0)
            {
                //int leftFigureBound = (int)TestDynamicFigure[0].X;
                for (int i = 0; i < _blocksCountInFigure; i++)
                {
                    TestDynamicFigure[i] += new Vector2(0 - leftFigureBound, 0);
                }
                if (TryPlaceFigureOnBoard(TestDynamicFigure))
                    _positionForDynamicFigure +=
            new Vector2(0 - leftFigureBound, 0);
            }
            if ((rightFigureBound = (int)TestDynamicFigure[_blocksCountInFigure - 1].X) >= _width)
            {
                //int rightFigureBound = (int)TestDynamicFigure[_blocksCountInFigure - 1].X;
                for (int i = 0; i < _blocksCountInFigure; i++)
                {
                    TestDynamicFigure[i] -= new Vector2(rightFigureBound - _width + 1, 0);
                }
                if (TryPlaceFigureOnBoard(TestDynamicFigure))
                    _positionForDynamicFigure -=
            new Vector2(rightFigureBound - _width + 1, 0);
            }

            if (TryPlaceFigureOnBoard(TestDynamicFigure))
            {
                _dynamicFigureModificationNumber = (_dynamicFigureModificationNumber + 1) % 4;
                // Clear dynamic fields
                for (int i = 0; i <= _dynamicFigure.GetUpperBound(0); i++)
                    ClearBoardField((int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y);
                _dynamicFigure = TestDynamicFigure;
                for (int i = 0; i <= _dynamicFigure.GetUpperBound(0); i++)
                {
                    _boardFields[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y] = FieldState.Dynamic;
                    _boardColor[(int)_dynamicFigure[i].X, (int)_dynamicFigure[i].Y] = _dynamicFigureColor;
                }
            }
        }

        public virtual void SortingVector2(ref Vector2[] vector, bool sortByX, int a, int b)
        {
            if (a >= b)
                return;
            int i = a;
            for (int j = a; j <= b; j++)
            {
                if (sortByX)
                {
                    if (vector[j].X <= vector[b].X)
                    {
                        Vector2 tempVector = vector[i];
                        vector[i] = vector[j];
                        vector[j] = tempVector;
                        i++;
                    }
                }
                else
                {
                    if (vector[j].Y <= vector[b].Y)
                    {
                        Vector2 tempVector = vector[i];
                        vector[i] = vector[j];
                        vector[j] = tempVector;
                        i++;
                    }
                }
            }
            int c = i - 1;
            SortingVector2(ref vector, sortByX, a, c - 1);
            SortingVector2(ref vector, sortByX, c + 1, b);
        }

        protected virtual void ClearBoardField(int i, int j)
        {
            _boardFields[i, j] = FieldState.Free;
            _boardColor[i, j] = -1;
        }


        public virtual void Skill()
        {

        }

        public virtual void Draw(SpriteBatch sBatch)
        {
            Vector2 relativeStartPos; //start position to draw stuff (relative to 0,0 of board) (idk board resolution tho)
            // Cycle through the board, print em all
            for (int i = 0; i < _width; i++)
                for (int j = 0; j < _height; j++)
                    if (_boardFields[i, j] != FieldState.Free) //draw ze board
                    {
                        //                             *24px                             *24px
                        relativeStartPos = new Vector2((1 + i) * _rectangles[0].Width, (5 + j) * _rectangles[0].Height); //rectangles[0] = area of a block(of a piece) (24x24px)
                        sBatch.Draw(_textures, _absoluteStartPos + relativeStartPos, _rectangles[_boardColor[i, j]], Color.White);
                    }

            // Draw next figures (previews)
            Queue<int>.Enumerator figure = _nextFigures.GetEnumerator();
            Queue<int>.Enumerator modification = _nextFiguresModification.GetEnumerator();
            for (int i = 0; i < _nextFigures.Count; i++)
            {
                figure.MoveNext();
                modification.MoveNext();
                for (int j = 0; j < _blocksCountInFigure; j++) //draw previews
                {
                    //                                      new Vector2(x, y + 5 * i) as left starts with 24* blah 1x = 24, 1y=24px
                    relativeStartPos = _rectangles[0].Height * (new Vector2(15, 5 + 5 * i) + _figures[figure.Current, modification.Current, j]); //todo, figure out how to make this relative
                    sBatch.Draw(_textures, _absoluteStartPos + relativeStartPos, _rectangles[figure.Current], Color.White);
                }
            }
        }
    }
}