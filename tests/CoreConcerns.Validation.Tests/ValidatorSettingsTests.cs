using FluentValidation;
using System.Globalization;

namespace CoreConcerns.Validation.Tests;

public class ValidatorSettingsTests
{
    readonly InlineValidator<string> _stringInlineValidator;

    public ValidatorSettingsTests()
    {
        _stringInlineValidator = new InlineValidator<string>();
    }

    [Theory]
    [InlineData("John Doe", true)] // Valid name
    [InlineData("Ana-Maria", true)] // Valid compound name
    [InlineData("O'Connor", true)] // Valid name with apostrophe
    [InlineData("12345", false)] // Invalid name with numbers
    [InlineData("", false)] // Invalid empty name
    public void HumanName_ShouldValidateCorrectly(string name, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).HumanName();

        var result = _stringInlineValidator.Validate(name);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("user@example.com", null, true)] // Valid email
    [InlineData("user@domain.com", "domain.com", true)] // Valid email with matching domain
    [InlineData("user@domain.com", "example.com", false)] // Invalid email with non-matching domain
    [InlineData("invalid-email", null, false)] // Invalid email format
    public void ValidEmailAddress_ShouldValidateCorrectly(string email, string domain, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).ValidEmailAddress(domain);

        var result = _stringInlineValidator.Validate(email);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("1234567890", null, null, true)] // Valid default format
    [InlineData("+11234567890", "+1", null, true)] // Valid with country code
    [InlineData("1234", null, null, false)] // Invalid - too short
    [InlineData("1234567890123456", null, null, false)] // Invalid - too long
    [InlineData("+11234567890", "+44", null, false)] // Invalid - wrong country code
    public void PhoneNumber_ShouldValidateCorrectly(string phone, string countryCode, string customPattern,
        bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).PhoneNumber(countryCode, customPattern);

        var result = _stringInlineValidator.Validate(phone);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("https://www.example.com", true)] // Valid HTTPS URL
    [InlineData("http://www.example.com", false)] // Invalid - not HTTPS
    [InlineData("www.example.com", false)] // Invalid - not a URL
    [InlineData("", false)] // Invalid - empty string
    public void MustBeValidHttpsUrl_ShouldValidateCorrectly(string url, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).MustBeValidHttpsUrl();

        var result = _stringInlineValidator.Validate(url);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("+1", true)] // Valid country code
    [InlineData("+44", true)] // Another valid country code
    [InlineData("123", false)] // Invalid - missing plus sign
    [InlineData("+12345", false)] // Invalid - too long
    [InlineData("", false)] // Invalid - empty string
    public void CountryPhoneCode_ShouldValidateCorrectly(string code, bool expectedIsValid)
    {
        var validator = new InlineValidator<string>();
        validator.RuleFor(x => x).CountryPhoneCode();

        var result = validator.Validate(code);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("08012345678", true)] // Valid Nigerian phone number
    [InlineData("+2348012345678", true)] // Valid with country code
    [InlineData("1234567890", false)] // Invalid - not Nigerian format
    [InlineData("080123", false)] // Invalid - too short
    public void NigerianPhoneNumber_ShouldValidateCorrectly(string phone, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).NigerianPhoneNumber();

        var result = _stringInlineValidator.Validate(phone);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("+441234567890", "+44", 10, true)] // Valid UK format (10 digits excluding country code)
    [InlineData("+33123456789", "+33", 9, true)] // Valid France format (9 digits excluding country code)
    [InlineData("1234567890", "", 10, true)] // Valid without country code (10 digits)
    [InlineData("+4412345", "+44", 10, false)] // Invalid - too short for UK format
    [InlineData("123456789012", "", 10, false)] // Invalid - too long for default 10-digit format
    public void CustomPhoneNumber_ShouldValidateCorrectly(string phone, string countryCode, int length,
        bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).CustomPhoneNumber(countryCode, length);

        var result = _stringInlineValidator.Validate(phone);

        Assert.Equal(expectedIsValid, result.IsValid);
    }


    [Theory]
    [InlineData("2000-01-01", 18, true)] // Age is above 18
    [InlineData("2010-01-01", 18, false)] // Age is below 18
    public void Age_ShouldValidateCorrectly(string birthDateString, int minimumAge, bool expectedIsValid)
    {
        var birthDate = DateTime.Parse(birthDateString);
        var validator = new InlineValidator<DateTime>();
        validator.RuleFor(x => x).Age(minimumAge);

        var result = validator.Validate(birthDate);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("NoSpaces", true)] // Valid - no whitespace
    [InlineData("Has Spaces", false)] // Invalid - contains spaces
    [InlineData("Tab\tSpace", false)] // Invalid - contains a tab
    public void NoWhiteSpace_ShouldValidateCorrectly(string input, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).NoWhiteSpace();

        var result = _stringInlineValidator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("2000-01-01", true)] // Date in the past
    [InlineData("2100-01-01", false)] // Future date
    public void IsDatePast_ShouldValidateCorrectly(string dateString, bool expectedIsValid)
    {
        var date = DateTime.Parse(dateString);
        var validator = new InlineValidator<DateTime>();
        validator.RuleFor(x => x).IsDatePast();

        var result = validator.Validate(date);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("2100-01-01", true)] // Future date
    [InlineData("2000-01-01", false)] // Date in the past
    public void IsDateFuture_ShouldValidateCorrectly(string dateString, bool expectedIsValid)
    {
        var date = DateTime.Parse(dateString);
        var validator = new InlineValidator<DateTime>();
        validator.RuleFor(x => x).IsDateFuture();

        var result = validator.Validate(date);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData(123.45, 2, true)] // Two decimal places
    [InlineData(123.456, 2, false)] // More than two decimal places
    public void DecimalScale_ShouldValidateCorrectly(decimal value, int scale, bool expectedIsValid)
    {
        var validator = new InlineValidator<decimal>();
        validator.RuleFor(x => x).DecimalScale(scale);

        var result = validator.Validate(value);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("12345", @"^\d+$", true)] // Matches numeric pattern
    [InlineData("abcde", @"^\d+$", false)] // Does not match numeric pattern
    public void RegexMatch_ShouldValidateCorrectly(string input, string pattern, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).RegexMatch(pattern);

        var result = _stringInlineValidator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData(true, true)] // Value is true
    [InlineData(false, false)] // Value is false
    public void IsTrue_ShouldValidateCorrectly(bool input, bool expectedIsValid)
    {
        var validator = new InlineValidator<bool>();
        validator.RuleFor(x => x).IsTrue();

        var result = validator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData(false, true)] // Value is false
    [InlineData(true, false)] // Value is true
    public void IsFalse_ShouldValidateCorrectly(bool input, bool expectedIsValid)
    {
        var validator = new InlineValidator<bool>();
        validator.RuleFor(x => x).IsFalse();

        var result = validator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData(new[] { "apple", "banana", "cherry" }, true)] // Unique list
    [InlineData(new[] { "apple", "banana", "apple" }, false)] // Not unique list
    public void UniqueList_ShouldValidateCorrectly(string[] list, bool expectedIsValid)
    {
        var validator = new InlineValidator<string[]>();
        validator.RuleFor(x => x).UniqueList();

        var result = validator.Validate(list);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("2000-01-01", 18, 30, true)] // Valid - age within range
    [InlineData("2010-01-01", 18, 30, false)] // Invalid - age below range
    public void AgeRange_ShouldValidateCorrectly(string birthDateString, int minAge, int maxAge, bool expectedIsValid)
    {
        var birthDate = DateTime.Parse(birthDateString);
        var validator = new InlineValidator<DateTime>();
        validator.RuleFor(x => x).AgeRange(minAge, maxAge);

        var result = validator.Validate(birthDate);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("$1,234.56", "en-US", true)] // Valid format for US culture
    [InlineData("1.234,56 €", "de-DE", true)] // Valid format for German culture
    [InlineData("$1234.56", "en-US", true)] // Valid simple format for US culture without thousand separator
    [InlineData("1,234.56", "en-US", false)] // Invalid format for US culture - missing currency symbol
    [InlineData("₦1,234.56", "en-NG", true)] // Valid format for Nigerian Naira
    [InlineData("GH₵1,234.56", "en-GH", true)] // Valid format for Ghanaian Cedis
    [InlineData("₦1234.56", "en-NG", true)] // Valid simple format for Naira without thousand separator
    [InlineData("1,234.56", "en-NG", false)] // Invalid format for Naira - missing currency symbol
    [InlineData("GH₵1234", "en-GH", true)] // Valid simple format for Cedis without decimals
    public void CurrencyFormat_ShouldValidateCorrectly(string input, string cultureName, bool expectedIsValid)
    {
        var cultureInfo = new CultureInfo(cultureName);
        _stringInlineValidator.RuleFor(x => x).CurrencyFormat(cultureInfo);

        var result = _stringInlineValidator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("2023-01-01", "2023-12-31", true)] // Valid - password not expired
    [InlineData("2023-01-01", "2022-12-31", false)] // Invalid - password expired
    public void PasswordExpiryDate_ShouldValidateCorrectly(string passwordDateString, string expiryDateString,
        bool expectedIsValid)
    {
        var passwordDate = DateTime.Parse(passwordDateString);
        var expiryDate = DateTime.Parse(expiryDateString);
        var validator = new InlineValidator<DateTime>();
        validator.RuleFor(x => x).PasswordExpiryDate(expiryDate);

        var result = validator.Validate(passwordDate);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("file_name.txt", true)] // Valid file name with extension
    [InlineData("file_name.", false)] // Invalid - ends with a period
    [InlineData(".file_name", false)] // Invalid - starts with a period
    [InlineData("file name.txt", false)] // Invalid - contains spaces
    public void FileName_ShouldValidateCorrectly(string input, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).FileName();

        var result = _stringInlineValidator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }


    [Theory]
    [InlineData(45.0, 90.0, true)] // Valid coordinates
    [InlineData(-91.0, 180.0, false)] // Invalid - latitude out of range
    public void LatitudeLongitude_ShouldValidateCorrectly(double latitude, double longitude, bool expectedIsValid)
    {
        var validator = new InlineValidator<(double Latitude, double Longitude)>();
        validator.RuleFor(x => x).LatitudeLongitude((LatitudeMin: -90, LatitudeMax: 90), (LongitudeMin: -180, LongitudeMax: 180));

        var result = validator.Validate((Latitude: latitude, Longitude: longitude));

        Assert.Equal(expectedIsValid, result.IsValid);
    }


    [Theory]
    [InlineData("valid-url-slug", true)] // Valid slug
    [InlineData("Invalid URL Slug", false)] // Invalid - contains spaces and uppercase letters
    public void UrlSlug_ShouldValidateCorrectly(string input, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).UrlSlug();

        var result = _stringInlineValidator.Validate(input);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("+254701234567", true)] // Valid Kenyan phone number
    [InlineData("+254791234567", true)] // Another valid Kenyan phone number
    [InlineData("0712345678", false)] // Invalid Kenyan phone number (missing country code)
    [InlineData("254712345678", false)] // Invalid Kenyan phone number (missing plus sign)
    [InlineData("+254123456789", false)] // Invalid Kenyan phone number (too long)
    public void KenyanPhoneNumber_ShouldValidateCorrectly(string phoneNumber, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).KenyanPhoneNumber();

        var result = _stringInlineValidator.Validate(phoneNumber);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("+233201234567", true)] // Valid Ghanaian phone number
    [InlineData("+233271234567", true)] // Another valid Ghanaian phone number
    [InlineData("0312345678", false)] // Invalid Ghanaian phone number (missing country code)
    [InlineData("233312345678", false)] // Invalid Ghanaian phone number (missing plus sign)
    [InlineData("+2331234567890", false)] // Invalid Ghanaian phone number (too long)]
    public void GhanaianPhoneNumber_ShouldValidateCorrectly(string phoneNumber, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).GhanaianPhoneNumber();

        var result = _stringInlineValidator.Validate(phoneNumber);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("+201234567890", true)] // Valid Egyptian phone number
    [InlineData("+202123456789", false)] // Invalid Egyptian phone number
    [InlineData("0123456789", false)] // Invalid Egyptian phone number (missing country code)
    [InlineData("20234567890", false)] // Invalid Egyptian phone number (missing plus sign)
    [InlineData("+20123456789012", false)] // Invalid Egyptian phone number (too long)
    public void EgyptianPhoneNumber_ShouldValidateCorrectly(string phoneNumber, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).EgyptianPhoneNumber();

        var result = _stringInlineValidator.Validate(phoneNumber);

        Assert.Equal(expectedIsValid, result.IsValid);
    }


    [Theory]
    [InlineData("+33123456789", true)] // Valid French phone number
    [InlineData("+332123456789", false)] // Invalid French phone number
    [InlineData("0123456789", false)] // Invalid French phone number (missing country code)
    [InlineData("331234567890", false)] // Invalid French phone number (missing plus sign)
    [InlineData("+3312345678901", false)] // Invalid French phone number (too long)
    public void FrenchPhoneNumber_ShouldValidateCorrectly(string phoneNumber, bool expectedIsValid)
    {
        _stringInlineValidator.RuleFor(x => x).FrenchPhoneNumber();

        var result = _stringInlineValidator.Validate(phoneNumber);

        Assert.Equal(expectedIsValid, result.IsValid);
    }

}