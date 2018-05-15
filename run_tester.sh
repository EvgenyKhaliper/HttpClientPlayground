#!/bin/bash

kill -9 $(ps aux | grep 'dotnet' | awk '{print $2}')
dotnet run -p Server/Server.csproj -c Release &