using System.Data;

Console.WriteLine("Hello welcome to the Dog Program!");

//user input
Console.WriteLine("What color is your dog?");
string furColor = Console.ReadLine();

Console.WriteLine("What is your dog's breed?");
string breed = Console.ReadLine();

Console.WriteLine("What is your dog's name?");
string name = Console.ReadLine();

Console.WriteLine("What is your dog's age?");
int age = Convert.ToInt32(Console.ReadLine());


Dog userDog = new Dog(furColor, breed, name, age);

Console.WriteLine("Your Dog: +" +
    userDog.name + " " + userDog.breed + " " +
    userDog.furColor + " " + userDog.age + " ");

Command();


void Command()
{
    Console.WriteLine("Should " + userDog.name + " sit, speck, fetch, or do a trick?");
    Console.WriteLine("type one of the following: sit | speak | trick | EXIT");
    string answer = Console.ReadLine();

    if(answer == "EXIT")
    {
        return;
    }
    else
    {
        if(answer == "speak")
        {
            userDog.talk();
        }
        else if (answer == "sit")
        {
            userDog.sit();
        }
        else if (answer == "fetch")
        {
            userDog.fetch();
        }
        else if (answer == "trick")
        {
            userDog.trick();
        }

        Command();
    }
}

public class Dog
{
    public string furColor;
    public string breed;
    public string name;
    public int age;

    public Dog(string fColor, string b, string n, int a)
    {
        furColor = fColor;
        breed = b;
        name = n;
        age = a;
    }

    public void sit()
    {
        Console.WriteLine(this.name + " is sitting...");
    }

    public void talk()
    {
        Console.WriteLine(this.name + ": Ruff! Ruff! Ruff!");
    }


    public void fetch()
    {
        Console.WriteLine(this.name + " is fetching...");
    }

    public void trick()
    {
        var rand = new Random();
        int randomNum = rand.Next(4);

        //Roll over
        if(randomNum <= 1)
        {
            Console.WriteLine(this.name + "roll over!");
        }
        //Play dead
        else if(randomNum > 1 && randomNum <= 2) 
        {
            Console.WriteLine(this.name + "play dead!");
        }
        //Hand shake
        else if (randomNum > 2 && randomNum <= 3)
        {
            Console.WriteLine(this.name + "shake your hand");
        }
        //Spin
        else
        {
            Console.WriteLine(this.name + "spin!");
        }


    }
}

