# streaming-grpc
.Net Core Streaming gRPC app that will stream Leaderboard changes.

# Setup
- dotnet new sln -n LeaderboardStreaming
- dotnet new grpc -n StreamingService
- dotnet sln add .\StreamingService\StreamingService.csproj
- change the Kestrel configuration to support Http1AndHttp2

# installing grpcurl
https://github.com/fullstorydev/grpcurl

grpcurl command
```
grpcurl -plaintext -d "{\"name\":\"Kelvin\"}" -proto greet.proto localhost:5150 greet.Greeter/SayHello
```