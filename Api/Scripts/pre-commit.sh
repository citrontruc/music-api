#!/bin/sh
echo "Pre-commit hook is running"
dotnet tool run csharpier format .
if [ $? -ne 0 ]; then
  echo "CSharpier formatting failed. Please fix formatting before committing."
  exit 1
fi