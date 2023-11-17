# rcggs-encoding

## SPA with .NET Backend and Angular/ReactJS Frontend - Coding Try-Out

### Project Overview
This repository hosts a Single Page Application (SPA) developed as part of a coding try-out. The application features a .NET 7 backend and a frontend built using Angular. It demonstrates a long-running job processing scenario with real-time updates on the UI.

### Core Functionality
- **Text Encoding**: Users can input text in the frontend, hit "Convert", and have the text encoded into base64 format.
- **Real-time Updates**: The encoded string is sent back to the client one character at a time, with random pauses of 1-5 seconds on the server before sending each character.
- **Interactive UI**: The UI displays the encoded string in real time, updating with each new character received.
- **Process Control**: Users can cancel the ongoing encoding process using a "Cancel" button.

### Technical Highlights
- **Backend**: .NET 7, showcasing the latest features and best practices in .NET development.
- **Frontend**: Angular, with a focus on a responsive and interactive user experience.
- **Styling**: Bootstrap used for neat and responsive UI design.
- **Architecture**: Business logic implemented as a service, with comprehensive unit tests.

### Repository Contents
- Source code for both the .NET backend and the Angular frontend.
- Unit tests covering the backend business logic.
- Documentation and setup instructions.

### Example Usage
- **Input**: "Hello, World!"
- **Output**: Base64 encoded string "SGVsbG8sIFdvcmxkIQ=="
- **Client Experience**:
  - User sees "S" after a random pause.
  - User then sees "SG" after another pause, and so on.


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What you need to install the software:

- [.NET 7.0 SDK](https://dotnet.microsoft.com/download/dotnet/7.0)
- [Node.js](https://nodejs.org/)
- [Angular CLI](https://angular.io/cli)

### Installing

A step-by-step guide to getting a development environment running:

1. **Clone the Repository**
   Clone the repository to your local machine.

2. **Start the API**
   Navigate to the API project directory and run the .NET application.
   ```bash
   cd path/to/api/project
   dotnet run
   ```
   The API will be available at `http://localhost:7280/`.

3. **Start the Frontend Application**
   Navigate to the frontend project directory and start the application.
   ```bash
   cd path/to/frontend/project
   npm install
   npm start
   ```
   The frontend application will be available at `http://localhost:4200/` (default Angular port).

### API Reference

The backend API is accessible at `http://localhost:7280/`. Below are the available endpoints:

- `POST /Encoding/start`: Starts the encoding process for the given input text.
- `GET /Encoding/get/{requestId}`: Retrieves the next character of the encoded string based on the provided request ID.
- `POST /Encoding/cancel/{requestId}`: Cancels the ongoing encoding process for the given request ID.

### Running the Tests

The project includes unit tests for the API. To run these tests, navigate to the test project directory and execute the following command:

```bash
dotnet test
```

These tests cover various scenarios for the encoding API, ensuring the correctness of its functionality.

## Authors

- **Aldrin Java** - *Initial work* - [AldrinJava](https://github.com/YourUsername)

