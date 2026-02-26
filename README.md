Overview
-
The Carbon Footprint Calculator is a cross-platform application developed using .NET technologies (C# and MAUI). 
The app calculates the approximate amount of carbon dioxide emitted by a person in a year. The primary goal of this application is
to help consumers become aware of and monitor their environmental impact. By understanding their data, users can deduce practical
ways to reduce their carbon footprint, ultimately helping the environment and themselves.

Technologies used:
-
-.NET MAUI & C# for frontend and core application logic;

-EntityFramework&SQL for local database;

Key Features:
-
The user inputs are organised into three main categories: travel, food and home. It also display a history tracking of past calculations.
The app takes in consideration:

-number of yearly flights;

-how often public transport is used;

-monthly kWh usage;

-amount of green energy used;

-type of diet;

-shopping habits;

The logic behind:
-
-it operates using a local database created with EntityFramework packages and DB Browser for SQLite  to stores the history 
of previously calculated CO2 quantities;

-for this project I experimented with Google Gemini, using it to generate foundational code and UI elements 
(like frames, spacing, and styling), which I then customized, debugged, and refined to fit the specific needs of the application;
