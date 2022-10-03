using B2NetStandard.Models;
using System.Threading.Tasks;

namespace B2NetStandard
{
    public interface IB2Client
	{
		IFiles Files { get; }
		ILargeFiles LargeFiles { get; }
		B2Capabilities Capabilities { get; }
		Task Initialize(B2Options options, bool authorizeOnInitialize = true);
	}
}
