!/bin/bash
run_cmd="dotnet run --server.urls https://*:443"
export PATH="$PATH:~/.dotnet/tools"
dotnet tool install --global dotnet-ef
set -e
until dotnet ef database update --context ApplicationDbContext; do
echo "SQL Server is starting up"
sleep 1
done
until dotnet ef database update --context XMIProjectContext; do
echo "SQL Server is starting up"
sleep 1
done
echo "SQL Server is up - executing command"
exec $run_cmd