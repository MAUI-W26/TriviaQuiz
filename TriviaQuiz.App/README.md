# TriviaQuiz.App

## Responsibility

This is the MAUI UI project.

It contains:

- Views
- ViewModels
- Navigation
- UI rendering logic

It is responsible only for presentation.

---

## Contains

Views:

- MainMenuPage
- GameSetupPage
- QuizPage
- ResultPage
- StatisticsPage

Factories:

- QuestionViewFactory

Shell navigation.

---

## Dependencies

Allowed:

- TriviaQuiz.Domain
- TriviaQuiz.Application

Forbidden:

- Direct API calls
- Direct SQLite access

All external interaction must go through services.

---

## GoF Patterns Implemented

Factory Method:

QuestionViewFactory creates views based on QuestionType.

---

## Architectural Rule

UI must never directly access:

- APIs
- SQLite
- JSON storage

Only Application services are allowed to coordinate logic.
