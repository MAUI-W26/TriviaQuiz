# TriviaQuiz.Application

## Responsibility

This project orchestrates application logic.

It acts as a coordination layer between:

- UI layer
- Trivia providers
- Storage systems

It contains use-case oriented services.

---

## Contains

Application services such as:

- QuizGameService
- StatisticsService

These services:

- Control quiz flow
- Track score
- Load/save progress
- Retrieve questions

---

## Dependencies

Allowed:

- TriviaQuiz.Domain
- TriviaQuiz.Infrastructure.Trivia
- TriviaQuiz.Infrastructure.Storage

Forbidden:

- MAUI UI components
- XAML
- ViewModels
- Views

---

## Purpose

Keeps UI thin.

Prevents business logic from leaking into ViewModels.

Improves testability.

---

## GoF Patterns Used

Facade:
- Provides simplified interfaces to UI layer

Strategy:
- Uses ITriviaProvider and IQuizStorage abstractions
