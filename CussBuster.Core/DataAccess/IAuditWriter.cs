namespace CussBuster.Core.DataAccess
{
	public interface IAuditWriter
	{
		void WriteToAudit(int wordId);
	}
}