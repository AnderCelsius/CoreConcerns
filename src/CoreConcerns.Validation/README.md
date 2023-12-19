# CoreConcerns.Validation Readme

## Overview
CoreConcerns.Validation is a comprehensive .NET library offering a suite of validation rules via extension methods, leveraging the FluentValidation framework. This library simplifies the creation of validation rules for various data types and formats, supporting custom and cultural validations, and providing a streamlined approach to enforcing business rules.

## Features
- Extensive validation methods for strings, numbers, dates, and more.
- Support for phone numbers from various countries with customization options.
- Email validation with domain-specific checks.
- Customizable regex pattern matching.
- Culture-specific currency format validation.
- Unique list checks, boolean value checks, and URL validation.
- Passport number format validation for multiple countries.
- Latitude and longitude range validation.

## Getting Started
To get started with CoreConcerns.Validation, follow these steps:

1. Install the package using NuGet:
    ```
    dotnet add package CoreConcerns.Validation
    ```
2. Add `using CoreConcerns.Validation;` to your C# file.

3. Implement validation rules using the extension methods provided by the library. Here's an example of a validator class:

```csharp
using FluentValidation;
using CoreConcerns.Validation;

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.Name).HumanName();
        RuleFor(p => p.Email).ValidEmailAddress("example.com");
        RuleFor(p => p.PhoneNumber).NigerianPhoneNumber();
        RuleFor(p => p.PassportNumber).NigerianPassportNumber();
        // Add more rules as needed
    }
}
```

## Documentation
The following is a list of key validation extension methods and their purpose:

- `HumanName`: Validates a human name with configurable length and character restrictions.
- `ValidEmailAddress`: Validates an email address, with an optional check against a specific domain.
- `PhoneNumber`: Validates a general phone number using a customizable pattern.
- `NigerianPhoneNumber`, `KenyanPhoneNumber`, etc.: Validates phone numbers specific to a country.
- `CurrencyFormat`: Validates currency format according to the provided `CultureInfo`.
- `MustBeValidHttpsUrl`: Checks if a string is a valid HTTPS URL.
- `CountryPhoneCode`: Validates country phone codes.
- `RegexMatch`: Matches a string against a provided regex pattern.
- `IsTrue`, `IsFalse`: Validates boolean values.
- `IsDatePast`, `IsDateFuture`: Validates date values against the current date.
- `DecimalScale`: Validates that a decimal number does not exceed a specified number of decimal places.
- `UniqueList`: Ensures all elements in a list are unique.
- `Age`, `AgeRange`: Validates ages against specific criteria.
- `LatitudeLongitude`: Validates geographical coordinates within specified ranges.
- `UrlSlug`: Validates that a string is a suitable URL slug.
- `NigerianNUBAN`: Validates Nigerian Uniform Bank Account Numbers.

## Best Practices
When using CoreConcerns.Validation, consider the following best practices:

- Ensure to handle validation failures gracefully, providing clear feedback to users.
- Regularly update the validation patterns to match the evolving formats.
- Use culture-specific validations where appropriate to accommodate internationalization.
- Customize validation rules to align with specific business logic and requirements.

## Contributing
Contributions to CoreConcerns.Validation are welcome. Please adhere to the contributing guidelines outlined in the repository.

## License
CoreConcerns.Validation is open-sourced software licensed under the MIT license.

## Support
If you encounter any issues or have questions, please file an issue on the GitHub repository.