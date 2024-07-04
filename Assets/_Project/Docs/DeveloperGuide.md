# Developer Guide
Welcome to the Quarter-Life Project.

## Contributing
We welcome anyone to contribute to our project. 

To contribute we ask that you follow these guidelines so we can 
keep the project organized and maintainable.

We have a dedicated discord server for the project where you can ask questions and
connect with the other developers/aspiring developers on the project. 

To get access to the discord server please contact [Rabb1T](https://github.com/Rabb1T-762)
on github. 

## Installation
Make sure you have the following installed:
- Unity hub
  - Unity editor version 2022.3.23f1 LTS
- Git
- Dotnet SDK version 8.0

Feel free to fork the repository so you can create and push to branches as you please.

Step 1: Fork The Repository:
```bash
https://github.com/Rabb1T-762/quarter-life.git
```
Step 2: Clone Your Repository:
```bash
git clone https ://github.com/<your-username>/quarter-life.git
```

Step 3: Switch To The Develop Branch:
```bash
git checkout develop
```

Step 4: Create A Feature/Fix Branch:
```bash
git checkout -b feature/<your-feature> develop
```

Step 5: Make Your Changes
Step 6: Make Sure You Have Tests For Your Code

Step 7: Push Your Changes To Your Fork:
```bash
git push origin feature/<your-feature>
```

Step 8: Create A Pull Request:
Create a pull request from your fork to the develop branch of the main repository.
- Go to the main repository on github
- Click on "New Pull Request"
- Select your branch and create the PR to merge into `develop`

### Important Notes
- Pull Requests: 
  - Ensure your PR includes a clear description of changes
  - Ensure your PR includes tests for your changes
- Code Review:
  - PRs will be reviewed by the project maintainers before merging
  - Please be patient and be prepared to make changes based on feedback
- Discussion:
  - Feel free to ask questions in the PR or in the discord server
  - Use issues or the discord server to propose changes or discuss ideas before starting work
 
## Code Style
Please follow the C# coding conventions as defined by Microsoft.

[C# Identifier Names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names)

[C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

### NOTE:
For performance reasons, LINQ will not be used in the project.

### Additional Clean Code Practices
- Use meaningful names for variables, functions, and classes
- Keep functions small and focused
  - Functions should do one thing and do it well
  - Prefer functions that do not exceed 12 lines
  - Functions longer than 24 lines should be flagged for refactoring
- Maximum Depth of 3
  - Any nesting beyond 3 levels should be refactored
- Prefer Early Return Over Maintaining State
  - Return early out of methods and functions
  - Avoid maintaining excessive state in a function execution
- Avoid Else Statements

## Testing
We will be using NUnit for testing. Please write tests for any new code you write.
No code will be merged into Develop or main without a test suite. 

For any questions on nunit please refer to the [NUnit Documentation](https://docs.nunit.org/)
For any question regarding testing see the curated references section in the discord server.

## Git Workflow
For any task/change branch off of develop and give the branch a descriptive name.
Once the task is complete and you have tests for your code, 
create a pull request to merge the branch back into develop. 

All the current test will need to pass and your code will need to be reviewed before it is merged into develop.

Only complete features will be merged into main.

Main will always maintain a working and tested version of the project.

### Helpful links
Main and develop will use semantic versioning.
[Semantic Versioning](https://semver.org/)

For information about tagging see:
[Git Tagging](https://git-scm.com/book/en/v2/Git-Basics-Tagging)

For any git related questions please refer to the [Git Handbook](https://guides.github.com/introduction/git-handbook/)

## Documentation
Please keep the documentation up to date.

There is a dedicated documentation folder in the project. 

Please add any new documentation to the folder and updated any related documentation.

It is preferred that you use markdown for documentation and add diagrams/images to describe your code. 

Diagrams and images go into the DiagramsAndImages folder in the documentation folder.
We are currently using [draw.io](https://draw.io/) for diagrams.

For information on making class and object diagrams in UML see:

Class Diagrams:
[Visual Paradigm Guide To Class Diagrams](https://www.visual-paradigm.com/guide/uml-unified-modeling-language/what-is-class-diagram/)

Object Diagrams:
[Visual Paradigm Guide To Object Diagrams](https://www.visual-paradigm.com/guide/uml-unified-modeling-language/what-is-object-diagram/)

If you need have questions or need help with documentation please ask in the discord server.