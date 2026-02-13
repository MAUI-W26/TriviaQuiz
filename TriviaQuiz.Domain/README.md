# TriviaQuiz.Domain

## Responsibility

This project defines the **core domain model** and **contracts** of the application.

It represents the business concepts of the quiz system independently of:

- UI framework (MAUI)
- APIs
- Database
- File system
- Networking

This layer must remain pure and technology-agnostic.

---

## Contains

### Entities

Core business objects:

- QuizQuestion
- AnswerOption
- QuizSession
- QuizStatistics

These represent the canonical internal model.

---

### Enums

Domain-specific enumerations:

- QuestionType
- Difficulty

---

### Contracts (Interfaces)

Abstractions implemented by infrastructure layers:

- ITriviaProvider
- IQuizStorage

The domain defines contracts but does not implement them.

---

### Requests

Request models used to query providers:

- TriviaRequest

---

## Dependencies

Allowed:

- System.*
- Microsoft.Extensions.Abstractions (optional)

Forbidden:

- MAUI
- SQLite
- HttpClient
- JSON libraries
- Any infrastructure dependency

---

## Dependency Direction

This project is the root of the architecture.

All other projects depend on Domain.

Domain depends on nothing.

---

## Purpose in GoF Architecture

Defines abstractions used by:

- Strategy pattern (ITriviaProvider, IQuizStorage)
- Adapter pattern targets (QuizQuestion canonical model)
- Facade pattern interfaces

---

## Stability Requirement

This project should change rarely.

It represents the stable foundation of the system.
