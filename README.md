# CoreConcerns

CoreConcerns is a suite of .NET class libraries designed to provide simplified and robust approaches to common cross-cutting concerns: caching, logging, and validation.

## Features

- In-memory and Redis caching with easy setup and flexible integration.
- Structured logging for easy diagnostics and troubleshooting.
- Fluent validation for business rules and data integrity.

## Projects

The CoreConcerns suite consists of the following projects:

- **CoreConcerns.Caching**: For caching implementations, including in-memory and Redis.
- **CoreConcerns.Logging**: For structured and configurable logging.
- **CoreConcerns.Validation**: For validation of business objects and data.

## Getting Started

### Installation

You can install each component of CoreConcerns separately via NuGet:

```shell
# For caching
dotnet add package CoreConcerns.Caching

# For logging
dotnet add package CoreConcerns.Logging

# For validation
dotnet add package CoreConcerns.Validation
```

Refer to the README.md of each project for detailed usage instructions.

## Contributing
We welcome contributions to any part of the CoreConcerns suite. Please read our contributing guidelines in each project's repository before submitting pull requests.




# CoreConcerns.Logging

The CoreConcerns.Logging library provides structured and configurable logging mechanisms as part of the CoreConcerns suite.

## Features

- Structured logging that is easy to read and query.
- Integration with various logging frameworks.
- Customizable log levels and output targets.

## Getting Started

### Installation

```shell
dotnet add package CoreConcerns.Logging
```

### Usage

Set up logging in your application's configuration and inject the logging provider into your services.

More detailed usage instructions and examples will be provided in the documentation.


# CoreConcerns.Validation

CoreConcerns.Validation is a library for validating business objects and ensuring data integrity as part of the CoreConcerns suite.

## Features

- Fluent validation rules for easy readability and maintenance.
- Integration with .NET data annotations.
- Support for complex validation scenarios.

## Getting Started

### Installation

```shell
dotnet add package CoreConcerns.Validation
```

### Usage

Define validation rules for your business objects and use the validation provider to enforce these rules in your application.

Examples and more comprehensive instructions will be provided in the documentation.
