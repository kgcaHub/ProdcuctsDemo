using System.ComponentModel.DataAnnotations;

namespace Products.Api;

public static class ValidationHelper
{
    public static bool TryValidate<T>(T obj, out Dictionary<string, string[]> errors)
    {
        var results = new List<ValidationResult>();
        var ctx = new ValidationContext(obj!);
        var ok = Validator.TryValidateObject(obj!, ctx, results, validateAllProperties: true);

        if (ok)
        {
            errors = new();
            return true;
        }

        errors = results
            .SelectMany(r => r.MemberNames.DefaultIfEmpty(string.Empty),
                        (r, m) => new { m, r.ErrorMessage })
            .GroupBy(x => x.m)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage ?? "Invalid").ToArray());

        return false;
    }

    public static IResult? ValidateOrProblem<T>(T obj)
    {
        return TryValidate(obj, out var errors)
            ? null
            : Results.ValidationProblem(errors);
    }
}