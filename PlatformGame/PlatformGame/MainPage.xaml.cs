namespace PlatformGame
{
    public partial class MainPage : ContentPage
    {
        private bool gridReady;
        private Player user;
        private Platform[] platformList = new Platform[4];
        public int level = 1;
        public int score = 0;
        public Label scoreLabel;
        public Label levelLabel;
        public double difficulty = 1000;


        public MainPage()
        {
            InitializeComponent();
        }
        private void Start_Button_Clicked(object sender, EventArgs e)
        {
            Fill_Grid();
            gridReady = true;
            Start_Button.IsEnabled = false;
        }

        private void Reset_Button_Clicked(object sender, EventArgs e)
        {
            Fill_Grid();
            gridReady = true;
            Reset_Button.IsEnabled = false;
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
                        platformList[i] = plat;
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


            foreach(Platform plat in platformList)
            {
                //set platform to move right
                //start platform oscillation
                plat.isMovingRight = true;
                plat.Oscillate(gameGrid, this);
            }


        }

        private Player Create_User()
        {
            Image img = new Image { Source = "dotnet_bot.png", ZIndex = 1 };
            Player user = new Player(img, 4, 2);
            return user;
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e) 
        {
            if(gridReady)
            {
                //Jump
                gridReady = user.Jump(gameGrid, platformList, gridReady, this);
            }
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

        public bool Jump(Grid g, Platform[] list , bool con, PlatformGame.MainPage page )
        {
            if(this.row == 0)
            {
                // reach top of game
            }
            else
            {
                g.SetRow(this.image, this.row -= 1);
                Platform platform = list[this.row];
                //Lab starts here 
                //if player row and col match Platform row and col
                    //score +1
                    //update scorre label

                if (platform.row == this.row && platform.col == this.col)
                {
                    page.score += 1;
                    page.scoreLabel.Text = "Score " + page.score;
                    platform.StopMovement();
                }
                //else
                else
                {

                }
            }
            return con;
        }
    }

    //Create Platform Class Here
    public class Platform
    {
        public BoxView boxView;
        public int row;
        public int col;
        public bool isMovingRight;
        public bool isMovingLeft;

        public Platform(BoxView b, int r, int c)
        {
            boxView = b; 
            row = r;
            col = c;
            isMovingRight = true;
            isMovingLeft = false;
        }

        public void StopMovement()
        {
            isMovingLeft = false;
            isMovingRight= false;
        }

        async public void Oscillate(Grid g, PlatformGame.MainPage page)
        {
            int boundaryLeft = 0;
            int boundaryRight = 4;
            var rand = new Random();
            await Task.Delay(rand.Next(1000));

            // move right
            MoveRight(g, boundaryLeft, boundaryRight, page);
        }

        async public void MoveRight(Grid g, int limLeft, int limRight, PlatformGame.MainPage page)
        {
            while(col < limRight && isMovingRight)
            {
                col++;
                g.SetColumn(boxView, col);
                // set delay based on difficulty
                await Task.Delay((int)page.difficulty);
            }
            if (isMovingRight)
            {
                isMovingLeft = true;
                isMovingRight = false;
                // move left
                MoveLeft(g, limLeft, limRight, page);
            }
        }
        async public void MoveLeft(Grid g, int limLeft, int limRight, PlatformGame.MainPage page)
        {
            while (col > limRight && isMovingLeft)
            {
                col++;
                g.SetColumn(boxView, col);
                // set delay based on difficulty
                await Task.Delay((int)page.difficulty);
            }
            if (isMovingLeft)
            {
                isMovingLeft = false;
                isMovingRight = true;
                MoveRight(g, limLeft, limRight, page);
            }
        }

    }
}