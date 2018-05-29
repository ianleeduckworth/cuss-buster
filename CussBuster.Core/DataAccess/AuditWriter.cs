using CussBuster.Core.Data.Entities;
using System;

namespace CussBuster.Core.DataAccess
{
	public class AuditWriter : IAuditWriter
	{
		private readonly CussBusterContext _context;

		public AuditWriter(CussBusterContext context)
		{
			_context = context;
		}

		public void LogUserCall(User user)
		{
			_context.CallLog.Add(new CallLog
			{
				UserId = user.UserId,
				EventDate = DateTime.UtcNow
			});

			_context.SaveChanges();
		}

		public void WriteToAudit(int wordId)
		{
			_context.WordAudit.Add(new WordAudit
			{
				WordId = wordId,
				EventDate = DateTime.UtcNow
			});

			_context.SaveChanges();
		}
	}
}
