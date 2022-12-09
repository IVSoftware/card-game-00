using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace card_game_00
{
    public partial class MainForm : Form
    {
        // Make instance of CardDeck
        Deck DeckInstance = new Deck();
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
            labelHandB1.Text = $"10 {Card.Clubs}";
            labelHandB2.Text = $"J {Card.Clubs}";
            labelHandB3.Text = $"Q {Card.Clubs}";
            labelHandB4.Text = $"K {Card.Clubs}";
            labelHandB5.Text = $"A {Card.Clubs}";
            buttonDeal.Click += dealTheCards;
        }
        private async void dealTheCards(object sender, EventArgs e)
        {
            buttonDeal.Refresh(); UseWaitCursor = true;
            // When a non-UI task might take some time, run it on a Task.
            await DeckInstance.Shuffle();
            // Now we need the instance of the Desk to get the
            // cards one-by-one so use the property we declared.
            setCard(labelHandA1, DeckInstance.Dequeue());
            setCard(labelHandA2, DeckInstance.Dequeue());
            setCard(labelHandA3, DeckInstance.Dequeue());
            setCard(labelHandA4, DeckInstance.Dequeue());
            setCard(labelHandA5, DeckInstance.Dequeue());
            setCard(labelHandB1, DeckInstance.Dequeue());
            setCard(labelHandB2, DeckInstance.Dequeue());
            setCard(labelHandB3, DeckInstance.Dequeue());
            setCard(labelHandB4, DeckInstance.Dequeue());
            setCard(labelHandB5, DeckInstance.Dequeue());
            UseWaitCursor = false;
            // Dum hack to make sure the cursor redraws.
            Cursor.Position = Point.Add(Cursor.Position, new Size(1,1));
        }
        private void setCard(Label label, Card card)
        {
            label.Text = card.ToString();
            switch (card.CardSuit)
            {
                case CardSuit.Hearts:
                case CardSuit.Diamonds:
                    label.ForeColor = Color.Red;
                    break;
                case CardSuit.Spades:
                case CardSuit.Clubs:
                    label.ForeColor = Color.Black;
                    break;
            }
        }
    }
    public enum CardValue
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
    }
    public enum CardSuit
    {
        Hearts,
        Spades,
        Clubs,
        Diamonds,
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
        private static Random _rando = new Random();
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
            var distinct = _unshuffled.Distinct().Count();
            Debug.Assert(distinct == 52, "Oops there are duplicate cards.");
        }
        public async Task Shuffle()
        {
            Clear();
            List<int> sequence = Enumerable.Range(0, 52).ToList();
            while (sequence.Count != 0)
            {
                int nextRand = _rando.Next(0, sequence.Count());
                Enqueue(_unshuffled[sequence[nextRand]]);
                sequence.RemoveAt(nextRand);
#if DEBUG
                var dups = this.GroupBy(_ => _.ToString()).Where(_=>_.Count() > 1).ToDictionary(_ => _.Key, _ => _.ToArray());
                Debug.Assert(!dups.Any(), "Oops there are duplicate cards.");
#endif
            }
            // Spin a wait cursor as a visual indicator that "something is happening".
            await Task.Delay(TimeSpan.FromMilliseconds(500)); 
        }
        public Card GetNext() => Dequeue();
    }
}
