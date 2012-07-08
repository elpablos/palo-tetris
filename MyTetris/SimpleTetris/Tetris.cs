using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Media;

namespace SimpleTetris
{
    /// <summary>
    /// Trida reprezentujici hru tetris.
    /// </summary>
    public class Tetris
    {
        #region Fields

        /// <summary>
        /// Pole vsech tetrisovych prvku a jejich rotaci.
        /// </summary>
        public static readonly int[][][] pieces = new int[][][]
        {
            // typ O
            new int[][] 
            {
                // prvni a jedina rotace
                new int[] 
                { 
                    0,0,0,0,
                    0,1,1,0,
                    0,1,1,0,
                    0,0,0,0
                }
            },
            // typ I
            new int[][]
            {
                // typ I - horizontalne
                new int[]
                {
                    0,0,0,0,
                    2,2,2,2,
                    0,0,0,0,
                    0,0,0,0
                }
                ,
                // typ I - vertikalne
                new int[]
                {
                    0,0,2,0,
                    0,0,2,0,
                    0,0,2,0,
                    0,0,2,0
                }
            },
            // typ S
            new int[][]
            {
                // typ S - horizontalne
                new int[]
                {
                    0,0,0,0,
                    0,0,3,3,
                    0,3,3,0,
                    0,0,0,0
                },
                // typ S - vertikalne
                new int[]
                {
                    0,0,3,0,
                    0,0,3,3,
                    0,0,0,3,
                    0,0,0,0
                }
            },
            // typ Z
            new int[][]
            {
                // typ Z - horizontalne
                new int[]
                {
                    0,0,0,0,
                    0,4,4,0,
                    0,0,4,4,
                    0,0,0,0
                },
                // typ Z - vertikalne
                new int[]
                {
                    0,0,0,4,
                    0,0,4,4,
                    0,0,4,0,
                    0,0,0,0
                }
            },
            // typ L
            new int[][]
            {
                // typ L - horizontalne, packou dolu
                new int[]
                {
                    0,0,0,0,
                    0,5,5,5,
                    0,5,0,0,
                    0,0,0,0
                },
                // typ L - vertikalne, packou doprava
                new int[]
                {
                    0,0,5,0,
                    0,0,5,0,
                    0,0,5,5,
                    0,0,0,0
                },
                // typ L - horizontalne, packou nahoru
                new int[]
                {
                    0,0,0,5,
                    0,5,5,5,
                    0,0,0,0,
                    0,0,0,0
                },
                // typ L - vertikalne, packou doleva
                new int[]
                { 
                    0,5,5,0,
                    0,0,5,0,
                    0,0,5,0,
                    0,0,0,0
                }
            },
            // typ J
            new int[][]
            {
                // typ J - horizontalne, packou dolu
                new int[]
                {
                    0,0,0,0,
                    0,6,6,6,
                    0,0,0,6,
                    0,0,0,0
                },
                // typ J - vertikalne, packou doprava
                new int[]
                {
                    0,0,6,6,
                    0,0,6,0,
                    0,0,6,0,
                    0,0,0,0
                },
                // typ J - horizontalne, packou nahoru
                new int[]
                {
                    0,6,0,0,
                    0,6,6,6,
                    0,0,0,0,
                    0,0,0,0
                },
                // typ J - vertikalne, packou doleva
                new int[]
                {
                    0,0,6,0,
                    0,0,6,0,
                    0,6,6,0,
                    0,0,0,0
                }
            },
            // typ T
            new int[][]
            {
                // typ T - horizontalne, packou dolu
                new int[]
                {
                    0,0,0,0,
                    0,7,7,7,
                    0,0,7,0,
                    0,0,0,0
                },
                // typ T - vertikalne, packou doprava
                new int[]
                {
                    0,0,7,0,
                    0,0,7,7,
                    0,0,7,0,
                    0,0,0,0
                },
                // typ T - horizontalne, packou nahoru
                new int[]
                {
                    0,0,7,0,
                    0,7,7,7,
                    0,0,0,0,
                    0,0,0,0
                },
                // typ T - vertikalne, packou doleva
                new int[]
                {
                    0,0,7,0,
                    0,7,7,0,
                    0,0,7,0,
                    0,0,0,0
                }
            }
        };

        /// <summary>
        /// Pozice X aktivniho prvku.
        /// </summary>
        private int _activeX;

        /// <summary>
        /// Pozice Y aktivniho prvku.
        /// </summary>
        private int _activeY;

        /// <summary>
        /// Rotace aktivniho prvku.
        /// </summary>
        private int _activeR;

        /// <summary>
        /// Reference na generator nahodilosti.
        /// </summary>
        private Random _rand;

