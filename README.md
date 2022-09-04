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
grpcurl -plaintext -d '{\"leaderboard\":1}' -proto leaderboard.proto localhost:5150 leaderboard.v1.LeaderboardService/Stream
```


## Solution intent

### Solution 1 - Leaderboard for all

1. gRPC -> Subscribe -> ReactiVex (Observable Shared Object)
1. Background Worker -> Push Updates -> ReactiVex

### Solution 2 - Leaderboard by game

1. gRPC -> Subscribe on Game XYZ -> ReactiVex (Observable Shared Object)
1. Background Worker -> Push Updates MANY GAMES -> ReactiVex


# TODOs
- [x] Create a Background worker to ingest data to change leaderboards
- [x] Create Reactivex shared object and expose methods to Publishe / Subscribe for observable objects
- [ ] Service Discovery available in this app - so it is not needed anymore to provide the proto file format
