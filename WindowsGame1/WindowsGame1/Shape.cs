/*
 ***********************************************************
 All the code below is created by Anton Yarkov aka OptikLab.
 **************** All rights reserved. 2011 ****************
 ***********************************************************
 */

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugs
{
    /// <summary>
    /// Shapetype.
    /// </summary>
    public enum Shapetype
    {
        /// <summary>
        /// 
        /// </summary>
        Empty,

        /// <summary>
        /// 
        /// </summary>
        Line,

        /// <summary>
        /// 
        /// </summary>
        Square,

        /// <summary>
        /// 
        /// </summary>
        LeftL,

        /// <summary>
        /// 
        /// </summary>
        RightL,

        /// <summary>
        /// 
        /// </summary>
        Pyramide,

        /// <summary>
        /// 
        /// </summary>
        LeftZ,

        /// <summary>
        /// 
        /// </summary>
        RightZ
    }

    /// <summary>
    /// Shape.
    /// </summary>
    internal class Shape
    {
        #region Contructor

        /// <summary>
        /// Creates new empty shape.
        /// </summary>
        public Shape() : this(Shapetype.Empty)
        {
        }

        /// <summary>
        /// Creates new shape of type.
        /// </summary>
        /// <param name="type">Shape type.</param>
        public Shape(Shapetype type)
        {
            Clear();
            _type = type;

            switch (_type)
            {
                case Shapetype.Line:
                    for (int i = 0; i < MAX_DIMENSION; i++)
                        _cells[0][i] = true;
                    break;

                case Shapetype.Square:
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                            _cells[i][j] = true;
                    break;

                case Shapetype.LeftL:
                    for (int i = 0; i < 3; i++)
                        _cells[0][i] = true;
                    _cells[1][2] = true;
                    break;

                case Shapetype.RightL:
                    for (int i = 0; i < 3; i++)
                        _cells[0][i] = true;
                    _cells[1][0] = true;
                    break;

                case Shapetype.Pyramide:
                    for (int i = 0; i < 3; i++)
                        _cells[1][i] = true;
                    _cells[0][1] = true;
                    break;

                case Shapetype.LeftZ:
                    _cells[0][0] = true; _cells[1][0] = true;
                    _cells[1][1] = true; _cells[2][1] = true;
                    break;

                case Shapetype.RightZ:
                    _cells[0][1] = true; _cells[1][0] = true;
                    _cells[1][1] = true; _cells[2][0] = true;
                    break;

                case Shapetype.Empty:
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// Rotates shape.
        /// </summary>
        public void Rotate()
        {
            switch (_type)
            {
                case Shapetype.Line:

                    break;
                case Shapetype.Square:
                    return;
                default:
                    break;
            }

            bool[][] tempShape = new bool[4][];

            // Create temp matrix.
            for (int i = 0; i < MAX_DIMENSION; i++)
            {
                tempShape[i] = new bool[MAX_DIMENSION];

                for (int j = 0; j < MAX_DIMENSION; j++)
                    tempShape[i][j] = false;
            }

            // Rotate to temp.
	        for(int j=3-1 , c=0; j>=0 ; j-- , c++)
		        for(int i=0; i<3; i++)
			        tempShape[c][i]=_cells[i][j];

            Clear();

            // Fill results.
	        for(int f=0; f<3; f++)
		        for(int d=0; d<3; d++)
			        _cells[f][d]=tempShape[f][d];
        }

        /// <summary>
        /// Clears current shape.
        /// </summary>
        public void Clear()
        {
            Debug.Assert(_initialized);

            for (int i = 0; i < MAX_DIMENSION; i++)
                for (int j = 0; j < MAX_DIMENSION; j++)
                    _cells[i][j] = false;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private void _InitializeMatrix()
        {
            for (int i = 0; i < MAX_DIMENSION; i++)
                _cells[i] = new bool[MAX_DIMENSION];

            _initialized = true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void _RotateLine()
        {
            if (_cells[0][0] == true)
            {
                Clear();
                for (int k = 0; k < MAX_DIMENSION; k++)
                    _cells[k][1] = true;
            }
            else
            {
                Clear();
                for (int k = 0; k < MAX_DIMENSION; k++)
                    _cells[0][k] = true;
            }
        }

        #endregion

        #region private fields

        /// <summary>
        /// Matrix dimension.
        /// </summary>
        private const int MAX_DIMENSION = 4;

        #endregion

        #region private fields

        /// <summary>
        /// Cells.
        /// </summary>
        private bool[][] _cells = new bool[4][];

        /// <summary>
        /// Current shape type.
        /// </summary>
        private Shapetype _type;

        /// <summary>
        /// Is initialized.
        /// </summary>
        private bool _initialized = false;

        #endregion
    }
}