        /// <summary>
        /// Casovac.
        /// </summary>
        private Timer _timer;

        #endregion // Fields

        #region Events

        /// <summary>
        /// Udalost, ktera je odpalena kdyz je vyzadovano prekresleni.
        /// </summary>
        public event EventHandler Repaint = delegate { };

        #endregion

        #region Properties

        /// <summary>
        /// Pole.
        /// </summary>
        public int[,] Board { get; private set; }

        /// <summary>
        /// Maximalni sirka pole.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Maximalni vyska pole.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Aktivni prvek.
        /// </summary>
        public int ActivePiece { get; private set; }

        /// <summary>
        /// Pozice X aktivniho prvku.
        /// </summary>
        public int ActiveX { get { return _activeX; } }

        /// <summary>
        /// Pozice Y aktivniho prvku.
        /// </summary>
        public int ActiveY { get { return _activeY; } }

        /// <summary>
        /// Rotace aktivniho prvku.
        /// </summary>
        public int ActiveR { get { return _activeR; } }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Konstruktor inicializujici tetris.
        /// </summary>
        /// <param name="width">maximalni sirka</param>
        /// <param name="height">maximalni vyska</param>
        public Tetris(int width, int height)
        {
            // predame maximalni vysku a sirku pole
            Width = width;
            Height = height;
            // inicializujeme pole
            //Board = new int[Height, Width];
            _rand = new Random();
            // casovac
            _timer = new Timer();
            _timer.Elapsed += Tick;
            // nastaveni pocatecnich hodnot
            Reset();
        }

        #endregion // Constructor

        #region Public methods

        /// <summary>
        /// Metoda, ktera odstartuje hru.
        /// </summary>
        public void Start()
        {
            _timer.Interval = 1000;
            _timer.Start();
        }

