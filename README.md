# CleaningRobot. Coding Test

## Project Structure

```text
|--> CleanignRobot.CleaningRobot
|--> CleaningRobot.Common
|--> CleaningRobot.Console
|--> CleaningRobot.Models
|--> CleaningRobot.Tests
\--> CleaningRobot.WebAPI
```

- **CleanignRobot.CleaningRobot**. NetStandart2.0 shared library. Cleaning Robot logic itself. Provides interface and interface implementation. Can be attached to other projects;
- **CleanignRobot.Common**. Common classes used in other projects. Contains some JSON converters to parse and produce JSON files wich describes robots input and output;
- **CleanignRobot.Console**. Console program used to work with robot;
- **CleanignRobot.Models**. NetStandart2.0 shared library with models used by other projects;
- **CleanignRobot.Tests**. MSTest project used to provide Unit Tests for **CleanignRobot.CleaningRobot**, **CleanignRobot.Common**, **CleanignRobot.WebAPI**;
- **CleanignRobot.WebAPI**. ASP.NET Core2 WebAPI project. Implements RESTfull microservice to work with robot.

Project folder also contains additional suddirrectories like __.vscode__ and __.git__. First is used to work with project in Microsot Visual Studio Code IDE and another -- is Git VCS repository.

## Requirements
To successfully build and run project **.NET Core 2.0** enviroment and CLI must be installed. To check if you have all you need, run ``shell`` (Linux) or ``cmd``(Windows) and execute:

```sh
dotnet --version
```
If reurned result is greater than 2.0.0 everything is ok. Otherwise, read [this](https://docs.microsoft.com/en-us/dotnet/core/) to see, how to install .NET Core on your system.

## Building and running

Here is list of executable projects:
- **CleanignRobot.Console**;
- **CleanignRobot.Tests**;
- **CleanignRobot.WebAPI**.

### CleanignRobot.Console
To run **CleanignRobot.Console** open ``shell`` or ``cmd``, navigate to folder what contains project and execute
```
dotnet run <input.json> <output.json>
```
Where *<input.json>* is path to JSON file with input data for cleaning robot, and *<output.json>* is path to resulting file. All project dependencies will be downloaded and builded automaticly by ``dotnet`` command. If you provide paths to JSON files correct the output will contains something like this:
```
0.			    X:3,	Y:0,	F:N,	B:80;
1.		C:TL,	X:3,	Y:0,	F:W,	B:79	-> OK;
2.		C:A,	X:2,	Y:0,	F:W,	B:77	-> OK;
3.		C:C,	X:2,	Y:0,	F:W,	B:72	-> OK;
4.		C:A,	X:1,	Y:0,	F:W,	B:70	-> OK;
5.		C:C,	X:1,	Y:0,	F:W,	B:65	-> OK;
6.		C:TR,	X:1,	Y:0,	F:N,	B:64	-> OK;
7.		C:A,	X:1,	Y:0,	F:N,	B:62	-> B1;
8.		B1:TR,	X:1,	Y:0,	F:E,	B:61	-> OK;
9.		B1:A,	X:2,	Y:0,	F:E,	B:59	-> OK;
10.		C:C,	X:2,	Y:0,	F:E,	B:54	-> OK;
```
It's robot command execution log and it show how robot execute each command and the result of each command.

### CleanignRobot.Tests
To run **CleanignRobot.Tests** open ``shell`` or ``cmd``, navigate to folder what contains project and execute
```
dotnet test
```
All project dependencies will be downloaded and builded automaticly by ``dotnet`` command. If everything is ok, you will see the output of **MSTest**:
```
CleaningRobot/CleaningRobot.Tests/bin/Debug/netcoreapp2.0/CleaningRobot.Tests.dll(.NETCoreApp,Version=v2.0)
Microsoft (R) Test Execution Command Line Tool Version 15.5.0
Copyright (c) Microsoft Corporation.  All rights reserved.

Starting test execution, please wait...

Total tests: 7. Passed: 7. Failed: 0. Skipped: 0.
Test Run Successful.
```

### CleanignRobot.WebAPI
To run **CleanignRobot.WebAPI** open ``shell`` or ``cmd``, navigate to folder what contains project and execute
```
dotnet run
```
All project dependencies will be downloaded and builded automaticly by ``dotnet`` command. If everything is ok, you will see the output of like this:
```
Hosting environment: Production
Content root path: /home/arhichief/projects/CleaningRobot/CleaningRobot.WebAPI
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```
Now you can send requests to RESTfull api using ``curl`` or ``Postman`` or another program what can create and send HTTP requests.

##### PUT /api/robot/robot_name
Create new robot called *robot_name*. 
```
PUT: http://localhost:5000/api/robot/Johny
-----
{
  "map": [
    ["S", "S", "S", "S"],
    ["S", "S", "C", "S"],
    ["S", "S", "S", "S"],
    ["S", "null", "S", "S"]
  ],
  "start": {"X": 3, "Y": 0, "facing": "N"},
  "commands": [ "TL","A","C","A","C","TR","A","C"],
  "battery": 80
}
```
Create request with ``curl``
```bash
curl -H "Content-Type: application/json" -X PUT -d '{"map": [["S", "S", "S", "S"],["S", "S", "C", "S"],["S", "S", "S", "S"],["S", "null", "S", "S"]],"start": {"X": 3, "Y": 0, "facing": "N"},"commands": [ "TL","A","C","A","C","TR","A","C"],"battery": 80}' http://localhost:5000/api/robot/Johny
```

##### GET /api/robot
Get list of all created robots
```
GET: http://localhost:5000/api/robot
```
Create request with ``curl``
```bash
curl -X GET  http://localhost:5000/api/robot
```

##### POST /api/robot/robot_name/execute
Ask robot *robot_name* to  execute list of commands
```
POST: http://localhost:5000/api/robot/Johny/execute
-----
[ "TL","A","C","A","C","TR","A","C"]
```
Create request with ``curl``
```bash
curl -H 'application/json' -X POST  http://localhost:5000/api/robot -d '[ "TL","A","C","A","C","TR","A","C"]' http://localhost:5000/api/robot/Johny/execute
```

##### GET /api/robot/robot_name
Get robot *robot_name* final result
```
GET: http://localhost:5000/api/robot/Johny
```
Create request with ``curl``
```bash
curl -X GET http://localhost:5000/api/robot/Johny
```

##### DELETE /api/robot/robot_name
Delete robot *robot_name*
```
DELETE: http://localhost:5000/api/robot/Johny
```
Create request with ``curl``
```bash
curl -X DELETE http://localhost:5000/api/robot/Johny
```

##### GET /api/robot/robot_name/log
Get robot *robot_name* command execution log
```
GET: http://localhost:5000/api/robot/Johny/log
```
Create request with ``curl``
```bash
curl -X GET http://localhost:5000/api/robot/Johny/log
```