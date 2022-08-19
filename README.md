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
grpcurl -plaintext -proto points.proto localhost:5150 leaderboard.v1.Points/Stream
```


# TODOs
- [ ] Service Discovery available in this app - so it is not needed anymore to provide the proto file format
- [ ] Create a Background worker to ingest data to change leaderboards
