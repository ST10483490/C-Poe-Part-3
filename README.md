# Cybersecurity Awareness Bot & Task Assistant (Part 3)

An interactive C# Windows Forms application designed to promote online safety through AI guidance, an interactive cybersecurity quiz game, SQL Server task management, simulated Natural Language Processing (NLP), and session activity logging.

##  Features & Architecture

### 1. Task Assistant & Database Integration
- Connected to **Microsoft SQL Server Express** (`CyberSecurityDB`) using `Microsoft.Data.SqlClient`.
- Stores tasks with titles, descriptions, and custom reminder timeframes in the `CyberTasks` table.
- Parameterized T-SQL execution to prevent SQL injection.

### 2. Cybersecurity Mini-Game (Quiz)
- Built-in engine (`QuizEngine.cs`) containing 11 questions across core topics:
  - Phishing & Scam Defense
  - Password Management & 2FA
  - Safe Browsing & Ransomware
- Features mixed question types (Multiple Choice and True/False), immediate educational feedback, and automated performance scoring.

### 3. Natural Language Processing (NLP) Simulation
- Utilizes Regular Expressions (`Regex`) and flexible keyword detection in `ChatEngine.cs`.
- Intelligently parses user intents such as:
  - *"Remind me to update my password tomorrow"*
  - *"Add a task to enable two-factor authentication"*

### 4. Activity Log Feature
- Tracks all key session actions via `ActivityLogger.cs`.
- Displays recent summaries when prompted with commands like:
  - *"Show activity log"*
  - *"What have you done for me?"*

---

##  Requirements & Setup Instructions

### Prerequisites
- Visual Studio 2022 (.NET 6.0 / 8.0 SDK or .NET Framework)
- Microsoft SQL Server Management Studio (SSMS) & SQL Express


