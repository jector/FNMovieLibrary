using System;
using Spectre.Console;

namespace FNMovieLibrary;

public class Menu
{
    public enum MenuOptions
    {
        AddMovie,
        UpdateMovie,
        ShowMovies,
        SearchMovies,
        AddUser,
        RattingMovie,
        ShowTopRatedMovie,
        Exit
    }

    public Menu() // default constructor
    {
    }

    public MenuOptions ChooseAction()
    {
        var menuOptions = Enum.GetNames(typeof(MenuOptions));

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose your [green]menu action[/]?")
                .AddChoices(menuOptions));

        return (MenuOptions) Enum.Parse(typeof(MenuOptions), choice);
    }

    public void Exit()
    {
        AnsiConsole.Write(
            new FigletText("Thanks!")
                //.LeftAligned()
                .LeftJustified()
                .Color(Color.Green));
    }
}