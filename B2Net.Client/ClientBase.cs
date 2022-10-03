using B2Net.Models;

namespace B2Net.Client
{
    public class ClientBase
    {
        public B2Options Options { get; set; }
        public ClientBase(string KeyId, string ApplicationKey)
        {
            Options = new B2Options()
            {
                KeyId = KeyId,
                ApplicationKey = ApplicationKey
            };
        }
    }
}
