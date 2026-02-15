# TriviaQuiz

## Overview

TriviaQuiz is a cross‑platform quiz application built with **.NET 9** and **.NET MAUI**, following a strict layered architecture and clean separation of concerns.

The system is composed of multiple independent projects, each with a clearly defined responsibility, allowing for testability, extensibility, and infrastructure substitution.

The application supports:

* External trivia API integration
* Persistent quiz session recovery
* Persistent statistics tracking
* Multi‑platform UI (Android, Windows, macOS, iOS)
* Strategy‑based storage (SQLite primary, JSON fallback)
* Fully decoupled domain and infrastructure layers

---

## Screenshots

![Screenshot 1](/Screenshots/Screenshot%202026-02-15%20093822.png)

![Screenshot 2](/Screenshots/Screenshot%202026-02-15%20093844.png)

![Screenshot 3](/Screenshots/Screenshot%202026-02-15%20093838.png)

![Screenshot 4](/Screenshots/Screenshot%202026-02-15%20093903.png)

![Screenshot 5](/Screenshots/Screenshot%202026-02-15%20093934.png)

![Screenshot 6](/Screenshots/Screenshot%202026-02-15%20093943.png)

---

## Solution Structure

Each project contains its own detailed README with internal design documentation.

### TriviaQuiz.Domain

Defines the core business model and contracts.

Responsibilities:

* Domain entities
* Value objects
* Enumerations
* Storage contracts
* Trivia provider contracts

See:

```
TriviaQuiz.Domain/README.md
```

---

### TriviaQuiz.Application

Implements application orchestration logic.

Responsibilities:

* Quiz lifecycle management
* Session coordination
* Statistics updates
* Application‑level services

Key service:

```
QuizService
```

See:

```
TriviaQuiz.Application/README.md
```

---

### TriviaQuiz.Infrastructure.Trivia

Implements external trivia provider access.

Responsibilities:

* API integration
* DTO mapping
* Provider failover strategy

Primary provider:

```
OpenTriviaDB
```

See:

```
TriviaQuiz.Infrastructure.Trivia/README.md
```

---

### TriviaQuiz.Infrastructure.Storage

Implements persistent storage strategies.

Responsibilities:

* Quiz session persistence
* Statistics persistence
* Storage abstraction

Primary storage:

```
SQLite
```

Fallback storage:

```
JSON
```

See:

```
TriviaQuiz.Infrastructure.Storage/README.md
```

---

### TriviaQuiz.App

Implements the MAUI user interface.

Responsibilities:

* Views
* ViewModels
* Navigation
* UI interaction

See:

```
TriviaQuiz.App/README.md
```

---

## Architectural Model

Layer dependency direction:

```
App
 ↓
Application
 ↓
Domain
 ↑
Infrastructure
```

Infrastructure depends on Domain.

Application depends on Domain and Infrastructure contracts.

UI depends only on Application.

Domain depends on nothing.

---

## Persistence Model

Session and statistics persistence use a Strategy pattern.

Primary:

```
SQLiteQuizStorage
```

Fallback:

```
JsonQuizStorage
```

Facade:

```
QuizStorageFacade
```

This ensures resilience against storage failures.

---

## Session Lifecycle

Session creation:

```
MainMenu
 → GameSetup
 → QuizService.StartNewSessionAsync
 → Session persisted
```

Session progression:

```
QuizService.SelectAnswerAsync
QuizService.AdvanceAsync
Session persisted after each change
```

Session completion:

```
QuizService.AdvanceAsync
 → marks session completed
 → updates statistics
```

Session cleanup:

```
ResultPage
 → QuizService.AbandonSessionAsync
 → storage.DeleteSessionAsync
```

Session recovery:

```
App start
 → QuizService.HasActiveSessionAsync
 → ResumeSessionAsync if exists
```

---

## Navigation Flow

Application states:

```
MainMenuPage
  ↓
GameSetupPage
  ↓
QuizPage
  ↓
ResultPage
  ↓
StatisticsPage
  ↓
MainMenuPage
```

Session exists only during QuizPage lifecycle.

StatisticsPage and MainMenuPage do not require session.

---

## Statistics Model

Statistics tracked:

* Games played
* Best score
* Total correct answers
* Total questions answered
* Average accuracy

Statistics persist across application restarts.

---

## Key Design Patterns Used

Strategy

* IQuizStorage
* SQLiteQuizStorage
* JsonQuizStorage

Adapter

* DTO → Domain mapping

Facade

* QuizStorageFacade
* QuizService

Factory Method

* QuestionViewFactory

Dependency Injection

* All services injected through MAUI container

---

## Requirements

Required SDK:

```
.NET SDK 9.0
```

MAUI workload:

```
dotnet workload install maui
```

Verify:

```
dotnet --version
```

Must output:

```
9.x.x
```

.NET 10 is not required.

---

## Running the Application

Restore dependencies:

```
dotnet restore
```

Build:

```
dotnet build
```

Run (Windows):

```
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

Run (Android emulator):

Use Visual Studio or:

```
dotnet build -t:Run -f net9.0-android
```

---

## Storage Location

SQLite database path:

Platform‑specific application data directory.

Example Windows:

```
C:\Users\<User>\AppData\Local\Packages\...\LocalState\triviaquiz.db
```

---

## Capabilities Summary

Supports:

* Persistent quiz sessions
* Crash‑safe recovery
* Persistent statistics
* API failover
* Platform‑independent architecture
* Full dependency injection
* Strict domain isolation

---

## Stability Guarantees

System ensures:

* No UI layer storage access
* No UI layer API access
* Fully isolated domain logic
* Recoverable application state
* Deterministic session lifecycle

---

## Future Extensions

Architecture allows easy addition of:

* New trivia providers
* Cloud storage
* User accounts
* Multiplayer sessions
* Leaderboards

No domain changes required.

---

## License

Private educational project.

