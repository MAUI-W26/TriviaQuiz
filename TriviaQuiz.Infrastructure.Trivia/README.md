# TriviaQuiz.Infrastructure.Trivia

## Responsibility

Retrieves trivia questions from external providers and adapts them into the canonical domain model.

This project fully encapsulates all external trivia API interaction and normalization logic.

It is responsible for:

* HTTP communication with external trivia providers
* Provider-specific request construction
* JSON deserialization into transport DTOs
* DTO → domain model normalization using adapters
* Primary + fallback provider orchestration
* Validation of normalized domain entities
* Logging of provider activity, failures, and fallback execution

Upper layers never interact with external APIs, DTOs, or provider logic directly.

They only consume domain entities and infrastructure services exposed via domain contracts.

---

## Architectural Role

This project implements the Infrastructure layer of the application.

It fulfills the domain contract:

```
IQuizQuestionService
```

and internally manages:

```
ITriviaProvider
TriviaProviderFactory
DTOs
Adapters
Validation
HTTP interaction
```

Application and UI layers remain completely isolated from provider-specific logic.

---

## Structure

### Providers

Concrete implementations of ITriviaProvider:

* OpenTriviaDbProvider (primary provider)
* TriviaApiProvider (fallback provider)

Each provider is responsible for:

* building provider-specific request URLs
* executing HTTP requests
* deserializing provider-specific DTOs
* mapping DTOs into canonical domain entities via adapters

Providers expose only normalized QuizQuestion objects.

---

### Adapters

* OpenTriviaAdapter
* TriviaApiAdapter

Adapters convert transport DTOs into canonical domain entities:

```
QuizQuestion
```

Adapters fully isolate provider-specific payload structure from the rest of the system.

---

### DTOs

Transport models used strictly for JSON deserialization.

Examples:

* OpenTriviaDbResponseDto
* OpenTriviaDbQuestionDto
* TriviaApiQuestionDto

DTOs never leave this project.

They are internal transport representations only.

---

### Services

#### QuizQuestionService

Facade service implementing:

```
IQuizQuestionService
```

Responsibilities:

* owns provider lifecycle through TriviaProviderFactory
* executes primary provider
* executes fallback providers when necessary
* enforces request fulfillment guarantees
* validates normalized questions
* logs execution flow and failures

This service is the only entry point exposed to upper layers.

---

### Factories

#### TriviaProviderFactory

Responsible for creating provider instances.

Factory responsibilities:

* create primary provider
* create fallback provider chain
* inject HttpClient and Logger dependencies
* encapsulate provider construction logic

Factory is internal infrastructure wiring and is not exposed outside this project.

---

### Utilities

#### OpenTriviaCategoryMap

Maps canonical domain category keys to OpenTriviaDB numeric category identifiers.

This resolves incompatibilities between canonical domain categories and provider-specific category systems.

---

#### QuestionValidator

Validates QuizQuestion invariants:

* option count validity
* correct index validity
* question text integrity

Prevents invalid data propagation to upper layers.

---

## Provider Orchestration Model

The provider execution strategy is:

```
Primary provider → OpenTriviaDB
Fallback provider → Trivia API
```

Execution flow:

```
QuizQuestionService
    → OpenTriviaDbProvider
        → success → return
        → insufficient data or failure
            → TriviaApiProvider fallback
                → return additional questions
```

The service guarantees the requested question count or throws a controlled exception.

---

## Category Model

Categories are defined in the Domain layer using:

```
TriviaCategoryRegistry
```

Infrastructure does not fetch categories from providers.

Categories are canonical, static, and independent of provider implementations.

Provider-specific mappings are handled internally via utilities such as OpenTriviaCategoryMap.

---

## External API Structural Differences

The two providers use fundamentally different response structures.

This project explicitly normalizes both formats.

### OpenTriviaDB — Wrapper Response Model

Example:

```json
{
  "response_code": 0,
  "results": [ ... ]
}
```

Characteristics:

* wrapped response envelope
* contains metadata field `response_code`
* requires explicit validation before accessing results

DTO structure:

```
OpenTriviaDbResponseDto
    → Results
        → OpenTriviaDbQuestionDto
```

Provider must validate `response_code` before mapping.

---

### Trivia API — Direct Array Response Model

Example:

```json
[
  {
    "id": "...",
    "question": { "text": "..." }
  }
]
```

Characteristics:

* no wrapper object
* root is directly an array of questions
* relies on HTTP status code for transport validation

DTO structure:

```
List<TriviaApiQuestionDto>
```

No response envelope validation is required.

---

### Normalization Outcome

Both providers ultimately produce:

```
List<QuizQuestion>
```

This ensures full provider abstraction.

Upper layers are never aware of provider-specific transport differences.

---

## Dependencies

Allowed:

* TriviaQuiz.Domain
* System.Net.Http
* System.Net.Http.Json
* System.Text.Json
* Microsoft.Extensions.Logging.Abstractions

Forbidden:

* MAUI UI components
* SQLite or storage logic
* Application layer logic
* Domain implementation logic

---

## Dependency Direction

Depends on:

```
TriviaQuiz.Domain
```

Used by:

```
TriviaQuiz.Application
```

Never used directly by MAUI.

---

## Design Patterns Implemented

Strategy

* ITriviaProvider
* OpenTriviaDbProvider
* TriviaApiProvider

Adapter

* OpenTriviaAdapter
* TriviaApiAdapter

Factory

* TriviaProviderFactory

Facade

* QuizQuestionService

Validation Layer Pattern

* QuestionValidator

---

## Stability Requirement

This project isolates all external API interaction.

Changes to provider APIs, DTO structure, or request formats must be handled exclusively in this layer.

Upper layers remain unaffected.

This ensures long-term architectural stability and provider replaceability.
