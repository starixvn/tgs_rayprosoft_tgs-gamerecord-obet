using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tgs.GameRecord.OBet.Common;

public record OBetGameRecordMessage
{
	[Required]
	[DataType(DataType.Url)]
	[DefaultValue(typeof(Uri), "https://sports-api.ob33club.com/")]
	public Uri ApiUrl { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "G4sL7r0kKFI5Mm317w99")]
	public string ApiKey { get; init; } = default!;
	[Required]
	[DefaultValue(typeof(string), "YYTES12")]
	public string AgentId { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-29T17:26:00+08:00")]
	public DateTimeOffset StartTime { get; init; } = default!;
	[Required]
	[DataType(DataType.DateTime)]
	[DefaultValue(typeof(DateTimeOffset), "2023-03-29T18:00:00+08:00")]
	public DateTimeOffset EndTime { get; init; } = default!;
}
