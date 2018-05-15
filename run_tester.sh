#!/bin/bash

kill -9 $(ps aux | grep 'dotnet' | awk '{print $2}')
dotnet run -p Tester/Tester.csproj -c Release