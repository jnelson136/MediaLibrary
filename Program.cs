using NLog;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
logger.Info(scrubbedFile);
MovieFile movieFile = new MovieFile(scrubbedFile);

string choice = "";
do
{
    // display choices to user
    Console.WriteLine("1) Add Movie");
    Console.WriteLine("2) Display All Movies");
    Console.WriteLine("3) Find Movie");
    Console.WriteLine("Enter to quit");
    // input selection
    choice = Console.ReadLine();
    logger.Info("User choice: {Choice}", choice);

    if (choice == "1")
    {
        // Add movie
        Movie movie = new Movie();
        // ask user to input movie title
        Console.WriteLine("Enter movie title");
        // input title
        movie.title = Console.ReadLine();
        // verify title is unique
        if (movieFile.isUniqueTitle(movie.title))
        {
            Console.WriteLine("Who is the director?");
            movie.director = Console.ReadLine();

            Console.WriteLine("What Is The Movie Runtime? h:m:s");
            string runtimeInput = Console.ReadLine();

            string[] hms = runtimeInput.Split(':');

            int hours = int.Parse(hms[0]);
            int minutes = int.Parse(hms[1]);
            int seconds = int.Parse(hms[2]);

            TimeSpan runtime = new TimeSpan(hours, minutes, seconds);

            movie.runningTime = runtime;

            // input genres
            string input;
            do
            {
                // ask user to enter genre
                Console.WriteLine("Enter genre (or done to quit)");
                // input genre
                input = Console.ReadLine();
                // if user enters "done"
                // or does not enter a genre do not add it to list
                if (input != "done" && input.Length > 0)
                {
                    movie.genres.Add(input);
                }

            } while (input != "done");
            // specify if no genres are entered
            if (movie.genres.Count == 0)
            {
                movie.genres.Add("(no genres listed)");
            }
            // add movie
            movieFile.AddMovie(movie);
        }

    }
    else if (choice == "2")
    {
        // Display All Movies
        foreach (Movie m in movieFile.Movies)
        {
            Console.WriteLine(m.Display());
        }
    }
    else if (choice == "3")
    {
        Console.WriteLine("What Movie Are You Looking For?");
        string movieSearch = Console.ReadLine();
        var Movies = movieFile.Movies.Where(m => m.title.Contains(movieSearch));

        if (Movies.Any())
        {
            Console.WriteLine("Searching...");
            Console.WriteLine("---------------------------------");

            foreach (Movie m in Movies)
            {
                Console.WriteLine($"There is {Movies.Count()} Movie With That Title");
                Console.WriteLine(m.Display());
            }
        }
        else
        {
            Console.WriteLine("Searching...");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("No Movies Found");
        }

    }

} while (choice == "1" || choice == "2");


logger.Info("Program ended");