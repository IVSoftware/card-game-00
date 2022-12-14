In your [post](https://stackoverflow.com/q/74744618/5438626) you ask how to "add all the methods from other classes into the class where the blackjack game will be played". Basically, when you declare an _instance_ of something like the `Deck` class in your `MainForm` then (as you phrased it) all of its members have beed "added" (in a manner of speaking) because you can access them using the member property that you declared. So that's the first thing we'll do in the MainForm. I will also mention that sometimes there are `static` or `const` members and these don't require an instance to use them, rather you would use the class name instead. You'll see examples of this in the `MainForm` constructor like `Card.Spades`.

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


By and large, I've learned more from code _samples_ where I can run and set breakpoints than I have from tutorials, so I put together one you can [clone](https://github.com/IVSoftware/card-game-00.git) but it's not a _Blackjack_ game I'll leave that to you so I don't take away your fun!

![screenshot](https://github.com/IVSoftware/card-game-00/blob/master/card-game-00/Screenshots/screenshot.png)

***
My sample uses the following simplified classes:

**Card**

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

***
**Deck**

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

***
**Enums**

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
