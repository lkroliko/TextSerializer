using System.Collections;
using MrRabbit.TextSerializer.Common.Attributes;

namespace MrRabbit.TextSerializer.SharedTests.Fakes;
[MessageId("124")]
public class FakeCollectionReceiveMessage : ReceiveMessage, ICollection
{
    public int MessageId { get; set; }

    public int Count => throw new NotImplementedException();

    public bool IsSynchronized => throw new NotImplementedException();

    public object SyncRoot => throw new NotImplementedException();


    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
