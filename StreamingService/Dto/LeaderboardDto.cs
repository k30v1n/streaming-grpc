namespace StreamingService.Dto;

public class LeaderboardDto
{
    public int LeaderBoardId { get; set; }
    public IReadOnlyCollection<PersonPointsDto> Data { get; set; }
}
