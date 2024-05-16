lint:
	dotnet csharpier . --check

lint-fix:
	dotnet csharpier .
	
restore:
	yarn install && dotnet tool restore
