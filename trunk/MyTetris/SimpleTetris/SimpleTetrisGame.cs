using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using SimpleTetris.Fields;
using SimpleTetris.Interfaces;

namespace SimpleTetris
{
    /// <summary>
    /// Trida reprezentujici hru tetris.
    /// </summary>
    public class SimpleTetrisGame : ITetrisGame
    {
        #region Fields

        /// <summary>
        /// Pozice X aktivniho prvku.
        /// </summary>
        private int _activeX;

        /// <summary>
        /// Pozice Y aktivniho prvku.
        /// </summary>
        private int _activeY;

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

        /// <summary>
        /// Udalost, ktera je odpalena kdykoliv, kdyz se objevi novy prvek.
        /// </summary>
        public event EventHandler NextPieceGenerated = delegate { };

        #endregion // Event

        #region Properties

        private static IList<IField> _fields;
        public static IList<IField> Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new List<IField>();
                    _fields.Add(new FieldO());
                    _fields.Add(new FieldI());
                    _fields.Add(new FieldJ());
                    _fields.Add(new FieldL());
                    _fields.Add(new FieldS());
                    _fields.Add(new FieldT());
                    _fields.Add(new FieldZ());
                }
                return _fields;
            }
        }

        /// <summary>
        /// Detekce hrani.
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

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
        public IField ActivePiece { get; private set; }

        /// <summary>
        /// Pozice X aktivniho prvku.
        /// </summary>
        public int ActiveX 
        { 
            get { return _activeX; }
            private set { _activeX = value; }
        }

        /// <summary>
        /// Pozice Y aktivniho prvku.
        /// </summary>
        public int ActiveY 
        { 
            get { return _activeY; }
            private set { _activeY = value; }
        }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Konstruktor inicializujici tetris.
        /// </summary>
        /// <param name="width">maximalni sirka</param>
        /// <param name="height">maximalni vyska</param>
        public SimpleTetrisGame(int width, int height)
        {
            // predame maximalni vysku a sirku pole
            Width = width;
            Height = height;

            _rand = new Random();
            // casovac
            _timer = new Timer();
            _timer.Elapsed += Tick;
            _timer.Interval = 1000;
            // nastaveni pocatecnich hodnot
            Reset();
        }

        #endregion // Constructor

        #region Public methods

        /// <summary>
        /// Priprava hry pred startem.
        /// </summary>
        public void Reset()
        {
            // ukonceni hry
            IsRunning = false;
            // pozastavime timer
            _timer.Stop();
            // inicializujeme hraci pole
            Board = new int[Height, Width];
            // vynulujeme pozici aktivniho pole
            _activeX = _activeY = 0;
            // vynulujeme aktivni prvek
            ActivePiece = null;
            // vykreslime plochu
            Repaint(this, EventArgs.Empty);
        }

        /// <summary>
        /// Odstartovani hry.
        /// </summary>
        public void Start()
        {
            // vygenerujeme nahodny prvek
            NextPiece();
            // hra zapocala
            IsRunning = true;
            // zapmene timer
            _timer.Start();
        }

        /// <summary>
        /// Pauza.
        /// </summary>
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                _timer.Stop();
            }
            else
            {
                IsRunning = true;
                _timer.Start();
            }
        }

        /// <summary>
        /// Posuv doleva.
        /// </summary>
        /// <returns>true, pokud lze</returns>
        public bool Left()
        {
            // kontrola, jestli lze pohyb provezt
            if (!IsSpaceFor(ActiveX - 1, ActiveY, ActivePiece.Pieces))
                return false;
            // posuv doleva
            ActiveX--;
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
            if (!IsSpaceFor(ActiveX + 1, ActiveY, ActivePiece.Pieces))
                return false;
            // posuv doprava
            ActiveX++;
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
            int rot = ActivePiece.Rotation;
            // 
            rot = (ActivePiece.Rotation + 1) % ActivePiece.MaxRotation;
            // kontrola. jestli lze rotovat
            if (!IsSpaceFor(ActiveX, ActiveY, ActivePiece.Pieces))
                return false;
            // ulozeni rotace
            ActivePiece.Rotation = rot;
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
            while (!CanParkPiece(ActiveX, ActiveY, ActivePiece.Pieces))
                ActiveY++;

            // odpaleni udalosti
            Repaint(this, EventArgs.Empty);
            return true;
        }

        /// <summary>
        /// Kontrola, jestli se aktivni prvek vejde na danou pozici.
        /// </summary>
        /// <param name="x">pozice X</param>
        /// <param name="y">pozice Y</param>
        /// <param name="piece">kosticky aktivniho prvku</param>
        /// <returns>true, pokud vejde</returns>
        public bool IsSpaceFor(int x, int y, int[] piece)
        {
            // pro vsechny kosticky aktivniho prvku
            for (int i = 0; i < piece.Length; i++)
            {
                // pokud je prvek nulovy, tak preskocime kontroly
                if (piece[i] == 0) continue;
                // pokud policko presahuje Y osu, neboli vysku pole, tak false
                if (y + i / 4 < 0 || y + i / 4 >= Height) return false;
                // pokud policko presahuje X osu, neboli sitku pole, tak false
                if (x + i % 4 < 0 || x + i % 4 >= Width) return false;
                // pokud policko v hracim poli je jiz obsazeny, tak false
                if (Board[y + i / 4, x + i % 4] != 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Kontrola, jestli lze aktivni prvek "dropnout".
        /// </summary>
        /// <param name="x">pozice X</param>
        /// <param name="y">pozice Y</param>
        /// <param name="piece">kosticky aktivniho prvku</param>
        /// <returns>true, pokud vejde</returns>
        public bool CanParkPiece(int x, int y, int[] piece)
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
                    if (y + m + 1 == Height || Board[y + m + 1, x + xp] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion // Public methods

        #region Event methods

        protected void Tick(object sender, ElapsedEventArgs e)
        {
            if (CanParkPiece(ActiveX, ActiveY, ActivePiece.Pieces))
            {
                FixPiece();
                NextPiece();
            }
            else
            {
                ActiveY++;
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
                if (ActivePiece.Pieces[i] != 0)
                {
                    Board[ActiveY + i / 4, ActiveX + i % 4] = ActivePiece.Pieces[i];
                }
            }
            ClearLines(ActiveY, ActiveY + 4);
        }

        /// <summary>
        /// Odstraneni prebytecnych radku.
        /// </summary>
        /// <param name="start">pocatek</param>
        /// <param name="end">konec</param>
        private void ClearLines(int start, int end)
        {
            if (end > Height) end = Height;

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
            for (int i = 0; i < Width; i++)
                Board[line, i] = 0;

            int[] cow = GetLine(line);
            for (int i = line - 1; i >= 0; i--)
                SetLine(i + 1, GetLine(i));
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
            int[] tmp = new int[Width];
            for (int i = 0; i < Width; i++)
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
            for (int i = 0; i < Width; i++)
            {
                if (Board[line, i] == 0) return false;
            }
            return true;
        }

        /// <summary>
        /// Generovani nahodneho dalsiho prvku.
        /// Kontrola konce hry.
        /// </summary>
        private void NextPiece()
        {
            // vygenerujeme nove aktivni policko
            ActivePiece = Fields.ToArray()[_rand.Next(Fields.Count)];
            // vybereme nahodnou rotaci
            ActivePiece.Rotation =_rand.Next(ActivePiece.MaxRotation);
            // zarovname policko na stred horniho hraciho pole
            ActiveX = Width / 2 - 2;
            ActiveY = 0;
            // zkontrolujeme, jestli lze prvek vubec vlozit
            if (!IsSpaceFor(ActiveX, ActiveY, ActivePiece.Pieces)) Reset();
            // odpalime udalost
            NextPieceGenerated(this, EventArgs.Empty);

        }

        #endregion // Private methods
    }
}
