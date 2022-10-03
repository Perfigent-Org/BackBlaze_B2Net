using B2Net.Client.IOpration;
using B2Net.Client.Opration;
using B2Net.Core.Settings;

namespace B2Net.Client
{
    public interface IB2NetClient
    {
        IKeyOpration Keys { get; }
        IBucketOpration Buckets { get; }
        IFileOpration Files { get; }
    }

    public class B2NetClient : ClientBase, IB2NetClient
    {
        public IKeyOpration Keys { get; }
        public IBucketOpration Buckets { get; }
        public IFileOpration Files { get; }

        public B2NetClient(B2Settings options) : base(options.KeyId, options.ApplicationKey)
        {
            Keys = new KeyOpration(new B2Client(Options));
            Buckets = new BucketOpration(new B2Client(Options));
            Files = new FileOpration(new B2Client(Options));
        }
    }
}
