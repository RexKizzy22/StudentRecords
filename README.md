# Student Records

This is a .NET console project containing F# scripts that does 
basic descriptive analysis over a couple of datasets (student records).

The datasets represent variations of student records some of which includes
missing and unclean data.

Student Records demonstrates the scripting capabilities of the F# language which
harnesses the incredible perfomance of the .NET runtime and handles domain modeling and 
data transformations quiet efficiently with its dynamic suite of data structures and sound type system.

## How to Run Student Records
The only prerequisite is to have the .NET 8 SDK installed on your machine.

Several scripts are written against particular datasets in the Samples directory.

To run the script for the well-formed student records:
- Uncomment the ```open Records``` import in ```Program.fs```
- Uncomment the first ```EntryPoint``` attribute and the commented out ```main``` function in ```Program.fs```
- Comment out the later ```EntryPoint``` attribute and the ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/StudentScores.csv"
```

To run the script for the student records with missing data:
- Uncomment the ```open MissingData``` import in ```Program.fs```
- Uncomment the first ```EntryPoint``` attribute and the commented out ```main``` function in ```Program.fs```
- Comment out the later ```EntryPoint``` attribute and the ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/StudentScoresNA.csv"
```

To run the script for the student records with data of students that are absent/excused:
- Uncomment the ```open AE``` import in ```Program.fs```
- Uncomment the first ```EntryPoint``` attribute and the commented out ```main``` function in ```Program.fs```
- Comment out the later ```EntryPoint``` attribute and the ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/StudentScoresAE.csv"
```

To run the script for the student records with numeric school codes:
- Uncomment the ```open Dictionary``` import in ```Program.fs```
- Uncomment the later ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Comment out the first ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/SchoolCodes.csv" "Sample/StudentScoresSchool.csv"
```

To run the script for the student records with alphabetic school codes:
- Change line 197 in ```Dictionary.fs``` to ```|> Array.map (Student.fromStringAlpha (SchoolCodeAlpha.load schoolCodeFilePath))```
- Uncomment the ```open Dictionary``` import in ```Program.fs```
- Uncomment the later ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Comment out the first ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/SchoolCodesAlpha.csv" "Sample/StudentScoresSchoolAlphaCodes.csv"
```

To run the script for the student records with extra school codes:
- Uncomment the ```open Dictionary``` import in ```Program.fs```
- Uncomment the later ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Comment out the first ```EntryPoint``` attribute and the following ```main``` function in ```Program.fs```
- Run the following command in the terminal
```bash
dotnet run "Samples/SchoolCodesAlpha.csv" "Sample/StudentScoresSchoolExtraCodes.csv"
```

## Demo
[Demo file][Demo]

[Demo]: demo.mov