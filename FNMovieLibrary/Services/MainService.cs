using System.Collections.Immutable;
using System.Globalization;
using Faker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using Spectre.Console;


namespace FNMovieLibrary.Services;

public class MainService : IMainService
    {
        
        private MovieContext _dbContext;
        private readonly IDbContextFactory<MovieContext> _dbContextFactory;

        //Default Constructor
        public MainService(IDbContextFactory<MovieContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            _dbContext = _dbContextFactory.CreateDbContext();
        }
        
        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                Console.WriteLine("");
                menuChoice = menu.ChooseAction();
                
                switch (menuChoice)
                {
                    case Menu.MenuOptions.AddMovie:
                        AddMovie();
                        break;
                    case Menu.MenuOptions.UpdateMovie:
                        
                        Update();
                        break;
                    case Menu.MenuOptions.ShowMovies:
                        ShowMovies();
                        break;
                    case Menu.MenuOptions.SearchMovies:
                        GetMovie();
                        break;
                    case Menu.MenuOptions.AddUser:
                        AddUser();
                        break;
                    case Menu.MenuOptions.RattingMovie:
                        UserRatingMovie();
                        break;
                    case Menu.MenuOptions.ShowTopRatedMovie:
                        TopRatedMovie();
                        break;
                        
                }
            } while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();
            
            Console.WriteLine("\nThanks for using the Movie Library!");
            
        }
        
        public void AddMovie()
        {
            var rule = new Rule("Add Movie");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("Enter new Movie: ");
            var addmovie = Console.ReadLine();
            Console.WriteLine("Enter movie year: ");
            var movieDate = Console.ReadLine();
            
            DateTime dt;
            DateTime.TryParseExact(movieDate, "yyyy", CultureInfo.CurrentCulture,
                DateTimeStyles.None, out dt);

            var movie = new Movie();
            movie.Title = addmovie + " (" + movieDate + ")";
            movie.ReleaseDate = dt;

            _dbContext.Movies.Add(movie);
            _dbContext.SaveChanges();

            var newMovie = _dbContext.Movies.FirstOrDefault(x => x.Title == movie.Title);
            var table = new Table();
            table.AddColumn(new TableColumn("Movie Title").Centered());
            table.AddColumn(new TableColumn("Movie Date").Centered());
            table.AddRow($"{newMovie.Title}", $"{newMovie.ReleaseDate}");
            AnsiConsole.Write(table.SquareBorder());
            AnsiConsole.Write(rule2);

        }
        
        public void Update()
        {
            var rule = new Rule("Update Movie");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("Enter Movie Name to Update: ");
            var oldMovie = Console.ReadLine();
            Console.WriteLine("Enter Updated Movie Name: ");
            var moUpdate = Console.ReadLine();

            var updateMovie = _dbContext.Movies.FirstOrDefault(x => x.Title == oldMovie);
            updateMovie.Title = moUpdate;

            _dbContext.Movies.Update(updateMovie);
            _dbContext.SaveChanges();

            Console.WriteLine("");
            var movieUpdated = _dbContext.Movies.FirstOrDefault(x => x.Title == updateMovie.Title);
            var table = new Table();
            table.AddColumn(new TableColumn("Old Movie Title").Centered());
            table.AddColumn(new TableColumn("New Movie Title").Centered());
            table.AddRow($"[silver]{oldMovie}[/]", $"{movieUpdated.Title}");
            
            AnsiConsole.Write(table.SquareBorder());
            AnsiConsole.Write(rule2);
        }
        
        public void ShowMovies()
        {
            var rule = new Rule("Show Movie/s");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("How many Movies do you want to see? ");
            int movie = Convert.ToInt32(Console.ReadLine());
            
            var movies = _dbContext.Movies;

            Console.WriteLine(" ");
            var table = new Table();

            int i = 1;
            
            table.AddColumn("The movies are :");
            foreach (var mov in movies.Take(movie))
            {
                table.AddRow($"({i++}) {mov.Title}");
            }

            AnsiConsole.Write(table.SquareBorder());
            AnsiConsole.Write(rule2);
            
        }
        
        public void GetMovie()
        {
            var rule = new Rule("Search Movie/s");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("Enter Movie Name or the first Letters: ");
            string mov = Console.ReadLine();
            
            var findmovie = _dbContext.Movies
                .Include(g => g.MovieGenres).ThenInclude(g => g.Genre);
            var movie = findmovie
                .Where(x => x.Title.Contains(mov) && x.Title.StartsWith(mov))
                .ToList();

            Console.WriteLine(" ");
            foreach (var mo in movie)
            {
                Console.WriteLine($"_ Movie: ({mo.Id}). {mo.Title}");
                foreach (var gener in mo.MovieGenres)
                {
                    Console.WriteLine($"\tGenre: {gener.Genre.Name}");
                }
            }
            
            AnsiConsole.Write(rule2);
        }
        
        public void AddUser()
        {
            /*
        // Faker.Net Users
            var user = _dbContext.Users.Where(u => u.Name == "");
            foreach (var us in user)
            {
                us.Name = $"{Faker.Name.First()} {Faker.Name.Last()}";
                _dbContext.Users.Update(us);
            }
            _dbContext.SaveChanges();
            */
            var rule = new Rule("Add New User");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("Enter New User: ");
            var addUserName = Console.ReadLine();
            Console.WriteLine("Enter User Age: ");
            int addUserAge = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter User Genre: (M or F)");
            var addUserGender = Console.ReadLine();
            Console.WriteLine("Enter User ZipCode: ");
            var addUserZipCode = Console.ReadLine();
            Console.WriteLine("Enter User Occupation: ");
            var addUserOccupation = Console.ReadLine();

            var user = new User();
            user.Name = addUserName;
            user.Age = addUserAge;
            user.Gender = addUserGender;
            user.ZipCode = addUserZipCode;

            var usersOccupation = _dbContext.Occupations.Where(x => x.Name == addUserOccupation).ToList();
            foreach (var oc in usersOccupation)
            {
                user.OccupationId = oc.Id;
            }

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            
            Console.WriteLine(" ");
            var table = new Table();
            var headersTable = new List<string>{"Name","Age", "Gender", "ZipCode", "Occupation"};

            foreach (var header in headersTable)
            {
                table.AddColumn(new TableColumn(header).Centered());
            }
            var newUser = _dbContext.Users.FirstOrDefault(x => x.Name == addUserName);
            table.AddRow($"{newUser.Name}", $"{newUser.Age}",$"{newUser.Gender}",$"{newUser.ZipCode}",$"{newUser.Occupation.Name}");

            AnsiConsole.Write(table.SquareBorder());
            AnsiConsole.Write(rule2);
        }

        public void UserRatingMovie()
        {
            var rule = new Rule("User add Rating to Movie");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            Console.WriteLine("Write your Full User Name: ");
            var fullUserName = Console.ReadLine();
            Console.WriteLine("Write Movie Name: ");
            var movieName = Console.ReadLine();
            Console.WriteLine("Write your Rate for 1 to 5: ");
            long movieRatting = Convert.ToInt64(Console.ReadLine());
            
            var ratting = new UserMovie();
            ratting.Rating = movieRatting;
            ratting.RatedAt = DateTime.Now;

            var userMovieRatting = _dbContext.UserMovies
                .Include(m => m.Movie)
                .Include(x => x.User)
                .ThenInclude(o => o.Occupation)
                .ToList();
            
            foreach (var usm in userMovieRatting.Where(x => x.Movie.Title == movieName ))
            {
                ratting.MovieId = usm.Movie.Id;
            }
            foreach (var usmo in userMovieRatting.Where(x => x.User.Name == fullUserName))
            {
                ratting.UserId = usmo.User.Id;
            }
            
            _dbContext.UserMovies.Add(ratting);
            _dbContext.SaveChanges();
            
            Console.WriteLine(" ");
            
            var table = new Table();
            var headersTable = new List<string>{"User name","User age", "User gender", "User ZipCode", 
                "User Occupation", "Movie Rated", "Movie Ratting"};
            
            foreach (var header in headersTable)
            {
                table.AddColumn(new TableColumn(header).Centered());
            }
            
            var newUser = _dbContext.UserMovies.FirstOrDefault(x => x.RatedAt == ratting.RatedAt);
            table.AddRow($"{newUser.User.Name}", $"{newUser.User.Age}",$"{newUser.User.Gender}",$"{newUser.User.ZipCode}"
                ,$"{newUser.User.Occupation.Name}", $"{newUser.Movie.Title}", $"{ratting.Rating}");

            AnsiConsole.Write(table.SquareBorder());
            AnsiConsole.Write(rule2);
        }

        public void TopRatedMovie()
        {
            var rule = new Rule("Top Rated Movie by Occupation");
            rule.RuleStyle("dim");
            var rule2 = new Rule();
            rule2.RuleStyle("dim");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);
            
            var users = _dbContext.Users
            .Include(x => x.Occupation)
            .Include(x => x.UserMovies)
            .ThenInclude(x => x.Movie).ToList();

            var occ = users.GroupBy(x => x.Occupation)
                .Select(x => new { Occupation = x.Key, UserCount = x.Count() });
            
            int i = 1;
            
            foreach (var o in (occ))
            {
                Console.WriteLine($"({i++}) Occupation: {o.Occupation.Name}, Count: {o.UserCount}");
                
                var userMovies = _dbContext.UserMovies.ToList();

                var um = userMovies
                    .Where(x => x.User.Occupation.Name == o.Occupation.Name)
                    .GroupBy(x => x.Movie.Title)
                    .Select(x => new { MovieTitle = x.Key, CountOfRatings = x.Count() })
                    .ToList();

                var mostRated = um.OrderByDescending(x => x.CountOfRatings).Take(1);

                foreach (var m in mostRated)
                {
                    Console.WriteLine($"\tMovie: {m.MovieTitle}, Number of Ratings: {m.CountOfRatings}");
                }
                AnsiConsole.Write(rule2);
            }
            AnsiConsole.Write(rule2);
        }

    }