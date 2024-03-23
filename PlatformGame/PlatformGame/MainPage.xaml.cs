using System.Runtime.InteropServices;

namespace PlatformGame
{
    public partial class MainPage : ContentPage
    {
        public bool gridReady;
        private Player user;
        private Platform[] platformList = new Platform[4];
        public int level = 1;
        public int score = 0;
        public Label scoreLabel;
        public Label levelLabel;
        public double difficulty = 1000;
        public CloudEnemy cloud;
        public Button startButton;


        public MainPage()
        {
            InitializeComponent();
            startButton = Start_Button;

        }
        private void Start_Button_Clicked(object sender, EventArgs e)
        {
            Fill_Grid(true);
            gridReady = true;
            Start_Button.IsEnabled = false;
        }

        private void Reset_Button_Clicked(object sender, EventArgs e)
        {
            gameGrid.Clear();
            Start_Button.IsEnabled = true;
            gridReady = true;
            score = 0;
            level = 1;
            Start_Button.Text = "Play!";
        }

        async public void Fill_Grid(bool start)
        {
            /*
             * store a random row position forr the enemy
             *     random number 0-4
             */
            var randCloudPosition = new Random();
            int cloudRow = randCloudPosition.Next(4);
            if (start)
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
                            Platform plat = new Platform(box, i, j);
                            platformList[i] = plat;
                            gameGrid.Add(box, plat.col, plat.row);
                        }
                        await Task.Delay(100);
                    }
                }

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
            
                await Task.Delay(100);
                gameGrid.Add(scoreLabel, 0, 4); //add score label
                await Task.Delay(100); //delay 100 seconds
                user = Create_User(); //create player object
                gameGrid.Add(user.image, user.col, user.row);
                await Task.Delay(100);
                gameGrid.Add(levelLabel, 4, 4); //add level label
                
                //crate a new Image for the cloud image 
                Image cloudIMG = new Image() { Source = "cloud_enemy.png" };
                //set enemy to a new CloudEnemy ojbect
                cloud = new CloudEnemy(cloudIMG, cloudRow, 4, user);
                //add the enemy to the grid
                gameGrid.Add(cloudIMG, 4, cloudRow);
                //start cloud oscillation
                cloud.Oscillate(gameGrid, this);
            }
            else
            {
                //reset user & platform -> Reset state
                user.row = 4;
                gameGrid.SetRow(user.image, user.row);
                //set the cloud row to the random number 
                cloud.row = cloudRow;
                //set the new row on the grid for the cloud
                gameGrid.SetRow(cloud.cloudImage, cloudRow);
                foreach (Platform plat in platformList) //resets plat columns
                {
                    plat.col = 0;
                    gameGrid.SetColumn(plat.boxView, plat.col);
                }
            }


            foreach (Platform plat in platformList)
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
            if (gridReady)
            {
                //Jump
                gridReady = user.Jump(gameGrid, platformList, gridReady, this);
                if (gridReady == false) { Start_Button.Text = "Game Over"; }
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

        public bool Jump(Grid g, Platform[] list, bool con, PlatformGame.MainPage page)
        {
            if (this.row == 0)
            {
                page.difficulty *= 0.9;
                page.level++;
                page.levelLabel.Text = "Level " + page.level;
                page.Fill_Grid(false);
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
                    con = false;
                }
            }
            return con;
        }

        public void Destroy(Grid g, PlatformGame.MainPage page)
        {
            //set gridReady to false
            page.gridReady = false;
            //remove the player imafe from grid using Grid.Remove()
            g.Remove(image);
            //set the start button text to "Game Over"
            page.startButton.Text = "Game Over";
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
            isMovingRight = false;
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
            while (col < limRight && isMovingRight)
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
                MoveLeft(g, limLeft, limRight, page); //Start movement
            }
        }
        async public void MoveLeft(Grid g, int limLeft, int limRight, PlatformGame.MainPage page)
        {
            while (col > limRight && isMovingLeft)
            {
                col--;
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


    public class CloudEnemy
    {
        // enemy attributes 
        public Image cloudImage;
        public int row;
        public int col;
        public Player user;

        //construntor
        public CloudEnemy(Image i, int r, int c, Player u)
        {
            cloudImage = i;
            row = r;
            col = c;
            user = u;
        }
        //Oscilattion function
        public void Oscillate(Grid g, Platform_MainPage page)
        {
            int boundaryLeft = 0;
            int boundaryRight = 4;
            MoveLeft(g, boundaryLeft, boundaryRight, page);
        }

        //MoveLeft function
        // ---> if enemy collides with user Destroy() user
        async public void MoveLeft(Grid g, int limitLeft, int limitRight, Platform_MainPage page)
        {
            while (col > limitLeft)
            {
                col--;
                g.SetColumn(cloudImage, col);
                if (col == user.col && row == user.row)
                {
                    user.Destroy(g, page);
                    return;
                }
                await Task.Delay(500);
            }

            MoveRight(g, limitLeft, limitRight, page);

        }
        //MoveRight Function
        // ---> if enemy collides with user Destroy() user
        async public void MoveRight(Grid g, int limitLeft, int limitRight, Platform_MainPage page)
        {
            while (col < limitRight)
            {
                col++;
                g.SetColumn(cloudImage, col);
                if (col == user.col && row == user.row)
                {
                    user.Destroy(g, page);
                    return;
                }
                await Task.Delay(500);
            }

            MoveLeft(g, limitLeft, limitRight, page);

        }
    }
}