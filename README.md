# Student API Documentation

Welcome to the documentation for the Student API, built using ASP.NET Core MVC with .NET 6.0. This API is designed to handle student-related data and provides various endpoints to interact with the data. In this documentation, you will find detailed information about each endpoint, request and response formats, error handling, and usage examples.

## Objectives:

The objectives of the Student API are as follows:

1. **Student Data Management**: The API aims to provide some of the CRUD (Create, Read, Update, Delete) operations to manage student-related data, including their personal information and enrollment in subjects.

2. **Subject and Topic Management**: The API facilitates the management of subjects and their associated topics, allowing administrators to add new subjects and topics to the database.

3. **Topic Completion Tracking**: The API enables the tracking of topic completion for each student, allowing administrators to mark topics as completed or not completed for individual students.

4. **Error Handling and Logging**: The API implements robust error handling and logging mechanisms to ensure proper tracking and resolution of any unexpected issues.


## Methodologies:

The following methodologies are used to achieve the objectives of the Student API:

1. **RESTful API Design**: The API is designed following REST (Representational State Transfer) principles, ensuring a clear and intuitive structure for endpoints, and adhering to standard HTTP methods (GET, POST, etc.).

2. **Model-View-Controller (MVC) Architecture**: The API follows the MVC pattern to separate concerns and maintain a structured codebase. The models represent data entities, views handle the presentation layer (JSON responses), and controllers manage the API endpoints' logic.

3. **Entity Framework Core**: The API uses Entity Framework Core to interact with the underlying database, making it easy to perform CRUD operations on the data models.

4. **Error Handling**: The API implements proper error handling to provide meaningful error responses to clients, ensuring they understand the cause of any issues.

5. **Logging**: The API utilizes a logging framework (Microsoft.Extensions.Logging) to log key events, such as endpoint calls, SQL-related errors, and exceptions, providing valuable information for debugging and monitoring.

## Best Practices:

To ensure the reliability, security, and maintainability of the Student API, the following best practices are employed:

1. **Input Validation**: All user input is thoroughly validated to prevent security vulnerabilities such as SQL injection and data manipulation attacks.

2. **Use of Asynchronous Operations**: Asynchronous programming is utilized for database operations to improve scalability and responsiveness, especially when dealing with multiple concurrent requests.

3. **Dependency Injection**: The API relies on dependency injection to manage dependencies and promote loosely coupled components, enhancing testability and modularity.

4. **Proper HTTP Status Codes**: The API returns appropriate HTTP status codes to indicate the success or failure of requests, adhering to standard conventions.

5. **Consistent Naming Conventions**: The API follows consistent naming conventions for endpoints, request/response properties, and database tables/columns, enhancing code readability and maintainability.

6. **Versioning**: The API may implement versioning to allow for future changes while maintaining backward compatibility for existing clients.

7. **Security Measures**: Security measures, such as secure authentication and authorization mechanisms, may be implemented to protect sensitive student data.

8. **Documentation**: The API documentation is comprehensive, ensuring developers have clear guidelines on how to use the API effectively and troubleshoot issues.

## Database Schema

To provide a comprehensive understanding of the Student API, below is the database schema used to store and manage student-related data:

### `CompletedTopics` Table

This table stores information about completed topics for individual students.

| Column     | Type      | Constraints            | Description                                     |
|------------|-----------|------------------------|-------------------------------------------------|
| Id         | SERIAL    | PRIMARY KEY            | Unique identifier for each completed topic.     |
| StudentId  | INTEGER   | NOT NULL               | ID of the student associated with the topic.    |
| TopicId    | INTEGER   | NOT NULL               | ID of the completed topic.                      |
| IsComplete | BOOLEAN   | NOT NULL               | Indicates whether the topic is completed or not.|

Constraints:
- FK_CompletedTopics_Students: FOREIGN KEY (StudentId) REFERENCES Students(StudentId) ON DELETE CASCADE
  (Foreign key constraint referencing the `StudentId` column in the `Students` table. This ensures that when a student is deleted, their completed topics are also deleted (CASCADE).)

### `Students` Table

This table stores information about individual students.

| Column     | Type      | Constraints            | Description                                     |
|------------|-----------|------------------------|-------------------------------------------------|
| StudentId  | SERIAL    | PRIMARY KEY            | Unique identifier for each student.             |
| FirstName  | VARCHAR   | NOT NULL               | First name of the student.                      |
| SecondName | VARCHAR   | NOT NULL               | Last name of the student.                       |
| Title      | VARCHAR   |                        | Title of the student (e.g., Mr., Ms.).          |
| Email      | VARCHAR   | NOT NULL               | Email address of the student.                   |
| DOB        | DATE      |                        | Date of birth of the student.                   |

### `Student_Subjects` Table

This table stores the many-to-many relationship between students and subjects. It indicates which subjects each student is enrolled in.

| Column     | Type      | Constraints            | Description                                     |
|------------|-----------|------------------------|-------------------------------------------------|
| Id         | SERIAL    | PRIMARY KEY            | Unique identifier for each record.              |
| StudentId  | INTEGER   | NOT NULL               | ID of the student associated with the subject.  |
| SubjectId  | INTEGER   | NOT NULL               | ID of the subject the student is enrolled in.   |

