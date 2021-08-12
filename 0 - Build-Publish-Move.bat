@echo off

dotnet restore

dotnet build --no-restore -c Release

move /Y Panosen.ElasticSearch\bin\Release\Panosen.ElasticSearch.*.nupkg D:\LocalSavoryNuget\
move /Y Panosen.ElasticSearch.Java\bin\Release\Panosen.ElasticSearch.Java.*.nupkg D:\LocalSavoryNuget\
move /Y Panosen.ElasticSearch.Java.Engine\bin\Release\Panosen.ElasticSearch.Java.Engine.*.nupkg D:\LocalSavoryNuget\
move /Y Panosen.ElasticSearch.Mapping\bin\Release\Panosen.ElasticSearch.Mapping.*.nupkg D:\LocalSavoryNuget\
move /Y Panosen.ElasticSearch.Mapping.Engine\bin\Release\Panosen.ElasticSearch.Mapping.Engine.*.nupkg D:\LocalSavoryNuget\

pause