namespace FunkyBank;

public static class CoreExtentions
{
    public static bool Validate(this IValidatable validatable)
    {
        var status = validatable?.IsValid();

        return status ?? false;
    }
}