using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace card_game_00
{
    public partial class MainForm : Form
    {
        // Make instance of CardDeck
        Deck Deck = new Deck();
        public MainForm()
        {
            InitializeComponent();
            Text = "Card Game";
            tableLayoutCards.Font = new Font("Sergoe UI Symbol", 9);
            // Static or const members of a class do not require an
            // instance. Use the class name to reference these members.
            labelHandA1.Text = $"10 {Card.Spades}";
            labelHandA2.Text = $"J {Card.Spades}";
            labelHandA3.Text = $"Q {Card.Spades}";
            labelHandA4.Text = $"K {Card.Spades}";
            labelHandA5.Text = $"A {Card.Spades}";
            labelHandB1.Text = $"10 {Card.Hearts}";
            labelHandB2.Text = $"J {Card.Hearts}";
            labelHandB3.Text = $"Q {Card.Hearts}";
            labelHandB4.Text = $"K {Card.Hearts}";
            labelHandB5.Text = $"A {Card.Hearts}";
            buttonDeal.Click += dealTheCards;
        }

        private async void dealTheCards(object sender, EventArgs e)
        {
            buttonDeal.Refresh(); UseWaitCursor = true;
            // When a non-UI task might take some time, run it on a Task.
            await Deck.Shuffle();
            // Now we need the instance of the Desk to get the
            // cards one-by-one so use the property we declared.
            labelHandA1.Text = Deck.Dequeue().ToString();
            labelHandA2.Text = Deck.Dequeue().ToString();
            labelHandA3.Text = Deck.Dequeue().ToString();
            labelHandA4.Text = Deck.Dequeue().ToString();
            labelHandA5.Text = Deck.Dequeue().ToString();
            labelHandB1.Text = Deck.Dequeue().ToString();
            labelHandB2.Text = Deck.Dequeue().ToString();
            labelHandB3.Text = Deck.Dequeue().ToString();
            labelHandB4.Text = Deck.Dequeue().ToString();
            labelHandB5.Text = Deck.Dequeue().ToString();
            UseWaitCursor = false;
            // Dum hack to make sure the cursor redraws.
            Cursor.Position = Point.Add(Cursor.Position, new Size(1,1));
        }
    }
    public enum CardValue
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
    public enum CardSuit
    {
        Hearts = 1,
        Spades = 2,
        Clubs = 3,
        Diamonds = 4
    }
    public class Card
    {
        // https://office-watch.com/2021/playing-card-suits-%E2%99%A0%E2%99%A5%E2%99%A6%E2%99%A3-in-word-excel-powerpoint-and-outlook/#:~:text=Insert%20%7C%20Symbols%20%7C%20Symbol%20and%20look,into%20the%20character%20code%20box.
        public const string Hearts = "\u2665";
        public const string Spades = "\u2660";
        public const string Clubs = "\u2663";
        public const string Diamonds = "\u2666";
        public CardValue CardValue { get; set; }
        public CardSuit CardSuit { get; set; }
        public override string ToString()
        {
            string value, suit = null;
            switch (CardValue)
            {
                case CardValue.Ace: value = "A"; break;
                case CardValue.Jack: value = "J"; break;
                case CardValue.Queen:  value = "Q"; break;
                case CardValue.King: value = "K"; break;
                default: value = $"{(int)CardValue}"; break;
            }
            switch (CardSuit)
            {
                case CardSuit.Hearts: suit = Hearts; break;
                case CardSuit.Spades: suit = Spades; break;
                case CardSuit.Clubs: suit = Clubs; break;
                case CardSuit.Diamonds: suit = Diamonds ; break;
            }
            return $"{value} {suit}";
        }
    }
    public class Deck : Queue<Card>
    {
        // Instantiate Random ONE time (not EVERY time).
        private readonly Random _rando = new Random();
        private readonly Card[] _unshuffled;
        public Deck()
        {
            List<Card> tmp = new List<Card>();
            foreach(CardValue value in Enum.GetValues(typeof(CardValue)))
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    tmp.Add(new Card { CardValue = value, CardSuit = suit });
                }
            }
            _unshuffled = tmp.ToArray();
        }
        public async Task Shuffle()
        {
            Clear();
            List<int> sequence = Enumerable.Range(0, 52).ToList();
            while(sequence.Count != 0)
            {
                int nextRand = _rando.Next(0, sequence.Count());
                Enqueue(_unshuffled[nextRand]);
                sequence.RemoveAt(nextRand);
            }
            // Spin a wait cursor as a visual indicator that "something is happening".
            await Task.Delay(TimeSpan.FromMilliseconds(500)); 
        }
        public Card GetNext() => Dequeue();
    }
}
