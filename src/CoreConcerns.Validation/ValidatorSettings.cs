using FluentValidation;
using System.Text.RegularExpressions;

namespace CoreConcerns.Validation;

/// <summary>
/// Provides extension methods for common validation rules.
/// </summary>
public static class ValidatorSettings
{

    #region Fields

    // Default regex pattern for phone number validation
    private const string DefaultPhoneNumberPattern = @"^\+?[1-9]\d{1,14}$"; // E.164 format as an example

    #endregion

    #region ValidationRules

    /// <summary>
    /// Validates that a string is a valid human name.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minLength">Minimum length of the name. Default is 1.</param>
    /// <param name="maxLength">Maximum length of the name. Default is 50.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, string> HumanName<T>(
        this IRuleBuilder<T, string> ruleBuilder, int minLength = 1, int maxLength = 50)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(minLength).WithMessage($"Name must be at least {minLength} characters long")
            .MaximumLength(maxLength).WithMessage($"Name cannot be more than {maxLength} characters long")
            .Matches(@"^[a-zA-Z\s-']+$").WithMessage("Name contains invalid characters");
    }

    /// <summary>
    /// Validates that a string is a valid email address and optionally checks against a specific domain.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="domain">The domain to validate against. Optional.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, string> ValidEmailAddress<T>(
        this IRuleBuilder<T, string> ruleBuilder, string? domain = null)
    {
        return ruleBuilder.Must(email => IsValidEmail(email, domain))
            .WithMessage(email =>
                domain != null
                    ? $"Email must be a valid address and match the domain '{domain}'."
                    : "Email must be a valid address.");
    }

    /// <summary>
    /// Validates that a string is a valid phone number. 
    /// Allows customization of the validation pattern and country code.
    /// </summary>
    /// <param name="ruleBuilder">The rule builder to which this rule is being added.</param>
    /// <param name="countryCode">Optional. The country code that the phone number should start with. If null, no country code validation is performed.</param>
    /// <param name="customPattern">Optional. A custom regex pattern for phone number validation. If null, a default E.164 format is used.</param>
    /// <returns>The rule builder options.</returns>
    public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder, string? countryCode = null, string? customPattern = null)
    {
        var pattern = customPattern ?? DefaultPhoneNumberPattern;

        return ruleBuilder.Must(phone => IsValidPhoneNumber(phone, countryCode, pattern))
            .WithMessage(phone =>
                countryCode != null
                    ? $"Phone number must match the country code '{countryCode}' and the specified format."
                    : "Invalid phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in Nigerian format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> NigerianPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidNigerianPhoneNumber)
            .WithMessage("Invalid Nigerian phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in US format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> USPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidUSPhoneNumber)
            .WithMessage("Invalid US phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in UK format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> UKPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidUKPhoneNumber)
            .WithMessage("Invalid UK phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in Indian format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> IndianPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidIndianPhoneNumber)
            .WithMessage("Invalid Indian phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in Canadian format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> CanadianPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidCanadianPhoneNumber)
            .WithMessage("Invalid Canadian phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in Australian format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> AustralianPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidAustralianPhoneNumber)
            .WithMessage("Invalid Australian phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in French format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> FrenchPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(IsValidFrenchPhoneNumber)
            .WithMessage("Invalid French phone number format.");
    }

    /// <summary>
    /// Validates if a phone number is in custom format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="countryCode">The custom country code to prepend to the phone number (optional).</param>
    /// <param name="length">The expected length of the phone number (default is 10).</param>
    /// <returns>The rule builder with the custom phone number validation.</returns>
    public static IRuleBuilderOptions<T, string> CustomPhoneNumber<T>(
        this IRuleBuilder<T, string> ruleBuilder, string countryCode = "", int length = 10)
    {
        return ruleBuilder.Must(phone => IsValidCustomPhoneNumber(phone, PrependPlusSign(countryCode), length))
            .WithMessage($"Invalid phone number format. Expected format: '+{countryCode}'+{length} digits.");
    }


    /// <summary>
    /// Validates that a string represents a valid HTTPS URL.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the URL validation.</returns>
    public static IRuleBuilderOptions<T, string> MustBeValidHttpsUrl<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(BeAValidHttpsUrl)
            .WithMessage("The URL must be a valid HTTPS URL.");
    }

    /// <summary>
    /// Validates that a string represents a valid country phone code.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the country phone code validation.</returns>
    public static IRuleBuilderOptions<T, string> CountryPhoneCode<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.NotEmpty().WithMessage("Country phone code is required")
            .Matches(@"^\+\d{1,4}$")
            .WithMessage("Country phone code must be a valid code starting with '+' and up to 4 digits.");
    }

    /// <summary>
    /// Validates that a string matches a given regular expression pattern.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="pattern">The regular expression pattern to match against.</param>
    /// <returns>The rule builder with the regex pattern validation.</returns>
    public static IRuleBuilderOptions<T, string> RegexMatch<T>(
        this IRuleBuilder<T, string> ruleBuilder, string pattern)
    {
        return ruleBuilder.Matches(pattern).WithMessage("String does not match the required pattern.");
    }

    /// <summary>
    /// Validates that a boolean value is true.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the true validation.</returns>
    public static IRuleBuilderOptions<T, bool> IsTrue<T>(
        this IRuleBuilder<T, bool> ruleBuilder)
    {
        return ruleBuilder.Equal(true).WithMessage("Value must be true.");
    }

    /// <summary>
    /// Validates that a DateTime value is in the past.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the past date validation.</returns>
    public static IRuleBuilderOptions<T, DateTime> IsDatePast<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder.LessThan(DateTime.Now).WithMessage("Date must be in the past.");
    }

    /// <summary>
    /// Validates that a boolean value is false.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the false validation.</returns>
    public static IRuleBuilderOptions<T, bool> IsFalse<T>(
        this IRuleBuilder<T, bool> ruleBuilder)
    {
        return ruleBuilder.Equal(false).WithMessage("Value must be false.");
    }

    /// <summary>
    /// Validates that a DateTime value is in the future.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the future date validation.</returns>
    public static IRuleBuilderOptions<T, DateTime> IsDateFuture<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder)
    {
        return ruleBuilder.GreaterThan(DateTime.Now).WithMessage("Date must be in the future.");
    }

    /// <summary>
    /// Validates that a string does not contain any whitespace characters.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the whitespace validation.</returns>
    public static IRuleBuilderOptions<T, string> NoWhiteSpace<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(s => s != null && !s.Any(char.IsWhiteSpace))
            .WithMessage("The field must not contain any whitespace characters.");
    }

    /// <summary>
    /// Validates that a decimal number has a specified maximum scale (number of decimal places).
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="scale">The maximum number of decimal places allowed.</param>
    /// <returns>The rule builder with the decimal scale validation.</returns>
    public static IRuleBuilderOptions<T, decimal> DecimalScale<T>(
        this IRuleBuilder<T, decimal> ruleBuilder, int scale)
    {
        return ruleBuilder.Must(value => HasValidDecimalScale(value, scale))
            .WithMessage($"Number must not have more than {scale} decimal places.");
    }

    /// <summary>
    /// Validates that all elements in an enumerable are unique.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TElement">The type of elements in the enumerable.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the uniqueness validation.</returns>
    public static IRuleBuilderOptions<T, IEnumerable<TElement>> UniqueList<T, TElement>(
        this IRuleBuilder<T, IEnumerable<TElement>> ruleBuilder)
    {
        return ruleBuilder.Must(list =>
            {
                var enumerable = list as TElement[] ?? list.ToArray();
                return list == null || enumerable.Distinct().Count() == enumerable.Count();
            })
            .WithMessage("All elements in the list must be unique.");
    }

    /// <summary>
    /// Validates that a DateTime represents an age greater than or equal to a specified minimum age.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minimumAge">The minimum age allowed.</param>
    /// <returns>The rule builder with the age validation.</returns>
    public static IRuleBuilderOptions<T, DateTime> Age<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder, int minimumAge)
    {
        return ruleBuilder.Must(birthDate =>
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;

            return age >= minimumAge;
        }).WithMessage($"Age must be at least {minimumAge} years.");
    }

    /// <summary>
    /// Validates that a DateTime represents an age within a specified range.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="minAge">The minimum age allowed.</param>
    /// <param name="maxAge">The maximum age allowed.</param>
    /// <returns>The rule builder with the age range validation.</returns>
    public static IRuleBuilderOptions<T, DateTime> AgeRange<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder, int minAge, int maxAge)
    {
        return ruleBuilder.Must(birthDate =>
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;
                if (birthDate.Date > today.AddYears(-age)) age--;
                return age >= minAge && age <= maxAge;
            })
            .WithMessage($"Age must be between {minAge} and {maxAge} years.");
    }

    /// <summary>
    /// Validates that a string represents a valid currency amount with a specified format.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="currencyFormat">The expected currency format (e.g., "$#,##0.00").</param>
    /// <returns>The rule builder with the currency format validation.</returns>
    public static IRuleBuilderOptions<T, string> CurrencyFormat<T>(
        this IRuleBuilder<T, string> ruleBuilder, string currencyFormat)
    {
        // Implement currency format validation logic based on the provided format.
        // You can use regular expressions or custom validation code here.

        // Example:
        string pattern = GetCurrencyFormatRegexPattern(currencyFormat);

        return ruleBuilder.Matches(pattern).WithMessage($"Invalid currency format. Expected format: {currencyFormat}");
    }

    /// <summary>
    /// Validates that a user's password has not expired based on a specified expiration date.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="expirationDate">The password expiration date.</param>
    /// <returns>The rule builder with the password expiration validation.</returns>
    public static IRuleBuilderOptions<T, DateTime> PasswordExpiryDate<T>(
        this IRuleBuilder<T, DateTime> ruleBuilder, DateTime expirationDate)
    {
        return ruleBuilder.Must(passwordDate => passwordDate <= expirationDate)
            .WithMessage($"Password has expired. Please reset your password.");
    }

    /// <summary>
    /// Validates that a string is a valid file name without special characters.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the file name validation.</returns>
    public static IRuleBuilderOptions<T, string> FileName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"^[a-zA-Z0-9\-_]+$")
            .WithMessage("File name contains invalid characters.");
    }

    /// <summary>
    /// Validates that latitude and longitude values fall within valid ranges.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="latitudeRange">The valid range of latitude values (e.g., -90 to 90).</param>
    /// <param name="longitudeRange">The valid range of longitude values (e.g., -180 to 180).</param>
    /// <returns>The rule builder with the latitude and longitude validation.</returns>
    public static IRuleBuilderOptions<T, (double Latitude, double Longitude)> LatitudeLongitude<T>(
        this IRuleBuilder<T, (double Latitude, double Longitude)> ruleBuilder,
        (double LatitudeMin, double LatitudeMax) latitudeRange,
        (double LongitudeMin, double LongitudeMax) longitudeRange)
    {
        return ruleBuilder.Must(coords =>
                coords.Latitude >= latitudeRange.LatitudeMin && coords.Latitude <= latitudeRange.LatitudeMax &&
                coords.Longitude >= longitudeRange.LongitudeMin && coords.Longitude <= longitudeRange.LongitudeMax)
            .WithMessage($"Invalid latitude or longitude values.");
    }

    /// <summary>
    /// Validates that a string can be used as a URL slug (e.g., no spaces, lowercase letters, hyphens).
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>The rule builder with the URL slug validation.</returns>
    public static IRuleBuilderOptions<T, string> UrlSlug<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"^[a-z0-9\-]+$")
            .WithMessage("Invalid URL slug format.");
    }

    #endregion

    #region Private Methods

    private static string GetCurrencyFormatRegexPattern(string currencyFormat)
    {
        // Escape special characters in the currency format
        var escapedFormat = Regex.Escape(currencyFormat);

        // Replace placeholders for currency symbol, thousands separator, and decimal separator
        escapedFormat = escapedFormat
            .Replace(@"\$\#", @"\$\d{1,3}") // Currency symbol (#)
            .Replace(@",", @"\,") // Thousands separator
            .Replace(@"\.", @"\."); // Decimal separator

        // Add anchors to ensure the pattern matches the entire string
        return $"^{escapedFormat}$";
    }

    private static bool HasValidDecimalScale(decimal value, int scale)
    {
        // Calculate the actual scale of the decimal
        var actualScale = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];

        // Check if the actual scale is less than or equal to the specified scale
        return actualScale <= scale;
    }

    private static bool BeAValidHttpsUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return false;
        }

        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
        {
            // Check the scheme is https and the Uri is well-formed.
            return uriResult.Scheme == Uri.UriSchemeHttps;
        }

        return false;
    }

    private static bool IsValidCustomPhoneNumber(string phone, string countryCode, int length)
    {
        // Constructing the regex pattern based on country code and length
        var pattern = $@"^{countryCode}\d{{{length}}}$";

        return Regex.IsMatch(phone, pattern);
    }

    private static string PrependPlusSign(string countryCode)
    {
        if (!string.IsNullOrEmpty(countryCode) && !countryCode.StartsWith("+"))
        {
            return "+" + countryCode;
        }

        return countryCode;
    }

    private static bool IsValidFrenchPhoneNumber(string phone)
    {
        // Regex pattern for French phone number with or without country code
        var pattern = @"^(\+33)?[1-9]\d{8}$";

        return Regex.IsMatch(phone, pattern);
    }

    private static bool IsValidAustralianPhoneNumber(string phone)
    {
        // Regex pattern for Australian phone number with or without country code
        var pattern = @"^(\+61)?[1-9]\d{8}$";

        return Regex.IsMatch(phone, pattern);
    }


    private static bool IsValidCanadianPhoneNumber(string phone)
    {
        // Regex pattern for Canadian phone number with or without country code
        var pattern = @"^(\+1)?[2-9]\d{2}[2-9]\d{2}\d{4}$";

        return Regex.IsMatch(phone, pattern);
    }


    private static bool IsValidIndianPhoneNumber(string phone)
    {
        // Regex pattern for Indian phone number with or without country code
        var pattern = @"^(\+91)?[6789]\d{9}$";

        return Regex.IsMatch(phone, pattern);
    }


    private static bool IsValidUKPhoneNumber(string phone)
    {
        // Regex pattern for UK phone number with or without country code
        var pattern = @"^(\+44)?[1-9]\d{9,10}$";

        return Regex.IsMatch(phone, pattern);
    }


    private static bool IsValidUSPhoneNumber(string phone)
    {
        // Regex pattern for US phone number with or without country code
        var pattern = @"^(\+1)?[2-9]\d{2}[2-9]\d{2}\d{4}$";

        return Regex.IsMatch(phone, pattern);
    }

    private static bool IsValidNigerianPhoneNumber(string phone)
    {
        // Regex pattern for validating Nigerian phone number with or without country code
        var pattern = @"^(\+234)?[789]\d{9}$";

        return Regex.IsMatch(phone, pattern);
    }

    private static bool IsValidEmail(string email, string? domain)
    {
        try
        {
            var emailAddress = new System.Net.Mail.MailAddress(email);
            if (domain == null)
            {
                return emailAddress.Address == email;
            }

            return emailAddress.Address == email && email.EndsWith("@" + domain);
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneNumber(string phone, string? countryCode, string pattern)
    {
        if (!Regex.IsMatch(phone, pattern))
        {
            return false;
        }

        if (countryCode != null)
        {
            return phone.StartsWith(countryCode);
        }

        return true;
    }

    #endregion
}