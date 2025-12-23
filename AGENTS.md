# Agent Guidelines for NMoneys-Web

## Project Overview
The project is structured as a .NET solution using the SDK-style project format. It leverages modern C# features and best practices to ensure maintainability and readability.

The project contains the following main components:
- **API**: A RESTful API built with ASP.NET Core and FastEndpoints, providing endpoints for currency information retrieval.
- **Web**: A Blazor Standalone WebAssembly that provides visuals for currency information.

### Key Technologies

* .NET 10 and C# 14
* ASP.NET Core with [FastEndpoints](https://fast-endpoints.com/)
* Swagger/OpenAPI for API documentation via [SwashFastEndpoints.Swagger](https://fast-endpoints.com/docs/swagger-support)
* Blazor WebAssembly
* [NUnit](https://docs.nunit.org/) and [NSubstitute](https://nsubstitute.github.io/) for testing
* [Alba](https://jasperfx.github.io/alba/) for integration testing
* [Mapster](https://github.com/MapsterMapper/Mapster) for object mapping
* [Lamar](https://jasperfx.github.io/lamar/) for dependency injection

## Build/Test Commands
- **Build**: `dotnet build nmoneys-web.slnx`
- **Test**: `dotnet test` (runs all tests)
- **Single test**: `dotnet test --project "TestProject" --filter "FullyQualifiedName=Namespace.ClassName.MethodName"`
- **Lint**: Enforced during build via `TreatWarningsAsErrors`, `EnforceCodeStyleInBuild`, and `CodeAnalysisTreatWarningsAsErrors`

## General Instructions
* use English for all code, comments and documentation
* make only high confidence changes, avoid speculative changes
* write code with maintainability in mind, adding comments as support only on the subject of why something is done a certain way
* when stuck:
  * ask a clarifying question or propose a short plan
  * do not make large speculative changes

## Code Style
- **Modern C#**: Prefer using latest C# features of the language version specified in the project files
- **Formatting**: Follow `.editorconfig` rules (tabs=4, PascalCase types/methods, camelCase locals/parameters)
- **Imports**: Organized with system directives first, separated by groups
- **Types**: Use built-in types (`int`, `string`) over BCL types
- **Naming**: Follow `editor.config` rules (PascalCase for types/methods/properties, camelCase for locals/parameters, `_camelCase` for private fields and local functions)
- **Error handling**: Use exceptions for exceptional cases, prefer null propagation for null checks
- **Nullable**: Enabled project-wide, use `?` for nullable reference types
- **Access modifiers**: Required for non-interface members. Modifier order: `public,private,protected,internal,file,const,static,extern,new,virtual,abstract,sealed,override,readonly,unsafe,required,volatile,async`
- **Expression-bodied**: Preferred for properties/accessors/indexers/methods with single expressions
- **Pattern matching**: Preferred over `as`/`is` with casts
- **Variable Declarations**: Prefer `var` when the type is obvious from the right-hand side, but use explicit types when the type is not clear. Use target-typed `new` expressions where applicable.
- **this**: don't use `this` keyword for instance members unless absolutely necessary to avoid ambiguity

### Code Patterns

* **Dependency Injection**: use constructor injection for dependencies, prefer interfaces for abstractions
  * prefer automatic Lamar registration
  * when different from the default, create registries that are to be scanned

### Api Project
* **Request/Response**: place request and response objects (REPR pattern) in the same file as the endpoint and name them after the endpoint with the `Request` and `Response`postfixes. Use records for immutability
* **ViewModels/DTOs**: extrat complex data inside request/response into models, using records for immutability
* **Endpoints**: Postfixes: `Listing` for returning multiple entitites, `Retrieval` for returning single entities
  * document endpoints with Swagger annotations
  * keep endpoint logic thin by moving actions to commands handled by the FastEndpoint's _Command Bus_

### Web Project

## Testing
- Test projects: `test/Api/Api.Tests.csproj` and `test/Web/Web.Tests.csproj`
- Run specific test: Use `--filter` with fully qualified name
- Prefer one test class per production file and one test class per test file
- test classes should be named after the production class they test with `Tester` suffix
- test methods are named using the pattern: MethodName_StateUnderTest_ExpectedBehavior
- tests should follow the Arrange-Act-Assert (AAA) pattern for clarity
- test both positive and negative scenarios
- tests should be able to run in any order or in parallel without affecting each other
- support types in tests are to be placed in a separate sub-folder named "Support" within the test project with _internal_ accessibility
- minimize the logic in tests, specially avoid branching logic
- there are going to be unit tests for classes with logic and integration tests for endpoint testing. Blazor components should also have unit tests
- **Test-First**: when asked to do so: write or update tests first, and then write the production code to make the tests pass.

### Frameworks
- Testing Framework: NUnit, Mocking Library: NSubstitute
- NUnit _Constraint Model_ must be used for assertions, using assertion chaining when it improves readability
- parametrized tests can be used to reduce code duplication when testing multiple input scenarios for similar logic

## Code Quality

* Avoid code duplication by using refactoring techniques like 'extract method' or 'extract class'.
* keep methods small and focused on a single task, following Clean Code principles
* keep names consistent
* prefer string interpolation over string concatenation or fomatting
* implement disposable pattern correctly when dealing with unmanaged resources
* declare arguments are non-nullable by default, using nullable reference types where applicable
* use `is null` or `is not null` for null checks instead of direct comparisons to `null`
* trust types null annotations and don't add null checking when the type system already guarantees non-nullability

### Agent Tool Usage Best Practices

To ensure efficient, safe, and accurate operations, adhere to the following best practices when utilizing the available tools:

*   **`read_file`**:
    *   Always use `read_file` to inspect file content before attempting any modifications (e.g., with `replace` or `write_file`). This ensures a current understanding of the file's state and context.
    *   For large files, utilize `limit` and `offset` parameters to read specific sections rather than attempting to load the entire file, which can be inefficient.
    *   Never make assumptions about file content or existence; always verify with `read_file` or `glob`.

*   **`replace`**:
    *   **Pre-read File**: Before any `replace` operation, `read_file` to obtain the exact `old_string` (including surrounding context, whitespace, and indentation) to avoid mismatches.
    *   **Exact Match**: `old_string` and `new_string` must be exact literal representations of the text to be replaced and the replacement text, respectively. Do not escape special characters unless they are literally part of the string.
    *   **Context is King**: For single replacements, always include at least 3 lines of context before and after the target text in `old_string`. This helps ensure the replacement targets the correct instance.
    *   **Atomic Changes**: Break down complex or multi-line changes into multiple, smaller `replace` calls. This improves readability, reduces the risk of errors, and makes changes easier to review.
    *   **Verify Changes**: After a `replace` operation, use `read_file` again to confirm that the changes have been applied correctly.
    *   **Multiple Replacements**: When replacing multiple occurrences, explicitly set `expected_replacements` to the anticipated number.

*   **`run_shell_command`**:
    *   **Explain Critical Commands**: Always provide a brief explanation to the user before executing commands that modify the file system or system state.
    *   **Prioritize Non-Interactive**: Prefer non-interactive commands. If an interactive command is necessary, inform the user they may need to provide input.
    *   **Output Management**: Use quiet flags (e.g., `-q`, `--silent`) or redirect output to temporary files (e.g., `command > /tmp/out.log 2> /tmp/err.log`) for commands that produce verbose output. Inspect relevant parts of the output as needed.
    *   **Build/Test/Lint**: Always identify and use the project-specific build, test, and lint commands as outlined in the "Build/Test Commands" section and the `.editorconfig`.

*   **`search_file_content` / `glob`**:
    *   **Efficiency**: Prefer `search_file_content` for content searches and `glob` for file path searches over general `run_shell_command("grep ...")` or `run_shell_command("find ...")` for their optimized performance and output handling.
    *   **Targeted Search**: Use `include` and `dir_path` parameters to narrow down searches, reducing noise and improving performance, especially in large repositories.
    *   **Pattern Accuracy**: Be precise with regular expressions for `search_file_content` and glob patterns for `glob`. Use `\b` for whole-word matching in regex when appropriate.

*   **General Tool Usage**:
    *   **Parallelism**: Execute multiple independent tool calls in parallel (e.g., multiple `read_file` or `search_file_content` calls) to improve efficiency when tasks don't have direct dependencies.
    *   **Output Token Efficiency**: Adhere strictly to guidelines for minimizing tool output tokens to manage context window usage effectively.

## Commit messages
* use conventional commits style `<type>[optional scope]: <description>` for commit messages. For example:
  - `feat(api): :sparkles: add new endpoint for currency listing`
  - `fix(web): :lipstick: resolve issue with currency symbol formatting`
* _gitmojis_ can be used at the start of commit description to improve readability, but are not mandatory

## Guardrails
* avoid introducing new dependencies unless absolutely necessary
* ask the developer before introducing new dependencies to avoid dependency bloat and ensure security
* ask before making large architectural changes
* ask before considering making any sort of commit
* prefer making multiple small changes that can be reviewed easily over a single large change
