using System.Runtime.Serialization;

namespace Cribbage.Utility
{
#if !SILVERLIGHT
    [DataContract]
#endif
    public struct SerializableKeyValuePair<K, V>
    {
#if !SILVERLIGHT
        [DataMember]
#endif
        public K Key { get; set; }

#if !SILVERLIGHT
        [DataMember]
#endif
        public V Value { get; set; }
    }
}
