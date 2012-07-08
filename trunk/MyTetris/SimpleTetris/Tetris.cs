using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SimpleTetris
{
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
        /// Aktivni policko.
        /// </summary>
        public int ActivePiece { get; private set; }

        public int ActiveX { get { return _activeX; } }
        public int ActiveY { get { return _activeY; } }
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
                rot = (_activeR+1) % (pieces[ActivePiece].Length);
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
                    Board[_activeY + i / 4,_activeX + i % 4] = pieces[ActivePiece][_activeR][i];
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
                if (Board[line,i] == 0) return false;
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
