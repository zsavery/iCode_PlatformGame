namespace PlatformGame
{
    public partial class MainPage : ContentPage
    {
        private Player user;
        public int level = 1;
        public int score = 0;
        public Label scoreLabel;
        public Label levelLabel;


        public MainPage()
        {
            InitializeComponent();
        }
        private void Start_Button_Clicked(object sender, EventArgs e)
        {
            Fill_Grid();

            Start_Button.IsEnabled = false;
        }

        private void Reset_Button_Clicked(object sender, EventArgs e)
        {

        }

        async public void Fill_Grid()
        {
            int rows = 5;
            int columns = 5;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    StackLayout unit = new StackLayout() { ZIndex = 0 };
                    unit.BackgroundColor = Colors.LightSkyBlue;
                    gameGrid.Add(unit, j, i);

                    await Task.Delay(100);

                    if (i < 4 && j == 0)
                    {
                        //Create BoxView and Platform Object
                        BoxView box = new BoxView()
                        {
                            HeightRequest = 20,
                            Color = Colors.Green,
                            VerticalOptions = LayoutOptions.End,
                            CornerRadius = 10,
                            ZIndex = 1
                        };
                        Platform plat = new Platform(box, i,j);

                        gameGrid.Add(box, plat.col, plat.row);
                    }
                    await Task.Delay(100);
                }
            }

            //Lab code here: 

            scoreLabel = new Label()
            {
                Text = "Score: " + score,
                FontSize = 30,
                ZIndex = 1,
                TextColor = Colors.White,
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            levelLabel = new Label()
            {
                Text = "Level: " + level,
                FontSize = 30,
                ZIndex = 1,
                TextColor = Colors.White,
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            //delay
            await Task.Delay(100);
            //add score label
            gameGrid.Add(scoreLabel, 0, 4);
            await Task.Delay(100);
            //create player object
            user = Create_User();
            gameGrid.Add(user.image, user.col, user.row);
            //delay
            await Task.Delay(100);
            //add level label
            gameGrid.Add(levelLabel, 4, 4);

        }

        private Player Create_User()
        {
            Image img = new Image { Source = "dotnet_bot.png", ZIndex = 1 };
            Player user = new Player(img, 4, 2);
            return user;
        }
    }

    public class Player
    {
        public Image image;
        public int row;
        public int col;


        //Constructor
        public Player(Image i, int r, int c)
        {
            image = i;
            row = r;
            col = c;
        }
    }

    //Create Platform Class Here
    public class Platform
    {
        public BoxView boxView;
        public int row;
        public int col;

        public Platform(BoxView b, int r, int c)
        {
            boxView = b; 
            row = r;
            col = c;
        }

    }
}