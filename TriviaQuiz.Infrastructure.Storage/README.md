# TriviaQuiz.Infrastructure.Storage

## Responsibility

This project implements persistence mechanisms for:

- Quiz progress
- Quiz statistics

It provides interchangeable storage strategies.

---

## Contains

### SQLite Implementation (Primary Strategy)

SQLiteQuizStorage

Uses sqlite-net-pcl to persist:

- QuizSession
- QuizStatistics

SQLite is the primary persistence mechanism.

---

### JSON File Implementation (Alternative Strategy)

JsonQuizStorage

Stores the same data using:

- System.Text.Json
- Local file system

Used for learning, testing, and fallback scenarios.

---

### Storage Facade

Optional storage facade that simplifies usage from application layer.

---

## Dependencies

Allowed:

- TriviaQuiz.Domain
- sqlite-net-pcl
- System.Text.Json
- File system APIs

Forbidden:

- MAUI UI components
- HttpClient
- Trivia providers

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
- IQuizStorage
- SQLiteQuizStorage
- JsonQuizStorage

Adapter:
- SQLite record ↔ domain model

Facade:
- Storage service abstraction