        /// <summary>
        /// Pauza.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Posuv doleva.
        /// </summary>
        /// <returns>true, pokud lze</returns>
        public bool Left()
        {
            // kontrola, jestli lze pohyb provezt
            if (!IsSpaceFor(_activeX - 1, _activeY, pieces[ActivePiece][_activeR]))
                return false;
            // posuv doleva
            _activeX--;
            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Posuv doprava.
        /// </summary>
        /// <returns>true, pokud ano</returns>
        public bool Right()
        {
            // kontrola, jestli lze pohyb provezt
            if (!IsSpaceFor(_activeX + 1, _activeY, pieces[ActivePiece][_activeR]))
                return false;
            // posuv doprava
            _activeX++;
            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Rotace.
        /// </summary>
        /// <returns>true, pokud lze</returns>
        public bool Rotate()
        {
            int rot = _activeR;
            // 
            rot = (_activeR + 1) % (pieces[ActivePiece].Length);
            // kontrola. jestli lze rotovat
            if (!IsSpaceFor(_activeX, _activeY, pieces[ActivePiece][rot]))
                return false;
            // ulozeni rotace
            _activeR = rot;
            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Posuv dolu.
        /// </summary>
        /// <returns>true, pokud lze</returns>
        public bool Drop()
        {
            // 
            while (!CanParkPiece(_activeX, _activeY, pieces[ActivePiece][_activeR]))
                _activeY++;

            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Zresetovani hry.
        /// </summary>
        public void Reset()
        {
            _timer.Stop();
            // promazeme pole.
            //Board.Initialize(); 
            Board = new int[Height, Width];
            // zresetujeme pozici aktivniho policka
            /// TODO mozna jinak
            _activeR = _activeY = _activeX = 0;
            // vygenerujeme novy aktivni prvek
            NextPiece();
            // 

        }

        /// <summary>
        /// Kontrola, jestli lze prvek "zaparkovat".
        /// </summary>
        /// <param name="posX">pozice X</param>
        /// <param name="posY">pozive Y</param>
        /// <param name="piece">polozky prvku</param>
        /// <returns>true, pokud lze</returns>
        public bool CanParkPiece(int posX, int posY, int[] piece)
        {
            for (int xp = 0; xp < 4; xp++)
            {
                int m = -1;
                for (int yp = 4 - 1; yp >= 0; yp--)
                {
                    if (piece[yp * 4 + xp] != 0)
                    {
                        m = yp;
                        break;
                    }
                }
                if (m >= 0)
                {
                    if (posY + m + 1 == Board.GetLength(0) || Board[posY + m + 1, posX + xp] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Kontrola, jestli je pro prvek na teto pozici prostor pro vykresleni.
        /// </summary>
        /// <param name="posX">pozice X</param>
        /// <param name="posY">pozice Y</param>
        /// <param name="piece">vycet policek prvku</param>
        /// <returns>true, pokud lze</returns>
        public bool IsSpaceFor(int posX, int posY, int[] piece)
        {
            int maxY = Board.GetLength(0);
            int maxX = Board.GetLength(1);
            for (int i = 0; i < piece.Length; i++)
            {
                if (piece[i] == 0) continue;
                if (posY + i / 4 < 0 || posY + i / 4 >= maxY) return false;
                if (posX + i % 4 < 0 || posX + i % 4 >= maxX) return false;
                if (Board[posY + i / 4, posX + i % 4] != 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Vraci barvu dle typu policka.
        /// </summary>
        /// <param name="colorNumber">cislo barvy</param>
        /// <returns>barva</returns>
        public static Brush PieceColor(int colorNumber)
        {
            Brush ret = Brushes.White;
            switch (colorNumber)
            {
                case 1:
                    ret = Brushes.Yellow;
                    break;
                case 2:
                    ret = Brushes.Blue;
                    break;
                case 3:
                    ret = Brushes.Red;
                    break;
                case 4:
                    ret = Brushes.Cyan;
                    break;
                case 5:
                    ret = Brushes.Green;
                    break;
                case 6:
                    ret = Brushes.Magenta;
                    break;
                case 7:
                    ret = Brushes.Black;
                    break;
                default:
                    break;
            }
            return ret;
        }

        #endregion // Public methods

        #region Event methods

        protected void Tick(object sender, ElapsedEventArgs e)
        {
            if (CanParkPiece(_activeX, _activeY, pieces[ActivePiece][_activeR]))
            {
                FixPiece();
                NextPiece();
            }
            else
            {
                _activeY++;
            }

            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
        }

        #endregion // Event methods

        #region Private methods

        /// <summary>
        /// Zafixovani polozky.
        /// Pokud je radka ucelena, tak ji odmazeme.
        /// </summary>
        private void FixPiece()
        {
            for (int i = 0; i < 16; i++)
            {
                if (pieces[ActivePiece][_activeR][i] != 0)
                {
                    Board[_activeY + i / 4, _activeX + i % 4] = pieces[ActivePiece][_activeR][i];
                }
            }
            ClearLines(_activeY, _activeY + 4);
        }

        /// <summary>
        /// Odstraneni prebytecnych radku.
        /// </summary>
        /// <param name="start">pocatek</param>
        /// <param name="end">konec</param>
        private void ClearLines(int start, int end)
        {
            if (end > Board.GetLength(0)) end = Board.GetLength(0);

            for (int i = start; i < end; i++)
            {
                if (LineFull(i)) RemoveLine(i);
            }
        }

        /// <summary>
        /// Odstraneni jedne radky.
        /// </summary>
        /// <param name="line">cislo radky</param>
        private void RemoveLine(int line)
        {
            for (int i = 0; i < Board.GetLength(1); i++)
                Board[line, i] = 0;

            int[] cow = GetLine(line);
            for (int i = line - 1; i >= 0; i--)
                SetLine(i + 1, GetLine(i));
            // Board[i + 1] = Board[i];
            // Board[0] = cow;
            SetLine(0, cow);
        }

        private void SetLine(int line, int[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Board[line, i] = data[i];
            }
        }

        /// <summary>
        /// Vrati vsechyn prvky na radku.
        /// </summary>
        /// <param name="line">radek</param>
        /// <returns>true, pokud ano</returns>
        private int[] GetLine(int line)
        {
            int[] tmp = new int[Board.GetLength(1)];
            for (int i = 0; i < Board.GetLength(1); i++)
            {
                tmp[i] = Board[line, i];
            }
            return tmp;
        }

        /// <summary>
        /// Kontrola, jestli je radka "plna".
        /// </summary>
        /// <param name="line">radka</param>
        /// <returns>true, pokud je plna</returns>
        private bool LineFull(int line)
        {
            for (int i = 0; i < Board.GetLength(1); i++)
            {
                if (Board[line, i] == 0) return false;
            }
            return true;
        }


        /// <summary>
        /// Pomocna metoda, ktera generuje nasledujici kousicek.
        /// </summary>
        private void NextPiece()
        {
            // vygenerujeme nove aktivni policko
            ActivePiece = _rand.Next(pieces.Length);
            // vybereme nahodnou rotaci
            _activeR = _rand.Next(pieces[ActivePiece].Length);
            // zarovname policko na stred horniho hraciho pole
            _activeX = Board.GetLength(1) / 2 - 2;
            _activeY = 0;
            // zkontrolujeme, jestli lze prvek vubec vlozit
            if (!IsSpaceFor(_activeX, _activeY, pieces[ActivePiece][_activeR])) Reset();
        }

        #endregion // Private methods
    }
}
