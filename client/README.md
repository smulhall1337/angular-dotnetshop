# First time setup

**Optional:**

Some parts of this program will use `#REGION`s a lot. be sure whatever you're working in has support for region annotations
and can fold and unfold them as needed. Most modern day IDEs will have an extension for it

If on Windows:    

Git Bash (https://gitforwindows.org/) for a better terminal experience

This program was originally generated with: 
---
Microsoft Visual Studio Community 2022 RC
Version 17.0.0 RC2
VisualStudio.17.Release/17.0.0+31815.197.rc2
Microsoft .NET Framework
Version 4.8.04084
---

it should just work out of the box if loading from the solution. However, different versions of VS studio vary greatly. If the latest version isn't working for you; try getting the version above  
there is

## Required

Make sure NodeJs+NPM is installed:
https://nodejs.dev/download/
Select latest LTS

run `npm` from command/shell to ensure its installed (might need to restart the terminal)

install angular cli
run `npm install -g @angular/cli` from a command prompt/shell
run `ng` to make sure its installed

open up a terminal/command prompt
`cd` to the `Snap` directory
run `npm install`

# Tailwind setup
(this shouldn't be necessary, but just incase styles.css gets messed up)
`cd` into src
`npx tailwindcss-cli build -i ./styles.scss -o ../dist/css/styles.css`
This compiles the `@tailwind` directives in the `src/styles.scss` file into CSS
and https://tailwindcss.com/docs/installation


#Running front end only  
`cd` into `/Snap` where the `package.json` file is  

run `npm start`

newer versions of NPM might complain about no argumanets. just run 
`npm run-script start` if it tells you to

some people on windows might get an error that says something like:

`digital envelope routines: unsupported`

I have no idea why this happens but running: 

`export NODE_OPTIONS=--openssl-legacy-provider` 

before

`npm run-script start` 

fixes it

I'm not sure if theres a way to set that permanently on windows, but you'll have to run those 2 commands 
every time you close and open a new terminal


# Angular auto generated stuff
# Snap

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 12.2.10.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.

# more microsoft VS studio crap

Installed Version: Community

.NET Core Debugging with WSL   1.0
.NET Core Debugging with WSL

ASP.NET and Web Tools 2019   17.0.776.61824
ASP.NET and Web Tools 2019

ASP.NET Web Frameworks and Tools 2019   17.0.776.61824
For additional information, visit https://www.asp.net/

Azure App Service Tools v3.0.0   17.0.776.61824
Azure App Service Tools v3.0.0

Azure Functions and Web Jobs Tools   17.0.776.61824
Azure Functions and Web Jobs Tools

C# Tools   4.0.0-6.21514.4+df45061e218c9b5813c5531bc06fb238a23e30f6
C# components used in the IDE. Depending on your project type and settings, a different version of the compiler may be used.

Common Azure Tools   1.10
Provides common services for use by Azure Mobile Services and Microsoft Azure Tools.

Microsoft JVM Debugger   1.0
Provides support for connecting the Visual Studio debugger to JDWP compatible Java Virtual Machines

Microsoft Library Manager   2.1.134+45632ee938.RR
Install client-side libraries easily to any web project

Microsoft MI-Based Debugger   1.0
Provides support for connecting Visual Studio to MI compatible debuggers

Microsoft Visual Studio Tools for Containers   1.2
Develop, run, validate your ASP.NET Core applications in the target environment. F5 your application directly into a container with debugging, or CTRL + F5 to edit & refresh your app without having to rebuild the container.

NuGet Package Manager   6.0.0
NuGet Package Manager in Visual Studio. For more information about NuGet, visit https://docs.nuget.org/

ProjectServicesPackage Extension   1.0
ProjectServicesPackage Visual Studio Extension Detailed Info

Razor (ASP.NET Core)   17.0.0.2150514+d9e17e7b7f06829e976f616716a987599c5c66e9
Provides languages services for ASP.NET Core Razor.

SQL Server Data Tools   17.0.62109.30190
Microsoft SQL Server Data Tools

TypeScript Tools   17.0.1001.2002
TypeScript Tools for Microsoft Visual Studio

Visual Basic Tools   4.0.0-6.21514.4+df45061e218c9b5813c5531bc06fb238a23e30f6
Visual Basic components used in the IDE. Depending on your project type and settings, a different version of the compiler may be used.

Visual F# Tools   17.0.0-beta.21472.3+f0b5108c92b92ba5ee440228aadba3bae79b43a3
Microsoft Visual F# Tools

Visual Studio Code Debug Adapter Host Package   1.0
Interop layer for hosting Visual Studio Code debug adapters in Visual Studio

Visual Studio Container Tools Extensions   1.0
View, manage, and diagnose containers within Visual Studio.

Visual Studio IntelliCode   2.2
AI-assisted development for Visual Studio.

Visual Studio Tools for Containers   1.0
Visual Studio Tools for Containers