Constraints:
- FK_Student_Subjects_Students: FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
  (Foreign key constraint referencing the `StudentId` column in the `Students` table.)
- FK_Student_Subjects_Subjects: FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId)
  (Foreign key constraint referencing the `SubjectId` column in the `Subjects` table.)

### `Subjects` Table

This table stores information about subjects available in the educational curriculum.

| Column     | Type      | Constraints            | Description                                     |
|------------|-----------|------------------------|-------------------------------------------------|
| SubjectId  | SERIAL    | PRIMARY KEY            | Unique identifier for each subject.             |
| Title      | VARCHAR   | NOT NULL               | Title of the subject.                           |
| ImageUrl   | VARCHAR   |                        | URL to an image representing the subject.       |
| Duration   | VARCHAR   |                        | Duration of the subject in minutes (optional).  |

### `Topics` Table

This table stores information about topics associated with each subject.

| Column     | Type      | Constraints            | Description                                     |
|------------|-----------|------------------------|-------------------------------------------------|
| TopicId    | SERIAL    | PRIMARY KEY            | Unique identifier for each topic.               |
| Title      | VARCHAR   | NOT NULL               | Title of the topic.                             |
| SubjectId  | INTEGER   | NOT NULL               | ID of the subject associated with the topic.    |

Constraints:
- FK_Topics_Subjects: FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId) ON DELETE CASCADE
  (Foreign key constraint referencing the `SubjectId` column in the `Subjects` table. This ensures that when a subject is deleted, its associated topics are also deleted (CASCADE).)

By utilizing this database schema, the Student API can efficiently manage student data, subjects, and topics, as well as track completed topics for each student. The relationship between students and subjects is handled through the `Student_Subjects` table, allowing for easy enrollment and dis-enrollment of students in various subjects. The CASCADE behavior in foreign key constraints ensures that data integrity is maintained when records in related tables are modified or deleted.

## Table of Compontents and their Contents

