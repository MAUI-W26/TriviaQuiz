# TriviaQuiz.Domain

## Responsibility

This project defines the **core domain model**, **reference data**, and **contracts** of the quiz application.

It represents business concepts independently of:

* UI framework (MAUI)
* External trivia APIs
* Database (SQLite)
* File system
* Networking
* Serialization

This layer is pure and technology-agnostic.

It is the authoritative source of truth for all domain concepts.

---

## Structure

### Contracts

Interfaces defining capabilities implemented by infrastructure layers.

* IQuizQuestionService
  Provides normalized quiz questions based on domain requests.

* IQuizStorage
  Provides persistence for quiz sessions and quiz statistics.

These abstractions ensure the domain remains independent from infrastructure implementation details.

Infrastructure layers implement these interfaces.

---

### Entities

Core business objects representing canonical quiz concepts:

* QuizQuestion
  Represents a fully normalized quiz question.

  Properties:

  * QuestionType
  * QuestionText
  * Options (list of answer strings)
  * CorrectIndex

  This model is canonical and provider-independent.

* QuizSession
  Represents the complete state of a quiz session.

  Includes:

  * Question list
  * CurrentQuestionIndex
  * SelectedAnswers
  * CorrectAnswers
  * Completion state

  This model enables session persistence and resume functionality.

* QuizStatistics
  Represents aggregated performance metrics across sessions.

* TriviaCategory
  Immutable Value Object representing a canonical quiz category.

  Each category has:

  * Key (canonical identifier)
  * DisplayName (UI presentation value)

  Categories are provider-independent.

---

### Reference Data Registry

Static authoritative category catalog:

* TriviaCategoryRegistry

Defines the complete supported set of quiz categories.

This registry provides:

* Stable category identity
* Provider-independent category representation
* Centralized reference data
* Elimination of string-based category errors
* Strong typing

External providers adapt their category systems to this registry.

Domain categories are not dynamically fetched from providers.

This ensures long-term category stability.

---

### Enums

Canonical domain enumerations:

* QuestionType

  Defines supported quiz formats:

  * Boolean
  * Choice

* Difficulty

  Defines supported quiz difficulty levels:

  * Easy
  * Medium
  * Hard

These values are canonical and independent of provider-specific representations.

Infrastructure providers map their difficulty and type systems to these enums.

---

### Requests

Domain-level query request model:

* TriviaRequest

Represents a quiz question request.

Includes:

* QuestionCount
* CategoryKey
* Difficulty
* IncludeBoolean
* IncludeChoice

This model describes domain intent, not provider-specific request parameters.

Infrastructure providers translate this request into provider-specific formats.

---

## Dependency Rules

Allowed:

* System.*

Forbidden:

* MAUI
* SQLite
* HttpClient
* JSON libraries
* Logging frameworks
* File system access
* External APIs
* Any infrastructure dependency

Domain must remain completely independent.

---

## Dependency Direction

This project is the root of the architecture.

All other projects depend on Domain.

Domain depends on nothing.

---

## Role in Architecture

Defines canonical models used by infrastructure and application layers.

Strategy pattern contracts:

* IQuizQuestionService
* IQuizStorage

Adapter pattern targets:

* QuizQuestion
* TriviaCategory

Facade pattern result types:

* QuizQuestionService returns QuizQuestion

Value Object pattern:

* TriviaCategory

Registry pattern:

* TriviaCategoryRegistry

Session state model:

* QuizSession enables persistence and resume capability

Statistics aggregation model:

* QuizStatistics

---

## Category Registry Design Rationale

Categories are defined statically rather than fetched from providers.

Reasons:

* Providers use incompatible category systems
* Providers may change category identifiers
* Providers may not expose complete category metadata

The registry ensures:

* Stable identifiers
* Consistent UI behavior
* Provider independence
* Compile-time safety

Infrastructure providers adapt to this registry using internal mapping utilities.

---

## Stability Requirement

This project is highly stable and changes rarely.

Infrastructure and UI layers must adapt to the domain.

Domain models must never adapt to external provider schemas.

This ensures architectural integrity and long-term maintainability.
