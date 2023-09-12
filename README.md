# BackBlaze Cloud Storage API for .NET (Standard/Core)

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Version](https://img.shields.io/badge/version-1.0.0-green.svg)

## Description

The BackBlaze Cloud Storage API for .NET (Standard/Core) is a versatile library that simplifies interactions with BackBlaze B2 Cloud Storage services for .NET applications. Whether you're building a web application, a desktop tool, or a backend service, this library provides easy-to-use methods for managing files, buckets, and more within your BackBlaze account.

## Table of Contents

1. [Installation](#installation)
2. [Usage](#usage)
3. [API Documentation](#api-documentation)
4. [Configuration](#configuration)
5. [Testing](#testing)
6. [Contributing](#contributing)
7. [License](#license)
8. [Contact Information](#contact-information)

## Installation

You can quickly add the BackBlaze Cloud Storage API library to your .NET project using [NuGet](https://www.nuget.org/packages/BackBlaze.Storage/):

```shell
dotnet add package BackBlaze.Storage
```

## Usage

Initializing the BackBlaze Client
To get started, you need to create an instance of the BackBlazeClient class and provide your BackBlaze account credentials:

```csharp
using BackBlaze.Storage;

var client = new BackBlazeClient("YourAccountID", "YourApplicationKey");
```

# Using the API Interfaces

Files Interface (IFiles)
The IFiles interface provides methods for working with files in BackBlaze storage:

```csharp
// Example usage of IFiles methods
    var file = await client.Delete("fileId", "fileName");
    var download = await client.DownloadById("fileId");
    var uploadUrl = await client.GetUploadUrl();
// ... and more
```

Buckets Interface (IBuckets)
The IBuckets interface provides methods for managing buckets in BackBlaze storage:

```csharp
// Example usage of IBuckets methods
    var bucket = await client.Create("bucketName", BucketTypes.Private);
    var bucketsList = await client.GetList();
// ... and more
```

Keys Interface (IKeys)
The IKeys interface provides methods for managing API keys in BackBlaze:

```csharp
// Example usage of IKeys methods
    var keysList = await client.GetList();
    var apiKey = await client.Create("keyName", new[] { "readFiles" });
// ... and more
```

Large Files Interface (ILargeFiles)
The ILargeFiles interface provides methods for working with large files in BackBlaze storage:

```csharp
// Example usage of ILargeFiles methods
    var largeFile = await client.StartLargeFile("largeFile.txt");
    var uploadPartUrl = await client.GetUploadPartUrl("fileId");
// ... and more
```

For more details on these interfaces and their methods, please refer to the API documentation.

## API Documentation

For in-depth information about the available API endpoints, request parameters, and response formats, consult the API Documentation.

## Configuration

The BackBlaze client can be configured using environment variables or a configuration file.

### API Configuration

For API configuration, you need to update the `appsettings.json` file with the following parameters under the `"B2Settings"` section:

```json
"B2Settings": {
    "KeyId": "YOUR KEY ID",
    "ApplicationKey": "YOUR APPLICATION KEY"
}
```
Ensure that you provide your actual BackBlaze API Key ID and Application Key in place of the placeholder values.

### Client Configuration

To configure the client, create a new instance of the B2NetClient class with the provided settings:

```csharp
public B2NetClient(B2Settings options) : base(options.KeyId, options.ApplicationKey)
```

This constructor takes the BackBlaze API Key ID and Application Key as parameters. Be sure to initialize it with the values from your appsettings.json or from your preferred configuration source.

## Testing

You can run the provided test cases to ensure the library functions correctly. Follow the instructions below to configure the testing environment.

### Configuration

To set up the testing environment, you'll need to update the `TestConstants` class parameters. Open the `TestConstants.cs` file and modify the following values:

```csharp
public static class TestConstants {
    public static string KeyId
    {
        get { return ""; } // Update with your BackBlaze Key ID
    }

    public static string AccountId {
        get { return ""; } // Update with your BackBlaze Account ID
    }

    public static string ApplicationKey {
        get { return ""; } // Update with your BackBlaze Application Key
    }

    public static string BucketId {
        get { return ""; } // Update with your BackBlaze Bucket ID
}
```

Ensure that you replace the placeholder values with your actual BackBlaze Account ID and Bucket ID where necessary.

After updating these constants, you can proceed to execute the test cases to verify the functionality of the library.

## Contributing

We welcome contributions! If you'd like to contribute to this project.

## License

This project is licensed under the MIT License. For detailed licensing information, please refer to the [LICENSE.md](LICENSE.md) file.

## Contact Information

If you have any questions, feedback, or encounter issues, please don't hesitate to reach out:

- Email: [hello@perfigent.com](mailto:hello@perfigent.com)

**LIFE RUNS ON CODE**