- [LecturesController](#lecturescontroller)
  - [GET /api/lectures](#get-apilectures)
  - LecturesController Error Handling
  - LecturesController Logging
- [StudentInfoController](#studentinfocontroller)
  - [GET /api/studentinfo](#get-apistudentinfo)
  - [GET /api/studentinfo/{studentId}](#get-apistudentinfostudentid)
  - [POST /api/studentinfo](#post-apistudentinfo)
  - [POST /api/studentinfo/{studentId}/update-topic](#post-apistudentinfostudentidupdate-topic)
  - StudentInfoController Error Handling
  - StudentInfoController Logging
- [SubjectsController](#subjectscontroller)
  - [GET /api/subjects](#get-apisubjects)
  - [POST /api/subjects](#post-apisubjects)
  - [POST /api/subjects/{subjectId}/topics](#post-apisubjectssubjectidtopics)
  - SubjectsController Error Handling
  - SubjectsController Logging
- [Conclusion](#conclusion)

## LecturesController

### GET /api/lectures

This endpoint retrieves a list of subjects along with their associated topics.

**Request:**
- No request parameters required.

**Response:**
- Success (200 OK): The endpoint returns a list of subjects with their associated topics.

**Error Responses:**
- 500 Internal Server Error: If an error occurs while querying the database or handling SQL-related errors.

### LecturesController Error Handling

The LecturesController implements error handling to provide informative responses in case of any issues.

- DbUpdateException: This exception is caught when there is an error while updating the database. It is used to handle SQL-related errors such as duplicate keys or foreign key violations. The SQL-specific error information is extracted, and an appropriate error message is returned.

- Generic Exception: Any other unexpected exception that occurs while fetching lectures with topics is logged, and a generic error message is returned.

### LecturesController Logging

The LecturesController logs key events using the logger interface provided in the constructor. It records the following events:

- "GetLecturesWithTopics endpoint was called.": Logs when the endpoint is called.
- "An error occurred while updating the database: {ex}": Logs SQL-related errors if they occur during database updates.
- "An error occurred while fetching lectures with topics: {ex}": Logs any other unexpected exceptions that occur while fetching lectures with topics.

## StudentInfoController

### GET /api/studentinfo

This endpoint retrieves a list of students along with the IDs of their associated subjects.

**Request:**
- No request parameters required.

**Response:**
- Success (200 OK): The endpoint returns a list of students with their associated subject IDs.

**Error Responses:**
- 500 Internal Server Error: If an error occurs while fetching students with subjects.

### GET /api/studentinfo/{studentId}

This endpoint retrieves a specific student along with the completion status of topics associated with that student.

**Request:**
- URL Parameters:
  - studentId (integer): The ID of the student to retrieve.

**Response:**
- Success (200 OK): The endpoint returns a JSON object containing the studentId and an array of completed topics along with their IDs and completion status.

**Error Responses:**
- 404 Not Found: If the student with the specified studentId does not exist, the endpoint returns a 404 Not Found response.
- 500 Internal Server Error: If an error occurs while processing the request.

### POST /api/studentinfo

This endpoint allows you to add a new student along with the subjects they are enrolled in.

**Request:**
- Request Body:
  - The request body should be a JSON object with the following properties:
    - FirstName (string, required): The first name of the student.
    - SecondName (string, required): The last name of the student.
    - Title (string): The title of the student (e.g., Mr., Ms.).
    - Email (string, required): The email address of the student.
    - Dob (DateTime): The date of birth of the student.
    - SubjectIds (array of integers): An optional array containing the IDs of the subjects the student is enrolled in.

**Response:**
- Success (200 OK): The endpoint returns a 200 OK response if the student is added successfully.

**Error Responses:**
- 400 Bad Request: If the request body is invalid (e.g., missing required properties or incorrect data types), the endpoint returns a 400 Bad Request response along with detailed error messages.

- 500 Internal Server Error: If an unexpected exception occurs while processing the request, the endpoint returns a 500 Internal Server Error response along with a generic error message.

### POST /api/studentinfo/{studentId}/update-topic

This endpoint allows you to update the completion status of a topic for a specific student.

**Request:**
- URL Parameters:
  - studentId (integer): The ID of the student whose topic completion status is to be updated.

- Request Body:
  - The request body should be a JSON object with the following properties:
    - TopicId (integer, required): The ID of the topic to update.
    - IsComplete (boolean, required): The new completion status of the topic (true for completed, false for not completed).

**Response:**
- Success (200 OK): The endpoint returns a 200 OK response if the topic completion status is updated successfully.

**Error Responses:**
- 400 Bad Request: If the request body is invalid or the TopicId is not provided or invalid, the endpoint returns a 400 Bad Request response along with a detailed error message.

- 404 Not Found: If the student with the specified studentId does not exist, the endpoint returns a 404 Not Found response.

### StudentInfoController Error Handling

The StudentInfoController implements error handling to provide informative responses in case of any issues.

- 404 Not Found: If a student with the specified studentId is not found in the database, the endpoint returns a 404 Not Found response.

- 400 Bad Request: If the request body is invalid or missing required properties, the endpoint returns a 400 Bad Request response with appropriate error messages.

- 500 Internal Server Error: If an unexpected exception occurs while processing the request, the endpoint returns a 500 Internal Server Error response along with a generic error message.

### StudentInfoController Logging

The StudentInfoController logs key events using the logger interface provided in the constructor. It records the following events:

- "Successfully retrieved student with ID {studentId}.": Logs when the GetStudentWithCompletedTopics endpoint is successfully called and returns the student details.

- "Successfully updated topic status for student with ID {studentId}.": Logs when the UpdateTopicStatus endpoint successfully updates the completion status of a topic for a specific student.

- "Invalid request data or TopicId.": Logs when an invalid request is made to the UpdateTopicStatus endpoint with missing or incorrect request data.

- "An error occurred while processing the request: {ex}": Logs the exception if any other error occurs during request processing for both the GetStudentWithCompletedTopics and UpdateTopicStatus endpoints.

## SubjectsController

### GET /api/subjects

This endpoint retrieves a list of all subjects available in the database.

**Request:**
- No request parameters required.

**Response:**
- Success (200 OK): The endpoint returns a list of subjects.

**Error Responses:**
- 500 Internal Server Error: If an error occurs while fetching subjects.

### POST /api/subjects

This endpoint allows you to add a new subject to the database.

**Request:**
- Request Body:
  - The request body should be a JSON object representing the new subject to be added. It should include the following properties:
    - Title (string, required): The title of the subject.

**Response:**
- Success (200 OK): The endpoint returns a 200 OK response along with the newly added subject in the response body.

**Error Responses:**
- 500 Internal Server Error: If an unexpected exception occurs while adding the subject to the database, the endpoint returns a 500 Internal Server Error response along with a generic error message.

### POST /api/subjects/{subjectId}/topics

This endpoint allows you to add a new topic to an existing subject.

**Request:**
- URL Parameters:
  - subjectId (integer): The ID of the subject to which the new topic will be added.

- Request Body:
  - The request body should be a JSON object representing the new topic to be added. It should include the following properties:
    - Title (string, required): The title of the topic.

**Response:**
- Success (200 OK): The endpoint returns a 200 OK response along with the newly added topic in the response body.

**Error Responses:**
- 404 Not Found: If the subject with the specified subjectId does not exist in the database, the endpoint returns a 404 Not Found response.

- 500 Internal Server Error: If an unexpected exception occurs while adding the topic to the subject, the endpoint returns a 500 Internal Server Error response along with a generic error message.

## SubjectsController Error Handling

The SubjectsController implements error handling to provide informative responses in case of any issues.

- 404 Not Found: If a subject with the specified subjectId is not found in the database, the endpoint returns a 404 Not Found response.

- 500 Internal Server Error: If an unexpected exception occurs while fetching subjects, adding a subject, or adding a topic to a subject, the endpoint returns a 500 Internal Server Error response along with a generic error message.

