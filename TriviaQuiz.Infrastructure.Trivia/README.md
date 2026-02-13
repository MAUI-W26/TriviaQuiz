# TriviaQuiz.Infrastructure.Trivia

## Responsibility

This project implements trivia question retrieval from external APIs.

It converts external API responses into the internal domain model.

The UI and application layers never interact directly with external APIs.

---

## Contains

### Providers (Strategy Pattern)

Concrete implementations of ITriviaProvider:

- OpenTriviaDbProvider
- TriviaApiProvider

Each provider handles one external API.

---

### Adapters (Adapter Pattern)

Maps external DTOs into QuizQuestion:

- OpenTriviaAdapter
- TriviaApiAdapter

External models are never exposed outside this layer.

---

### DTOs

Raw API response models used for JSON deserialization.

These types are isolated to this project.

---

### Services (Facade Pattern)

QuizQuestionService

Coordinates:

- Provider selection
- Filtering
- Validation
- Aggregation

Provides a simplified interface to the rest of the application.

---

### Factories (Factory Method)

Creates provider instances based on request configuration.

Example:

TriviaProviderFactory

---

## Dependencies

Allowed:

- TriviaQuiz.Domain
- System.Net.Http
- System.Text.Json

Forbidden:

- MAUI UI components
- SQLite
- Preferences
- File system persistence

---

## Dependency Direction

Depends on:

- TriviaQuiz.Domain

Used by:

- TriviaQuiz.Application
- TriviaQuiz.App

---

## GoF Patterns Implemented

Strategy:
- ITriviaProvider
- Concrete providers

Adapter:
- API DTO → QuizQuestion

Facade:
- QuizQuestionService

Template Method:
- TriviaProviderBase
