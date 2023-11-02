using FluentValidation;

namespace MyProger.Mciro.SearchAPI.Command.Search.SearchJob;

public class SearchJobCommandValidator
    : AbstractValidator<SearchJobCommand>
{
    public SearchJobCommandValidator()
    {
        RuleFor(searchJobCommand =>
                searchJobCommand.Query).NotEqual(string.Empty)
            .WithMessage("You don't enter the searching query");
    }
}