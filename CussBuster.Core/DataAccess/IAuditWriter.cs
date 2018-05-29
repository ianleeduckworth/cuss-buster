using CussBuster.Core.Data.Entities;

namespace CussBuster.Core.DataAccess
{
	public interface IAuditWriter
	{
		void WriteToAudit(int wordId);
		void LogUserCall(User user);
	}
}