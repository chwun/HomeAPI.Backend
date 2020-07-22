using System;

namespace HomeAPI.Backend.Common
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }

		DateTime UtcNow { get; }
	}
}