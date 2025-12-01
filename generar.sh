#!/bin/bash

# Nombre del proyecto base por el argumento 1

if [ -z "$1" ]; then
    echo "Por favor, proporciona un nombre para el proyecto."
    exit 1
fi

solutionName=$1

mkdir -p "$solutionName"

cd $solutionName
srcDir="src"
testDir="test"

echo "📁 Creando estructura de carpetas..."
mkdir -p "$srcDir"
mkdir -p "$testDir"

echo "📦 Creando solución..."
dotnet new sln -n "$solutionName"

echo "🚀 Creando proyectos..."
dotnet new webapi -n "$solutionName.Api" -o "$srcDir/$solutionName.Api"
dotnet new classlib -n "$solutionName.Application" -o "$srcDir/$solutionName.Application"
dotnet new classlib -n "$solutionName.Domain" -o "$srcDir/$solutionName.Domain"
dotnet new classlib -n "$solutionName.Infrastructure" -o "$srcDir/$solutionName.Infrastructure"
dotnet new xunit -n "$solutionName.Tests" -o "$testDir/$solutionName.Tests"

echo "🔗 Agregando referencias entre proyectos..."
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" reference \
           "$srcDir/$solutionName.Domain/$solutionName.Domain.csproj"

dotnet add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" reference \
           "$srcDir/$solutionName.Application/$solutionName.Application.csproj" \
           "$srcDir/$solutionName.Domain/$solutionName.Domain.csproj"

dotnet add "$srcDir/$solutionName.Api/$solutionName.Api.csproj" reference \
           "$srcDir/$solutionName.Application/$solutionName.Application.csproj" \
           "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj"

dotnet add "$testDir/$solutionName.Tests/$solutionName.Tests.csproj" reference \
           "$srcDir/$solutionName.Domain/$solutionName.Domain.csproj" \
           "$srcDir/$solutionName.Application/$solutionName.Application.csproj"

echo "📌 Agregando proyectos a la solución..."
dotnet sln "$solutionName.sln" add "$srcDir/$solutionName.Api/$solutionName.Api.csproj"
dotnet sln "$solutionName.sln" add "$srcDir/$solutionName.Application/$solutionName.Application.csproj"
dotnet sln "$solutionName.sln" add "$srcDir/$solutionName.Domain/$solutionName.Domain.csproj"
dotnet sln "$solutionName.sln" add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj"
dotnet sln "$solutionName.sln" add "$testDir/$solutionName.Tests/$solutionName.Tests.csproj"

echo "✅ Proyecto '$solutionName' generado con arquitectura hexagonal."


echo "Instalando paquetes"

#dotnet add "$srcDir/$solutionName.Api/$solutionName.Api.csproj" package Orchestrix.Mediator 
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package Microsoft.Extensions.Configuration
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package Microsoft.Extensions.Logging.Abstractions
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package FluentValidation.AspNetCore
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package FluentValidation.DependencyInjectionExtensions
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package Mediator.SourceGenerator --version 3.0.*
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package Mediator.Abstractions --version 3.0.*
dotnet add "$srcDir/$solutionName.Application/$solutionName.Application.csproj" package ErrorOr
 

dotnet add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" package Microsoft.EntityFrameworkCore
dotnet add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" package Microsoft.EntityFrameworkCore.Tools
dotnet add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" package Npgsql.EntityFrameworkCore.PostgreSQL


dotnet add "$srcDir/$solutionName.Infrastructure/$solutionName.Infrastructure.csproj" Microsoft.EntityFrameworkCore.Design

dotnet add "$srcDir/$solutionName.Api/$solutionName.Api.csproj" Microsoft.EntityFrameworkCore.Design
#dotnet add package Microsoft.Extensions.Logging.Abstractions
#dotnet add package Microsoft.Extensions.Configuration